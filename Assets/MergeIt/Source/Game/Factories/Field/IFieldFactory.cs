// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Game.Field;
using UnityEngine;

namespace MergeIt.Game.Factories.Field
{
    public interface IFieldFactory
    {
        FieldPresenter CreateField(Transform parent);
        FieldCellComponent CreateFieldCell(Transform parent, GridPoint gridPoint);
    }
}