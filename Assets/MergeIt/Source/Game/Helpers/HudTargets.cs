// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using MergeIt.Game.Enums;
using UnityEngine;

namespace MergeIt.Game.Helpers
{
    public static class HudTargets
    {
        private static Dictionary<HudPanelType, Transform> _hudTargets = new();

        public static void AddTarget(HudPanelType hudPanelType, Transform transform)
        {
            _hudTargets[hudPanelType] = transform;
        }

        public static Transform GetTarget(HudPanelType hudPanelType)
        {
            _hudTargets.TryGetValue(hudPanelType, out Transform target);

            return target;
        }
    }
}