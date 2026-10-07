// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Field.Actions;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;

namespace MergeIt.Game.Services
{
    public class GameFieldActionsService : IGameFieldActionsService, IInitializable, IDisposable
    {
        private readonly List<IFieldActionProcessor> _fieldActionProcessors = new();

        [Introduce]
        private IMessageBus _messageBus;

        public void Dispose()
        {
            _messageBus.RemoveListener<ClickElementMessage>(OnClickElementMessageHandler);
            _messageBus.RemoveListener<EndDragElementMessage>(OnEndDragElementMessageHandler);
        }

        public void Initialize()
        {
            _messageBus.AddListener<ClickElementMessage>(OnClickElementMessageHandler);
            _messageBus.AddListener<EndDragElementMessage>(OnEndDragElementMessageHandler);

            _fieldActionProcessors.Add(new FieldMergeProcessor());
            _fieldActionProcessors.Add(new FieldGenerationProcessor());
            _fieldActionProcessors.Add(new FieldConsumableProcessor());
        }

        private void OnClickElementMessageHandler(ClickElementMessage message)
        {
            for (int i = 0; i < _fieldActionProcessors.Count; i++)
            {
                _fieldActionProcessors[i].ProcessClick(message.Cell);
            }
        }

        private void OnEndDragElementMessageHandler(EndDragElementMessage message)
        {
            for (int i = 0; i < _fieldActionProcessors.Count; i++)
            {
                _fieldActionProcessors[i].ProcessEndDrag(message.FromPoint, message.ToGameObject);
            }
        }
    }
}