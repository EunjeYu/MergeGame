// Copyright (c) 2024, Awessets

using System;
using Newtonsoft.Json;

namespace MergeIt.Core.Evolutions
{
    [Serializable]
    public class EvolutionProgressData
    {
        [JsonProperty("id")]
        public string EvolutionId { get; set; }
        
        [JsonProperty("p")]
        public int Progress { get; set; }
    }
}