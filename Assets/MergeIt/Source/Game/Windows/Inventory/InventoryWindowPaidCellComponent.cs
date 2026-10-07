// Copyright (c) 2024, Awessets

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.Windows.Inventory
{
    public class InventoryWindowPaidCellComponent : MonoBehaviour
    {

        [SerializeField]
        private Button _buyButton;

        [SerializeField]
        private TMP_Text _priceLabel;

        [SerializeField]
        private Image _priceIcon;

        public void Awake()
        {
            _buyButton.onClick.AddListener(OnBuyButtonClick);
        }

        public void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClick);
        }
        public event Action BuyCellEvent;

        public void Setup(string priceText, Sprite priceImage)
        {
            _priceLabel.text = priceText;
            _priceIcon.sprite = priceImage;
        }

        private void OnBuyButtonClick()
        {
            BuyCellEvent?.Invoke();
        }
    }
}