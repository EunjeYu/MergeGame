// Copyright (c) 2024, Awessets

using System;
using Newtonsoft.Json;

namespace MergeIt.Core.FieldElements
{
    [Serializable]
    public class SavedGeneratorParameters
    {
        [JsonProperty("a")]
        public int AvailableToDrop { get; set; }
        [JsonProperty("s")]
        public long StartChargingTime { get; set; }
        [JsonProperty("d")]
        public int DroppedElements { get; set; }
        [JsonProperty("c")]
        public int ChargedCount { get; set; }
        
        public void CopyFrom(IGeneratorParameters other)
        {
            AvailableToDrop = other.AvailableToDrop;
            StartChargingTime = other.StartChargingTime;
            DroppedElements = other.DroppedElements;
            ChargedCount = other.ChargedCount;
        }
    }
}