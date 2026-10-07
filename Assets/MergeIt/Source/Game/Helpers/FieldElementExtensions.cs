// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;

namespace MergeIt.Game.Helpers
{
    public static class FieldElementExtensions
    {
        public static ElementConfig GetNextInEvolution(this IFieldElement fieldElement)
        {
            ElementConfig elementConfig = fieldElement.ConfigParameters.ElementConfig;
            List<ElementConfig> evolutionChain = fieldElement.ConfigParameters.EvolutionData.Chain;
            
            int index = evolutionChain.IndexOf(elementConfig);
            if (index == evolutionChain.Count - 1)
            {
                return null;
            }

            index += 1;

            return evolutionChain[index];
        }
        
        public static ElementConfig GetPreviousInEvolution(this IFieldElement fieldElement)
        {
            ElementConfig elementConfig = fieldElement.ConfigParameters.ElementConfig;
            List<ElementConfig> evolutionChain = fieldElement.ConfigParameters.EvolutionData.Chain;
            
            int index = evolutionChain.IndexOf(elementConfig);
            if (index == 0)
            {
                return null;
            }

            index -= 1;

            return evolutionChain[index];
        }
        
        public static string GetDescription(this ElementConfig config)
        {
            string description = config.CommonSettings.Description;

            if (config.CommonSettings.IsConsumable)
            {
                description = string.Format(description, config.CommonSettings.ConsumableSettings.Value);
            }

            return description;
        }
    }
}