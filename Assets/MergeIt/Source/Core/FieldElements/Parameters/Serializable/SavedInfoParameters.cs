// Copyright (c) 2024, Awessets

using System;
using Newtonsoft.Json;

namespace MergeIt.Core.FieldElements
{
    [Serializable]
    public class SavedInfoParameters
    {
        [JsonProperty("p")]
        public GridPoint LogicPosition { get; set; }
        [JsonProperty("b")]
        public bool IsBlocked { get; set; }
    }
}