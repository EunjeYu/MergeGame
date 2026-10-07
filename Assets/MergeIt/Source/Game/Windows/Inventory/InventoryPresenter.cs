// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Services;
using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Windows;
using MergeIt.Game.Factories.Inventory;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.Game.Windows.ElementInfo;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Windows.Inventory
{
    public class InventoryPresenter : WindowPresenter<InventoryWindow, InventoryModel>
    {
        private IConfigsService _configsService;
        private ICurrencyService _currencyService;
        private IGameFieldService _fieldService;
        private int _initialSize;
        private IInventoryFactory _inventoryFactory;
        private IInventoryService _inventoryService;
        private InventoryServiceModel _inventoryServiceModel;

        private List<InventoryWindowItemPair> _itemComponents;
        private InventoryWindowPaidCellComponent _paidCell;
        private CurrencySettings[] _paidCellsPrices;

        protected override void OnInitialize(IWindowArgs args = null)
        {
            base.OnInitialize(args);

            _inventoryService = DiContainer.Get<IInventoryService>();
            _inventoryServiceModel = DiContainer.Get<InventoryServiceModel>();
            _configsService = DiContainer.Get<IConfigsService>();
            _inventoryFactory = DiContainer.Get<IInventoryFactory>();
            _currencyService = DiContainer.Get<ICurrencyService>();
            _fieldService = DiContainer.Get<IGameFieldService>();

            _paidCellsPrices = _configsService.InventoryConfig.PaidCells;
            _initialSize = _configsService.InventoryConfig.InitialCapacity;

            FillGrid();
        }

        protected override void OnDestroyWindow()
        {
            base.OnDestroyWindow();

            for (int i = 0; i < _itemComponents.Count; i++)
            {
                InventoryWindowItemPair item = _itemComponents[i];
                item.Component.InfoClickEvent -= OnItemInfoClick;
                item.Component.ItemClickEvent -= OnItemClick;
            }

            if (_paidCell)
            {
                _paidCell.BuyCellEvent -= OnBuyCellClick;
            }
        }

        private void FillGrid()
        {
            _itemComponents = new List<InventoryWindowItemPair>();
            var inventoryElements = _inventoryServiceModel.InventoryElements;
            int elementsCount = inventoryElements.Count;
            if (elementsCount > 0)
            {
                for (int i = 0; i < elementsCount; i++)
                {
                    IFieldElement element = inventoryElements[i];
                    InventoryWindowItemComponent cell = _inventoryFactory.CreateInventoryWindowItem(element);

                    if (cell)
                    {
                        cell.ItemClickEvent += OnItemClick;
                        cell.InfoClickEvent += OnItemInfoClick;

                        _itemComponents.Add(new InventoryWindowItemPair(cell, element));

                        PlaceCellInGrid(cell.transform);
                    }
                }
            }

            int diff = _inventoryServiceModel.InventorySize - elementsCount;
            for (int i = 0; i < diff; i++)
            {
                CreateEmptyCell();
            }

            TrySetupPaidCell(true);
        }

        private void OnItemClick(InventoryWindowItemComponent item)
        {
            item.ItemClickEvent -= OnItemClick;
            InventoryWindowItemPair itemPair = _itemComponents.FirstOrDefault(x => x.Component == item);
            if (itemPair != null)
            {
                var freeCell = _fieldService.GetFreeCell();

                if (freeCell != null)
                {
                    if (_inventoryService.Remove(itemPair.Element))
                    {
                        int index = itemPair.Component.transform.GetSiblingIndex();

                        Object.Destroy(itemPair.Component.gameObject);

                        CreateEmptyCell(index);

                        // TODO: animate 

                        MessageBus.Fire(new RemoveFromInventoryMessage
                        {
                            FieldElement = itemPair.Element
                        });

                        MessageBus.Fire(new CreateElementMessage
                        {
                            NewElement = itemPair.Element,
                            FromPosition = null,
                            ToPoint = freeCell.Value
                        });
                    }
                }
            }
        }

        private void OnItemInfoClick(InventoryWindowItemComponent item)
        {
            IFieldElement fieldElement = _itemComponents.FirstOrDefault(x => x.Component == item)?.Element;
            var infoArgs = new ElementInfoArgs {ElementConfig = fieldElement?.ConfigParameters.ElementConfig};

            WindowSystem.OpenWindow<ElementInfoPresenter>(enableBlackout: true, args: infoArgs);
        }

        private void OnBuyCellClick()
        {
            int priceIndex = _inventoryServiceModel.InventorySize - _initialSize;
            if (_currencyService.TryPay(_paidCellsPrices[priceIndex]))
            {
                _inventoryServiceModel.InventorySize++;

                GameObject emptyCell = _inventoryFactory.CreateWindowEmptyCell();
                PlaceCellInGrid(emptyCell.transform);

                if (_paidCell)
                {
                    int index = _paidCell.transform.GetSiblingIndex();
                    emptyCell.transform.SetSiblingIndex(index);

                    TrySetupPaidCell();
                }
            }
        }

        private void TrySetupPaidCell(bool createCell = false)
        {
            int index = _inventoryServiceModel.InventorySize - _initialSize;
            if (index < _paidCellsPrices.Length)
            {
                if (createCell)
                {
                    _paidCell = _inventoryFactory.CreateWindowPaidCell();
                    _paidCell.BuyCellEvent += OnBuyCellClick;

                    PlaceCellInGrid(_paidCell.transform);
                }

                CurrencySettings priceSettings = _paidCellsPrices[index];
                string priceText = priceSettings.Amount.ToString();
                Sprite priceSprite = _configsService.GetCurrencyIcon(priceSettings.Currency);

                _paidCell.Setup(priceText, priceSprite);
            }
            else if (_paidCell)
            {
                _paidCell.BuyCellEvent -= OnBuyCellClick;

                Object.Destroy(_paidCell.gameObject);

                _paidCell = null;
            }
        }

        private void CreateEmptyCell(int siblingIndex = -1)
        {
            GameObject cell = _inventoryFactory.CreateWindowEmptyCell();

            PlaceCellInGrid(cell.transform, siblingIndex);
        }

        private void PlaceCellInGrid(Transform cell, int siblingIndex = -1)
        {
            cell.SetParent(View.ItemsGrid);
            cell.localScale = Vector3.one;

            if (siblingIndex != -1)
            {
                cell.SetSiblingIndex(siblingIndex);
            }
        }
    }
}