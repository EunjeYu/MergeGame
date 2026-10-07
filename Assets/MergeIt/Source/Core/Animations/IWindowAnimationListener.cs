// Copyright (c) 2024, Awessets

namespace MergeIt.Core.Animations
{
    public interface IWindowAnimationListener
    {
        void OnOpenStarted();
        void OnOpenFinished();
        void OnCloseStarted();
        void OnCloseFinished();
    }
}
