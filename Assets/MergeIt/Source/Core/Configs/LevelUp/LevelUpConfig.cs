// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Configs.LevelUp
{
    [CreateAssetMenu(fileName = "LevelUpConfig", menuName = "Merge Toolkit/Level Up Config")]
    public class LevelUpConfig : ScriptableObject
    {
        public LevelUpParameters[] LevelUp;
    }

}