// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs;
using MergeIt.Core.Configs.Effects;
using MergeIt.Core.Configs.Hints;
using MergeIt.Core.Configs.Icons;
using MergeIt.Core.Configs.Inventory;
using MergeIt.Core.Configs.LevelUp;
using UnityEngine;

namespace MergeIt.Game.Configs
{
    [CreateAssetMenu(fileName = "ConfigsContainer", menuName = "Merge Toolkit/Configs container")]
    public class ConfigsContainer : ScriptableObject
    {
        [SerializeField]
        private ElementPrefab[] _prefabs;
        
        [SerializeField]
        private GameConfig _gameConfig;
        
        [SerializeField]
        private GameIconsConfig _iconsConfig;
        
        [SerializeField]
        private InventoryConfig _inventoryConfig;
        
        [SerializeField]
        private LevelConfig _levelConfig;
        
        [SerializeField]
        private LevelUpConfig _levelUpConfig;
        
        [SerializeField]
        private HintsConfig _hintsConfig;

        [SerializeField]
        private EffectConfig[] _effectsConfigs;
        
        public ElementPrefab[] Prefabs
        {
            get => _prefabs;
        }
        
        public GameConfig GameConfig
        {
            get => _gameConfig;
        }
        
        public GameIconsConfig IconsConfig
        {
            get => _iconsConfig;
        }
        
        public InventoryConfig InventoryConfig
        {
            get => _inventoryConfig;
        }
        
        public LevelConfig LevelConfig
        {
            get => _levelConfig;
        }
        
        public LevelUpConfig LevelUpConfig
        {
            get => _levelUpConfig;
        }
        
        public EffectConfig[] EffectConfigs
        {
            get => _effectsConfigs;
        }
        
        public HintsConfig HintsConfig
        {
            get => _hintsConfig;
        }
    }
}