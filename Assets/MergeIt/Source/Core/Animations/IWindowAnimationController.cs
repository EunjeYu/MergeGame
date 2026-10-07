// Copyright (c) 2024, Awessets

namespace MergeIt.Core.Animations
{
    public interface IWindowAnimationController
    {
        void Initialize(IWindowAnimationListener listener);

        void OpenWindow();
        void CloseWindow();
        void OnOpenEnd();
        void OnCloseEnd();
    }
}