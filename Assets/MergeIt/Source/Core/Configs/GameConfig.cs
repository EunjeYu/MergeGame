// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Merge Toolkit/Game config")]
    public class GameConfig : ScriptableObject
    {
        [Tooltip("Max count of rechargeable energy.")]
        public int EnergyCap;

        [Tooltip("Required time to restore 1 energy point.")]
        public int EnergyRestoreTime;
        public int InitialSoftCurrency;
        public int InitialHardCurrency;
        public int InitialSplittersCount;

        [Tooltip("Name of element's generation level, that would be displayed.")]
        public string LevelString;
        [Tooltip("Used in case you use a saving strategy that performs game save every n seconds")]
        public float SaveInterval = 10;
    }
}