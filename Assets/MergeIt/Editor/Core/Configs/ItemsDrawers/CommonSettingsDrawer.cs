// Copyright (c) 2024, Awessets

using System.IO;
using MergeIt.Core.Configs.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs.ItemsDrawers
{
    public class CommonSettingsDrawer : BaseSettingsDrawer
    {
        private readonly ElementCommonSettings _settings;

        private Toggle _consumable;
        private Toggle _expanded;
        private ObjectField _icon;
        private Box _commonSettings;

        public CommonSettingsDrawer(VisualElement parent, ElementConfig config) : base(parent, config)
        {
            _settings = config.CommonSettings;
        }

        public override void Draw()
        {
            _icon = Root.Q<ObjectField>("IconContainer");
            _icon.objectType = typeof(FieldElementIconComponent);
            _icon.RegisterValueChangedCallback(OnImageChanged);

            _commonSettings = Root.Q<Box>("CommonSettings");

            var sellParams = Root.Q<CurrencyComponent>("SellParameters");
            sellParams.Bind(new SerializedObject(Config));

            var splitParams = Root.Q<CurrencyComponent>("SplitParameters");
            splitParams.Bind(new SerializedObject(Config));

            var unlockParams = Root.Q<CurrencyComponent>("UnlockParameters");
            unlockParams.Bind(new SerializedObject(Config));
            
            var consumableContainer = Root.Q<VisualElement>("ConsumableContainer");
            consumableContainer.style.display =
                _settings.IsConsumable ? StylesConstants.DisplayFlex : StylesConstants.DisplayNone;

            _consumable = Root.Q<Toggle>("IsConsumable");
            _consumable.RegisterValueChangedCallback(OnConsumableChanged);
            
            _expanded = Root.Q<Toggle>("CommonSettingsFoldout");
            _expanded.value = _settings.Expanded;
            _expanded.RegisterValueChangedCallback(OnExpandedChanged);
            
            Arrow = Root.Q<Label>("Arrow");
            
            SetImagePlaceholder(Config.CommonSettings.Icon);

            Expand(_settings.Expanded);
        }

        public override void Dispose()
        {
            base.Dispose();

            _consumable.UnregisterValueChangedCallback(OnConsumableChanged);
            _expanded.UnregisterValueChangedCallback(OnExpandedChanged);
            _icon.UnregisterValueChangedCallback(OnImageChanged);
            
            Parent.Remove(Root);
        }

        private void OnConsumableChanged(ChangeEvent<bool> evt)
        {
            var container = Root.Q<VisualElement>("ConsumableContainer");
            container.style.display = evt.newValue ? StylesConstants.DisplayFlex : StylesConstants.DisplayNone;
        }

        private void OnExpandedChanged(ChangeEvent<bool> evt)
        {
            Expand(evt.newValue);
        }

        protected override void OnExpand(bool expand)
        {
            _settings.Expanded = expand;
            _commonSettings.style.display = expand ? StylesConstants.DisplayFlex : StylesConstants.DisplayNone;
        }

        private void SetImagePlaceholder(FieldElementIconComponent placeholder)
        {
            if (!placeholder)
            {
                return;
            }
            
            Sprite sprite = null;
            UnityEngine.UI.Image image = placeholder.GetImage();
            if (image)
            {
                sprite = image.sprite;
            }
            
            var icon = Root.Q<Image>("ItemImage");
            icon.image = sprite ? sprite.texture : null;
        }

        private void OnImageChanged(ChangeEvent<Object> evt)
        {
            SetImagePlaceholder(evt.newValue as FieldElementIconComponent);
        }

        protected override VisualTreeAsset GetUxml()
        {
            return (VisualTreeAsset)EditorGUIUtility.Load(Path.Combine(Constants.InspectorResourcesRoot,
                "CommonInspector.uxml"));
        }
    }
}