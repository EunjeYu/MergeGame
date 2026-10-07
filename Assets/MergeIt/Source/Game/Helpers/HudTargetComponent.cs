// Copyright (c) 2024, Awessets

using MergeIt.Game.Enums;
using UnityEngine;

namespace MergeIt.Game.Helpers
{
    public class HudTargetComponent : MonoBehaviour
    {
        [SerializeField]
        private HudPanelType _hudPanelType;

        private void Awake()
        {
            HudTargets.AddTarget(_hudPanelType, transform);
        }
    }
}