// Copyright (c) 2024, Awessets

using MergeIt.Game.Enums;
using MergeIt.Game.Helpers;

namespace MergeIt.Game.HUD
{
    public abstract class HudPanelBase : UserListenerComponent, IHudPanel
    {
        public virtual HudPanelType Type { get; }

        protected virtual void Start()
        {
            HudTargets.AddTarget(Type, transform);
        }
    }
}