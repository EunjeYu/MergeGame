// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Evolutions;

namespace MergeIt.Core.Services
{
    public interface IEvolutionsService
    {
        List<ElementConfig> GetEvolutionChain(EvolutionData evolutionData);
        List<ElementConfig> GetGeneratedBy(ElementConfig config);
        List<ElementConfig> GetGenerates(ElementConfig config);

        void SetupEvolutionsProgress(IEvolutionsProgressData data);
        IEvolutionsProgressData GetData();
        void CreateEvolutionsProgress();
        void UpdateProgress(ElementConfig elementId);
        int GetEvolutionProgress(string id);
    }
}