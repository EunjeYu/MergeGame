// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class SwapElementsMessage : IMessage
    {
        public GridPoint From { get; set; }
        public GridPoint To { get; set; }
    }
}