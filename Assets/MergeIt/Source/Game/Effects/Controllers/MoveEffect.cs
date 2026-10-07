// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Helpers;
using MergeIt.Game.Effects.Configs;
using MergeIt.Game.Effects.Parameters;
using MergeIt.Game.Enums;
using MergeIt.Game.Helpers;
using UnityEngine;

namespace MergeIt.Game.Effects.Controllers
{
    public class MoveEffect : BaseEffect
    {
        private readonly MoveEffectConfig _effectConfig;

        private float _progress;
        private Vector3 _moveFrom;
        private Vector3 _moveTo;
        
        private AnimationCurve _scaleCurve;
        
        public MoveEffect()
        {
            _effectConfig = ConfigsService.GetEffectConfig<MoveEffectConfig>(EffectName.MoveElement);
        }

        public override void Setup(Transform target, IEffectParameters effectParameters = null, Action finishedCallback = null)
        {
            base.Setup(target, effectParameters, finishedCallback);
            
            if (effectParameters is MoveEffectParameters moveEffectParameters)
            {
                Setup(moveEffectParameters.FromPosition, moveEffectParameters.ToPosition, moveEffectParameters.TargetInHud);
            }
        }

        public void Setup(Vector3 moveFrom, Vector3 moveTo, HudPanelType targetInHud)
        {
            _moveFrom = moveFrom;
            _moveTo = moveTo;

            Target.position = _moveFrom;

            if (targetInHud != 0)
            {
                _moveTo = HudTargets.GetTarget(targetInHud).position;
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
                    Target.position = _moveTo;
                    Target.localScale = Vector3.one;
                    
                    Finish();
                }
            }
        }
    }
}