// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Types;

namespace MergeIt.Core.FieldElements
{
    public class InfoParameters : IInfoParameters
    {
        public GridPoint LogicPosition { get; set; }
        public bool IsBlocked { get; set; }
        public ElementType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}