// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Elements;

namespace MergeIt.Core.Configs.LevelUp
{
    [Serializable]
    public class LevelUpParameters
    {
        public int Experience;
        public ElementConfig[] Bonuses;
    }
}