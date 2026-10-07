// Copyright (c) 2024, Awessets

using System;
using MergeIt.Game.Effects.Parameters;
using UnityEngine;

namespace MergeIt.Game.Effects.Controllers
{
    public interface IEffect
    {
        void Start();
        void Update(); 
        void Setup(Transform target, IEffectParameters effectParameters = null, Action finishedCallback = null); 
    }
}