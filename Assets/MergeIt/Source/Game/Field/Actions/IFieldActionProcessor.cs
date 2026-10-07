// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using UnityEngine;

namespace MergeIt.Game.Field.Actions
{
    public interface IFieldActionProcessor
    {
        void ProcessClick(FieldCellComponent cellComponent);
        void ProcessEndDrag(GridPoint fromPoint, GameObject toGameObject);
    }
}