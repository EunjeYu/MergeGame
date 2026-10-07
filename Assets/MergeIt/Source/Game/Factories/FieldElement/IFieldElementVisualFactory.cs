// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;

namespace MergeIt.Game.Factories.FieldElement
{
    public interface IFieldElementVisualFactory
    {
        IFieldElementPresenter CreateFieldElement(IFieldElement element);
    }
}