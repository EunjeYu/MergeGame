// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Schemes;
using UnityEngine;

namespace MergeIt.Core.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Merge Toolkit/Level config")]
    public class LevelConfig : ScriptableObject
    {
        public int FieldWidth;
        public int FieldHeight;
        public SchemeObject EvolutionsScheme;
        public List<LevelElementData> FieldElementsData;
    }
}