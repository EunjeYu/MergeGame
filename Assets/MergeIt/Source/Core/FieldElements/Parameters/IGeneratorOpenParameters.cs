// Copyright (c) 2024, Awessets

using MergeIt.Core.Utils;

namespace MergeIt.Core.FieldElements
{
    public interface IGeneratorOpenParameters
    {
        bool IsOpening { get; }
        long StartOpeningTime { get; set; }
        Bindable<float> RemainingTime { get; set; }

        void CopyFrom(SavedGeneratorOpenParameters other);
    }
}