// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Types;
using MergeIt.Core.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InfoPanel
{
    public class ElementOpenInfo : ElementInfo
    {
        [SerializeField]
        private TMP_Text _openTime;

        [SerializeField]
        private Button _openButton;

        public override ElementActionType ActionType => ElementActionType.Open;

        protected override void OnStart()
        {
            base.OnStart();

            _openButton.onClick.AddListener(ActionButtonClick);
        }

        protected override bool OnTrySetup()
        {
            if (SelectedElement.InfoParameters.Type == ElementType.Generator &&
                !SelectedElement.InfoParameters.IsBlocked)
            {
                if (SelectedElement.GeneratorOpenParameters is {IsOpening: false})
                {
                    int openTime =
                        SelectedElement.ConfigParameters.ElementConfig.GeneratorSettings.OpenTime;

                    gameObject.SetActive(true);

                    _openTime.text = openTime.FormatTime();

                    return true;
                }
            }

            return false;
        }

        protected override void Destroy()
        {
            base.Destroy();

            _openButton.onClick.RemoveListener(ActionButtonClick);
        }
    }
}