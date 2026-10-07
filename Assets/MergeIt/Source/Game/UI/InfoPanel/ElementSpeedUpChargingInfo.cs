// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InfoPanel
{
    public class ElementSpeedUpChargingInfo : ElementInfo
    {
        [SerializeField]
        private TMP_Text _costLabel;

        [SerializeField]
        private Image _costTypeIcon;

        [SerializeField]
        private Button _skipChargeButton;

        public override ElementActionType ActionType
        {
            get => ElementActionType.SkipCharging;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _skipChargeButton.onClick.AddListener(OnActionButtonClick);
        }

        protected override bool OnTrySetup()
        {
            if (SelectedElement.InfoParameters.Type == ElementType.Generator &&
                !SelectedElement.InfoParameters.IsBlocked)
            {
                CurrencySettings skipChargeCostSettings =
                    SelectedElement.ConfigParameters.ElementConfig.GeneratorSettings.SkipChargeCostSettings;

                bool generatorCharging = SelectedElement.GeneratorParameters.AvailableToDrop == 0;

                if (generatorCharging && skipChargeCostSettings.Amount > 0)
                {
                    gameObject.SetActive(true);

                    _costTypeIcon.sprite = ConfigsService.GetCurrencyIcon(skipChargeCostSettings.Currency);
                    _costLabel.text = skipChargeCostSettings.Amount.ToString();

                    return true;
                }
            }

            return false;
        }

        protected override void Destroy()
        {
            base.Destroy();

            _skipChargeButton.onClick.RemoveListener(OnActionButtonClick);
        }
    }
}