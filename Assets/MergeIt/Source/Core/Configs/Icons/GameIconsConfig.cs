// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Configs.Icons
{
    [CreateAssetMenu(fileName = "GameIconsConfig", menuName = "Merge Toolkit/Game Icons Config")]
    public class GameIconsConfig : ScriptableObject
    {
        [SerializeField]
        private IconParameters[] _infoPanelCurrencyIcons;

        public IconParameters[] InfoPanelCurrencyIcons
        {
            get => _infoPanelCurrencyIcons;
        }
    }
}