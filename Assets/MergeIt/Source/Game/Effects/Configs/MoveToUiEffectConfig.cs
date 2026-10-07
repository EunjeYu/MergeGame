// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Effects;
using UnityEngine;

namespace MergeIt.Game.Effects.Configs
{
    [CreateAssetMenu(fileName = "MoveToUiEffectConfig", menuName = "Merge Toolkit/Effects/Move To UI Effect Config")]
    public class MoveToUiEffectConfig : EffectConfig
    {
        public AnimationCurve PositionXCurve;
        public AnimationCurve PositionYCurve;
        public AnimationCurve ScaleCurve;
    }
}