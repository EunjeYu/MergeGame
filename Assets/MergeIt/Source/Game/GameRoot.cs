// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;
using MergeIt.Core.WindowSystem;
using MergeIt.Core.WindowSystem.Factory;
using MergeIt.Game.Configs.Services;
using MergeIt.Game.Converters;
using MergeIt.Game.Effects;
using MergeIt.Game.Factories.ElementInfo;
using MergeIt.Game.Factories.Field;
using MergeIt.Game.Factories.FieldElement;
using MergeIt.Game.Factories.Icons;
using MergeIt.Game.Factories.Inventory;
using MergeIt.Game.Field;
using MergeIt.Game.Managers;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.Game.Services.Saves;
using MergeIt.Game.Services.Saves.Strategies;
using MergeIt.Game.Windows.ElementInfo;
using MergeIt.Game.Windows.Inventory;
using MergeIt.Game.Windows.UserProgress;
using MergeIt.SimpleDI;

namespace MergeIt.Game
{
    public class GameRoot : DiRoot
    {
        protected override void OnInstall()
        {
            DiContainer.RegisterInterfacesFor<MessageBus>().AsSingleton();

            DiContainer.RegisterInterfacesFor<GameService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<GameFieldService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<GameFieldActionsService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<GameSaveEveryIntervalService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<UserService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<UserProgressService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<InventoryService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<EnergyService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<GeneratorService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<InfoPanelService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<CurrencyService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<ElementService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<ElementsStockService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<EvolutionsService>().AsSingleton();

            DiContainer.RegisterInterfacesFor<FieldLogic>().AsSingleton();

            DiContainer.RegisterInterfacesFor<EffectsFactory>().AsSingleton();
            DiContainer.RegisterInterfacesFor<EffectsManager>().AsSingleton();

            DiContainer.RegisterInterfacesFor<ConfigsService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<ConfigProcessor>().AsSingleton();
            DiContainer.RegisterInterfacesFor<GameLoadService>().AsSingleton();
            DiContainer.RegisterInterfacesFor<ResourcesLoaderService>().AsSingleton();

            DiContainer.RegisterInterfacesFor<FieldElementVisualFactory>().AsSingleton();
            DiContainer.RegisterInterfacesFor<FieldElementFactory>().AsSingleton();
            DiContainer.RegisterInterfacesFor<FieldFactory>().AsSingleton();
            DiContainer.RegisterInterfacesFor<InventoryFactory>().AsSingleton();
            DiContainer.RegisterInterfacesFor<ElementInfoFactory>().AsSingleton();
            DiContainer.RegisterInterfacesFor<IconFactory>().AsSingleton();
            
            DiContainer.RegisterInterfacesFor<HintsManager>().AsSingleton();

            DiContainer.Register<FieldLogicModel>().AsSingleton();
            DiContainer.Register<GameServiceModel>().AsSingleton();
            DiContainer.Register<UserServiceModel>().AsSingleton();
            DiContainer.Register<InventoryServiceModel>().AsSingleton();
            DiContainer.Register<ElementsStockServiceModel>().AsSingleton();

            DiContainer.RegisterInterfacesFor<WindowsSystem>().AsSingleton();
            DiContainer.RegisterInterfacesFor<WindowFactory>().AsSingleton();
            
            DiContainer.RegisterInterfacesFor<JsonSerializeStrategy>().AsSingleton();
        }

        protected override void Run()
        {
            RegisterWindows();

            var messageBus = DiContainer.Get<IMessageBus>();
            messageBus.Fire<StartGameMessage>();
        }

        private void RegisterWindows()
        {
            WindowsStorage.Register<InventoryPresenter, InventoryWindow>(WindowName.Inventory, SortingLayers.Main);
            WindowsStorage.Register<UserInfoPresenter, UserInfoWindow>(WindowName.UserInfo, SortingLayers.Main);
            WindowsStorage.Register<ElementInfoPresenter, ElementInfoWindow>(WindowName.ElementInfo, SortingLayers.Popup);
            WindowsStorage.Register<ElementInfoPresenter, ElementInfoWindow>(WindowName.ElementInfo, SortingLayers.Popup);
        }

        private void OnDestroy()
        {
            DiContainer.Clear();
        }
    }
}