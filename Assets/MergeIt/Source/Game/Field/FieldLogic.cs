// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;

namespace MergeIt.Game.Field
{
    public class FieldLogic : IFieldLogic, IInitializable, IDisposable
    {
        [Introduce]
        private IMessageBus _messageBus;

        [Introduce]
        private FieldLogicModel _fieldLogicModel;

        public void Initialize()
        {
            _messageBus.AddListener<CreateElementMessage>(OnCreateElementMessageHandler);
            _messageBus.AddListener<RemoveElementMessage>(OnRemoveElementMessageHandler);
            _messageBus.AddListener<SwapElementsMessage>(OnSwapElementsMessageHandler);
            _messageBus.AddListener<SplitElementMessage>(OnSplitElementMessageHandler);
            _messageBus.AddListener<MergeElementsMessage>(OnMergeElementMessageHandler);
        }

        public void Dispose()
        {
            _messageBus.RemoveListener<CreateElementMessage>(OnCreateElementMessageHandler);
            _messageBus.RemoveListener<RemoveElementMessage>(OnRemoveElementMessageHandler);
            _messageBus.RemoveListener<SwapElementsMessage>(OnSwapElementsMessageHandler);
            _messageBus.RemoveListener<SplitElementMessage>(OnSplitElementMessageHandler);
            _messageBus.RemoveListener<MergeElementsMessage>(OnMergeElementMessageHandler);
        }

        private void OnCreateElementMessageHandler(CreateElementMessage message)
        {
            IFieldElement fieldElement = message.NewElement;
            fieldElement.InfoParameters.LogicPosition = message.ToPoint;
            
            _fieldLogicModel.FieldElements[message.ToPoint] = fieldElement;
        }

        private void OnRemoveElementMessageHandler(RemoveElementMessage message)
        {
            _fieldLogicModel.FieldElements.Remove(message.RemoveAtPoint);
        }

        private void OnSwapElementsMessageHandler(SwapElementsMessage message)
        {
            var fromPoint = message.From;
            var toPoint = message.To;

            IFieldElement firstElement = _fieldLogicModel.FieldElements[fromPoint];

            if (_fieldLogicModel.FieldElements.TryGetValue(toPoint, out IFieldElement secondElement))
            {
                secondElement.InfoParameters.LogicPosition = fromPoint;
                firstElement.InfoParameters.LogicPosition = toPoint;

                _fieldLogicModel.FieldElements[fromPoint] = secondElement;
                _fieldLogicModel.FieldElements[toPoint] = firstElement;
            }
            else
            {
                firstElement.InfoParameters.LogicPosition = toPoint;

                _fieldLogicModel.FieldElements[toPoint] = firstElement;
                _fieldLogicModel.FieldElements.Remove(fromPoint);
            }
        }

        private void OnSplitElementMessageHandler(SplitElementMessage message)
        {
            IFieldElement fieldElement1 = message.SplitElement1;
            IFieldElement fieldElement2 = message.SplitElement2;

            GridPoint initPoint = message.SplitElement1.InfoParameters.LogicPosition;
            GridPoint secondPoint = message.SplitElement2.InfoParameters.LogicPosition;

            _fieldLogicModel.FieldElements[initPoint] = fieldElement1;
            _fieldLogicModel.FieldElements[secondPoint] = fieldElement2;
        }

        private void OnMergeElementMessageHandler(MergeElementsMessage message)
        {
            _fieldLogicModel.FieldElements[message.NewElement.InfoParameters.LogicPosition] = message.NewElement;
        }
    }
}