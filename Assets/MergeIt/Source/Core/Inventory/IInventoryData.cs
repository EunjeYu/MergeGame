// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Saves;

namespace MergeIt.Core.Inventory
{
    public interface IInventoryData : ISavable
    {
        int InventorySize { get; set; }
        FieldElementData[] InventoryElements { get; set; }
    }
}