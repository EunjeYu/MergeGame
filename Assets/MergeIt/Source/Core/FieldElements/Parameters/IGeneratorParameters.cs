// Copyright (c) 2024, Awessets

using MergeIt.Core.Utils;

namespace MergeIt.Core.FieldElements
{
    public interface IGeneratorParameters
    {
        bool Charging { get; }
        
        int AvailableToDrop { get; set; }
        int ChargedCount { get; set; }
        long StartChargingTime { get; set; }
        Bindable<float> MinDropChargeTime { get; set; }
        float MinDropFullChargeTime { get; set; }
        float RemainChargeTime { get; set; }
        int DroppedElements { get; set; }

        void CopyFrom(SavedGeneratorParameters other);
    }

}