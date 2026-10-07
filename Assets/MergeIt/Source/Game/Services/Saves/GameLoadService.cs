// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Services.Saves.Strategies;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Services.Saves
{
    public class GameLoadService : IGameLoadService
    {
        [Introduce]
        private IMessageBus _messageBus;
        
        [Introduce]
        private ISerializeStrategy _serializeStrategy;

        public T Load<T>() where T : class, ISavable
        {
            var data = _serializeStrategy.Load<T>();
            data = null;
            return data;
        }
    }
}