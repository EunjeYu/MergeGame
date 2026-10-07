// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Game.UI.InfoPanel;

namespace MergeIt.Game.Messages
{
    public class ElementActionMessage : IMessage
    {
        public IFieldElement Element { get; set; }
        public ElementActionType ActionType { get; set; }
    }
}