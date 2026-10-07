// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class GeneratorOpenedMessage : IMessage
    {
        public GridPoint GeneratorPoint { get; set; }
    }
}