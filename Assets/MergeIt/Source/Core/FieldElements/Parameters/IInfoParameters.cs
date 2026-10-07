// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Types;

namespace MergeIt.Core.FieldElements
{
    public interface IInfoParameters
    {
        GridPoint LogicPosition { get; set; }
        bool IsBlocked { get; set; }
        public ElementType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}