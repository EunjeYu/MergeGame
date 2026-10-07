// Copyright (c) 2024, Awessets

using MergeIt.Core.Saves;

namespace MergeIt.Core.ElementsStock
{
    public interface IElementsStockData : ISavable
    {
        string[] Elements { get; set; }
    }
}