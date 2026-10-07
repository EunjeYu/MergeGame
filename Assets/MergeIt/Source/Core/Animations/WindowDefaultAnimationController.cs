// Copyright (c) 2024, Awessets

namespace MergeIt.Core.Animations
{
    public class WindowDefaultAnimationController : IWindowAnimationController
    {
        private IWindowAnimationListener _listener;
        
        public void Initialize(IWindowAnimationListener listener)
        {
            _listener = listener;
        }
        
        public void OpenWindow()
        {
            _listener.OnOpenStarted();
            OnOpenEnd();
        }
        
        public void CloseWindow()
        {
            _listener.OnCloseStarted();
            OnCloseEnd();
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