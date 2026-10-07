// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Configs;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Effects;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Hints;
using MergeIt.Core.Configs.Inventory;
using MergeIt.Core.Configs.LevelUp;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Services;
using MergeIt.Game.Field.Elements;
using UnityEngine;

namespace MergeIt.Game.Configs.Services
{
    public class ConfigsService : IConfigsService
    {
        private readonly ConfigsServiceModel _model = new();

        private ConfigsContainer _configsContainer;

        public LevelConfig LevelConfig
        {
            get => _model.LevelConfig;
        }

        public InventoryConfig InventoryConfig
        {
            get => _model.InventoryConfig;
        }

        public GameConfig GameConfig
        {
            get => _model.GameConfig;
        }

        public HintsConfig HintsConfig
        {
            get => _model.HintsConfig;
        }

        public async UniTask Load()
        {
            var requestResult = await Resources.LoadAsync<ConfigsContainer>(ConfigsConstants.ConfigsPath);

            _configsContainer = requestResult as ConfigsContainer;

            if (_configsContainer)
            {
                _model.StoreElementsPrefabs(_configsContainer.Prefabs);
                _model.StoreGameConfig(_configsContainer.GameConfig);
                _model.StoreIconsConfig(_configsContainer.IconsConfig);
                _model.StoreInventoryConfig(_configsContainer.InventoryConfig);
                _model.StoreLevelConfig(_configsContainer.LevelConfig);
                _model.StoreLevelUpConfig(_configsContainer.LevelUpConfig);
                _model.StoreEffectsConfigs(_configsContainer.EffectConfigs);
                _model.StoreHintsConfig(_configsContainer.HintsConfig);
            }
            else
            {
                Debug.Log("Something went wrong while loading configs container.");
            }
        }
        

        public Sprite GetCurrencyIcon(CurrencyType type)
        {
            Sprite icon = _model.IconsConfig.InfoPanelCurrencyIcons.FirstOrDefault(x => x.CurrencyType == type)?.CurrencyIcon;

            return icon;
        }

        public EvolutionData GetEvolutionData(string evolutionId)
        {
            return _model.LevelConfig.EvolutionsScheme.Evolution.FirstOrDefault(x => x.Id == evolutionId);
        }

        public string GetEvolutionIdByElement(ElementConfig element)
        {
            return GetEvolutionByElement(element)?.Id;
        }

        public EvolutionData GetEvolutionByElement(ElementConfig element)
        {
            return _model.LevelConfig.EvolutionsScheme.Evolution.FirstOrDefault(x => x.Chain.Contains(element));
        }

        public T GetEffectConfig<T>(string type) where T : EffectConfig
        {
            _model.EffectConfigs.TryGetValue(type, out EffectConfig effectConfig);

            return effectConfig as T;
        }

        public LevelUpParameters GetLevelUpData(int currentLevel)
        {
            return _model.LevelUpConfig.LevelUp[currentLevel - 1];
        }

        public IFieldElementView GetElementPrefab(ElementType type)
        {
            _model.Prefabs.TryGetValue(type, out FieldElementView view);

            return view;
        }
        
        public IEnumerable<ElementConfig> GetConfigs(Func<ElementConfig, bool> predicate = null)
        {
            if (predicate != null)
            {
                return _model.ElementConfigs.Where(predicate);
            }

            return _model.ElementConfigs;
        }
        
        public ElementConfig GetConfig(string id)
        {
            return _model.ElementConfigs.FirstOrDefault(x => x.Id == id);
        }
    }
}