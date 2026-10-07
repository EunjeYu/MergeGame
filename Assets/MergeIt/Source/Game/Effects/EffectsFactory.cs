// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.FieldElements;
using MergeIt.Game.Effects.Controllers;
using MergeIt.Game.Effects.Parameters;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Effects
{
    public class EffectsFactory : IEffectsFactory
    {
        [Introduce]
        private IEffectsManager _effectsManager;

        public void CreateEffect<T>(IFieldElementPresenter target, IEffectParameters effectParameters = null,
            Action finishedCallback = null)
            where T : IEffectWithPresenter, IEffect, new()
        {
            var effectController = new T();
            effectController.Setup(target, effectParameters, finishedCallback);
            _effectsManager.AddEffect(effectController);
        }
        
        public void CreateEffect<T>(RectTransform target, IEffectParameters effectParameters = null,
            Action finishedCallback = null)
            where T : IEffect, new()
        {
            var effectController = new T();
            effectController.Setup(target, effectParameters, finishedCallback);
            _effectsManager.AddEffect(effectController);
        }
    }
}