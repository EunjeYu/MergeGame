// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MergeIt.Game.Field
{
    public class FieldCellComponent : MonoBehaviour, 
        IDragHandler, 
        IPointerDownHandler, 
        IPointerUpHandler,
        IBeginDragHandler,
        IEndDragHandler
    {
        private readonly float _pointerClickTime = 0.2f;

        [SerializeField]
        private GameObject _selector;
        
        [SerializeField]
        private Transform _objectContainer;

        private RectTransform _rectTransform;
        private IFieldElementPresenter _fieldElementPresenter;
        private GridPoint _gridPoint;

        private GameServiceModel _gameServiceModel;
        private IMessageBus _messageBus;
        
        private Vector3 _offset;
        private Camera _mainCamera;
        private Transform _transform;

        private float _pointerDownTime;
        
        public IFieldElementPresenter FieldElementPresenter => _fieldElementPresenter;
        
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


        public Transform ObjectContainer
        {
            get => _objectContainer;
        }
        
        public GridPoint Point
        {
            get => _gridPoint;
        }

        public void Initialize(GridPoint gridPoint)
        {
            _gridPoint = gridPoint;
            _messageBus = DiContainer.Get<IMessageBus>();
            _gameServiceModel = DiContainer.Get<GameServiceModel>();
            
            _messageBus.AddListener<ResetPositionMessage>(OnResetPositionMessageHandler);
        }

        public void ToggleSelector(bool activate)
        {
            _selector.SetActive(activate);
            
            if (activate)
            {
                _messageBus.Fire<ResetHintsMessage>();
            }

            if (!activate && _fieldElementPresenter != null)
            {
                _fieldElementPresenter.GetModel().Selected = false;
            }
        }

        public void AttachObject(IFieldElementPresenter presenter, bool resetPosition = true)
        {
            if (presenter != null)
            {
                _fieldElementPresenter = presenter;
                _fieldElementPresenter.SetParent(ObjectContainer, resetPosition);
                _fieldElementPresenter.SetPoint(Point);
            }
            else
            {
                ToggleSelector(false);
            }
        }

        public void DetachObject()
        {
            _fieldElementPresenter = null;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_fieldElementPresenter?.IsBusy == false)
            {
                _offset = _fieldElementPresenter.Transform.position - _gameServiceModel.MainCamera.ScreenToWorldPoint(eventData.position);
                _pointerDownTime = Time.realtimeSinceStartup;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_fieldElementPresenter?.IsBusy == false)
            {
                var pointerUpTime = Time.realtimeSinceStartup;
                var diff = pointerUpTime - _pointerDownTime;
        
                if (diff <= _pointerClickTime)
                {
                    SendClick();
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_fieldElementPresenter?.IsAvailable == true)
            {
                _fieldElementPresenter.Transform.position = _gameServiceModel.MainCamera.ScreenToWorldPoint(eventData.position) + _offset;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_fieldElementPresenter?.IsAvailable == true)
            {
                _fieldElementPresenter.Select(true);
                _fieldElementPresenter.StartDrag();

                _messageBus.Fire(new ActivateHintsMessage {Active = false});
                
                SetVisualSelection(false);
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_fieldElementPresenter?.IsAvailable == true)
            {
                _fieldElementPresenter.EndDrag();
                SendEndDrag(eventData);
                
                _messageBus.Fire(new ActivateHintsMessage {Active = true});
            }
        }
        
        private void SetVisualSelection(bool activate)
        {
            ToggleSelector(activate);
        }

        private void OnResetPositionMessageHandler(ResetPositionMessage message)
        {
            if (message.From == Point)
            {
                SetVisualSelection(true);
            }
        }

        private void SendClick()
        {
            var clickMessage = new ClickElementMessage
            {
                Cell = this
            };

            _messageBus.Fire(clickMessage);
        }

        private void SendEndDrag(PointerEventData eventData)
        {
            var raycastGameObject = eventData.pointerCurrentRaycast.gameObject;
            
            var endDragMessage = new EndDragElementMessage
            {
                FromPoint = Point,
                ToGameObject = raycastGameObject,
                Position = eventData.position
            };

            _messageBus.Fire(endDragMessage);
        }

        private void OnDestroy()
        {
            _messageBus.RemoveListener<ResetPositionMessage>(OnResetPositionMessageHandler);
        }
    }
}