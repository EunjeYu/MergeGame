// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using UnityEngine;

namespace MergeIt.Game.Factories.Icons
{
    public interface IIconFactory
    {
        FieldElementIconComponent CreateIcon(ElementConfig config, Transform parent = null);
    }
}