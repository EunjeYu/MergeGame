// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Types;
using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs
{
    public static class StylesConstants
    {
        public static readonly Dictionary<ElementType, string> NodeStyles = new()
        {
            {ElementType.Regular, "nodeBorderSimple"},
            {ElementType.Generator, "nodeBorderGenerator"}
        };
        
        public static StyleEnum<DisplayStyle> DisplayNone = new(DisplayStyle.None);
        public static StyleEnum<DisplayStyle> DisplayFlex = new(DisplayStyle.Flex);
        public static StyleEnum<Position> AbsolutePosition = new(Position.Absolute);
        public static StyleEnum<Position> RelativePosition = new(Position.Relative);
        public static StyleLength Length0 = new(0f);
        public const string InvalidNode = "nodeInvalid";
    }
}