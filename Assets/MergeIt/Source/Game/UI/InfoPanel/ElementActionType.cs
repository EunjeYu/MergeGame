// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Game.UI.InfoPanel
{
    [Flags]
    public enum ElementActionType
    {
        None = 0,
        Common = 1 << 0,
        SkipCharging = 1 << 1,
        SkipOpening = 1 << 2,
        Sell = 1 << 3,
        Split = 1 << 4,
        Open = 1 << 5,
        Unlock = 1 << 6
    }
}