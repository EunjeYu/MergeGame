// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;

namespace MergeIt.Core.Services
{
    public interface IGeneratorsService
    {
        void TryOpen(IFieldElement generator);
        void TrySkipOpening(IFieldElement generator);
        void TrySkipCharging(IFieldElement generator);
    }
}