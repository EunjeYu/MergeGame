// Copyright (c) 2024, Awessets

using System.Collections.Generic;

namespace MergeIt.Core.FieldElements
{
    public class ProduceParameters : IProduceParameters
    {
        public List<GeneratableFieldElement> Elements { get; set; }
    }
}