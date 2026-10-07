// Copyright (c) 2024, Awessets

using System;
using UnityEngine;

namespace MergeIt.Core.MVP
{
    public abstract class View : MonoBehaviour, IView
    {
        public event Action InitializeEvent;
        public event Action DestroyEvent;

        public GameObject GameObject
        {
            get => gameObject;
        }

        public virtual void Initialize()
        {
            InitializeEvent?.Invoke();
        }
        
        protected virtual void OnDestroy()
        {
            DestroyEvent?.Invoke();
        }
    }
}