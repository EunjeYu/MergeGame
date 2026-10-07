// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;
using MergeIt.Game.Field;

namespace MergeIt.Game.Messages
{
    public class ClickElementMessage : IMessage
    {
        public FieldCellComponent Cell { get; set; }
    }
}