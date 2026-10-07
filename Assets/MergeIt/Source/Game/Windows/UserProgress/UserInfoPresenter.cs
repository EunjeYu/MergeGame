// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Core.WindowSystem.Windows;
using MergeIt.Game.Effects;
using MergeIt.Game.Effects.Controllers;
using MergeIt.Game.Effects.Parameters;
using MergeIt.Game.Enums;
using MergeIt.Game.Factories.ElementInfo;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.Game.Windows.ElementInfo;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Windows.UserProgress
{
    public class UserInfoPresenter : WindowPresenter<UserInfoWindow, UserInfoModel>
    {
        private readonly Dictionary<ElementInfoItemComponent, ElementConfig> _elementConfigs = new();
        private IEffectsFactory _effectsFactory;
        private IElementInfoFactory _elementInfoFactory;
        private IElementsStockService _elementsStockService;

        private IMessageBus _messageBus;
        private IUserProgressService _userProgressService;
        private UserServiceModel _userServiceModel;

        protected override void OnInitialize(UserInfoWindow view)
        {
            base.OnInitialize(view);

            _userServiceModel = DiContainer.Get<UserServiceModel>();
            _userProgressService = DiContainer.Get<IUserProgressService>();
            _elementsStockService = DiContainer.Get<IElementsStockService>();
            _elementInfoFactory = DiContainer.Get<IElementInfoFactory>();
            _effectsFactory = DiContainer.Get<IEffectsFactory>();

            _messageBus = DiContainer.Get<IMessageBus>();
            _messageBus.AddListener<LevelUpdatedMessage>(LevelUpdatedMessageHandler);

            View.LevelUpEvent += LevelUpEventHandler;

            CheckLevelUpButtonState();
            FillElements();
            UpdateProgress();
        }

        protected override void OnDestroyWindow()
        {
            base.OnDestroyWindow();

            _messageBus.RemoveListener<LevelUpdatedMessage>(LevelUpdatedMessageHandler);

            View.LevelUpEvent -= LevelUpEventHandler;

            base.OnDestroyWindow();

            foreach (var elementConfig in _elementConfigs)
            {
                if (elementConfig.Key)
                {
                    elementConfig.Key.ClickEvent -= OnClickElement;
                }
            }

            _elementConfigs.Clear();
        }

        private void LevelUpEventHandler()
        {
            _messageBus.Fire(new LevelUpMessage());
        }

        private void CheckLevelUpButtonState()
        {
            bool canLevelUp = _userProgressService.CanLevelUp();
            View.EnableLevelUpButton(canLevelUp);
        }

        private void FillElements()
        {
            var prizes = _userProgressService.GetLevelUpPrizes();

            if (prizes is {Length: > 0})
            {
                for (int i = 0; i < prizes.Length; i++)
                {
                    ElementConfig prize = prizes[i];
                    ElementInfoItemComponent elementInfo =
                        _elementInfoFactory.CreateElementWindowItem(prize, ElementInfoType.UserProgressWindow);

                    if (elementInfo != null)
                    {
                        _elementConfigs[elementInfo] = prize;
                        elementInfo.ClickEvent += OnClickElement;
                        elementInfo.transform.SetParent(View.PrizesContainer);
                        elementInfo.transform.localScale = Vector3.one;
                    }
                }
            }
        }

        private void UpdateProgress()
        {
            int currentMaxExp = _userProgressService.GetCurrentLevelMaxExp();
            int currentExp = _userServiceModel.Experience.Value;
            int level = _userServiceModel.Level.Value;

            View.UpdateProgress(level, currentExp, currentMaxExp);
        }

        private void OnClickElement(ElementInfoItemComponent component)
        {
            if (_elementConfigs.TryGetValue(component, out ElementConfig config))
            {
                var args = new ElementInfoArgs
                {
                    ElementConfig = config
                };

                WindowSystem.OpenWindow<ElementInfoPresenter>(true, true, args);
            }
        }

        private void LevelUpdatedMessageHandler(LevelUpdatedMessage _)
        {
            View.EnableLevelUpButton(false);
            int effectsCount = _elementConfigs.Count;
            foreach (var config in _elementConfigs)
            {
                _elementsStockService.Add(config.Value);

                RectTransform elementTransform = config.Key.RectTransform;

                var parameters = new MoveEffectParameters(elementTransform.position, default, HudPanelType.ElementStock, EffectName.MoveToStock);
                _effectsFactory.CreateEffect<MoveToUiEffect>(config.Key.RectTransform, parameters, () =>
                {
                    effectsCount--;
                    Object.Destroy(elementTransform.gameObject);

                    CheckEffects();
                });
            }

            void CheckEffects()
            {
                if (effectsCount == 0)
                {
                    _elementConfigs.Clear();
                    CheckLevelUpButtonState();
                    FillElements();
                    UpdateProgress();

                    _messageBus.Fire<UpdateStockMessage>();
                }
            }
        }
    }
}