// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;

namespace MergeIt.Core.Services
{
    public interface IUserProgressService
    {
        int GetCurrentLevelMaxExp();
        bool CanLevelUp();
        ElementConfig[] GetLevelUpPrizes();
    }
}