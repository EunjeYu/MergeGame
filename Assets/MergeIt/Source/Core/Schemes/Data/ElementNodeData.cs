// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Elements;
using UnityEngine;

namespace MergeIt.Core.Schemes.Data
{
    [Serializable]
    public class ElementNodeData : IElementNodeData
    {
        [SerializeField] private string _id = Guid.NewGuid().ToString();
        [SerializeField] private ElementConfig _elementConfig;
        [SerializeField] private Rect _position;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public ElementConfig ElementConfig
        {
            get => _elementConfig;
            set => _elementConfig = value;
        }

        public Rect Position
        {
            get => _position;
            set => _position = value;
        }
        
        public T Copy<T>() where T : IElementNodeData, new()
        {
            var copiedData = new T
            {
                Position = Position,
                ElementConfig = ElementConfig
            };

            return copiedData;
        }
    }

}