// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Data;
using UnityEngine;

namespace MergeIt.Core.Schemes
{
    public class SchemeObject : ScriptableObject
    {
        public SchemeData SchemeData;

        public List<EvolutionData> Evolution
        {
            get => SchemeData?.EvolutionsData;
        }
    }
}