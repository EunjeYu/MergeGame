// Copyright (c) 2024, Awessets

using System;
using UnityEngine;

namespace MergeIt.Core.Configs.Windows
{
    [Serializable]
    public class LayerParameters : IEquatable<LayerParameters>, IComparable<LayerParameters>
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private int _order;

        public string Name
        {
            get => _name;
        }

        public int Order
        {
            get => _order;
        }

        public bool Equals(LayerParameters other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _name == other._name && _order == other._order;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LayerParameters)obj);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(_name, _order);
        }
        
        public int CompareTo(LayerParameters other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return _order.CompareTo(other._order);
        }
    }
}