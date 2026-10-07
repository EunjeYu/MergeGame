// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;
using MergeIt.Game.ElementsStock;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.TopPanel
{
    public class TopPanelComponent : MonoBehaviour
    {
        [SerializeField]
        private ElementsStockComponent _stockComponent;

        private IMessageBus _messageBus;
        
        private void Start()
        {
            _messageBus = DiContainer.Get<IMessageBus>();
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
        }

        private void OnDestroy()
        {
            _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
        }
        
        private void OnLoadedGameMessageHandler(LoadedGameMessage _)
        {
            _stockComponent.Initialize();
        }
    }
}