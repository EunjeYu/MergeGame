// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.ElementsStock;
using MergeIt.Core.Saves;
using Newtonsoft.Json;

namespace MergeIt.Game.ElementsStock
{
    [Serializable, Savable("stock", "dat")]
    public class ElementsStockData : IElementsStockData
    {
        [JsonProperty("e")]
        public string[] Elements { get; set; }
    }
}