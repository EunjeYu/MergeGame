// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;
using MergeIt.Core.MVP;
using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Messages;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Core.WindowSystem.Windows
{
    public abstract class WindowPresenter<TWindow, TModel> : Presenter<TWindow, TModel>, IWindowPresenter
        where TWindow : Component, IWindow
        where TModel : WindowModel, new()
    {
        protected IMessageBus MessageBus;
        protected IWindowSystem WindowSystem;

        private WindowState _state;

        public string Layer { get; private set; }

        public WindowState State
        {
            get => _state;
        }

        public void Initialize(IView view, string layer, IWindowArgs windowArgs = null)
        {
            base.Initialize(view);

            MessageBus = DiContainer.Get<IMessageBus>();
            WindowSystem = DiContainer.Get<IWindowSystem>();

            Layer = layer;

            OnInitialize(windowArgs);
        }

        public void Show()
        {
            _state = WindowState.Opening;
            View.ShowStartEvent += OnWindowStartShowing;
            View.ShowEndEvent += OnWindowShown;
            View.Show();
        }

        public void Hide()
        {
            _state = WindowState.Hiding;
            InitiateClosing();
        }

        public void Close()
        {
            _state = WindowState.Closing;
            InitiateClosing();
        }

        public void SetWindowActive(bool active)
        {
            View.gameObject.SetActive(active);
        }

        public void SetWindowLayer()
        {
            View.SetLayer(Layer);
        }

        public void DestroyWindow()
        {
            OnDestroyWindow();

            View.InitiateCloseEvent -= OnInitiateCloseEvent;

            Object.Destroy(View.gameObject);
        }

        protected virtual void OnInitialize(IWindowArgs args = null)
        {
        }

        protected virtual void OnDestroyWindow()
        {
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            if (View)
            {
                View.InitiateCloseEvent -= OnInitiateCloseEvent;
            }
        }

        private void InitiateClosing()
        {
            View.CloseStartEvent += OnWindowStartClosing;
            View.CloseEndEvent += OnWindowClosed;
            View.Close();
        }

        private void OnInitiateCloseEvent()
        {
            var message = new CloseWindowMessage {Presenter = this};
            MessageBus.Fire(message);
        }

        protected virtual void OnWindowStartShowing()
        {
            SetWindowActive(true);

            View.ShowStartEvent -= OnWindowStartShowing;
        }

        protected virtual void OnWindowShown()
        {
            View.ShowEndEvent -= OnWindowShown;
            View.InitiateCloseEvent += OnInitiateCloseEvent;
            
            _state = WindowState.Opened;
        }
        
        protected virtual void OnWindowStartClosing()
        {
            View.CloseStartEvent -= OnWindowStartClosing;
            View.InitiateCloseEvent -= OnInitiateCloseEvent;
        }

        protected virtual void OnWindowClosed()
        {
            View.CloseEndEvent -= OnWindowClosed;

            if (_state == WindowState.Closing)
            {
                _state = WindowState.Closed;
                SetWindowActive(false);
                DestroyWindow();
            }
            else if (_state == WindowState.Hiding)
            {
                _state = WindowState.Hidden;
                
            }
        }
    }
}