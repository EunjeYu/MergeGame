// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class SplitElementMessage : IMessage
    {
        public IFieldElement SplitElement1 { get; set; }
        public IFieldElement SplitElement2 { get; set; }
    }
}