// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Elements;
using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs.ItemsDrawers
{
    public abstract class BaseSettingsDrawer : IDisposable
    {
        protected readonly VisualElement Parent;
        protected readonly VisualElement Root;
        protected readonly ElementConfig Config;
        protected Label Arrow;
        
        public BaseSettingsDrawer(VisualElement parent, ElementConfig config)
        {
            Config = config;
            Root = new VisualElement();
            Parent = parent;
            
            VisualTreeAsset visualTreeAsset = GetUxml();
            visualTreeAsset.CloneTree(Root);

            Parent.Add(Root);
        }

        public abstract void Draw();
        
        public virtual void Hide()
        {
            Root.style.display = StylesConstants.DisplayNone;
        }

        public virtual void Show()
        {
            Root.style.display = StylesConstants.DisplayFlex;
        }

        protected void Expand(bool expand)
        {
            OnExpand(expand);
            
            RotateArrow(expand);
        }

        protected abstract void OnExpand(bool expand);

        protected void RotateArrow(bool rotate)
        {
            Arrow.style.rotate = rotate ? new StyleRotate(new Rotate(90)) : new StyleRotate(new Rotate(0));
        }

        protected abstract VisualTreeAsset GetUxml();
        
        public virtual void Dispose()
        {
        }
    }
}