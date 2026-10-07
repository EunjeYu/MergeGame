// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Saves;
using MergeIt.Core.User;
using Newtonsoft.Json;

namespace MergeIt.Game.User
{
    [Serializable, Savable("user", "dat")]
    public class UserData : IUserData
    {
        [JsonProperty("n")]
        public string Name { get; set; }
        [JsonProperty("e")]
        public int Energy { get; set; }
        [JsonProperty("s")]
        public int SoftCurrency { get; set; }
        [JsonProperty("h")]
        public int HardCurrency { get; set; }
        [JsonProperty("sp")]
        public int Splitters { get; set; }
        [JsonProperty("l")]
        public int Level { get; set; }
        [JsonProperty("ex")]
        public int Experience { get; set; }
        [JsonProperty("er")]
        public long EnergyRestoringStartTime { get; set; }
    }
}