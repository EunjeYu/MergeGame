// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs;
using MergeIt.Core.Messages;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;
using UnityEngine;

namespace MergeIt.Game.Services
{
    public class EnergyService : IEnergyService, IInitializable, IDisposable
    {
        private GameConfig _config;

        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IGameSaveService _gameSaveService;

        [Introduce]
        private IMessageBus _messageBus;

        private int _skipSeconds;

        [Introduce]
        private UserServiceModel _userServiceModel;

        public void Dispose()
        {
            _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.RemoveListener<EnergyRestoredMessage>(OnEnergyRestoredMessageHandler);
            _messageBus.RemoveListener<SkipTimeMessage>(OnSkipTimeMessageHandler);

            _userServiceModel.Energy.Unsubscribe(OnEnergyChanged);
        }

        public void Initialize()
        {
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.AddListener<EnergyRestoredMessage>(OnEnergyRestoredMessageHandler);
            _messageBus.AddListener<SkipTimeMessage>(OnSkipTimeMessageHandler);
        }

        private void OnLoadedGameMessageHandler(LoadedGameMessage message)
        {
            _config = _configsService.GameConfig;
            _userServiceModel.Energy.Subscribe(OnEnergyChanged, true);
        }

        private void OnEnergyRestoredMessageHandler(EnergyRestoredMessage restoredMessage)
        {
            int newEnergy = _userServiceModel.Energy.Value + 1;
            newEnergy = Mathf.Clamp(newEnergy, 0, _config.EnergyCap);

            _userServiceModel.EnergyRestoringStartTime = -1;
            _userServiceModel.Energy.Value = newEnergy;
        }

        private void OnSkipTimeMessageHandler(SkipTimeMessage message)
        {
            if (_userServiceModel.EnergyRestoringStartTime != -1)
            {
                _skipSeconds = message.Seconds;
                int energyRestoredPoints = _skipSeconds / _config.EnergyRestoreTime;
                _skipSeconds %= _config.EnergyRestoreTime;

                int energyRestored = energyRestoredPoints + _userServiceModel.Energy.Value;
                energyRestored = Mathf.Clamp(energyRestored, 0, _config.EnergyCap);
                _userServiceModel.Energy.Value = energyRestored;

                if (energyRestoredPoints == 0)
                {
                    OnEnergyChanged(energyRestored);
                }
            }
        }

        private void OnEnergyChanged(int value)
        {
            if (value < _config.EnergyCap)
            {
                if (_userServiceModel.EnergyRestoringStartTime == -1)
                {
                    _userServiceModel.EnergyRestoringStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }
                else
                {
                    _userServiceModel.EnergyRestoringStartTime -= _skipSeconds;
                }
            }
            else
            {
                _userServiceModel.EnergyRestoringStartTime = -1;
                _skipSeconds = 0;
            }

            CheckTime();

            _gameSaveService.Save(GameSaveType.User);
        }

        private void CheckTime()
        {
            if (_userServiceModel.EnergyRestoringStartTime > -1)
            {
                long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                long diffTime = currentTime - _userServiceModel.EnergyRestoringStartTime;
                long remainingTime = _config.EnergyRestoreTime - diffTime;

                SendStartTimer(remainingTime);
            }
            else
            {
                SendStartTimer(-1);
            }
        }

        private void SendStartTimer(long remainingTime)
        {
            var message = new StartChargingMessage
            {
                RemainingTime = remainingTime
            };

            _messageBus.Fire(message);
        }
    }
}