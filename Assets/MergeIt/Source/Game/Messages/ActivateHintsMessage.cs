// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class ActivateHintsMessage : IMessage
    {
        public bool Active { get; set; }
    }
}