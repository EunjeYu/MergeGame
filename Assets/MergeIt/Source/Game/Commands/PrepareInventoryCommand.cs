// Copyright (c) 2024, Awessets

using MergeIt.Core.Commands;
using MergeIt.Core.Services;
using MergeIt.Game.Factories.Inventory;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Commands
{
    public class PrepareInventoryCommand : Command
    {
        private readonly IGameLoadService _gameLoadService = DiContainer.Get<IGameLoadService>();
        private readonly IInventoryService _inventoryService = DiContainer.Get<IInventoryService>();

        public override void Execute()
        {
            var inventoryData = _gameLoadService.Load<InventoryData>();

            if (inventoryData == null)
            {
                _inventoryService.CreateInventory();
            }
            else
            {
                _inventoryService.SetupInventory(inventoryData);
            }
        }
    }
}