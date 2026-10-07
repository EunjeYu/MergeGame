// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Core.Schemes.Data
{
    [Serializable]
    public class EdgeData
    {
        public string OutputNodeID;
        public string InputNodeID;
        public string OutputPortName;
        public string InputPortName;
    }
}