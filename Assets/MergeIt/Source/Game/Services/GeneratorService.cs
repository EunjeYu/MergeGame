// Copyright (c) 2024, Awessets

using System;
using System.Linq;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Field;
using MergeIt.Game.Helpers;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;
using UnityEngine;

namespace MergeIt.Game.Services
{
    public class GeneratorService : IGeneratorsService, IInitializable, IDisposable, IUpdatable
    {
        [Introduce] private IConfigsService _configsService;

        [Introduce] private ICurrencyService _currencyService;

        [Introduce] private FieldLogicModel _fieldLogicModel;

        [Introduce] private IMessageBus _messageBus;

        [Introduce] private IGameSaveService _saveService;

        [Introduce] private UserServiceModel _userServiceModel;

        public void Initialize()
        {
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.AddListener<RemoveElementMessage>(OnRemoveElementMessageHandler);
            _messageBus.AddListener<MergeElementsMessage>(MergeElementsMessageHandler);
            _messageBus.AddListener<SplitElementMessage>(SplitElementsMessageHandler);
            _messageBus.AddListener<CreateElementMessage>(CreateElementMessageHandler);
            _messageBus.AddListener<SkipTimeMessage>(OnSkipTimeMessageHandler);
        }

        public void Dispose()
        {
            _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.RemoveListener<RemoveElementMessage>(OnRemoveElementMessageHandler);
            _messageBus.RemoveListener<MergeElementsMessage>(MergeElementsMessageHandler);
            _messageBus.RemoveListener<SplitElementMessage>(SplitElementsMessageHandler);
            _messageBus.RemoveListener<CreateElementMessage>(CreateElementMessageHandler);
            _messageBus.RemoveListener<SkipTimeMessage>(OnSkipTimeMessageHandler);
        }

        public void TryOpen(IFieldElement generator)
        {
            if (_fieldLogicModel.OpeningGenerator != null)
            {
                Debug.Log("Another generator is already opening.");

                return;
            }

            ElementConfig generatorConfig = generator.ConfigParameters.ElementConfig;
            generator.GeneratorOpenParameters.StartOpeningTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            generator.GeneratorOpenParameters.RemainingTime.Value = generatorConfig.GeneratorSettings.OpenTime;
            _fieldLogicModel.OpeningGenerator = generator;

            _messageBus.Fire(new GeneratorOpenStartMessage
            {
                GeneratorPoint = generator.InfoParameters.LogicPosition
            });
        }

        public void TrySkipOpening(IFieldElement generator)
        {
            ElementGeneratorSettings generatorSettings = generator.ConfigParameters.ElementConfig.GeneratorSettings;
            if (_currencyService.TryPay(generatorSettings.SkipOpenCostSettings))
            {
                ClearOpenableGenerator(generator);

                _messageBus.Fire(new GeneratorOpenedMessage
                {
                    GeneratorPoint = generator.InfoParameters.LogicPosition
                });
            }
        }

        public void TrySkipCharging(IFieldElement generator)
        {
            ElementGeneratorSettings generatorSettings = generator.ConfigParameters.ElementConfig.GeneratorSettings;
            if (_currencyService.TryPay(generatorSettings.SkipChargeCostSettings))
            {
                generator.SkipCharging();

                _messageBus.Fire(new GeneratorRestoredMessage
                {
                    GeneratorPoint = generator.InfoParameters.LogicPosition
                });

                _messageBus.Fire(new CheckGeneratorMessage
                {
                    GeneratorPoint = generator.InfoParameters.LogicPosition
                });
            }
        }

