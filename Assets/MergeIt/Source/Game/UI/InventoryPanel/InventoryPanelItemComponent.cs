// Copyright (c) 2024, Awessets

using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InventoryPanel
{
    public class InventoryPanelItemComponent : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _maxLevelIcon;

        private RectTransform _rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
        public event Action<InventoryPanelItemComponent> ClickEvent;

        private void OnClick()
        {
            ClickEvent?.Invoke(this);
        }
    }
}