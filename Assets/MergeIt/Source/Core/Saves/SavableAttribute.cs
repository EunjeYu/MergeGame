// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Core.Saves
{
    public class SavableAttribute : Attribute
    {
        public string Name { get; }
        public string  Extension { get; }

        public SavableAttribute(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }
    }
}