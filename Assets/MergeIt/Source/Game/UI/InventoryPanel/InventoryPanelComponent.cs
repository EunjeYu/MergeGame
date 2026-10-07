// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Factories.Inventory;
using MergeIt.Game.Field;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InventoryPanel
{
    public class InventoryPanelComponent : MonoBehaviour
    {
        [SerializeField]
        private Button _openButton;

        [SerializeField]
        private RectTransform _itemsContent;

        private float _contentHeight;
        private FieldLogicModel _fieldLogicModel;
        private IGameFieldService _gameFieldService;
        private IInventoryFactory _inventoryFactory;
        private IInventoryService _inventoryService;
        private InventoryServiceModel _inventoryServiceModel;
        private readonly HashSet<InventoryPanelItemPair> _items = new();

        private IMessageBus _messageBus;
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _openButton.onClick.AddListener(OnOpenInventoryClick);

            _inventoryFactory = DiContainer.Get<IInventoryFactory>();
            _fieldLogicModel = DiContainer.Get<FieldLogicModel>();
            _inventoryServiceModel = DiContainer.Get<InventoryServiceModel>();
            _inventoryService = DiContainer.Get<IInventoryService>();
            _gameFieldService = DiContainer.Get<IGameFieldService>();

            _messageBus = DiContainer.Get<IMessageBus>();
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.AddListener<EndDragElementMessage>(OnEndDragElementMessageHandler);

            _contentHeight = _itemsContent.rect.height;
        }

        private void OnDestroy()
        {
            _openButton.onClick.RemoveListener(OnOpenInventoryClick);

            if (_messageBus != null)
            {
                _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
                _messageBus.RemoveListener<EndDragElementMessage>(OnEndDragElementMessageHandler);
                _messageBus.RemoveListener<RemoveFromInventoryMessage>(OnRemoveFromInventoryMessageHandler);
            }
        }

        private void OnLoadedGameMessageHandler(LoadedGameMessage _)
        {
            _messageBus.AddListener<RemoveFromInventoryMessage>(OnRemoveFromInventoryMessageHandler);

            var elements = _inventoryServiceModel.InventoryElements;

            if (elements != null)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    CreateItem(elements[i]);
                }
            }
        }

        private void OnEndDragElementMessageHandler(EndDragElementMessage message)
        {
            if (!message.ToGameObject)
            {
                return;
            }

            if (_inventoryServiceModel.IsFull())
            {
                return;
            }

            if (!_fieldLogicModel.FieldElements.TryGetValue(message.FromPoint, out IFieldElement element))
            {
                return;
            }

            bool isInRect = RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, message.Position, Camera.main);

            if (isInRect)
            {
                CreateItem(element);

                _inventoryService.Add(element);

                var remove = new RemoveElementMessage
                {
                    RemoveAtPoint = message.FromPoint
                };
                _messageBus.Fire(remove);
            }
        }

        private void OnRemoveFromInventoryMessageHandler(RemoveFromInventoryMessage message)
        {
            RemoveItemFromInventory(message.FieldElement);
        }

        private void CreateItem(IFieldElement fieldElement)
        {
            InventoryPanelItemComponent component = _inventoryFactory.CreateInventoryPanelItem(fieldElement);
            component.transform.SetParent(_itemsContent);
            component.transform.localScale = Vector3.one;
            component.RectTransform.sizeDelta = new Vector2(_contentHeight, _contentHeight);

            component.ClickEvent += OnItemClick;

            _items.Add(new InventoryPanelItemPair(component, fieldElement));
        }

        private void OnItemClick(InventoryPanelItemComponent item)
        {
            Vector3 fromPosition = item.gameObject.transform.position;
            var freeCell = _gameFieldService.GetFreeCell();
            if (freeCell != null)
            {
                item.ClickEvent -= OnItemClick;
                InventoryPanelItemPair pair = _items.FirstOrDefault(x => x.Component == item);
                if (pair != null)
                {
                    pair.Element.InfoParameters.LogicPosition = freeCell.Value;
                    _messageBus.Fire(new CreateElementMessage
                    {
                        NewElement = pair.Element,
                        FromPosition = fromPosition,
                        ToPoint = freeCell.Value
                    });

                    _inventoryService.Remove(pair.Element);
                    Destroy(pair.Component.gameObject);

                    _items.Remove(pair);
                }
            }
        }

        private void RemoveItemFromInventory(IFieldElement fieldElement)
        {
            InventoryPanelItemPair pair = _items.FirstOrDefault(x => x.Element == fieldElement);

            if (pair is {Component: not null})
            {
                Destroy(pair.Component.gameObject);
                _items.Remove(pair);
            }
        }

        private void OnOpenInventoryClick()
        {
            _inventoryService.OpenWindow();
        }
    }
}