// Copyright (c) 2024, Awessets

using MergeIt.Core.Animations;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.MVP;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MergeIt.Game.Field.Elements
{
    public class FieldElementView : View, IFieldElementView
    {
        public UnityAction ResetStateEvent;
        
        [SerializeField]
        private GameObject _blocker;

        private IAnimationController _animationController;
        
        public GraphicRaycaster GraphicRaycaster => GetComponent<GraphicRaycaster>();

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

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
        
        public IAnimationController AnimationController
        {
            get { return _animationController ??= GetComponent<IAnimationController>(); }
        }

        private RectTransform _rectTransform;
        private Canvas _canvas;
        private FieldElementModel _model;
        private FieldElementIconComponent _iconComponent;
        
        // private void Start()
        // {
        //     RectTransform rectTransform = GetComponent<RectTransform>();
        //     rectTransform.Stretch();
        // }

        public void Lock(bool isLocked)
        {
            if (!_iconComponent)
            {
                _iconComponent = GetComponentInChildren<FieldElementIconComponent>();
            }
            
            _iconComponent.SetBlocked(isLocked);
            _blocker.SetActive(isLocked);
        }

        public void ResetState()
        {
            ResetStateEvent?.Invoke();
        }
    }
}