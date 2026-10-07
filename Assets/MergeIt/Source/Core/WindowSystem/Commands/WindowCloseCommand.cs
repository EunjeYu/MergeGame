// Copyright (c) 2024, Awessets

using MergeIt.Core.Commands;
using MergeIt.Core.Helpers;
using MergeIt.Core.WindowSystem.Windows;

namespace MergeIt.Core.WindowSystem.Commands
{
    public sealed class WindowCloseCommand : Command, IMonoUpdateHandler
    {
        private IWindowPresenter _presenter;
        
        public WindowCloseCommand(IWindowPresenter presenter)
        {
            MonoEventsListener.Instance.SubscribeOnUpdate(this);    
            
            _presenter = presenter;
            if (_presenter.State != WindowState.Closing &&
                _presenter.State != WindowState.Closed)
            {
                _presenter.Close();
            }
            else
            {
                Finish();
            }
        }
        
        public void Update()
        {
            if (_presenter is {State: WindowState.Closed})
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