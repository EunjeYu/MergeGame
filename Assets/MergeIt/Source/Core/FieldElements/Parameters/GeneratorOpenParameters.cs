// Copyright (c) 2024, Awessets

using MergeIt.Core.Utils;

namespace MergeIt.Core.FieldElements
{
    public class GeneratorOpenParameters : IGeneratorOpenParameters
    {
        public bool IsOpening => StartOpeningTime != 0;
        public long StartOpeningTime { get; set; }
        public Bindable<float> RemainingTime { get; set; } = new();

        public void CopyFrom(SavedGeneratorOpenParameters other)
        {
            StartOpeningTime = other.StartOpeningTime;
        }
    }
}