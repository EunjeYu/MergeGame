// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InfoPanel
{
    public class ElementUnlockInfo : ElementInfo
    {
        [SerializeField]
        private TMP_Text _costLabel;

        [SerializeField]
        private Image _costTypeIcon;

        [SerializeField]
        private Button _unlockButton;

        public override ElementActionType ActionType
        {
            get => ElementActionType.Unlock;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _unlockButton.onClick.AddListener(ActionButtonClick);
        }

        protected override bool OnTrySetup()
        {
            CurrencySettings unlockCostSettings =
                SelectedElement.ConfigParameters.ElementConfig.CommonSettings.UnlockCostSettings;

            if (unlockCostSettings.Amount > 0 && SelectedElement.InfoParameters.IsBlocked)
            {
                gameObject.SetActive(true);

                _costTypeIcon.sprite = ConfigsService.GetCurrencyIcon(unlockCostSettings.Currency);
                _costLabel.text = unlockCostSettings.Amount.ToString();

                return true;
            }

            return false;
        }

        protected override void Destroy()
        {
            base.Destroy();

            _unlockButton.onClick.RemoveListener(ActionButtonClick);
        }
    }
}