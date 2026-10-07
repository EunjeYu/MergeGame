// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Core.WindowSystem;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.HUD
{
    public class HudComponent : MonoBehaviour
    {
        [SerializeField]
        private ProgressComponent _progressComponent;

        [SerializeField]
        private SoftCurrencyComponent _softCurrencyComponent;

        [SerializeField]
        private HardCurrencyComponent _hardCurrencyComponent;

        [SerializeField]
        private EnergyComponent _energyComponent;

        private UserServiceModel _userServiceModel;
        private IUserProgressService _userProgressService;
        private IMessageBus _messageBus;

        private void Start()
        {
            _messageBus = DiContainer.Get<IMessageBus>();
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.AddListener<LevelUpdatedMessage>(OnLevelUpdatedMessageHandler);
        }

        private void OnDestroy()
        {
            _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.RemoveListener<LevelUpdatedMessage>(OnLevelUpdatedMessageHandler);
        }

        private void OnLoadedGameMessageHandler(LoadedGameMessage message)
        {
            _userServiceModel = DiContainer.Get<UserServiceModel>();
            _userProgressService = DiContainer.Get<IUserProgressService>();
            var windowSystem = DiContainer.Get<IWindowSystem>();

            int maxExp = _userProgressService.GetCurrentLevelMaxExp();
            _progressComponent.Initialize(windowSystem);   
            _progressComponent.SetMaxProgress(maxExp);
            _progressComponent.ApplyModel(_userServiceModel);
            _progressComponent.UpdateProgress();

            _energyComponent.ApplyModel(_userServiceModel);
            _softCurrencyComponent.ApplyModel(_userServiceModel);
            _hardCurrencyComponent.ApplyModel(_userServiceModel);
        }

        private void OnLevelUpdatedMessageHandler(LevelUpdatedMessage message)
        {
            _progressComponent.SetMaxProgress(message.NextLevelExp);
            _progressComponent.UpdateProgress();
        }
    }
}