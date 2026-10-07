// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using UnityEngine;

namespace MergeIt.Core.Schemes.Data
{
    public interface IElementNodeData
    {
        string Id { get; set; }
        ElementConfig ElementConfig { get; set; }
        Rect Position { get; set; }

        T Copy<T>() where T : IElementNodeData, new();
    }
}