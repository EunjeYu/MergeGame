// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using MergeIt.Core.Configs;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Services;
using MergeIt.Game.Field;
using MergeIt.Game.Helpers;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Converters
{
    public class ConfigProcessor : IConfigProcessor
    {
        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private FieldLogicModel _fieldLogicModel;

        public FieldData Convert(LevelConfig levelConfig)
        {
            var saveFieldData = new FieldData
            {
                FieldHeight = levelConfig.FieldHeight,
                FieldWidth = levelConfig.FieldWidth,
                SavedElementsData = new List<FieldElementData>()
            };

            foreach (LevelElementData levelElementData in levelConfig.FieldElementsData)
            {
                FieldElementData savedElementData = ConvertToFieldElementData(levelElementData);
                saveFieldData.SavedElementsData.Add(savedElementData);
            }

            return saveFieldData;
        }

        public FieldData BuildLevel()
        {
            var levelConfig = _configsService.LevelConfig;
            var elements = _fieldLogicModel.FieldElements;

            var saveFieldData = new FieldData
            {
                FieldHeight = levelConfig.FieldHeight,
                FieldWidth = levelConfig.FieldWidth,
                SavedElementsData = new List<FieldElementData>()
            };

            foreach (IFieldElement fieldElement in elements.Values)
            {
                var savedElementData = ConvertToFieldElementData(fieldElement);
                saveFieldData.SavedElementsData.Add(savedElementData);
            }

            return saveFieldData;
        }

        public FieldElementData ConvertToFieldElementData(LevelElementData levelElementData)
        {
            ElementConfig elementConfig = levelElementData.Element;
            FieldElementData fieldElementData = null;

            if (elementConfig != null)
            {
                fieldElementData = new FieldElementData
                {
                    InfoParameters = new SavedInfoParameters
                    {
                        LogicPosition = levelElementData.Position,
                        IsBlocked = levelElementData.IsBlocked
                    },
                    ConfigParameters = new SavedConfigParameters
                    {
                        ElementId = levelElementData.Element.Id,
                        EvolutionId = levelElementData.EvolutionId
                    }
                };

                switch (elementConfig.Type)
                {
                    case ElementType.Generator:
                        var generatorParameters = new SavedGeneratorParameters
                        {
                            DroppedElements = 0
                        };
                        
                        if (elementConfig.GeneratorSettings.Charged)
                        {
                            generatorParameters.AvailableToDrop = elementConfig.GeneratorSettings.MaxDrop;
                        }
                        else
                        {
                            generatorParameters.StartChargingTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        }

                        fieldElementData.GeneratorParameters = generatorParameters;

                        if (elementConfig.GeneratorSettings.NeedOpen)
                        {
                            fieldElementData.GeneratorOpenParameters = new SavedGeneratorOpenParameters();
                        }

                        break;
                }
            }

            return fieldElementData;
        }

        public FieldElementData ConvertToFieldElementData(IFieldElement fieldElement)
        {
            var fieldElementData = new FieldElementData
            {
                ConfigParameters = new SavedConfigParameters
                {
                    ElementId = fieldElement.ConfigParameters.ElementConfig.Id,
                    EvolutionId = fieldElement.ConfigParameters.EvolutionData.Id
                },

                InfoParameters = new SavedInfoParameters
                {
                    LogicPosition = fieldElement.InfoParameters.LogicPosition,
                    IsBlocked = fieldElement.InfoParameters.IsBlocked
                }
            };

            if (fieldElement.GeneratorParameters != null)
            {
                var generatorParameters = new SavedGeneratorParameters
                {
                    AvailableToDrop = fieldElement.GeneratorParameters.AvailableToDrop,
                    StartChargingTime = fieldElement.GeneratorParameters.StartChargingTime,
                    DroppedElements = fieldElement.GeneratorParameters.DroppedElements,
                    ChargedCount = fieldElement.GeneratorParameters.ChargedCount,
                };

                fieldElementData.GeneratorParameters = generatorParameters;

                if (fieldElement.GeneratorOpenParameters != null)
                {
                    fieldElementData.GeneratorOpenParameters = new SavedGeneratorOpenParameters
                    {
                        StartOpeningTime = fieldElement.GeneratorOpenParameters.StartOpeningTime
                    };
                }
            }

            return fieldElementData;
        }
        
        public IFieldElement ConvertToFieldElement(FieldElementData data)
        {
            ElementConfig elementConfig = _configsService.GetConfig(data.ConfigParameters.ElementId);

            IFieldElement fieldElement = new FieldElement();

            fieldElement.ConfigParameters = CreateConfigParameters(data.ConfigParameters);
            fieldElement.InfoParameters = CreateInfoParameters(data.InfoParameters, elementConfig);
            fieldElement.ProducedByParameters = CreateProducedByParameters(elementConfig);

            switch (elementConfig.Type)
            {
                case ElementType.Generator:
                    ElementGeneratorSettings generatorSettings = elementConfig.GeneratorSettings;
                    fieldElement.GeneratorParameters = CreateGeneratorParameters(data.GeneratorParameters);
                    fieldElement.ProduceParameters = CreateProduceParameters(generatorSettings);

                    if (data.GeneratorOpenParameters != null && 
                        generatorSettings.NeedOpen)
                    {
                        fieldElement.GeneratorOpenParameters = CreateGeneratorOpenParameters(data.GeneratorOpenParameters);
                    }

                    fieldElement.UpdateGenerator();
                    break;
            }

            return fieldElement;
        }
        
        private IConfigParameters CreateConfigParameters(SavedConfigParameters parameters)
        {
            ElementConfig elementConfig = _configsService.GetConfig(parameters.ElementId);
            EvolutionData evolutionData = _configsService.GetEvolutionData(parameters.EvolutionId);

            return new ConfigParameters
            {
                ElementConfig = elementConfig,
                EvolutionData = evolutionData
            };
        }

        private IGeneratorParameters CreateGeneratorParameters(SavedGeneratorParameters data)
        {
            var generatorParameters = new GeneratorParameters();

            generatorParameters.CopyFrom(data);

            return generatorParameters;
        }

        private IInfoParameters CreateInfoParameters(SavedInfoParameters parameters, ElementConfig config)
        {
            return new InfoParameters
            {
                LogicPosition = parameters.LogicPosition,
                IsBlocked = parameters.IsBlocked,
                Name = config.CommonSettings.Name,
                Description = config.GetDescription(),
                Type = config.Type
            };
        }

        private IProduceParameters CreateProduceParameters(ElementGeneratorSettings generatorSettings)
        {
            IProduceParameters produceParameters = new ProduceParameters();
            produceParameters.Elements = new List<GeneratableFieldElement>();

            foreach (GeneratableElement generatableElement in generatorSettings.GenerateItems)
            {
                ElementConfig generatableConfig = generatableElement.Element;
                produceParameters.Elements.Add(new GeneratableFieldElement
                {
                    Config = generatableConfig,
                    Possibility = generatableElement.Possibility
                });
            }

            return produceParameters;
        }

        private IGeneratorOpenParameters CreateGeneratorOpenParameters(SavedGeneratorOpenParameters data)
        {
            IGeneratorOpenParameters generatorOpenParameters = new GeneratorOpenParameters();

            generatorOpenParameters.CopyFrom(data);

            return generatorOpenParameters;
        }

        private IProducedByParameters CreateProducedByParameters(ElementConfig element)
        {
            IProducedByParameters producedByParameters = null;
            foreach (ElementConfig elementConfig in _configsService.GetConfigs())
            {
                if (elementConfig.Type != ElementType.Generator ||
                    elementConfig == element)
                {
                    continue;
                }
                
                ElementGeneratorSettings generatorParameters = elementConfig.GeneratorSettings;
                if (generatorParameters.GenerateItems.Exists(x => x.Element == elementConfig))
                {
                    if (producedByParameters == null)
                    {
                        producedByParameters = new ProducedByParameters();
                        producedByParameters.Elements = new List<ElementConfig>();
                    }
            
                    producedByParameters.Elements.Add(elementConfig);
                }
            }

            return producedByParameters;
        }
    }
}