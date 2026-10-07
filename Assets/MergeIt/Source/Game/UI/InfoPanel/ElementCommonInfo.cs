// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using TMPro;
using UnityEngine;

namespace MergeIt.Game.UI.InfoPanel
{
    public class ElementCommonInfo : ElementInfo
    {
        [SerializeField]
        private TMP_Text _nameLabel;

        [SerializeField]
        private TMP_Text _descriptionLabel;

        public override ElementActionType ActionType
        {
            get => ElementActionType.Common;
        }

        protected override bool OnTrySetup()
        {
            gameObject.SetActive(true);

            IInfoParameters infoParameters = SelectedElement.InfoParameters;

            _nameLabel.text = infoParameters.Name;
            _descriptionLabel.text = infoParameters.Description;

            return true;
        }
    }
}