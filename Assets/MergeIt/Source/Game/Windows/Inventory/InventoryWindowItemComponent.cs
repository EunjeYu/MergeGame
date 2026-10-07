// Copyright (c) 2024, Awessets

using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.Windows.Inventory
{
    public class InventoryWindowItemComponent : MonoBehaviour
    {

        [SerializeField]
        private Button _itemButton;

        [SerializeField]
        private Button _infoButton;

        [SerializeField]
        private RectTransform _iconContainer;

        public void Awake()
        {
            _itemButton.onClick.AddListener(OnItemClick);
            _infoButton.onClick.AddListener(OnInfoClick);
        }

        public void OnDestroy()
        {
            _itemButton.onClick.RemoveListener(OnItemClick);
            _infoButton.onClick.RemoveListener(OnInfoClick);
        }
        public event Action<InventoryWindowItemComponent> ItemClickEvent;
        public event Action<InventoryWindowItemComponent> InfoClickEvent;

        public void SetIcon(RectTransform rectTransform)
        {
            rectTransform.SetParent(_iconContainer);
            rectTransform.localScale = Vector3.one;
            ;
        }

        private void OnItemClick()
        {
            ItemClickEvent?.Invoke(this);
        }

        private void OnInfoClick()
        {
            InfoClickEvent?.Invoke(this);
        }
    }
}