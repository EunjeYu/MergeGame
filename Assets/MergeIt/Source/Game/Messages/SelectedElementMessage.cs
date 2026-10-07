// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class SelectedElementMessage : IMessage
    {
        public GridPoint Point { get; set; }
    }
}