// Copyright (c) 2024, Awessets

using System;
using UnityEngine;

namespace MergeIt.Core.Animations
{
    public abstract class AnimationControllerBase : MonoBehaviour, IAnimationController
    {
        public abstract void Initialize(IAnimationListener listener);
        
        public virtual void SetState(string state)
        {
            throw new NotImplementedException();
        }
        
        public virtual void SetState(int state)
        {
            throw new NotImplementedException();
        }
        
        public virtual void SetState<T>(T state) where T : Enum
        {
            throw new NotImplementedException();
        }
    }
}