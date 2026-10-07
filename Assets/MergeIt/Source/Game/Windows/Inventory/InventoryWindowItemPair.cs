// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;

namespace MergeIt.Game.Windows.Inventory
{
    public class InventoryWindowItemPair
    {

        public InventoryWindowItemPair(InventoryWindowItemComponent component, IFieldElement fieldElement)
        {
            Component = component;
            Element = fieldElement;
        }

        public InventoryWindowItemComponent Component { get; }

        public IFieldElement Element { get; }
    }
}