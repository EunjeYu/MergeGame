// Copyright (c) 2024, Awessets

using MergeIt.Core.Commands;
using MergeIt.Game.Effects.Controllers;

namespace MergeIt.Game.Commands
{
    public class EffectCommand : Command
    {
        private IEffect _effect;
        
        public EffectCommand(IEffect effect)
        {
            _effect = effect;
        }
        
        public override void Execute()
        {
            base.Execute();
            
        }
    }
}