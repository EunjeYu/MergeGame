// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using UnityEngine;

namespace MergeIt.Game.Messages
{
    public class CreateElementMessage : IMessage
    {
        public IFieldElement NewElement { get; set; }
        public Vector3? FromPosition { get; set; }
        public GridPoint ToPoint { get; set; }
    }

}