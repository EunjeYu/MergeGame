// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Effects.Controllers;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;

namespace MergeIt.Game.Effects
{
    public class EffectsManager : IEffectsManager, IUpdatable
    {
        private readonly List<IEffect> _effectControllers = new();
        
        [Introduce]
        private IMessageBus _messageBus;
        
        [Introduce]
        private IConfigsService _configsService;

        public void AddEffect(IEffect effectController)
        {
            _effectControllers.Add(effectController);
            effectController.Start();
        }

        public void Update()
        {
            for (int i = 0; i < _effectControllers.Count; i++)
            {
                IEffect effect = _effectControllers[i];
                effect.Update();
            }
        }
        
        private void OnEffectFinished(IEffect effectController)
        {
            _effectControllers.Remove(effectController);
        }
    }
}