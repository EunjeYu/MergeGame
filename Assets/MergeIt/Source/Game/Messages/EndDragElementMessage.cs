// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using UnityEngine;

namespace MergeIt.Game.Messages
{
    public class EndDragElementMessage : IMessage
    {
        public GridPoint FromPoint { get; set; }
        public GameObject ToGameObject { get; set; }
        public Vector2 Position { get; set; }
    }
}