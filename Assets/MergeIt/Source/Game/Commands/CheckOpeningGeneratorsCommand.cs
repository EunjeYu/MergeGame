// Copyright (c) 2024, Awessets

using MergeIt.Core.Commands;
using MergeIt.Game.Field;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Commands
{
    public class CheckOpeningGeneratorsCommand : Command
    {
        private readonly FieldLogicModel _fieldLogicModel = DiContainer.Get<FieldLogicModel>();

        public override void Execute()
        {
            
        }
    }
}