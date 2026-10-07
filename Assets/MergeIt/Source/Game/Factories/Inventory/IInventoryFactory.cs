// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Game.UI.InventoryPanel;
using MergeIt.Game.Windows.Inventory;
using UnityEngine;

namespace MergeIt.Game.Factories.Inventory
{
    public interface IInventoryFactory
    {
        InventoryPanelItemComponent CreateInventoryPanelItem(IFieldElement fieldElement);
        InventoryWindowItemComponent CreateInventoryWindowItem(IFieldElement fieldElement);
        InventoryWindowPaidCellComponent CreateWindowPaidCell();
        GameObject CreateWindowEmptyCell();
    }
}