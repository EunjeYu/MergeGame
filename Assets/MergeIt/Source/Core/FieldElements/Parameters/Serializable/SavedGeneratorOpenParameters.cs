// Copyright (c) 2024, Awessets

using System;
using Newtonsoft.Json;

namespace MergeIt.Core.FieldElements
{
    [Serializable]
    public class SavedGeneratorOpenParameters
    {
        [JsonProperty("s")]
        public long StartOpeningTime { get; set; }
        
        public void CopyFrom(IGeneratorOpenParameters other)
        {
            StartOpeningTime = other.StartOpeningTime;
        }
    }
}