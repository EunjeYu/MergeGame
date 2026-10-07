// Copyright (c) 2024, Awessets

using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs
{
    public class CurrencyComponent : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CurrencyComponent, CurrencyComponentsTraits> { }
        
        public string Title { get; set; }
        public string TypeBinding { get; set; }
        public string AmountBinding { get; set; }

        public CurrencyComponent()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachedToPanel);
        }
        
        private void OnAttachedToPanel(AttachToPanelEvent evt)
        {
            Draw();
        }

        private void Draw()
        {
            var styleSheet =
                (StyleSheet)EditorGUIUtility.Load(Path.Combine(Constants.InspectorResourcesRoot,
                    "ItemsInspectorStyle.uss"));
            var visualTreeAsset =
                (VisualTreeAsset)EditorGUIUtility.Load(Path.Combine(Constants.InspectorResourcesRoot, "Configs",
                    "CurrencyComponent.uxml"));

            visualTreeAsset.CloneTree(this);

            styleSheets.Add(styleSheet);
            
            Label titleLabel = this.Q<Label>("Title");
            titleLabel.text = Title;

            EnumField enumField = this.Q<EnumField>("CurrencyTypeField");
            enumField.bindingPath = TypeBinding;
            
            IntegerField integerField = this.Q<IntegerField>("AmountField");
            integerField.bindingPath = AmountBinding;
        }
    }
}