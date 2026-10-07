// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.MVP;

namespace MergeIt.Core.WindowSystem.Windows
{
    public interface IWindow : IView
    {
        event Action InitiateCloseEvent;

        event Action ShowStartEvent;
        event Action ShowEndEvent;
        event Action CloseStartEvent;
        event Action CloseEndEvent;

        void Show();
        void Close();
        void SetLayer(string layer);
    }
}