// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Animations;
using MergeIt.Core.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Core.WindowSystem.Windows
{
    public abstract class WindowBase : View, IWindow, IWindowAnimationListener
    {
        public event Action InitiateCloseEvent;

        public event Action ShowStartEvent;
        public event Action ShowEndEvent;
        public event Action CloseStartEvent;
        public event Action CloseEndEvent;

        [SerializeField]
        protected Button CloseButton;

        [SerializeField]
        protected Button ClickOutsideArea;

        public Canvas Canvas
        {
            get
            {
                if (!_canvas)
                {
                    _canvas = GetComponent<Canvas>();
                }

                return _canvas;
            }
        }

        public IWindowAnimationController AnimationController
        {
            get
            {
                if (_animationController == null)
                {
                    _animationController = GetComponent<IWindowAnimationController>() ?? new WindowDefaultAnimationController();
                    _animationController.Initialize(this);
                }

                return _animationController;
            }
        }

        private Canvas _canvas;
        private IWindowAnimationController _animationController;

        public virtual void Show()
        {
            SubscribeOnClose();
            AnimationController.OpenWindow();
        }

        public virtual void Close()
        {
            UnsubscribeFromClose();
            AnimationController.CloseWindow();
        }

        public virtual void SetLayer(string layer)
        {
            if (!string.IsNullOrEmpty(layer))
            {
                Canvas.overrideSorting = true;
                Canvas.sortingLayerName = layer;
            }
            else
            {
                Canvas.overrideSorting = false;
                Canvas.sortingLayerName = Configs.Windows.SortingLayers.Default;
            }
        }

        public virtual void OnOpenStarted()
        {
            ShowStartEvent?.Invoke();
        }

        public virtual void OnOpenFinished()
        {
            ShowEndEvent?.Invoke();
        }

        public virtual void OnCloseStarted()
        {
            CloseStartEvent?.Invoke();
        }

        public virtual void OnCloseFinished()
        {
            CloseEndEvent?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnsubscribeFromClose();
        }

        private void SubscribeOnClose()
        {
            CloseButton.onClick.AddListener(OnCloseButtonClick);

            if (ClickOutsideArea)
            {
                ClickOutsideArea.onClick.AddListener(OnCloseButtonClick);
            }
        }

        private void UnsubscribeFromClose()
        {
            CloseButton.onClick.RemoveListener(OnCloseButtonClick);

            if (ClickOutsideArea)
            {
                ClickOutsideArea.onClick.RemoveListener(OnCloseButtonClick);
            }
        }

        private void OnCloseButtonClick()
        {
            InitiateCloseEvent?.Invoke();
        }
    }
}