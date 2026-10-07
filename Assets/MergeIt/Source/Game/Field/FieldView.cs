// Copyright (c) 2024, Awessets

using MergeIt.Core.MVP;
using UnityEngine;

namespace MergeIt.Game.Field
{
    public class FieldView : View
    {
        [SerializeField]
        private RectTransform _cellsContainer;
        
        [SerializeField]
        private float _cellSpacing;

        public RectTransform CellsContainer => _cellsContainer;
        public float CellSpacing => _cellSpacing;
    }
}