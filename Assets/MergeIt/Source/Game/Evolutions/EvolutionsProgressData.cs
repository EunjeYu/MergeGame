// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using MergeIt.Core.Evolutions;
using MergeIt.Core.Saves;
using Newtonsoft.Json;

namespace MergeIt.Game.Evolutions
{
    [Serializable, Savable("evoprogress", "dat")]
    public class EvolutionsProgressData : IEvolutionsProgressData
    {
        [JsonProperty("ep")]
        public List<EvolutionProgressData> EvolutionsProgress { get; } = new();
    }
}