// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.FieldElements;
using MergeIt.Game.Effects.Controllers;
using MergeIt.Game.Effects.Parameters;
using UnityEngine;

namespace MergeIt.Game.Effects
{
    public interface IEffectsFactory
    {
        void CreateEffect<T>(IFieldElementPresenter target, IEffectParameters effectParameters = null, Action finishedCallback = null)
            where T : IEffectWithPresenter, IEffect, new();

        void CreateEffect<T>(RectTransform target, IEffectParameters effectParameters = null, Action finishedCallback = null)
            where T : IEffect, new();
    }
}