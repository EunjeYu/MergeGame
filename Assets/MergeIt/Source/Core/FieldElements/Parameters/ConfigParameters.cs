// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;

namespace MergeIt.Core.FieldElements
{
    public class ConfigParameters : IConfigParameters
    {
        public ElementConfig ElementConfig { get; set; }
        public EvolutionData EvolutionData { get; set; }
    }
}