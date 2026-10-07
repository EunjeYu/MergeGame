// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.FieldElements;
using MergeIt.Game.Field;

namespace MergeIt.Game.Converters
{
    public interface IConfigProcessor
    {
        FieldData Convert(LevelConfig levelConfig);
        FieldElementData ConvertToFieldElementData(IFieldElement fieldElement);
        FieldElementData ConvertToFieldElementData(LevelElementData levelElementData);
        FieldData BuildLevel();
        IFieldElement ConvertToFieldElement(FieldElementData data);
    }
}