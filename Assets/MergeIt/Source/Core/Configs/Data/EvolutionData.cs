// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using MergeIt.Core.Configs.Elements;
using UnityEngine;

namespace MergeIt.Core.Configs.Data
{
    [Serializable]
    public class EvolutionData : IEquatable<EvolutionData>
    {
        [SerializeField] private string _guid;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private bool _discovered;
        [SerializeField] private List<ElementConfig> _chain;
        private IEquatable<EvolutionData> _equatableImplementation;

        public EvolutionData(string guid, string name, string description, bool discovered)
        {
            _guid = guid;
            _name = name;
            _description = description;
            _discovered = discovered;
            _chain = new List<ElementConfig>();
        }

        public string Name
        {
            get => _name;
        }

        public string Description
        {
            get => _description;
        }

        public bool Discovered
        {
            get => _discovered;
        }
        
        public List<ElementConfig> Chain
        {
            get => _chain;
        }

        public string Id
        {
            get => _guid;
        }

        public bool Equals(EvolutionData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _guid == other._guid;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EvolutionData)obj);
        }
        
        public override int GetHashCode()
        {
            return (_guid != null ? _guid.GetHashCode() : 0);
        }
    }
}