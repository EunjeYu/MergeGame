// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Configs;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Effects;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Hints;
using MergeIt.Core.Configs.Icons;
using MergeIt.Core.Configs.Inventory;
using MergeIt.Core.Configs.LevelUp;
using MergeIt.Core.Configs.Types;
using MergeIt.Game.Field.Elements;

namespace MergeIt.Game.Configs.Services
{
    public class ConfigsServiceModel
    {
        private GameConfig _gameConfig;
        private GameIconsConfig _iconsConfig;
        private InventoryConfig _inventoryConfig;
        private LevelConfig _levelConfig;
        private LevelUpConfig _levelUpConfig;
        private Dictionary<string, EffectConfig> _effectConfigs;
        private Dictionary<ElementType, FieldElementView> _elementsPrefabs;
        private List<ElementConfig> _elementConfigs = new();
        private HintsConfig _hintsConfig;
        
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

        public Dictionary<string, EffectConfig> EffectConfigs
        {
            get => _effectConfigs;
        }

        public List<ElementConfig> ElementConfigs
        {
            get => _elementConfigs;
        }

        public Dictionary<ElementType, FieldElementView> Prefabs
        {
            get => _elementsPrefabs;
        }
        
        public HintsConfig HintsConfig
        {
            get => _hintsConfig;
        }

        public void StoreElementsPrefabs(ElementPrefab[] prefabs)
        {
            _elementsPrefabs = prefabs.ToDictionary(key => key.Type, value => value.Prefab);
        }

        public void StoreGameConfig(GameConfig config)
        {
            _gameConfig = config;
        }

        public void StoreIconsConfig(GameIconsConfig config)
        {
            _iconsConfig = config;
        }

        public void StoreInventoryConfig(InventoryConfig config)
        {
            _inventoryConfig = config;
        }

        public void StoreLevelConfig(LevelConfig config)
        {
            _levelConfig = config;

            ExtractElementsConfigs(_levelConfig);
        }

        public void StoreLevelUpConfig(LevelUpConfig config)
        {
            _levelUpConfig = config;
        }

        public void StoreEffectsConfigs(EffectConfig[] configs)
        {
            _effectConfigs = configs.ToDictionary(config => config.Name, config => config);
        }

        private void ExtractElementsConfigs(LevelConfig levelConfig)
        {
            List<EvolutionData> evolutionScheme = levelConfig.EvolutionsScheme.Evolution;

            for (int i = 0; i < evolutionScheme.Count; i++)
            {
                EvolutionData evolutionData = evolutionScheme[i];

                foreach (ElementConfig elementConfig in evolutionData.Chain)
                {
                    _elementConfigs.Add(elementConfig);
                }
            }
        }
        
        public void StoreHintsConfig(HintsConfig hintsConfig)
        {
            _hintsConfig = hintsConfig;
        }
    }
}