// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Helpers;
using MergeIt.Game.Effects.Configs;
using MergeIt.Game.Effects.Parameters;
using MergeIt.Game.Helpers;
using UnityEngine;

namespace MergeIt.Game.Effects.Controllers
{
    public class MoveToUiEffect : BaseEffect
    {
        private MoveToUiEffectConfig _effectConfig;

        private float _progress;
        private Vector3 _moveFrom;
        private Vector3 _moveTo;

        private AnimationCurve _scaleCurve;

        public override void Setup(Transform target, IEffectParameters effectParameters = null, Action finishedCallback = null)
        {
            base.Setup(target, effectParameters, finishedCallback);

            if (effectParameters?.OverriddenName != null)
            {
                _effectConfig = ConfigsService.GetEffectConfig<MoveToUiEffectConfig>(effectParameters.OverriddenName);
            }
            else
            {
                _effectConfig = ConfigsService.GetEffectConfig<MoveToUiEffectConfig>(EffectName.MoveToUiElement);
            }

            if (effectParameters is MoveEffectParameters moveEffectParameters)
            {
                _moveTo = HudTargets.GetTarget(moveEffectParameters.TargetInHud).position;
                _moveFrom = moveEffectParameters.FromPosition;
            }

            _scaleCurve = CurvesHelper.CopyCurveFrom(_effectConfig.ScaleCurve, Target.localScale.x);
        }

        public override void Update()
        {
            if (Started)
            {
                _progress += Time.deltaTime / _effectConfig.Duration;
                
                float posX = _effectConfig.PositionXCurve.Evaluate(_progress);
                float posY = _effectConfig.PositionYCurve.Evaluate(_progress);
                float scale = _scaleCurve.Evaluate(_progress);
                
                Vector3 position = Vector3.Lerp(_moveFrom, _moveTo, _progress);
                position.x += posX;
                position.y += posY;
                
                Target.position = position;
                Target.localScale = new Vector3(scale, scale, 1f);

                if (_progress >= 1f)
                {
                    Finish();
                }
            }
        }
    }
}