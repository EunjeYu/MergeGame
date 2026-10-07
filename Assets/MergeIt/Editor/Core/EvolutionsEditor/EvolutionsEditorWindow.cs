// Copyright (c) 2024, Awessets

using System;
using System.IO;
using MergeIt.Core.Schemes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MergeIt.Editor.EvolutionsEditor
{
    public class EvolutionsEditorWindow : EditorWindow
    {
        private EvolutionsGraphView _graphView;

        private StyleSheet _windowStyle;
        private VisualTreeAsset _windowTree;
        private bool _initialized;
        private SchemeObject _currentScheme;

        public bool Initialized
        {
            get => _initialized;
        }

        [MenuItem("Window/Merge Toolkit/Evolutions", false, 1)]
        public static void ShowWindow()
        {
            var window = GetWindow<EvolutionsEditorWindow>(true, "Evolutions Editor");

            if (!window.Initialized)
            {
                window.Initialize();
            }

            window.Show();
        }

        public void Initialize()
        {
            if (!_initialized)
            {
                _windowStyle =
                    (StyleSheet)EditorGUIUtility.Load(Path.Combine(Constants.EvolutionsEditorResourcesRoot,
                        "EvolutionsEditorStyle.uss"));
                _windowTree =
                    (VisualTreeAsset)EditorGUIUtility.Load(Path.Combine(Constants.EvolutionsEditorResourcesRoot,
                        "EvolutionsEditorWindow.uxml"));

                VisualElement root = rootVisualElement;
                VisualElement windowRoot = _windowTree.CloneTree();

                windowRoot.AddToClassList("root");

                var toolbarMenu = windowRoot.Q<ToolbarMenu>("FileMenu");
                toolbarMenu.menu.AppendAction("Open...", _ => OnLoadButtonClicked());
                toolbarMenu.menu.AppendSeparator();
                toolbarMenu.menu.AppendAction("Save", _ => OnSaveButtonClicked(), _ => CanSaveAndExport());
                toolbarMenu.menu.AppendAction("Save As...", _ => OnSaveAsButtonClicked());

                _graphView = windowRoot.Q<EvolutionsGraphView>("EvoGraph");
                _graphView.AddManipulator(new ContentDragger());
                _graphView.AddManipulator(new SelectionDragger());
                _graphView.AddManipulator(new RectangleSelector());
                _graphView.AddManipulator(new ClickSelector());
                
                _graphView.SetupZoom(0.1f, 3f);
                
                root.Add(windowRoot);
                root.styleSheets.Add(_windowStyle);

                _initialized = true;
            }
        }

        public void OnEnable()
        {
            Initialize();
        }

        public void OnDisable()
        {
            _initialized = false;
        }
        
        public void OnFocus()
        {
            _graphView?.Validate();
        }

        public void OnDestroy()
        {
            _graphView.ClearNodes();
        }

        public void Reload(SchemeObject scheme)
        {
            _currentScheme = scheme;

            SchemeData data = scheme.SchemeData;

            _graphView.Restore(data);
            _graphView.ClearSelection();
        }

        private void OnLoadButtonClicked()
        {
            var path = EditorUtility.OpenFilePanelWithFilters("Load scheme", "Assets", new[] {"Saved schemes", "asset"});
            if (path.Length != 0)
            {
                try
                {
                    string relativePath = null;
                    if (path.StartsWith(Application.dataPath))
                    {
                        relativePath = "Assets" + path.Substring(Application.dataPath.Length);
                    }

                    var currentScheme = AssetDatabase.LoadAssetAtPath<SchemeObject>(relativePath);
                    Reload(currentScheme);
                }
                catch (Exception e)
                {
                    Debug.Log($"{e}");
                }
            }
        }

        private void OnSaveAsButtonClicked()
        {
            SchemeData schemeData = _graphView.Save();

            if (schemeData != null)
            {
                var path = EditorUtility.SaveFilePanelInProject("Save scheme", "New_scheme", "asset", "Save scheme to file");
                if (path.Length != 0)
                {
                    var existedScheme = AssetDatabase.LoadAssetAtPath<SchemeObject>(path);

                    if (existedScheme != null)
                    {
                        existedScheme.SchemeData = schemeData;
                        EditorUtility.SetDirty(existedScheme);

                        _currentScheme = existedScheme;
                    }
                    else
                    {
                        var newScheme = CreateInstance<SchemeObject>();
                        newScheme.SchemeData = schemeData;

                        _currentScheme = newScheme;

                        AssetDatabase.CreateAsset(newScheme, path);
                        AssetDatabase.SaveAssets();
                    }
                    
                    Reload(_currentScheme);
                }
            }
            else
            {
                Debug.LogWarning("Can't save scheme because of errors.");
            }
        }

        private void OnSaveButtonClicked()
        {
            SchemeData schemeData = _graphView.Save();

            if (schemeData != null)
            {
                _currentScheme.SchemeData = schemeData;
                EditorUtility.SetDirty(_currentScheme);
                AssetDatabase.SaveAssets();
                
                Reload(_currentScheme);
            }
            else
            {
                Debug.LogWarning("Can't save scheme because of errors.");
            }
        }

        private DropdownMenuAction.Status CanSaveAndExport()
        {
            if (_currentScheme)
            {
                return DropdownMenuAction.Status.Normal;
            }

            return DropdownMenuAction.Status.Disabled;
        }
    }
}