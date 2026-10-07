// Copyright (c) 2024, Awessets

using MergeIt.Core.Saves;

namespace MergeIt.Core.User
{
    public interface IUserData : ISavable
    {
        string Name { get; set; }
        int Energy { get; set; }
        int SoftCurrency { get; set; }
        int HardCurrency { get; set; }
        int Splitters { get; set; }
        int Level { get; set; }
        int Experience { get; set; }
        long EnergyRestoringStartTime { get; set; }
    }
}