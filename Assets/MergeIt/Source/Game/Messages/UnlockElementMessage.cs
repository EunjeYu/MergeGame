// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class UnlockElementMessage : IMessage
    {
        public IFieldElement Element { get; set; }
    }
}