// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Commands;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Evolutions;
using MergeIt.Game.Field;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Commands
{
    public class CheckEvolutionsProgressCommand : Command
    {
        private readonly IGameLoadService _gameLoadService = DiContainer.Get<IGameLoadService>();
        private readonly IGameSaveService _gameSaveService = DiContainer.Get<IGameSaveService>();
        private readonly IEvolutionsService _evolutionsService = DiContainer.Get<IEvolutionsService>();
        private readonly FieldLogicModel _fieldLogicModel = DiContainer.Get<FieldLogicModel>();

        public override void Execute()
        {
            var evolutionsProgressData = _gameLoadService.Load<EvolutionsProgressData>();

            if (evolutionsProgressData == null)
            {
                _evolutionsService.CreateEvolutionsProgress();
            }
            else
            {
                _evolutionsService.SetupEvolutionsProgress(evolutionsProgressData);
            }

            UpdateProgress();
        }

        private void UpdateProgress()
        {
            Dictionary<GridPoint, IFieldElement> fieldElements = _fieldLogicModel.FieldElements;
            foreach (KeyValuePair<GridPoint, IFieldElement> fieldElementPair in fieldElements)
            {
                ElementConfig elementConfig = fieldElementPair.Value.ConfigParameters.ElementConfig;
                _evolutionsService.UpdateProgress(elementConfig);
            }

            _gameSaveService.Save(GameSaveType.EvolutionsProgress);
        }
    }
}