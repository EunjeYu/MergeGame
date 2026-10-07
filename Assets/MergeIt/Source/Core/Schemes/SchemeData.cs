// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Schemes.Data;

namespace MergeIt.Core.Schemes
{
    [Serializable]
    public class SchemeData
    {
        public List<ElementNodeData> ElementsNodesData;
        public List<EvolutionNodeData> EvolutionsNodesData;
        public List<EdgeData> Edges;

        public List<EvolutionData> EvolutionsData;
    }
}