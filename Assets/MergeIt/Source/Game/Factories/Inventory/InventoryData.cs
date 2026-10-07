// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Inventory;
using MergeIt.Core.Saves;
using Newtonsoft.Json;

namespace MergeIt.Game.Factories.Inventory
{
    [Serializable, Savable("inventory", "dat")]
    public class InventoryData : IInventoryData
    {
        [JsonProperty("s")]
        public int InventorySize { get; set; }
        [JsonProperty("e")]
        public FieldElementData[] InventoryElements { get; set; }
    }
}