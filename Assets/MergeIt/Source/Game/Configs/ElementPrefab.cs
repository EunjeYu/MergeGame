// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Types;
using MergeIt.Game.Field.Elements;
using UnityEngine;

namespace MergeIt.Game.Configs
{
    [Serializable]
    public class ElementPrefab
    {
        [SerializeField]
        private ElementType _type;
        
        [SerializeField]
        private FieldElementView _prefab;

        public ElementType Type
        {
            get => _type;
        }

        public FieldElementView Prefab
        {
            get => _prefab;
        }
    }
}