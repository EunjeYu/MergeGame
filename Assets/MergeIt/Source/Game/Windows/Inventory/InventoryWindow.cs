// Copyright (c) 2024, Awessets

using MergeIt.Core.WindowSystem.Windows;
using UnityEngine;

namespace MergeIt.Game.Windows.Inventory
{
    public class InventoryWindow : WindowBase
    {
        [SerializeField]
        private RectTransform _itemsGrid;

        public RectTransform ItemsGrid
        {
            get => _itemsGrid;
        }
    }
}