// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
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
using UnityEngine;

namespace MergeIt.Core.Services
{
    public interface IConfigsService
    {
        UniTask Load();
        LevelConfig LevelConfig { get; }
        GameConfig GameConfig { get; }
        InventoryConfig InventoryConfig { get; }
        HintsConfig HintsConfig { get; }
        Sprite GetCurrencyIcon(CurrencyType type);
        EvolutionData GetEvolutionData(string evolutionId);
        string GetEvolutionIdByElement(ElementConfig element);
        EvolutionData GetEvolutionByElement(ElementConfig element);
        T GetEffectConfig<T>(string type) where T : EffectConfig;
        LevelUpParameters GetLevelUpData(int currentLevel);
        IFieldElementView GetElementPrefab(ElementType type);
        IEnumerable<ElementConfig> GetConfigs(Func<ElementConfig, bool> predicate = null);
        ElementConfig GetConfig(string id);
    }
}