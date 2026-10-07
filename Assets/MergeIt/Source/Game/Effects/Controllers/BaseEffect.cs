// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Services;
using MergeIt.Game.Effects.Parameters;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Effects.Controllers
{
    public abstract class BaseEffect : IEffect
    {
        protected Transform Target;
        protected Animator Animator;
        protected Action FinishedCallback;

        protected readonly IConfigsService ConfigsService = DiContainer.Get<IConfigsService>();
        
        public bool Started { get; set; }

        public virtual void Setup(Transform target, IEffectParameters effectParameters = null, Action finishedCallback = null)
        {
            Target = target;
            FinishedCallback = finishedCallback;
        }

        protected void Finish()
        {
            Started = false;
            FinishedCallback?.Invoke();
            FinishedCallback = null;

            if (Animator)
            {
                Animator.enabled = true;
            }
            
            OnFinished();
        }
        
        protected virtual void OnStarted()
        {
            
        }

        protected virtual void OnFinished()
        {
            
        }
        
        public virtual void Start()
        {
            Started = true;

            Target.TryGetComponent(out Animator);

            if (Animator)
            {
                Animator.enabled = false;
            }
            
            OnStarted();
        }
        
        public abstract void Update();
    }

}