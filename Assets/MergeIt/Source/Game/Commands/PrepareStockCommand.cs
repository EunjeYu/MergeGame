// Copyright (c) 2024, Awessets

using MergeIt.Core.Commands;
using MergeIt.Core.Services;
using MergeIt.Game.ElementsStock;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Commands
{
    public class PrepareStockCommand : Command
    {
        private readonly IGameLoadService _gameLoadService = DiContainer.Get<IGameLoadService>();
        private readonly IElementsStockService _stockService = DiContainer.Get<IElementsStockService>();

        public override void Execute()
        {
            var stockData = _gameLoadService.Load<ElementsStockData>();

            if (stockData == null)
            {
                _stockService.CreateStock();
            }
            else
            {
                _stockService.SetupStock(stockData);
            }
        }
    }
}