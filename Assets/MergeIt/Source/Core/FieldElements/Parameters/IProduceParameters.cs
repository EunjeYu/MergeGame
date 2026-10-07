// Copyright (c) 2024, Awessets

using System.Collections.Generic;

namespace MergeIt.Core.FieldElements
{
    public interface IProduceParameters
    {
        List<GeneratableFieldElement> Elements { get; set; }
    }
}