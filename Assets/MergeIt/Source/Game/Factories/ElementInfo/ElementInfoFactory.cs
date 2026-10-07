// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Elements;
using MergeIt.Game.Windows.ElementInfo;
using UnityEngine;

namespace MergeIt.Game.Factories.ElementInfo
{
    public class ElementInfoFactory : IElementInfoFactory
    {
        private static Dictionary<ElementInfoType, string> PrefabsLocations = new()
        {
            {ElementInfoType.InfoWindow, "Prefabs/Windows/ElementInfo/ElementInfoWindowItem"},
            {ElementInfoType.UserProgressWindow, "Prefabs/Windows/UserProgress/UserProgressWindowItem"}
        };
        
        public ElementInfoItemComponent CreateElementWindowItem(ElementConfig elementConfig,
            ElementInfoType infoType = ElementInfoType.InfoWindow, bool isLocked = false)
        {
            if (PrefabsLocations.TryGetValue(infoType, out string itemInfoPath))
            {
                GameObject itemPanelObject = Resources.Load<GameObject>(itemInfoPath);

                if (itemPanelObject)
                {
                    var panelItemObject = Object.Instantiate(itemPanelObject);
                    if (panelItemObject.TryGetComponent(out ElementInfoItemComponent resultComponent))
                    {
                        FieldElementIconComponent iconPrototype = elementConfig.GetIconComponent();

                        var icon = Object.Instantiate(iconPrototype, panelItemObject.transform);
                        var iconRectTransform = icon.GetComponent<RectTransform>();

                        iconPrototype.SetBlocked(isLocked);

                        resultComponent.SetIcon(iconRectTransform, isLocked);
                    }

                    return resultComponent;
                }
            }

            return null;
        }
        
        public ElementInfoItemComponent CreateUnknownElementWindowItem(ElementInfoType infoType = ElementInfoType.InfoWindow)
        {
            if (PrefabsLocations.TryGetValue(infoType, out string itemInfoPath))
            {
                GameObject itemPanelObject = Resources.Load<GameObject>(itemInfoPath);

                if (itemPanelObject)
                {
                    var panelItemObject = Object.Instantiate(itemPanelObject);
                    if (panelItemObject.TryGetComponent(out ElementInfoItemComponent resultComponent))
                    {
                        resultComponent.SetUnknown();
                    }

                    return resultComponent;
                }
            }

            return null;
        }
    }
}