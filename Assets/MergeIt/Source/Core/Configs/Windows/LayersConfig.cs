// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Configs.Windows
{
    [CreateAssetMenu(fileName = "LayersConfig", menuName = "Merge Toolkit/Windows/Layers config")]
    public class LayersConfig : ScriptableObject
    {
        [SerializeField]
        private LayerParameters[] _layers;

        public LayerParameters[] Layers
        {
            get => _layers;
        }
    }
}