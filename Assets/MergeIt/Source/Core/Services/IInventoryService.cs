// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Inventory;

namespace MergeIt.Core.Services
{
    public interface IInventoryService
    {
        void CreateInventory();
        void SetupInventory(IInventoryData inventoryData);
        IInventoryData GetData();
        void Add(IFieldElement element);
        bool Remove(IFieldElement fieldElement);
        void OpenWindow();
    }
}