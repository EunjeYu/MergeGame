// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Core.WindowSystem.Windows
{
    public struct WindowCreateInfo : IEquatable<WindowCreateInfo>
    {
        public string LayerName { get; set; }
        public string PrefabPath { get; set; }
        public Type Type { get; set; }
        
        public bool Equals(WindowCreateInfo other)
        {
            return LayerName == other.LayerName && PrefabPath == other.PrefabPath && Type == other.Type;
        }
        
        public override bool Equals(object obj)
        {
            return obj is WindowCreateInfo other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(LayerName, PrefabPath, Type);
        }
    }
}