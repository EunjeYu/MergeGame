// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Helpers;
using MergeIt.Game.Effects.Configs;
using MergeIt.Game.Effects.Parameters;
using UnityEngine;

namespace MergeIt.Game.Effects.Controllers
{
    public class JumpEffect : BaseEffect
    {
        private readonly JumpEffectConfig _effectConfig;
        
        private float _progress;
        private Vector2 _initialPosition;
        
        private AnimationCurve _jumpCurve;
        private AnimationCurve _scaleCurve;
        private Vector3 _initialScale;

        public JumpEffect()
        {
            _effectConfig = ConfigsService.GetEffectConfig<JumpEffectConfig>(EffectName.JumpElement);
        }
        
        public override void Update()
        {
            if (Started)
            {
                _progress += Time.deltaTime / _effectConfig.Duration;

                float posY = _jumpCurve.Evaluate(_progress);
                float scale = _scaleCurve.Evaluate(_progress);
                
                Vector3 position = Target.position;
                Vector3 localScale = Target.localScale;
                position.y = posY;
                localScale.x = scale;
                localScale.y = scale;
                
                Target.position = position;
                Target.localScale = localScale;

                if (_progress >= 1f)
                {
                    Target.position = _initialPosition;
                    Target.localScale = _initialScale;

                    Finish();
                }
            }
        }

        public override void Setup(Transform target, IEffectParameters effectParameters = null, Action finishedCallback = null)
        {
            base.Setup(target, effectParameters, finishedCallback);
            
            _initialPosition = target.position;
            _initialScale = target.localScale;
            
            _jumpCurve = CurvesHelper.CopyCurveFrom(_effectConfig.CurveY, _initialPosition.y);
            _scaleCurve = CurvesHelper.CopyCurveFrom(_effectConfig.CurveY, _initialScale.x);
        }
    }
}