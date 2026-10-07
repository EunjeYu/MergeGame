// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Saves;
using Newtonsoft.Json;

namespace MergeIt.Game.Field
{
    [Serializable, Savable("field", "dat")]
    public class FieldData : ISavable
    {
        [JsonProperty("w")]
        public int FieldWidth;
        [JsonProperty("h")]
        public int FieldHeight;
        [JsonProperty("e")]
        public List<FieldElementData> SavedElementsData;
    }
}