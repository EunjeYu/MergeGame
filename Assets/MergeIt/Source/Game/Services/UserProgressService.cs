// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.LevelUp;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;

namespace MergeIt.Game.Services
{
    public class UserProgressService : IUserProgressService, IInitializable, IDisposable
    {
        [Introduce]
        private IConfigsService _configService;

        [Introduce]
        private IMessageBus _messageBus;

        [Introduce]
        private UserServiceModel _userServiceModel;

        public void Dispose()
        {
            _messageBus.RemoveListener<LevelUpMessage>(OnLevelUpMessageHandler);
        }

        public void Initialize()
        {
            _messageBus.AddListener<LevelUpMessage>(OnLevelUpMessageHandler);
        }

        public int GetCurrentLevelMaxExp()
        {
            LevelUpParameters levelUpParameters = _configService.GetLevelUpData(_userServiceModel.Level.Value);

            return levelUpParameters.Experience;
        }

        public bool CanLevelUp()
        {
            return _userServiceModel.Experience.Value >= GetCurrentLevelMaxExp();
        }

        public ElementConfig[] GetLevelUpPrizes()
        {
            LevelUpParameters levelUpParameters = _configService.GetLevelUpData(_userServiceModel.Level.Value);

            if (levelUpParameters != null)
            {
                return levelUpParameters.Bonuses;
            }

            return null;
        }

        private void OnLevelUpMessageHandler(LevelUpMessage message)
        {
            if (CanLevelUp())
            {
                int userLevel = _userServiceModel.Level.Value;
                LevelUpParameters currentLevelParameters = _configService.GetLevelUpData(userLevel);

                int experienceDiff = _userServiceModel.Experience.Value - currentLevelParameters.Experience;
                if (experienceDiff >= 0)
                {
                    userLevel++;

                    LevelUpParameters nextLevelParameters = _configService.GetLevelUpData(userLevel);

                    if (nextLevelParameters != null)
                    {
                        _userServiceModel.Level.Value = userLevel;
                        _userServiceModel.Experience.SetValueSilently(experienceDiff);

                        SendLevelUpdated(nextLevelParameters.Experience);
                    }
                }
            }
        }

        private void SendLevelUpdated(int maxExp)
        {
            var message = new LevelUpdatedMessage {NextLevelExp = maxExp};

            _messageBus.Fire(message);
        }
    }
}