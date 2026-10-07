// Copyright (c) 2024, Awessets

using MergeIt.Core.Schemes.Data;

namespace MergeIt.Editor.EvolutionsEditor
{
    public interface IEvolutionNode : ISaveNode
    {
        string Name { get; }
        string Description { get; }
        bool Discovered { get; }
        IEvolutionNodeData Data { get; }
    }
}