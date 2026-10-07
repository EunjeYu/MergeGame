// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Helpers;
using MergeIt.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InfoPanel
{
    public class ElementSpeedUpOpeningInfo : ElementInfo
    {
        [SerializeField]
        private TMP_Text _costLabel;

        [SerializeField]
        private TMP_Text _openingTimerLabel;

        [SerializeField]
        private Image _costTypeIcon;

        [SerializeField]
        private Button _skipOpeningButton;

        private Bindable<float> _remainingTime;

        public override ElementActionType ActionType
        {
            get => ElementActionType.SkipOpening;
        }

        protected override void OnStart()
        {
            base.OnStart();
            _skipOpeningButton.onClick.AddListener(OnActionButtonClick);
        }

        protected override bool OnTrySetup()
        {
            if (SelectedElement.InfoParameters.Type == ElementType.Generator &&
                !SelectedElement.InfoParameters.IsBlocked)
            {
                CurrencySettings skipOpenCostSettings =
                    SelectedElement.ConfigParameters.ElementConfig.GeneratorSettings.SkipOpenCostSettings;

                IGeneratorOpenParameters openParameters = SelectedElement.GeneratorOpenParameters;

                if (openParameters is { IsOpening: true } && 
                    openParameters.RemainingTime.Value > 0f)
                {
                    gameObject.SetActive(true);

                    _remainingTime?.Unsubscribe(OnRemainingTimeChanged);
                    _remainingTime = openParameters.RemainingTime;
                    _remainingTime.Subscribe(OnRemainingTimeChanged);
                    _openingTimerLabel.text = _remainingTime.Value.FormatTime();
                    _costTypeIcon.sprite = ConfigsService.GetCurrencyIcon(skipOpenCostSettings.Currency);

                    if (skipOpenCostSettings.Amount == 0)
                    {
                        _skipOpeningButton.gameObject.SetActive(false);
                    }
                    else
                    {
                        _costLabel.text = skipOpenCostSettings.Amount.ToString();
                    }

                    return true;
                }
            }

            return false;
        }

        protected override void OnClear()
        {
            base.OnClear();

            _remainingTime?.Unsubscribe(OnRemainingTimeChanged);
            _remainingTime = null;
        }

        private void OnRemainingTimeChanged(float newValue)
        {
            _openingTimerLabel.text = newValue.FormatTime();
        }
    }
}