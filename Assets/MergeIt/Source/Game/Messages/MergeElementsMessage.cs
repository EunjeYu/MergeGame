// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;

namespace MergeIt.Game.Messages
{
    public class MergeElementsMessage : IMessage
    {
        public GridPoint From { get; set; }
        public IFieldElement NewElement { get; set; }
    }

}