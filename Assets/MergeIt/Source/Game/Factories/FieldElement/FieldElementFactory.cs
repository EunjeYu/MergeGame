// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Services;
using MergeIt.Game.Converters;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Factories.FieldElement
{
    public class FieldElementFactory : IFieldElementFactory
    {
        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IConfigProcessor _configProcessor;
        

        public IFieldElement CreateFieldElement(FieldElementData data)
        {
            return _configProcessor.ConvertToFieldElement(data);
        }

        public IFieldElement CreateFieldElement(LevelElementData data)
        {
            FieldElementData fieldElementData = _configProcessor.ConvertToFieldElementData(data);

            return _configProcessor.ConvertToFieldElement(fieldElementData);
        }

        public IFieldElement CreateFieldElement(ElementConfig elementConfig, GridPoint point, bool isBlocked = false)
        {
            FieldElementData fieldElementData = CreateFieldElementData(elementConfig, point, isBlocked);

            return _configProcessor.ConvertToFieldElement(fieldElementData);
        }

        private FieldElementData CreateFieldElementData(ElementConfig elementConfig, GridPoint point, bool isBlocked)
        {
            FieldElementData fieldElementData = new FieldElementData();
            ElementConfig config = elementConfig;
            string evolutionId = _configsService.GetEvolutionIdByElement(elementConfig);

            fieldElementData.ConfigParameters = new SavedConfigParameters
            {
                ElementId = elementConfig.Id,
                EvolutionId = evolutionId
            };

            fieldElementData.InfoParameters = new SavedInfoParameters
            {
                LogicPosition = point,
                IsBlocked = isBlocked
            };

            switch (config.Type)
            {
                case ElementType.Generator:
                    var generatorParameters = new SavedGeneratorParameters();

                    if (config.GeneratorSettings.Charged)
                    {
                        generatorParameters.AvailableToDrop = config.GeneratorSettings.MaxDrop;
                    }
                    else
                    {
                        generatorParameters.StartChargingTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    }

                    fieldElementData.GeneratorParameters = generatorParameters;

                    if (config.GeneratorSettings.NeedOpen)
                    {
                        var generatorOpenParameters = new SavedGeneratorOpenParameters
                        {
                            StartOpeningTime = 0
                        };

                        fieldElementData.GeneratorOpenParameters = generatorOpenParameters;
                    }

                    break;
            }

            return fieldElementData;
        }
    }
}