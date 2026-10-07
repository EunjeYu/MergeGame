// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Schemes.Data;

namespace MergeIt.Editor.EvolutionsEditor
{
    public interface IElementNode : ISaveNode
    {
        ElementConfig Config { get; }
        
        IElementNodeData Data { get; }

        bool Validate();
    }
}