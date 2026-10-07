// Copyright (c) 2024, Awessets

using MergeIt.Core.WindowSystem.Windows;

namespace MergeIt.Core.WindowSystem.Data
{
    public interface IWindowOpenParameters
    {
        bool ClosePrevious { get; set; }
        bool NeedBlackout { get; set; }
        IWindowPresenter Presenter { get; set; }
    }
}