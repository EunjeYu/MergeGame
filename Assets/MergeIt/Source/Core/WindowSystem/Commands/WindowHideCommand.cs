// Copyright (c) 2024, Awessets

using MergeIt.Core.Commands;
using MergeIt.Core.Helpers;
using MergeIt.Core.WindowSystem.Windows;

namespace MergeIt.Core.WindowSystem.Commands
{
    public sealed class WindowHideCommand : Command, IMonoUpdateHandler
    {
        private IWindowPresenter _presenter;
        
        public WindowHideCommand(IWindowPresenter presenter)
        {
            MonoEventsListener.Instance.SubscribeOnUpdate(this);    
            
            _presenter = presenter;
            if (_presenter.State != WindowState.Hiding &&
                _presenter.State != WindowState.Hidden)
            {
                _presenter.Hide();
            }
            else
            {
                Finish();
            }
        }
        
        public void Update()
        {
            if (_presenter is {State: WindowState.Hidden})
            {
                _presenter = null;
                Finish();
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            MonoEventsListener.Instance.UnsubscribeFromUpdate(this);    
        }
    }
}