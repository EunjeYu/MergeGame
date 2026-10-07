// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs;
using MergeIt.Core.Messages;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Core.User;
using MergeIt.Game.User;
using MergeIt.SimpleDI;
using Random = System.Random;

namespace MergeIt.Game.Services
{
    public class UserService : IUserService
    {
        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IMessageBus _messageBus;

        [Introduce]
        private IGameSaveService _saveService;

        [Introduce]
        private UserServiceModel _userServiceModel;

        public void CreateUser()
        {
            GameConfig config = _configsService.GameConfig;
            var userData = new UserData
            {
                Name = $"User{new Random().Next(ushort.MinValue, ushort.MaxValue)}",
                Energy = config.EnergyCap,
                Experience = 0,
                SoftCurrency = config.InitialSoftCurrency,
                HardCurrency = config.InitialHardCurrency,
                Splitters = config.InitialSplittersCount,
                Level = 1
            };

            SetupUser(userData);

            _saveService.Save(GameSaveType.User);
        }

        public void SetupUser(IUserData userData)
        {
            _userServiceModel.Set(userData);
        }
    }

}