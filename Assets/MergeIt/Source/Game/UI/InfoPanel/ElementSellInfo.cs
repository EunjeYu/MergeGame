// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InfoPanel
{
    public class ElementSellInfo : ElementInfo
    {
        [SerializeField]
        private TMP_Text _costLabel;

        [SerializeField]
        private Image _costTypeIcon;

        [SerializeField]
        private Button _sellButton;

        public override ElementActionType ActionType
        {
            get => ElementActionType.Sell;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _sellButton.onClick.AddListener(ActionButtonClick);
        }

        protected override bool OnTrySetup()
        {
            CurrencySettings sellCostSettings =
                SelectedElement.ConfigParameters.ElementConfig.CommonSettings.SellCostSettings;

            if (sellCostSettings.Amount > 0 && !SelectedElement.InfoParameters.IsBlocked)
            {
                if (SelectedElement.InfoParameters.Type == ElementType.Generator)
                {
                    if (SelectedElement.GeneratorParameters.AvailableToDrop == 0)
                    {
                        return false;
                    }
                }

                gameObject.SetActive(true);

                _costTypeIcon.sprite = ConfigsService.GetCurrencyIcon(sellCostSettings.Currency);
                _costLabel.text = sellCostSettings.Amount.ToString();

                return true;
            }

            return false;
        }

        protected override void Destroy()
        {
            base.Destroy();

            _sellButton.onClick.RemoveListener(ActionButtonClick);
        }
    }
}