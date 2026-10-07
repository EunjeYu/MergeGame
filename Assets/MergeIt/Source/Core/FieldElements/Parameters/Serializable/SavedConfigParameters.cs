// Copyright (c) 2024, Awessets

using System;
using Newtonsoft.Json;

namespace MergeIt.Core.FieldElements
{
    [Serializable]
    public class SavedConfigParameters
    {
        [JsonProperty("id")]
        public string ElementId { get; set; }
        [JsonProperty("eid")]
        public string EvolutionId { get; set; }
    }
}