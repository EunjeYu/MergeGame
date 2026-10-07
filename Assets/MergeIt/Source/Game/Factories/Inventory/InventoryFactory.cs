// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Game.UI.InventoryPanel;
using MergeIt.Game.Windows.Inventory;
using UnityEngine;

namespace MergeIt.Game.Factories.Inventory
{
    public class InventoryFactory : IInventoryFactory
    {
        private const string PanelItemPath = "Prefabs/InventoryPanel/InventoryPanelItem";
        private const string WindowItemPath = "Prefabs/Windows/Inventory/InventoryWindowItem";
        private const string WindowEmptyItemPath = "Prefabs/Windows/Inventory/InventoryWindowEmptyCell";
        private const string WindowPaidCellPath = "Prefabs/Windows/Inventory/InventoryWindowPaidCell";

        public InventoryPanelItemComponent CreateInventoryPanelItem(IFieldElement fieldElement)
        {
            GameObject itemPanelObject = Resources.Load<GameObject>(PanelItemPath);

            if (itemPanelObject)
            {
                GameObject panelItemObject = Object.Instantiate(itemPanelObject);
                FieldElementIconComponent iconPrototype = fieldElement.ConfigParameters.ElementConfig.GetIconComponent();
                
                var icon = Object.Instantiate(iconPrototype, panelItemObject.transform);
                var iconRectTransform = icon.GetComponent<RectTransform>();
                iconRectTransform.SetAsFirstSibling();
                
                var resultComponent = panelItemObject.GetComponent<InventoryPanelItemComponent>();
                return resultComponent;
            }

            return null;
        }
        
        public InventoryWindowItemComponent CreateInventoryWindowItem(IFieldElement fieldElement)
        {
            GameObject itemPanelObject = Resources.Load<GameObject>(WindowItemPath);

            if (itemPanelObject)
            {
                var panelItemObject = Object.Instantiate(itemPanelObject);
                if (panelItemObject.TryGetComponent(out InventoryWindowItemComponent resultComponent))
                {
                    FieldElementIconComponent iconPrototype = fieldElement.ConfigParameters.ElementConfig.GetIconComponent();
                
                    var icon = Object.Instantiate(iconPrototype, panelItemObject.transform);
                    var iconRectTransform = icon.GetComponent<RectTransform>();
                    
                    resultComponent.SetIcon(iconRectTransform);
                }
                
                return resultComponent;
            }

            return null;
        }
        
        public InventoryWindowPaidCellComponent CreateWindowPaidCell()
        {
            GameObject itemPanelObject = Resources.Load<GameObject>(WindowPaidCellPath);

            if (itemPanelObject)
            {
                var panelItemObject = Object.Instantiate(itemPanelObject);
                
                var resultComponent = panelItemObject.GetComponent<InventoryWindowPaidCellComponent>();
                return resultComponent;
            }

            return null;
        }
        
        public GameObject CreateWindowEmptyCell()
        {
            GameObject itemPanelObject = Resources.Load<GameObject>(WindowEmptyItemPath);

            if (itemPanelObject)
            {
                var panelItemObject = Object.Instantiate(itemPanelObject);
                return panelItemObject.gameObject;
            }

            return null;
        }
    }

}