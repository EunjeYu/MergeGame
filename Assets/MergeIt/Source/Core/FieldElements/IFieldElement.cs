// Copyright (c) 2024, Awessets

namespace MergeIt.Core.FieldElements
{
    public interface IFieldElement
    {
        IConfigParameters ConfigParameters { get; set; }
        IInfoParameters InfoParameters { get; set; }
        IGeneratorParameters GeneratorParameters { get; set; }
        IGeneratorOpenParameters GeneratorOpenParameters { get; set; }
        IProduceParameters ProduceParameters { get; set; }
        IProducedByParameters ProducedByParameters { get; set; }
    }
}