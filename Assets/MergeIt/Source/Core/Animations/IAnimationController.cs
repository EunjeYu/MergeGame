// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Core.Animations
{
    public interface IAnimationController
    {
        void Initialize(IAnimationListener listener);

        void SetState(string state);
        void SetState(int state);
        void SetState<T>(T state) where T : Enum;
    }
}