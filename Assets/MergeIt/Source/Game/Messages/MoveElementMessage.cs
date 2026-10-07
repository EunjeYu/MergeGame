// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class MoveElementMessage : IMessage
    {
        public IFieldElementView FieldElementView { get; set; }
        public GridPoint FromPoint { get; set; }
        public GridPoint ToPoint { get; set; }
    }
}