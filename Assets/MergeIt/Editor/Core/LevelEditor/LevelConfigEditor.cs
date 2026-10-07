// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs;
using UnityEditor;
using UnityEngine;

namespace MergeIt.Editor.LevelEditor
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                LevelEditorWindow.Show(target as LevelConfig);
            }
        }
    }
}