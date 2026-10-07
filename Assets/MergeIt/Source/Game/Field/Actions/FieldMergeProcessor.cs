// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Factories.FieldElement;
using MergeIt.Game.Helpers;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Field.Actions
{
    public class FieldMergeProcessor : FieldActionProcessorBase
    {
        private readonly IFieldElementFactory _fieldElementFactory = DiContainer.Get<IFieldElementFactory>();
        private readonly IEvolutionsService _evolutionsService = DiContainer.Get<IEvolutionsService>();
        private readonly FieldLogicModel _fieldLogicModel = DiContainer.Get<FieldLogicModel>();

        public override void ProcessClick(FieldCellComponent cellComponent)
        {
            base.ProcessClick(cellComponent);

            var presenter = cellComponent.FieldElementPresenter;
            if (presenter != null)
            {
                presenter.GetModel().ClicksCount++;

                if (!presenter.GetModel().Selected)
                {
                    presenter.Select(true);
                }
            }
        }

        public override void ProcessEndDrag(GridPoint fromPoint, GameObject toGameObject)
        {
            base.ProcessEndDrag(fromPoint, toGameObject);

            GridPoint toPoint = GridPoint.Default;

            if (toGameObject)
            {
                if (toGameObject.TryGetComponent(out FieldCellComponent toCell))
                {
                    toPoint = toCell.Point;

                    if (fromPoint.Equals(toPoint))
                    {
                        SendResetPositionMessage(fromPoint);

                        return;
                    }

                    IFieldElementPresenter toPresenter = toCell.FieldElementPresenter;

                    if (toPresenter != null)
                    {
                        var mergedResult = TryMerge(fromPoint, toPoint);
                        if (mergedResult != null)
                        {
                            SendRemoveElement(fromPoint);
                            SendRemoveElement(toPoint);
                            SendMergeMessage(fromPoint, mergedResult);

                            SaveService.Save(GameSaveType.Field);
                        }
                        else if (!toPresenter.IsLocked)
                        {
                            SendSwapMessage(fromPoint, toPoint);
                            SaveService.Save(GameSaveType.Field);
                        }
                        else
                        {
                            SendResetPositionMessage(fromPoint);
                        }
                    }
                    else
                    {
                        SendSwapMessage(fromPoint, toPoint);
                        SaveService.Save(GameSaveType.Field);
                    }
                }
                else
                {
                    SendResetPositionMessage(fromPoint);
                }
            }
            else
            {
                SendResetPositionMessage(fromPoint);
            }
        }

        private IFieldElement TryMerge(GridPoint fromPoint, GridPoint toPoint)
        {
            IFieldElement firstElement = FieldLogicModel.FieldElements[fromPoint];
            IFieldElement secondElement = FieldLogicModel.FieldElements[toPoint];

            ElementConfig firstId = firstElement.ConfigParameters.ElementConfig;
            ElementConfig secondId = secondElement.ConfigParameters.ElementConfig;

            if (firstId == secondId)
            {
                ElementConfig nextInEvolution = firstElement.GetNextInEvolution();
                if (nextInEvolution)
                {
                    IFieldElement newElement = _fieldElementFactory.CreateFieldElement(nextInEvolution, toPoint);
                    _evolutionsService.UpdateProgress(newElement.ConfigParameters.ElementConfig);

                    if (firstElement.InfoParameters.Type == ElementType.Generator &&
                        newElement.InfoParameters.Type == ElementType.Generator)
                    {
                        SetupNewGenerator(firstElement, secondElement, newElement);
                    }

                    return newElement;
                }

                return null;
            }

            return null;
        }

        private void SendResetPositionMessage(GridPoint fromPoint)
        {
            var resetMessage = new ResetPositionMessage
            {
                From = fromPoint
            };

            MessageBus.Fire(resetMessage);
        }

        private void SendMergeMessage(GridPoint fromPoint, IFieldElement newElement)
        {
            var resetMessage = new MergeElementsMessage
            {
                From = fromPoint,
                NewElement = newElement
            };

            MessageBus.Fire(resetMessage);
        }

        private void SendSwapMessage(GridPoint fromPoint, GridPoint toPoint)
        {
            var resetMessage = new SwapElementsMessage
            {
                From = fromPoint,
                To = toPoint
            };

            MessageBus.Fire(resetMessage);
        }

        private void SendRemoveElement(GridPoint elementPosition)
        {
            var message = new RemoveElementMessage
            {
                RemoveAtPoint = elementPosition
            };

            MessageBus.Fire(message);
        }

        private void SetupNewGenerator(IFieldElement generator1, IFieldElement generator2, IFieldElement newGenerator)
        {
            if (!newGenerator.ConfigParameters.ElementConfig.GeneratorSettings.Charged)
            {
                var parameters1 = generator1.GeneratorParameters;
                var parameters2 = generator2.GeneratorParameters;

                int chargedCount = parameters1.ChargedCount + parameters2.ChargedCount;
                int availableCount = parameters1.AvailableToDrop + parameters2.AvailableToDrop;
                int allCount = chargedCount + availableCount;

                newGenerator.GeneratorParameters.ChargedCount = allCount;
                newGenerator.UpdateGenerator();
            }
        }
    }
}