// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;

namespace MergeIt.Core.FieldElements
{
    public interface IConfigParameters
    {
        ElementConfig ElementConfig { get; set; }
        EvolutionData EvolutionData { get; set; }
    }
}