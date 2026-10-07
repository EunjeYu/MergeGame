// Copyright (c) 2024, Awessets

using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Windows;

namespace MergeIt.Core.WindowSystem
{
    public interface IWindowSystem
    {
        void OpenWindow<TPresenter>(bool closePrevious = false, bool enableBlackout = false, IWindowArgs args = null)
            where TPresenter : class, IWindowPresenter, new();

        void CloseWindow(IWindowPresenter presenter);
    }
}