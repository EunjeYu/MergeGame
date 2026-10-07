// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Configs.Hints
{
    [CreateAssetMenu(fileName = "HintsConfig", menuName = "Merge Toolkit/Hints Config")]
    public class HintsConfig : ScriptableObject
    {
        [SerializeField]
        private float _interval;

        public float Interval
        {
            get => _interval;
        }
    }
}