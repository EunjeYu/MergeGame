// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class SkipTimeMessage : IMessage
    {
        public int Seconds { get; set; }
    }
}