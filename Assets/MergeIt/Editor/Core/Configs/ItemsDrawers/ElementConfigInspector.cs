// Copyright (c) 2024, Awessets

using System;
using System.IO;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs.ItemsDrawers
{
    [CustomEditor(typeof(ElementConfig))]
    public class ElementConfigInspector : UnityEditor.Editor
    {
        private VisualElement _root;
        private ElementConfig _target;
        private static CommonSettingsDrawer _commonSettingsDrawer;
        private static GeneratorSettingsInspectorDrawer _generatorSettingsDrawer;

        public ElementConfig Target
        {
            get
            {
                if (_target == null)
                {
                    _target = target as ElementConfig;
                }

                return _target;
            }
        }

        public VisualElement Root
        {
            get => _root;
        }

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();

            GetUxml().CloneTree(Root);

            var typeField = Root.Q<EnumField>("ElementType");
            typeField.RegisterValueChangedCallback(OnElementTypeChanged);

            var stylesheet = GetStylesheet();
            if (!Root.styleSheets.Contains(stylesheet))
            {
                Root.styleSheets.Add(stylesheet);
            }

            _commonSettingsDrawer?.Dispose();
            _commonSettingsDrawer = null;

            _generatorSettingsDrawer?.Clear();
            _generatorSettingsDrawer = null;

            Refresh(Target.Type);

            serializedObject.ApplyModifiedProperties();

            return Root;
        }

        private StyleSheet GetStylesheet()
        {
            return (StyleSheet)EditorGUIUtility.Load(Path.Combine(Constants.InspectorResourcesRoot,
                "ItemsInspectorStyle.uss"));
        }

        private void OnElementTypeChanged(ChangeEvent<Enum> changeEvent)
        {
            if (changeEvent.newValue == null)
            {
                return;
            }

            if (Enum.TryParse(changeEvent.newValue.ToString(), out ElementType type))
            {
                Debug.Log($"Value changed to ({type}), Target type now is {Target.Type}");

                Refresh(type);
            }
        }

        private void Refresh(ElementType type)
        {
            if (_commonSettingsDrawer == null)
            {
                _commonSettingsDrawer = new CommonSettingsDrawer(Root, Target);
                _commonSettingsDrawer.Draw();
            }

            switch (type)
            {
                case ElementType.Regular:
                    if (_generatorSettingsDrawer != null)
                    {
                        _generatorSettingsDrawer.Hide();
                    }

                    break;

                case ElementType.Generator:
                    if (_generatorSettingsDrawer != null)
                    {
                        _generatorSettingsDrawer.Show();
                    }
                    else
                    {
                        _generatorSettingsDrawer = new GeneratorSettingsInspectorDrawer(Root, Target);
                        _generatorSettingsDrawer.Draw();
                    }

                    break;
            }
        }
        
        private void OnDisable()
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        private VisualTreeAsset GetUxml()
        {
            return (VisualTreeAsset)EditorGUIUtility.Load(Path.Combine(Constants.InspectorResourcesRoot,
                "ElementInspector.uxml"));
        }
    }
}