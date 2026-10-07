// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Helpers;
using MergeIt.Game.Helpers;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InfoPanel
{
    public class ElementSplitInfo : ElementInfo
    {
        [SerializeField]
        private TMP_Text _costLabel;

        [SerializeField]
        private TMP_Text _splittersAmountLabel;

        [SerializeField]
        private Button _splitButton;

        private UserServiceModel _userServiceModel;

        public override ElementActionType ActionType
        {
            get => ElementActionType.Split;
        }

        protected override void OnStart()
        {
            base.OnStart();

            TrySetupUser();

            _splitButton.onClick.AddListener(ActionButtonClick);
        }

        protected override bool OnTrySetup()
        {
            TrySetupUser();

            ElementCommonSettings commonSettings = SelectedElement.ConfigParameters.ElementConfig.CommonSettings;

            if (!SelectedElement.InfoParameters.IsBlocked &&
                SelectedElement.GetPreviousInEvolution() &&
                commonSettings.SplitCostSettings.Amount > 0)
            {
                gameObject.SetActive(true);

                _costLabel.text = commonSettings.SplitCostSettings.Amount.ToString();

                return true;
            }

            return false;
        }

        protected override void Destroy()
        {
            base.Destroy();

            _splitButton.onClick.RemoveListener(ActionButtonClick);
            _userServiceModel?.Splitters.Unsubscribe(OnSplittersValueChanged);
        }

        private void TrySetupUser()
        {
            if (_userServiceModel == null)
            {
                _userServiceModel = DiContainer.Get<UserServiceModel>();
                _userServiceModel.Splitters.Subscribe(OnSplittersValueChanged, true);
            }
        }

        private void OnSplittersValueChanged(int newValue)
        {
            MainThreadDispatcher.Instance.RunOnMainThread(() => { _splittersAmountLabel.text = _userServiceModel.Splitters.Value.ToString(); });
        }
    }
}