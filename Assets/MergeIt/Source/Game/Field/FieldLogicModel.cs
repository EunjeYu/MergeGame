// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.FieldElements;
using UnityEngine;

namespace MergeIt.Game.Field
{
    public class FieldLogicModel
    {
        public Dictionary<GridPoint, FieldCellComponent> CellComponents { get; } = new();
        public Dictionary<GridPoint, IFieldElement> FieldElements { get; } = new();
        public IFieldElement OpeningGenerator { get; set; }
        public List<IFieldElement> AllGenerators { get; set; } = new();
        public int FieldWidth { get; set; }
        public int FieldHeight { get; set; }
        public float CellSize { get; set; }
        public Rect FieldRect { get; set; }
        public float PositionOffset { get; set; }
        public float CellSpacing { get; set; }
        
        public Vector3 GetPosition(GridPoint fromPoint)
        {
            if (CellComponents.TryGetValue(fromPoint, out FieldCellComponent cell))
            {
                return cell.ObjectContainer.position;
            }
            
            return Vector3.zero;
        }
    }
}