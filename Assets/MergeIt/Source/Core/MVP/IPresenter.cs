// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Core.MVP
{
    public interface IPresenter : IDisposable
    {
        void Initialize(IView view);
    }
}