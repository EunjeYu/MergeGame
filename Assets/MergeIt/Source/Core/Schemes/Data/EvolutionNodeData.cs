// Copyright (c) 2024, Awessets

using System;
using UnityEngine;

namespace MergeIt.Core.Schemes.Data
{
    [Serializable]
    public class EvolutionNodeData : IEvolutionNodeData
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private bool _discovered;
        [SerializeField] private Rect _position;
        
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    _id = Guid.NewGuid().ToString();
                }

                return _id;
            }
            set => _id = value;
        }

        public bool Discovered
        {
            get => _discovered;
            set => _discovered = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }
        
        public Rect Position
        {
            get => _position;
            set => _position = value;
        }
        
        public T Copy<T>() where T : IEvolutionNodeData, new()
        {
            var copiedData = new T
            {
                Position = Position,
                Discovered = Discovered,
                Name = Name,
                Description = Description
            };

            return copiedData;
        }
    }
}