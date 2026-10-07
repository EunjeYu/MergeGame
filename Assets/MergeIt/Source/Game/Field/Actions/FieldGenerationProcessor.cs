// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Helpers;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Field.Actions
{
    public class FieldGenerationProcessor : FieldActionProcessorBase
    {
        private readonly UserServiceModel _userServiceModel = DiContainer.Get<UserServiceModel>();
        private readonly IGameFieldService _gameFieldService = DiContainer.Get<IGameFieldService>();
        private readonly FieldLogicModel _fieldLogicModel = DiContainer.Get<FieldLogicModel>();

        public override void ProcessClick(FieldCellComponent cellComponent)
        {
            base.ProcessClick(cellComponent);

            if (cellComponent.FieldElementPresenter?.GetModel().ClicksCount < 2)
            {
                return;
            }

            if (!FieldLogicModel.FieldElements.TryGetValue(cellComponent.Point, out IFieldElement fieldElement))
            {
                return;
            }

            IGeneratorParameters generatorParameters = fieldElement.GeneratorParameters;
            if (generatorParameters == null)
            {
                return;
            }

            IGeneratorOpenParameters generatorOpenParameters = fieldElement.GeneratorOpenParameters;
            if (generatorOpenParameters != null)
            {
                Debug.Log("Generator should be open or opening.");
                return;
            }

            if (_userServiceModel.Energy.Value == 0)
            {
                Debug.Log("Not enough energy.");
                return;
            }

            GridPoint? freePoint = _gameFieldService.GetNearCell(cellComponent.Point);
            //GetFreeCell();

            if (!freePoint.HasValue)
            {
                SendFieldFull();
                return;
            }

            ElementGeneratorSettings generatorSettings = fieldElement.ConfigParameters.ElementConfig.GeneratorSettings;

            if (generatorParameters.AvailableToDrop > 0)
            {
                GeneratableElement candidate = fieldElement.GetCandidateToGenerate();

                if (candidate == null)
                {
                    SendGeneratorBusy(fieldElement.InfoParameters.LogicPosition);
                    return;
                }
                
                IFieldElement generatedElement =
                    _gameFieldService.CreateNewElement(candidate.Element, freePoint.Value, generatorSettings.GenerateBlocked);
                
                var position = fieldElement.InfoParameters.LogicPosition;

                var generateResult = fieldElement.TryGenerate();
                if (generateResult.HasValue)
                {
                    if (!generateResult.Value)
                    {
                        SendRemoveElement(position);
                    }
                    else
                    {
                        SendCheckGenerator(position);
                    }
                }
                else
                {
                    return;
                }

                _userServiceModel.Energy.Value--;
                
                SendCreateElement(generatedElement, position, freePoint.Value);
                SaveService.Save(GameSaveType.Field);
            }
            else
            {
                SendGeneratorBusy(fieldElement.InfoParameters.LogicPosition);
            }
        }

        private void SendCreateElement(IFieldElement newElement, GridPoint fromPoint, GridPoint toPoint)
        {
            var message = new CreateElementMessage
            {
                FromPosition = _fieldLogicModel.GetPosition(fromPoint),
                ToPoint = toPoint,
                NewElement = newElement
            };
            
            MessageBus.Fire(message);
        }

        private void SendRemoveElement(GridPoint elementPosition)
        {
            var message = new RemoveElementMessage
            {
                RemoveAtPoint = elementPosition
            };

            MessageBus.Fire(message);
        }

        private void SendCheckGenerator(GridPoint generatorPosition)
        {
            var message = new CheckGeneratorMessage
            {
                GeneratorPoint = generatorPosition
            };

            MessageBus.Fire(message);
        }

        private void SendFieldFull()
        {
            Debug.Log($"Generation impossible. Field is full.");

            var message = new FieldFullMessage();

            MessageBus.Fire(message);
        }

        private void SendGeneratorBusy(GridPoint point)
        {
            Debug.Log($"Generation impossible. Generator charging.");

            var message = new GeneratorBusyMessage
            {
                Point = point,
            };

            MessageBus.Fire(message);
        }
    }
}