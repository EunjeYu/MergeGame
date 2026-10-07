// Copyright (c) 2024, Awessets

using System.IO;
using MergeIt.Core.Configs.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs.ItemsDrawers
{
    public class GeneratorSettingsBaseDrawer : BaseSettingsDrawer
    {
        private readonly ElementGeneratorSettings _settings;
        private Toggle _expanded;
        private Toggle _isUnlimitedToggle;
        private Toggle _needOpenToggle;
        private IntegerField _openTime;
        private Toggle _generateBlocked;
        private VisualElement _stackContainer;
        private VisualElement _needOpenContainer;
        private Box _generatorSettings;

        public GeneratorSettingsBaseDrawer(VisualElement parent, ElementConfig config) : base(parent, config)
        {
            _settings = config.GeneratorSettings;
        }

        public override void Draw()
        {
            _stackContainer = Root.Q<VisualElement>("StackContainer");
            _needOpenContainer = Root.Q<VisualElement>("NeedOpenContainer");
            _isUnlimitedToggle = Root.Q<Toggle>("IsUnlimited");
            _needOpenToggle = Root.Q<Toggle>("NeedOpen");
            
            var skipOpenParams = Root.Q<CurrencyComponent>("SkipOpenParameters");
            skipOpenParams.Bind(new SerializedObject(Config));
            
            var skipChargeParams = Root.Q<CurrencyComponent>("SkipChargeParameters");
            skipChargeParams.Bind(new SerializedObject(Config));

            _generatorSettings = Root.Q<Box>("GeneratorSettings");
            
            _isUnlimitedToggle.RegisterValueChangedCallback(OnUnlimitedChanged);
            _needOpenToggle.RegisterValueChangedCallback(OnNeedOpenChanged);
            
            _stackContainer.style.display = _settings.IsUnlimited ? StylesConstants.DisplayNone : StylesConstants.DisplayFlex;
            _needOpenContainer.style.display = _settings.NeedOpen ? StylesConstants.DisplayFlex : StylesConstants.DisplayNone;

            _expanded = Root.Q<Toggle>("GeneratorSettingsFoldout");
            _expanded.value = _settings.Expanded;
            _expanded.RegisterValueChangedCallback(OnExpandedChanged);
            
            Arrow = Root.Q<Label>("Arrow");
            
            Expand(_settings.Expanded);
        }
        
        public void Clear()
        {
            OnClear();
            
            Parent.Remove(Root);
        }

        protected virtual void OnClear()
        {
            _isUnlimitedToggle.UnregisterValueChangedCallback(OnUnlimitedChanged);
            _needOpenToggle.UnregisterValueChangedCallback(OnNeedOpenChanged);
        }

        public override void Dispose()
        {
            base.Dispose();

            Clear();
        }
        
        private void OnExpandedChanged(ChangeEvent<bool> evt)
        {
            Expand(evt.newValue);
        }
        
        private void OnUnlimitedChanged(ChangeEvent<bool> evt)
        {
            _stackContainer.style.display = evt.newValue ? StylesConstants.DisplayNone : StylesConstants.DisplayFlex;
        }
        
        private void OnNeedOpenChanged(ChangeEvent<bool> evt)
        {
            _needOpenContainer.style.display = evt.newValue ? StylesConstants.DisplayFlex : StylesConstants.DisplayNone;
        }

        protected override void OnExpand(bool expand)
        {
            _settings.Expanded = expand;
            _generatorSettings.style.display = 
                expand ? StylesConstants.DisplayFlex : StylesConstants.DisplayNone;
        }

        protected override VisualTreeAsset GetUxml()
        {
            return (VisualTreeAsset)EditorGUIUtility.Load(Path.Combine(Constants.InspectorResourcesRoot,
                "GeneratorInspector.uxml"));
        }
    }
}