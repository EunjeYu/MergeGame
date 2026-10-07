// Copyright (c) 2024, Awessets

using MergeIt.Game.Enums;
using UnityEngine;

namespace MergeIt.Game.Effects.Parameters
{
    public class MoveEffectParameters : IEffectParameters
    {
        private readonly Vector3 _fromPosition;
        private readonly Vector3 _toPosition;
        private readonly HudPanelType _targetInHud;

        public string OverriddenName { get; }

        public Vector3 FromPosition
        {
            get => _fromPosition;
        }

        public Vector3 ToPosition
        {
            get => _toPosition;
        }

        public HudPanelType TargetInHud
        {
            get => _targetInHud;
        }

        public MoveEffectParameters(Vector3 fromPosition, Vector3 toPosition = default, HudPanelType targetInHud = 0, string overriddenName = null)
        {
            OverriddenName = overriddenName;
            
            _fromPosition = fromPosition;
            _toPosition = toPosition;
            _targetInHud = targetInHud;
        }
    }
}