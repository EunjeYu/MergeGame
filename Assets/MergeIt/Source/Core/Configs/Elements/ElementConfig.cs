// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Types;
using UnityEngine;

namespace MergeIt.Core.Configs.Elements
{
    [CreateAssetMenu(fileName = "ElementConfig", menuName = "Merge Toolkit/Element")]
    public class ElementConfig : ScriptableObject
    {
        [SerializeField]
        private ElementType _type;
        
        [SerializeField]
        private ElementCommonSettings _commonSettings;
        
        [SerializeField]
        private ElementGeneratorSettings _generatorSettings;
        
        [SerializeField]
        private string _id;

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
        }
        
        public ElementType Type
        {
            get => _type;
        }

        public ElementCommonSettings CommonSettings
        {
            get => _commonSettings;
            set => _commonSettings = value;
        }
        
        public ElementGeneratorSettings GeneratorSettings
        {
            get => _generatorSettings;
            set => _generatorSettings = value;
        }

        public FieldElementIconComponent GetIconComponent()
        {
            return _commonSettings?.Icon;
        }

        protected void GenerateGuid()
        {
            _id = Guid.NewGuid().ToString();
        }
    }
}