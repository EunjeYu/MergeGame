// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Saves;

namespace MergeIt.Core.Evolutions
{
    public interface IEvolutionsProgressData : ISavable
    {
        List<EvolutionProgressData> EvolutionsProgress { get; }
    }
}