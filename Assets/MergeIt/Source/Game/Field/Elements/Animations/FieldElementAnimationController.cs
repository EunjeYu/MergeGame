// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Animations;
using MergeIt.Core.FieldElements;
using UnityEngine;

namespace MergeIt.Game.Field.Elements.Animations
{
    [RequireComponent(typeof(Animator))]
    public class FieldElementAnimationController : AnimationControllerBase
    {
        private IAnimationListener _listener;
        private Animator _animator;

        private static readonly Dictionary<FieldElementState, int> StatesHash = new();
        
        static FieldElementAnimationController()
        {
            StatesHash[FieldElementState.Idle] = Animator.StringToHash(FieldElementState.Idle.ToString());
            StatesHash[FieldElementState.Hint] = Animator.StringToHash(FieldElementState.Hint.ToString());
        }

        public static int GetFieldElementState(FieldElementState state)
        {
            StatesHash.TryGetValue(state, out int hash);

            return hash;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Initialize(IAnimationListener listener)
        {
            _listener = listener;
        }

        public override void SetState<T>(T state)
        {
            var concreteState = (FieldElementState)(object)state;

            if (StatesHash.TryGetValue(concreteState, out int hash))
            {
                switch (concreteState)
                {
                    case FieldElementState.Idle:
                        _animator.Play(hash);
                        break;
                    
                    case FieldElementState.Hint:
                        _animator.SetTrigger(hash);
                        break;
                }
            }
        }

        private void OnDisable()
        {
            _listener?.ResetAnimationState();
        }
    }
}