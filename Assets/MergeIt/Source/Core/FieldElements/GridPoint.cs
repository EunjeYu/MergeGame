// Copyright (c) 2024, Awessets

using System;
using System.Diagnostics;
using UnityEngine;

namespace MergeIt.Core.FieldElements
{
    [Serializable, DebuggerDisplay("{ToString()}")]
    public struct GridPoint : IEquatable<GridPoint>, IComparable<GridPoint>
    {
        public static bool operator ==(GridPoint p1, GridPoint p2)
        {
            return p1.Equals(p2);
        }
        
        public static bool operator !=(GridPoint p1, GridPoint p2)
        {
            return !p1.Equals(p2);
        }
        
        public static readonly GridPoint Default = new GridPoint(-1, -1);
        
        [SerializeField]
        private int _x;
        
        [SerializeField]
        private int _y;
        
        public readonly int X
        {
            get => _x;
        }

        public readonly int Y
        {
            get => _y;
        }

        public GridPoint(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public GridPoint Copy()
        {
            return new GridPoint(_x, _y);
        }
        
        public static GridPoint Create(int row, int column)
        {
            return new GridPoint(row, column);
        }
        
        public bool Equals(GridPoint other)
        {
            return _x == other._x && _y == other._y;
        }
        
        public int CompareTo(GridPoint other)
        {
            int xComparison = _x.CompareTo(other._x);
            if (xComparison != 0) return xComparison;
            return _y.CompareTo(other._y);
        }
        
        public override bool Equals(object obj)
        {
            return obj is GridPoint other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _x * 397 ^ _y;
            }
        }

        public override string ToString()
        {
            return $"(Row: {_x}, Column: {_y})";
        }
    }
}