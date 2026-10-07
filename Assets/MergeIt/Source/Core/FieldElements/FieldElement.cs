// Copyright (c) 2024, Awessets

using System.Diagnostics;

namespace MergeIt.Core.FieldElements
{
    [DebuggerDisplay("{ToString()}")]
    public class FieldElement : IFieldElement
    {
        public IConfigParameters ConfigParameters { get; set; }
        public IInfoParameters InfoParameters { get; set; }
        public IGeneratorParameters GeneratorParameters { get; set; }
        public IGeneratorOpenParameters GeneratorOpenParameters { get; set; }
        public IProduceParameters ProduceParameters { get; set; }
        public IProducedByParameters ProducedByParameters { get; set; }

        public override string ToString()
        {
            return $"Type: {InfoParameters.Type}, Pos: {InfoParameters.LogicPosition}, Blocked: {InfoParameters.IsBlocked}";
        }
    }
}