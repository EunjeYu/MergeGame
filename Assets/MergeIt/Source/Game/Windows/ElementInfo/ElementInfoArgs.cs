// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.WindowSystem.Data;

namespace MergeIt.Game.Windows.ElementInfo
{
    public class ElementInfoArgs : WindowArgs
    {
        public ElementConfig ElementConfig { get; set; }
    }
}