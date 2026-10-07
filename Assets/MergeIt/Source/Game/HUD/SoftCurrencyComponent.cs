// Copyright (c) 2024, Awessets

using MergeIt.Game.Enums;
using MergeIt.Game.Services;
using TMPro;
using UnityEngine;

namespace MergeIt.Game.HUD
{
    public class SoftCurrencyComponent : HudPanelBase
    {
        [SerializeField]
        private TMP_Text _value;
        
        public override HudPanelType Type
        {
            get => HudPanelType.SoftCurrency;
        }
        
        protected override void OnApplyModel(UserServiceModel userServiceModel)
        {
            UserServiceModel.SoftCurrency.Subscribe(OnSoftCurrencyChanged, true);
        }

        private void OnDestroy()
        {
            UserServiceModel.SoftCurrency.Unsubscribe(OnSoftCurrencyChanged);
        }

        private void OnSoftCurrencyChanged(int current)
        {
            _value.text = $"{current}";
        }
    }
}