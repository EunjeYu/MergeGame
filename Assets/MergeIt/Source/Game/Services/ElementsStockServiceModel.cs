// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Elements;

namespace MergeIt.Game.Services
{
    public class ElementsStockServiceModel
    {
        public Queue<ElementConfig> Elements { get; } = new();
    }
}