// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Elements;
using UnityEngine;
using UnityEngine.Pool;

namespace MergeIt.Game.Factories.Icons
{
    public class IconFactory : IIconFactory
    {
        private readonly Dictionary<ElementConfig, IObjectPool<FieldElementIconComponent>> _icons = new();
        
        public FieldElementIconComponent CreateIcon(ElementConfig config, Transform parent = null)
        {
            FieldElementIconComponent iconComponent;
            if (!_icons.TryGetValue(config, out IObjectPool<FieldElementIconComponent> iconsPool))
            {
                iconsPool = new ObjectPool<FieldElementIconComponent>(
                    () =>
                    {
                        FieldElementIconComponent iconPrototype = config.GetIconComponent();
                        GameObject iconContainer = Object.Instantiate(iconPrototype.gameObject);
                        iconContainer.TryGetComponent(out FieldElementIconComponent icon);

                        return icon;
                    },
                    OnGetIcon,
                    OnReleaseIcon,
                    OnDestroyIcon,
                    defaultCapacity: 2,
                    maxSize: 20);

                _icons[config] = iconsPool;
            }
            
            iconComponent = GetIconComponent(iconsPool, parent);

            return iconComponent;
        }

        private FieldElementIconComponent GetIconComponent(IObjectPool<FieldElementIconComponent> iconsPool, Transform parent)
        {
            var iconComponent = iconsPool.Get();
                 
            iconComponent.transform.SetParent(parent);
            iconComponent.transform.localScale = Vector3.one;
            iconComponent.RectTransform.SetAsFirstSibling();

            return iconComponent;
        }
        
        private void OnGetIcon(FieldElementIconComponent obj)
        {
            obj.gameObject.SetActive(true);
        }
        
        private void OnReleaseIcon(FieldElementIconComponent obj)
        {
            obj.gameObject.SetActive(false);
        }
        
        private void OnDestroyIcon(FieldElementIconComponent obj)
        {
            Object.Destroy(obj.gameObject);
        }
    }

}