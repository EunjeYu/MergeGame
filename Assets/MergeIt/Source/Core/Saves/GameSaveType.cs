// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Core.Saves
{
    [Flags]
    public enum GameSaveType
    {
        Field = 1 << 0,
        Inventory = 1 << 1,
        User = 1 << 2,
        Stock = 1 << 3,
        EvolutionsProgress = 1 << 4,
        All = Field | Inventory | User | Stock | EvolutionsProgress
    }
}