        public void Update()
        {
            if (_fieldLogicModel != null)
            {
                float deltaTime = Time.unscaledDeltaTime;
                for (var index = _fieldLogicModel.AllGenerators.Count - 1; index >= 0; index--)
                {
                    var generator = _fieldLogicModel.AllGenerators[index];
                    IGeneratorOpenParameters generatorOpenParameters = generator.GeneratorOpenParameters;
                    IGeneratorParameters parameters = generator.GeneratorParameters;
                    if (parameters.Charging)
                    {
                        if (parameters.MinDropChargeTime.Value > 0f)
                        {
                            parameters.MinDropChargeTime.Value -= deltaTime;
                        }

                        parameters.RemainChargeTime -= deltaTime;

                        if (parameters.RemainChargeTime <= 0f)
                        {
                            generator.ChargeGenerator();
                        }

                        if (parameters.MinDropChargeTime.Value <= 0f)
                        {
                            _messageBus.Fire(new GeneratorRestoredMessage
                            {
                                GeneratorPoint = generator.InfoParameters.LogicPosition
                            });
                        }
                    }
                    else if (generatorOpenParameters is { IsOpening: true })
                    {
                        generatorOpenParameters.RemainingTime.Value -= deltaTime;

                        if (generatorOpenParameters.RemainingTime.Value <= 0f)
                        {
                            ClearOpenableGenerator(generator);

                            _messageBus.Fire(new GeneratorOpenedMessage
                            {
                                GeneratorPoint = generator.InfoParameters.LogicPosition
                            });
                        }
                    }
                }
            }
        }

        private void OnLoadedGameMessageHandler(LoadedGameMessage message)
        {
            var elements = _fieldLogicModel.FieldElements;
            IFieldElement openingGenerator = elements.FirstOrDefault(x =>
                x.Value.GeneratorOpenParameters is { IsOpening: true }).Value;

            _fieldLogicModel.OpeningGenerator = openingGenerator;
            _fieldLogicModel.AllGenerators = elements
                .Where(x => x.Value.GeneratorParameters != null)
                .Select(x => x.Value).ToList();
        }

        private void OnSkipTimeMessageHandler(SkipTimeMessage message)
        {
            for (var index = _fieldLogicModel.AllGenerators.Count - 1; index >= 0; index--)
            {
                var fieldElement = _fieldLogicModel.AllGenerators[index];
                if (fieldElement.GeneratorParameters == null)
                {
                    continue;
                }

                if (fieldElement.GeneratorOpenParameters is { IsOpening: true })
                {
                    fieldElement.TrySkipOpeningTime(message.Seconds);
                }

                if (fieldElement.GeneratorParameters.Charging)
                {
                    fieldElement.TrySkipChargingTime(message.Seconds);

                    _messageBus.Fire(new CheckGeneratorMessage
                    {
                        GeneratorPoint = fieldElement.InfoParameters.LogicPosition
                    });
                }
            }
        }

        private void OnRemoveElementMessageHandler(RemoveElementMessage message)
        {
            IFieldElement removedGenerator =
                _fieldLogicModel.AllGenerators
                    .FirstOrDefault(x => x.InfoParameters.LogicPosition == message.RemoveAtPoint);

            if (removedGenerator != null)
            {
                if (_fieldLogicModel.OpeningGenerator == removedGenerator)
                {
                    _fieldLogicModel.OpeningGenerator = null;
                }

                _fieldLogicModel.AllGenerators.Remove(removedGenerator);
            }
        }

        private void MergeElementsMessageHandler(MergeElementsMessage message)
        {
            CheckCreatedElement(message.NewElement);
        }

        private void SplitElementsMessageHandler(SplitElementMessage message)
        {
            var position = message.SplitElement1.InfoParameters.LogicPosition;

            var generator =
                _fieldLogicModel.AllGenerators.FirstOrDefault(x => x.InfoParameters.LogicPosition == position);

            if (generator != null)
            {
                _fieldLogicModel.AllGenerators.Remove(generator);
            }

            CheckCreatedElement(message.SplitElement1);
            CheckCreatedElement(message.SplitElement2);
        }

        private void CreateElementMessageHandler(CreateElementMessage message)
        {
            CheckCreatedElement(message.NewElement);
        }

        private void ClearOpenableGenerator(IFieldElement generator)
        {
            generator.GeneratorOpenParameters.RemainingTime.Value = 0f;
            generator.GeneratorOpenParameters = null;
            _fieldLogicModel.OpeningGenerator = null;
        }

        private void CheckCreatedElement(IFieldElement element)
        {
            if (element.InfoParameters.Type == ElementType.Generator)
            {
                element.UpdateGenerator();
                _fieldLogicModel.AllGenerators.Add(element);
            }
        }
    }
}