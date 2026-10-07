// Copyright (c) 2024, Awessets

using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Windows;
using UnityEngine;

namespace MergeIt.Core.WindowSystem.Factory
{
    public interface IWindowFactory
    {
        RectTransform GetRoot();
        BlackoutComponent GetBlackout(RectTransform parent);
        TPresenter CreateWindow<TPresenter>(WindowCreateInfo prefabPath, Transform parent, IWindowArgs windowArgs = null)
            where TPresenter : class, IWindowPresenter, new();
    }
}