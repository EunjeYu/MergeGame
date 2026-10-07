// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Effects;
using UnityEngine;

namespace MergeIt.Game.Effects.Configs
{
    [CreateAssetMenu(fileName = "MoveEffectConfig", menuName = "Merge Toolkit/Effects/Move Effect Config")]
    public class MoveEffectConfig : EffectConfig
    {
        public AnimationCurve PositionXCurve;
        public AnimationCurve PositionYCurve;
        public AnimationCurve ScaleCurve;
    }

}