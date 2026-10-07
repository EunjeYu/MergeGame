// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Commands;
using MergeIt.Core.Configs;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Converters;
using MergeIt.Game.Factories.FieldElement;
using MergeIt.Game.Field;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Commands
{
    public class PrepareFieldCommand : Command
    {
        private readonly IGameLoadService _gameLoadService = DiContainer.Get<IGameLoadService>();
        private readonly IFieldElementFactory _fieldElementFactory = DiContainer.Get<IFieldElementFactory>();
        private readonly IGameSaveService _saveService = DiContainer.Get<IGameSaveService>();
        private readonly IConfigProcessor _configProcessor = DiContainer.Get<IConfigProcessor>();
        private readonly GameServiceModel _gameServiceModel = DiContainer.Get<GameServiceModel>();
        private readonly FieldLogicModel _fieldLogicModel = DiContainer.Get<FieldLogicModel>();
        private readonly IConfigsService _configsService = DiContainer.Get<IConfigsService>();

        public override void Execute()
        {
            var loadedLevel = _gameLoadService.Load<FieldData>();
            
            if (loadedLevel != null)
            {
                _fieldLogicModel.FieldHeight = loadedLevel.FieldHeight;
                _fieldLogicModel.FieldWidth = loadedLevel.FieldWidth;
                
                Dictionary<GridPoint, IFieldElement> fieldElements = _fieldLogicModel.FieldElements;

                foreach (FieldElementData fieldElementData in loadedLevel.SavedElementsData)
                {
                    IFieldElement fieldElement = _fieldElementFactory.CreateFieldElement(fieldElementData);

                    fieldElements.Add(fieldElement.InfoParameters.LogicPosition, fieldElement);
                }
            }
            else
            {
                LevelConfig levelConfig = _configsService.LevelConfig;
                _fieldLogicModel.FieldHeight = levelConfig.FieldHeight;
                _fieldLogicModel.FieldWidth = levelConfig.FieldWidth;
                
                Dictionary<GridPoint, IFieldElement> fieldElements = _fieldLogicModel.FieldElements;

                foreach (LevelElementData fieldElementData in levelConfig.FieldElementsData)
                {
                    IFieldElement fieldElement = _fieldElementFactory.CreateFieldElement(fieldElementData);

                    fieldElements.Add(fieldElement.InfoParameters.LogicPosition, fieldElement);
                }

                loadedLevel = _configProcessor.BuildLevel();
            }

            _gameServiceModel.LoadedLevel = loadedLevel;
            _saveService.Save(GameSaveType.Field);
        }
    }
}