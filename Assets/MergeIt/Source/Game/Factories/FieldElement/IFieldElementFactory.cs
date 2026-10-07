// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;

namespace MergeIt.Game.Factories.FieldElement
{
    public interface IFieldElementFactory
    {
        IFieldElement CreateFieldElement(FieldElementData data);
        IFieldElement CreateFieldElement(LevelElementData data);
        IFieldElement CreateFieldElement(ElementConfig elementConfig, GridPoint point, bool isBlocked = false);
    }
}