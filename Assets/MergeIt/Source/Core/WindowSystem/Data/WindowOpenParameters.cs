// Copyright (c) 2024, Awessets

using MergeIt.Core.WindowSystem.Windows;

namespace MergeIt.Core.WindowSystem.Data
{
    public class WindowOpenParameters : IWindowOpenParameters
    {
        public bool ClosePrevious { get; set; }
        public bool NeedBlackout { get; set; }
        public IWindowPresenter Presenter { get; set; }
    }
}