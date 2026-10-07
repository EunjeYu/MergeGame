// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.IO;
using MergeIt.Core.WindowSystem.Windows;

namespace MergeIt.Core.WindowSystem
{
    public static class WindowsStorage
    {
        private const string WindowsBasePath = "Prefabs/Windows";
        
        private static readonly Dictionary<Type, WindowCreateInfo> ControllersLayers = new();

        public static void Register<TPresenter, TWindow>(string prefabName, string layerName)
            where TPresenter : IWindowPresenter
            where TWindow : WindowBase
        {
            ControllersLayers[typeof(TPresenter)] = new WindowCreateInfo
            {
                Type = typeof(TWindow),
                LayerName = layerName,
                PrefabPath = Path.Combine(WindowsBasePath, prefabName)
            };
        }

        public static WindowCreateInfo GetWindowInfo<TPresenter>()
            where TPresenter : IWindowPresenter
        {
            Type type = typeof(TPresenter);

            return GetWindowInfo(type);
        }

        public static WindowCreateInfo GetWindowInfo(Type type)
        {
            ControllersLayers.TryGetValue(type, out WindowCreateInfo windowInfo);

            return windowInfo;
        }
    }

}