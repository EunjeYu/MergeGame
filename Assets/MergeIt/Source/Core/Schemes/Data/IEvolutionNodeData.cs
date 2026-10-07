// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Schemes.Data
{
    public interface IEvolutionNodeData
    {
        string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Discovered { get; set; }
        
        Rect Position { get; set; }
        
        T Copy<T>() where T : IEvolutionNodeData, new();
    }
}