// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;

namespace MergeIt.Game.UI.InventoryPanel
{
    public class InventoryPanelItemPair
    {
        public InventoryPanelItemPair(InventoryPanelItemComponent component, IFieldElement fieldElement)
        {
            Component = component;
            Element = fieldElement;
        }

        public InventoryPanelItemComponent Component { get; }

        public IFieldElement Element { get; }
    }
}