// Copyright (c) 2024, Awessets

using System;
using Cysharp.Threading.Tasks;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Helpers;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Converters;
using MergeIt.Game.ElementsStock;
using MergeIt.Game.Evolutions;
using MergeIt.Game.Factories.Inventory;
using MergeIt.Game.Field;
using MergeIt.Game.Services.Saves.Strategies;
using MergeIt.Game.User;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;
using UnityEditor;
using UnityEngine;

namespace MergeIt.Game.Services.Saves
{
    public class GameSaveOnQuitService : IGameSaveService, IInitializable, IDisposable, IMonoApplicationQuitHandler
    {
        [Introduce]
        private IConfigProcessor _configProcessor;

        [Introduce]
        private IEvolutionsService _evolutionsService;

        [Introduce]
        private FieldLogicModel _fieldLogicModel;

        [Introduce]
        private GameServiceModel _gameServiceModel;

        [Introduce]
        private IInventoryService _inventoryService;

        [Introduce]
        private IElementsStockService _stockService;

        [Introduce]
        private UserServiceModel _userServiceModel;
        
        [Introduce]
        private ISerializeStrategy _serializeStrategy;

        public void Dispose()
        {
            MonoEventsListener.Instance.UnsubscribeFromApplicationQuit(this);
        }

        public async UniTask Save(GameSaveType gameSaveType)
        {
            if (gameSaveType != GameSaveType.All)
            {
                Debug.Log("This save strategy works only on application quit.");

                return;
            }

            await SaveLevel();

            await SaveUser();

            await SaveInventory();

            await SaveStock();

            await SaveEvolutionsProgress();
        }

        public void Initialize()
        {
            MonoEventsListener.Instance.SubscribeOnApplicationQuit(this);
        }

        public async void OnApplicationQuit()
        {
            await Save(GameSaveType.All);
        }

        private async UniTask SaveLevel()
        {
            FieldData loadedLevel = _gameServiceModel.LoadedLevel;
            loadedLevel.SavedElementsData.Clear();

            foreach (var fieldElement in _fieldLogicModel.FieldElements)
            {
                FieldElementData savedElement = _configProcessor.ConvertToFieldElementData(fieldElement.Value);
                loadedLevel.SavedElementsData.Add(savedElement);
            }

            await SaveAsync(loadedLevel);
        }

        private async UniTask SaveUser()
        {
            UserData userData = _userServiceModel.GetData();

            await SaveAsync(userData);
        }

        private async UniTask SaveStock()
        {
            var stockData = _stockService.GetData() as ElementsStockData;

            await SaveAsync(stockData);
        }

        private async UniTask SaveInventory()
        {
            var inventoryData = _inventoryService.GetData() as InventoryData;

            await SaveAsync(inventoryData);
        }

        private async UniTask SaveEvolutionsProgress()
        {
            var evolutionsProgressData = _evolutionsService.GetData() as EvolutionsProgressData;

            await SaveAsync(evolutionsProgressData);
        }

        private async UniTask SaveAsync<T>(T data) where T : class, ISavable
        {
            await _serializeStrategy.Save(data);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}