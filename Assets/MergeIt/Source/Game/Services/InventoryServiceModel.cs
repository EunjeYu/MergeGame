// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.FieldElements;

namespace MergeIt.Game.Services
{
    public class InventoryServiceModel
    {
        public int InventorySize { get; set; }
        public List<IFieldElement> InventoryElements { get; } = new();

        public bool IsFull()
        {
            return InventoryElements.Count == InventorySize;
        }
    }
}