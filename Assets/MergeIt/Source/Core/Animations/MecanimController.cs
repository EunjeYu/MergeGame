// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Animations
{
    [RequireComponent(typeof(Animator))]
    public class MecanimController : AnimationControllerBase
    {
        private Animator _animator;
        private IAnimationListener _listener;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Initialize(IAnimationListener listener)
        {
            _listener = listener;
        }

        public override void SetState(string state)
        {
            _animator.Play(state);
        }
        
        public override void SetState(int state)
        {
            _animator.Play(state);
        }
        
        public override void SetState<T>(T state)
        {
            _animator.Play(state.ToString());
        }
    }

}