// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Elements;

namespace MergeIt.Core.FieldElements
{
    public class ProducedByParameters : IProducedByParameters
    {
        public List<ElementConfig> Elements { get; set; }
    }
}