// Copyright (c) 2024, Awessets

using MergeIt.Core.Schemes;
using UnityEditor;
using UnityEngine;

namespace MergeIt.Editor.EvolutionsEditor
{
    [CustomEditor(typeof(SchemeObject))]
    public class SchemeObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                var window = EditorWindow.GetWindow<EvolutionsEditorWindow>(true, "Evolutions Editor");

                if (!window.Initialized)
                {
                    window.Initialize();
                }

                window.Show();
                window.Reload(target as SchemeObject);
            }
        }
    }
}