// Copyright (c) 2024, Awessets

using MergeIt.Core.Helpers;
using UnityEngine;

namespace MergeIt.SimpleDI
{
    public abstract class DiRoot : MonoBehaviour
    {
        protected abstract void OnInstall();
        
        protected virtual void Run()
        {
            
        }
        
        private void Awake()
        {
            _ = MainThreadDispatcher.Instance;
            Install();
            Run();
        }

        private void Update()
        {
            DiContainer.Update();
        }

        private void Install()
        {
            OnInstall();
            PostInstall();
        }

        private void PostInstall()
        {
            DiContainer.PostProcess();
        }
    }
}