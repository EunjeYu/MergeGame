// Copyright (c) 2024, Awessets

using UnityEditor.Experimental.GraphView;

namespace MergeIt.Editor.EvolutionsEditor
{
    public interface ISaveNode
    {
        string Id { get; }
        void SaveData();
        Port GetPort(string portName);
    }
}