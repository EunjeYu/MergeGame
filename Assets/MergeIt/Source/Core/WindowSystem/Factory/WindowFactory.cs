// Copyright (c) 2024, Awessets

using MergeIt.Core.Helpers;
using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Windows;
using UnityEngine;

namespace MergeIt.Core.WindowSystem.Factory
{
    public class WindowFactory : IWindowFactory
    {
        private const string RootPath = "Prefabs/Windows/WindowsRoot";
        private const string Blackout = "Prefabs/Windows/Blackout";

        public RectTransform GetRoot()
        {
            var root = Resources.Load<Canvas>(RootPath);

            if (root)
            {
                root.worldCamera = Camera.main;
                GameObject gameObject = Object.Instantiate(root.gameObject);

                return gameObject.GetComponent<RectTransform>();
            }

            return null;
        }

        public BlackoutComponent GetBlackout(RectTransform parent)
        {
            var blackoutObject = Resources.Load<GameObject>(Blackout);

            if (blackoutObject)
            {
                GameObject gameObject = Object.Instantiate(blackoutObject, parent);

                if (gameObject)
                {
                    if (gameObject.TryGetComponent(out RectTransform rectTransform))
                    {
                        rectTransform.Stretch();
                        rectTransform.SetAsFirstSibling();
                    }
                }

                gameObject.TryGetComponent(out BlackoutComponent blackoutComponent);

                return blackoutComponent;
            }

            return null;
        }

        public TPresenter CreateWindow<TPresenter>(WindowCreateInfo createInfo, Transform parent, IWindowArgs windowArgs)
            where TPresenter : class, IWindowPresenter, new()
        {
            if (!string.IsNullOrEmpty(createInfo.PrefabPath))
            {
                var presenter = new TPresenter();
                GameObject windowPrototype = Resources.Load<GameObject>(createInfo.PrefabPath);
                GameObject windowObject = Object.Instantiate(windowPrototype, parent);

                if ((windowObject ? windowObject.GetComponent(createInfo.Type) : null) is IWindow window)
                {
                    window.Initialize();

                    presenter.Initialize(window, createInfo.LayerName, windowArgs);
                    presenter.SetWindowLayer();
                    presenter.SetWindowActive(false);
                }
                
                return presenter;
            }

            return null;
        } 
    }
}