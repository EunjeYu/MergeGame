// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Helpers;
using MergeIt.Core.Messages;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Converters;
using MergeIt.Game.ElementsStock;
using MergeIt.Game.Evolutions;
using MergeIt.Game.Factories.Inventory;
using MergeIt.Game.Field;
using MergeIt.Game.Messages;
using MergeIt.Game.Services.Saves.Strategies;
using MergeIt.Game.User;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;
using UnityEditor;
using UnityEngine;

namespace MergeIt.Game.Services.Saves
{
    public class GameSaveEveryIntervalService : IGameSaveService, IInitializable, IDisposable, IUpdatable, IMonoApplicationQuitHandler
    {
        private readonly HashSet<GameSaveType> _saveQueue = new();
        private bool _canSave;
        private float _time;
        private float _interval;

        [Introduce]
        private IConfigProcessor _configProcessor;

        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IEvolutionsService _evolutionsService;

        [Introduce]
        private FieldLogicModel _fieldLogicModel;

        [Introduce]
        private GameServiceModel _gameServiceModel;

        [Introduce]
        private IInventoryService _inventoryService;

        [Introduce]
        private IMessageBus _messageBus;

        [Introduce]
        private IElementsStockService _stockService;
        
        [Introduce]
        private UserServiceModel _userServiceModel;
        
        [Introduce]
        private ISerializeStrategy _serializeStrategy;

        public async UniTask Save(GameSaveType gameSaveType)
        {
            _saveQueue.Add(gameSaveType);

            await UniTask.Yield();
        }

        public void Initialize()
        {
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            MonoEventsListener.Instance.SubscribeOnApplicationQuit(this);
        }
        
        public void Dispose()
        {
            _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            MonoEventsListener.Instance.UnsubscribeFromApplicationQuit(this);
        }
        
        public async void OnApplicationQuit()
        {
            await TrySave(GameSaveType.All);
        }

        public async void Update()
        {
            if (_canSave)
            {
                _time += Time.unscaledDeltaTime;

                if (_time >= _interval)
                {
                    _canSave = false;
                    await TrySave();

                    _time = 0f;
                    _canSave = true;
                }
            }
        }

        private async void OnLoadedGameMessageHandler(LoadedGameMessage _)
        {
            _interval = _configsService.GameConfig.SaveInterval;
            _canSave = true;

            await TrySave();
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

        private async UniTask TrySave()
        {
            if (_saveQueue.Count > 0)
            {
                var saves = _saveQueue.Distinct().ToArray();
                foreach (GameSaveType saveType in saves)
                {
                    _saveQueue.Remove(saveType);
                    await TrySave(saveType);
                }
            }
        }

        private async UniTask TrySave(GameSaveType gameSaveType)
        {
            if ((gameSaveType & GameSaveType.Field) == GameSaveType.Field)
            {
                await SaveLevel();
            }

            if ((gameSaveType & GameSaveType.User) == GameSaveType.User)
            {
                await SaveUser();
            }

            if ((gameSaveType & GameSaveType.Inventory) == GameSaveType.Inventory)
            {
                await SaveInventory();
            }

            if ((gameSaveType & GameSaveType.Stock) == GameSaveType.Stock)
            {
                await SaveStock();
            }

            if ((gameSaveType & GameSaveType.EvolutionsProgress) == GameSaveType.EvolutionsProgress)
            {
                await SaveEvolutionsProgress();
            }
        }
    }
}