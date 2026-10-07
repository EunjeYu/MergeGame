// Copyright (c) 2024, Awessets

using System;
using UnityEngine;

namespace MergeIt.Core.MVP
{
    public interface IView
    {
        event Action InitializeEvent;
        event Action DestroyEvent;
        
        GameObject GameObject { get; }
        
        void Initialize();
    }
}