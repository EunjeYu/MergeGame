// Copyright (c) 2024, Awessets

using System;
using Newtonsoft.Json;

namespace MergeIt.Core.FieldElements
{
    [Serializable]
    public class FieldElementData
    {
        [JsonProperty("cp")]
        public SavedConfigParameters ConfigParameters;
        [JsonProperty("ip")]
        public SavedInfoParameters InfoParameters;
        [JsonProperty("gp")]
        public SavedGeneratorParameters GeneratorParameters;
        [JsonProperty("go")]
        public SavedGeneratorOpenParameters GeneratorOpenParameters;
    }
}