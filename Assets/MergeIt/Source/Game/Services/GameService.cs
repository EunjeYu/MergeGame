// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Commands;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Commands;
using MergeIt.Game.Converters;
using MergeIt.Game.Factories.Field;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;

namespace MergeIt.Game.Services
{
    public class GameService : IGameService, IInitializable, IDisposable
    {
        [Introduce]
        private IConfigProcessor _configProcessor;

        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IFieldFactory _fieldFactory;

        [Introduce]
        private IGameLoadService _gameLoadService;

        [Introduce]
        private GameServiceModel _gameServiceModel;

        [Introduce]
        public IMessageBus _messageBus;

        [Introduce]
        private IGameSaveService _saveService;

        public void Dispose()
        {
            _messageBus.RemoveListener<StartGameMessage>(StartGameMessageHandler);
        }

        public void Initialize()
        {
            _messageBus.AddListener<StartGameMessage>(StartGameMessageHandler);
        }

        private async void StartGameMessageHandler(StartGameMessage message)
        {
            var manager = new CommandManager();

            manager.Add(new LoadConfigsCommand());
            manager.Add(new PrepareUserCommand());
            manager.Add(new PrepareEnergyCommand());
            manager.Add(new PrepareStockCommand());
            manager.Add(new PrepareInventoryCommand());
            manager.Add(new PrepareFieldCommand());
            manager.Add(new CheckEvolutionsProgressCommand());

            await manager.RunAsync();

            _messageBus.Fire<LoadedGameMessage>();
        }
    }
}