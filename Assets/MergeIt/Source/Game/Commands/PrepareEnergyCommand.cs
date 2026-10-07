// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Commands;
using MergeIt.Core.Configs;
using MergeIt.Core.Services;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Commands
{
    public class PrepareEnergyCommand : Command
    {
        private readonly UserServiceModel _userServiceModel = DiContainer.Get<UserServiceModel>();
        private readonly IConfigsService _configsServiceModel = DiContainer.Get<IConfigsService>();
        
        public override void Execute()
        {
            CheckEnergy();
        }
        
        private void CheckEnergy()
        {
            GameConfig config = _configsServiceModel.GameConfig;
            int currentEnergy = _userServiceModel.Energy.Value;

            if (currentEnergy >= config.EnergyCap)
            {
                _userServiceModel.EnergyRestoringStartTime = -1;
            }
            else
            {
                long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                long elapsedTime = currentTime - _userServiceModel.EnergyRestoringStartTime;
                long restoredEnergy = elapsedTime / config.EnergyRestoreTime;

                currentEnergy += (int)restoredEnergy;
                int actualEnergy = Math.Min(currentEnergy, config.EnergyCap);
                _userServiceModel.Energy.Value = actualEnergy;

                if (actualEnergy < config.EnergyCap)
                {
                    _userServiceModel.EnergyRestoringStartTime += config.EnergyRestoreTime * restoredEnergy;
                }
                else
                {
                    _userServiceModel.EnergyRestoringStartTime = -1;
                }
            }
        }
    }
}