// Copyright (c) 2024, Awessets

using System.Linq;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.ElementsStock;
using MergeIt.Core.Messages;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.ElementsStock;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Services
{
    public class ElementsStockService : IElementsStockService
    {
        [Introduce]
        private IConfigsService _configsService;

        private ElementConfig _currentElement;

        [Introduce]
        private IMessageBus _messageBus;

        [Introduce]
        private IGameSaveService _saveService;

        [Introduce]
        private ElementsStockServiceModel _serviceModel;

        public void CreateStock()
        {
            var stockData = new ElementsStockData();
            SetupStock(stockData);

            _saveService.Save(GameSaveType.Stock);
        }

        public void SetupStock(IElementsStockData stockData)
        {
            SetModel(stockData);
        }

        public void Add(ElementConfig elementConfig)
        {
            bool stockIsEmpty = !_serviceModel.Elements.Any();
            _serviceModel.Elements.Enqueue(elementConfig);

            if (stockIsEmpty)
            {
                _currentElement = elementConfig;
            }

            _saveService.Save(GameSaveType.Stock);
        }

        public bool Remove()
        {
            return _serviceModel.Elements.TryDequeue(out _);
        }

        public ElementConfig GetNext()
        {
            _serviceModel.Elements.TryPeek(out ElementConfig config);
            _currentElement = config;
            return _currentElement;
        }

        public ElementConfig GetCurrent()
        {
            return _currentElement;
        }

        public IElementsStockData GetData()
        {
            var data = new ElementsStockData();

            if (_serviceModel.Elements.Count > 0)
            {
                data.Elements = _serviceModel.Elements.Select(x => x.Id).ToArray();
            }

            return data;
        }
        
        private void SetModel(IElementsStockData data)
        {
            if (data?.Elements != null)
            {
                foreach (string configId in data.Elements)
                {
                    ElementConfig config = _configsService.GetConfig(configId);
                    _serviceModel.Elements.Enqueue(config);
                }
            }
        }
    }
}