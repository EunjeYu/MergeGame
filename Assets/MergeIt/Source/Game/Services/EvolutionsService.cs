// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Evolutions;
using MergeIt.Core.Messages;
using MergeIt.Core.Saves;
using MergeIt.Core.Schemes;
using MergeIt.Core.Services;
using MergeIt.Game.Evolutions;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Services
{
    public class EvolutionsService : IEvolutionsService
    {
        private readonly EvolutionsServiceModel _serviceModel = new();

        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IMessageBus _messageBus;

        [Introduce]
        private IGameSaveService _saveService;

        public List<ElementConfig> GetEvolutionChain(EvolutionData evolutionData)
        {
            var chain = evolutionData?.Chain;

            return chain;
        }

        public List<ElementConfig> GetGeneratedBy(ElementConfig config)
        {
            var generators = _configsService.GetConfigs(x =>
            {
                ElementGeneratorSettings generatorSettings = x.GeneratorSettings;
                return generatorSettings != null && generatorSettings.GenerateItems.Any(el => el.Element == config);

            }).ToList();

            return generators;
        }

        public List<ElementConfig> GetGenerates(ElementConfig config)
        {
            List<ElementConfig> generatesElements = null;
            if (config.GeneratorSettings != null)
            {
                generatesElements = new List<ElementConfig>();
                var generatedItems = config.GeneratorSettings.GenerateItems;

                for (int i = 0; i < generatedItems.Count; i++)
                {
                    GeneratableElement generatedItem = generatedItems[i];

                    ElementConfig elementConfig = generatedItem.Element;

                    if (elementConfig != null)
                    {
                        generatesElements.Add(elementConfig);
                    }
                }
            }

            return generatesElements;
        }

        public void UpdateProgress(ElementConfig elementId)
        {
            EvolutionData evolution = _configsService.GetEvolutionByElement(elementId);

            if (evolution == null || evolution.Discovered)
            {
                return;
            }

            string evolutionId = evolution.Id;

            if (_serviceModel.EvolutionsProgress.TryGetValue(evolutionId, out int progress))
            {
                int index = evolution.Chain.IndexOf(elementId);
                int order = index + 1;

                if (index != -1 && order > progress)
                {
                    _serviceModel.EvolutionsProgress[evolutionId] = order;
                }
            }
        }

        public int GetEvolutionProgress(string id)
        {
            _serviceModel.EvolutionsProgress.TryGetValue(id, out int progress);

            return progress;
        }

        public void SetupEvolutionsProgress(IEvolutionsProgressData data)
        {
            var evolutions = data.EvolutionsProgress;

            if (evolutions != null)
            {
                for (int i = 0; i < evolutions.Count; i++)
                {
                    EvolutionProgressData evolutionData = evolutions[i];
                    _serviceModel.EvolutionsProgress[evolutionData.EvolutionId] = evolutionData.Progress;
                }
            }
        }

        public IEvolutionsProgressData GetData()
        {
            var evolutions = new EvolutionsProgressData();

            foreach (var evoProgressItem in _serviceModel.EvolutionsProgress)
            {
                var evoProgressData = new EvolutionProgressData
                {
                    Progress = evoProgressItem.Value,
                    EvolutionId = evoProgressItem.Key
                };

                evolutions.EvolutionsProgress.Add(evoProgressData);
            }

            return evolutions;
        }

        public void CreateEvolutionsProgress()
        {
            SchemeObject evolutionsConfig = _configsService.LevelConfig.EvolutionsScheme;
            var evolutions = new EvolutionsProgressData();

            for (int i = 0; i < evolutionsConfig.Evolution.Count; i++)
            {
                EvolutionData evolution = evolutionsConfig.Evolution[i];

                _serviceModel.EvolutionsProgress[evolution.Id] = evolution.Discovered ? evolution.Chain.Count : 1;
            }

            SetupEvolutionsProgress(evolutions);

            _saveService.Save(GameSaveType.EvolutionsProgress);
        }
    }
}