// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Effects;
using UnityEngine;

namespace MergeIt.Game.Effects.Configs
{
    [CreateAssetMenu(fileName = "JumpEffectConfig", menuName = "Merge Toolkit/Effects/Jump Effect Config")]
    public class JumpEffectConfig : EffectConfig
    {
        public AnimationCurve CurveY;
        public AnimationCurve ScaleCurve;
    }
}