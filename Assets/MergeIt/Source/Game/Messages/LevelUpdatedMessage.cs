// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class LevelUpdatedMessage : IMessage
    {
        public int NextLevelExp { get; set; }
    }
}