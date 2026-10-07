// Copyright (c) 2024, Awessets

using MergeIt.Core.MVP;
using MergeIt.Core.WindowSystem.Data;

namespace MergeIt.Core.WindowSystem.Windows
{
    public interface IWindowPresenter : IPresenter
    {
        string Layer { get; }
        WindowState State { get; }
        
        void Initialize(IView view, string layer, IWindowArgs windowArgs = null);
        void Show();
        void Hide();
        void Close();
        void SetWindowActive(bool active);
        void SetWindowLayer();
    }
}