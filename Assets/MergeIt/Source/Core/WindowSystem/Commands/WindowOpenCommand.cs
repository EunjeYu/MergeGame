// Copyright (c) 2024, Awessets

using MergeIt.Core.Commands;
using MergeIt.Core.Helpers;
using MergeIt.Core.WindowSystem.Windows;

namespace MergeIt.Core.WindowSystem.Commands
{
    public sealed class WindowOpenCommand : Command, IMonoUpdateHandler
    {
        private IWindowPresenter _presenter;
        
        public WindowOpenCommand(IWindowPresenter presenter)
        {
            MonoEventsListener.Instance.SubscribeOnUpdate(this);    
            
            _presenter = presenter;
            if (_presenter.State != WindowState.Opening &&
                _presenter.State != WindowState.Opened)
            {
                _presenter.Show();
            }
            else
            {
                Finish();
            }
        }
        
        public void Update()
        {
            if (_presenter is {State: WindowState.Opened})
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