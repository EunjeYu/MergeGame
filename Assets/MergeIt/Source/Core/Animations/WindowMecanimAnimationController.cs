// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Animations
{
    [RequireComponent(typeof(Animator))]
    public class WindowMecanimAnimationController : MonoBehaviour, IWindowAnimationController
    {
        [SerializeField]
        private Animator _animator;
        
        [SerializeField]
        private string _openTrigger;
        
        [SerializeField]
        private string _closeTrigger;

        private int _openHash;
        private int _closeHash;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _openHash = Animator.StringToHash(_openTrigger);
            _closeHash = Animator.StringToHash(_closeTrigger);
        }

        private IWindowAnimationListener _listener;
        
        public void Initialize(IWindowAnimationListener listener)
        {
            _listener = listener;
        }
        
        public void OpenWindow()
        {
            _listener.OnOpenStarted();
            _animator.SetTrigger(_openHash);
        }
        
        public void CloseWindow()
        {
            _listener.OnCloseStarted();
            _animator.SetTrigger(_closeHash);
        }
        
        public void OnOpenEnd()
        {
            _listener.OnOpenFinished();
        }

        public void OnCloseEnd()
        {
            _listener.OnCloseFinished();
        }
    }

}