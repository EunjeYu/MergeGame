// Copyright (c) 2024, Awessets

using MergeIt.Game.Effects.Controllers;

namespace MergeIt.Game.Effects
{
    public interface IEffectsManager
    {
        void AddEffect(IEffect effectController);
    }
}