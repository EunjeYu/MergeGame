// Copyright (c) 2024, Awessets

using MergeIt.Core.User;
using MergeIt.Core.Utils;
using MergeIt.Game.User;

namespace MergeIt.Game.Services
{
    public class UserServiceModel
    {
        public Bindable<string> Name { get; } = new();
        public Bindable<int> Energy { get; } = new();
        public Bindable<int> SoftCurrency { get; } = new();
        public Bindable<int> HardCurrency { get; } = new();
        public Bindable<int> Splitters { get; } = new();
        public Bindable<int> Level { get; } = new();
        public Bindable<int> Experience { get; } = new();

        public long EnergyRestoringStartTime { get; set; }

        public void Set(IUserData data)
        {
            Name.Value = data.Name;
            Energy.Value = data.Energy;
            SoftCurrency.Value = data.SoftCurrency;
            HardCurrency.Value = data.HardCurrency;
            Splitters.Value = data.Splitters;
            Level.Value = data.Level;
            Experience.Value = data.Experience;
            EnergyRestoringStartTime = data.EnergyRestoringStartTime;
        }

        public UserData GetData()
        {
            var userData = new UserData
            {
                Name = Name.Value,
                Energy = Energy.Value,
                Experience = Experience.Value,
                SoftCurrency = SoftCurrency.Value,
                HardCurrency = HardCurrency.Value,
                Splitters = Splitters.Value,
                Level = Level.Value,
                EnergyRestoringStartTime = EnergyRestoringStartTime
            };

            return userData;
        }
    }
}