// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class RemoveFromInventoryMessage : IMessage
    {
        public IFieldElement FieldElement { get; set; }
    }
}