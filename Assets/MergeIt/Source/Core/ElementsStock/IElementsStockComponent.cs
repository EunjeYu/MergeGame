// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;

namespace MergeIt.Core.ElementsStock
{
    public interface IElementsStockComponent
    {
        void SetupElement(ElementConfig elementConfig);
        void PopElement();
    }
}