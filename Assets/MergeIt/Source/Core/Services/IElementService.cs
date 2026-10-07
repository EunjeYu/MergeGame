// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;

namespace MergeIt.Core.Services
{
    public interface IElementService
    {
        void TrySell(IFieldElement fieldElement);
        void TryUnlock(IFieldElement fieldElement);
        void TrySplit(IFieldElement fieldElement);
    }
}