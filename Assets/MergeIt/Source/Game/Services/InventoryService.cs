// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Inventory;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Inventory;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Core.WindowSystem;
using MergeIt.Game.Converters;
using MergeIt.Game.Factories.Inventory;
using MergeIt.Game.Windows.Inventory;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Services
{
    public class InventoryService : IInventoryService
    {
        [Introduce]
        private IConfigProcessor _configProcessor;

        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IGameSaveService _saveService;

        [Introduce]
        private InventoryServiceModel _serviceModel;

        [Introduce]
        private IWindowSystem _windowSystem;

        public void CreateInventory()
        {
            InventoryConfig inventoryConfig = _configsService.InventoryConfig;
            var inventoryData = new InventoryData
            {
                InventorySize = inventoryConfig.InitialCapacity
            };

            SetupInventory(inventoryData);

            _saveService.Save(GameSaveType.Inventory);
        }

        public void SetupInventory(IInventoryData inventoryData)
        {
            _serviceModel.InventorySize = inventoryData.InventorySize;
            var elements = new List<IFieldElement>();

            if (inventoryData.InventoryElements != null)
            {
                for (int i = 0; i < inventoryData.InventoryElements.Length; i++)
                {
                    FieldElementData elementData = inventoryData.InventoryElements[i];
                    IFieldElement element = _configProcessor.ConvertToFieldElement(elementData);
                    elements.Add(element);
                }

                _serviceModel.InventoryElements.AddRange(elements);
            }
        }

        public IInventoryData GetData()
        {
            var data = new InventoryData();
            data.InventorySize = _serviceModel.InventorySize;

            if (_serviceModel.InventoryElements != null)
            {
                data.InventoryElements = new FieldElementData[_serviceModel.InventoryElements.Count];

                for (int i = 0; i < _serviceModel.InventoryElements.Count; i++)
                {
                    IFieldElement element = _serviceModel.InventoryElements[i];
                    FieldElementData elementData = _configProcessor.ConvertToFieldElementData(element);
                    data.InventoryElements[i] = elementData;
                }
            }

            return data;
        }

        public void Add(IFieldElement fieldElement)
        {
            _serviceModel.InventoryElements.Add(fieldElement);
            _saveService.Save(GameSaveType.Inventory);
        }

        public bool Remove(IFieldElement fieldElement)
        {
            bool result = _serviceModel.InventoryElements.Remove(fieldElement);

            if (result)
            {
                _saveService.Save(GameSaveType.Inventory);
            }

            return result;
        }

        public void OpenWindow()
        {
            _windowSystem.OpenWindow<InventoryPresenter>(enableBlackout: true);
        }
    }
}