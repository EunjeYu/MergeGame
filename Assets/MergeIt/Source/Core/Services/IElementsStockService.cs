// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.ElementsStock;

namespace MergeIt.Core.Services
{
    public interface IElementsStockService
    {
        void CreateStock();
        void SetupStock(IElementsStockData stockData);
        void Add(ElementConfig elementConfig);
        bool Remove();
        ElementConfig GetNext();
        ElementConfig GetCurrent();
        IElementsStockData GetData();
    }
}