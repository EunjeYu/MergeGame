// Copyright (c) 2024, Awessets

using MergeIt.Game.Enums;
using MergeIt.Game.Services;
using TMPro;
using UnityEngine;

namespace MergeIt.Game.HUD
{
    public class HardCurrencyComponent : HudPanelBase
    {
        [SerializeField]
        private TMP_Text _value;
        
        public override HudPanelType Type
        {
            get => HudPanelType.HardCurrency;
        }

        protected override void OnApplyModel(UserServiceModel userServiceModel)
        {
            UserServiceModel.HardCurrency.Subscribe(OnHardCurrencyChanged, true);
        }

        private void OnDestroy()
        {
            UserServiceModel.HardCurrency.Unsubscribe(OnHardCurrencyChanged);
        }
        
        private void OnHardCurrencyChanged(int current)
        {
            _value.text = $"{current}";
        }
    }
}