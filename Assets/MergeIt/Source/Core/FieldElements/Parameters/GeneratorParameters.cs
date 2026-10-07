// Copyright (c) 2024, Awessets

using MergeIt.Core.Utils;

namespace MergeIt.Core.FieldElements
{
    public class GeneratorParameters : IGeneratorParameters
    {
        public bool Charging => StartChargingTime != 0;
        
        public int AvailableToDrop { get; set; }
        public int ChargedCount { get; set; }
        public long StartChargingTime { get; set; }
        public float RemainChargeTime { get; set; }
        public float MinDropFullChargeTime { get; set; }
        public int DroppedElements { get; set; }
        
        public Bindable<float> MinDropChargeTime { get; set; } = new();

        public void CopyFrom(SavedGeneratorParameters other)
        {
            AvailableToDrop = other.AvailableToDrop;
            StartChargingTime = other.StartChargingTime;
            DroppedElements = other.DroppedElements;
            ChargedCount = other.ChargedCount;
        }
    }
}