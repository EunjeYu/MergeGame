// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Game.Windows.ElementInfo;

namespace MergeIt.Game.Factories.ElementInfo
{
    public interface IElementInfoFactory
    {
        ElementInfoItemComponent CreateElementWindowItem(ElementConfig elementConfig, ElementInfoType infoType = ElementInfoType.InfoWindow, bool isLocked = false);

        ElementInfoItemComponent CreateUnknownElementWindowItem(ElementInfoType infoType = ElementInfoType.InfoWindow);
    }
}