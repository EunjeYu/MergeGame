// Copyright (c) 2024, Awessets

using MergeIt.Core.Helpers;
using UnityEditor;
using UnityEngine;

namespace MergeIt.Editor.Helpers
{
    [CustomEditor( typeof( SlicedFilledImage ) ), CanEditMultipleObjects]
    public class SlicedFilledImageEditor : UnityEditor.Editor
    {
        private SerializedProperty _spriteProp, _colorProp;
        private GUIContent _spriteLabel;

        private void OnEnable()
        {
            _spriteProp = serializedObject.FindProperty( "m_Sprite" );
            _colorProp = serializedObject.FindProperty( "m_Color" );
            _spriteLabel = new GUIContent( "Source Image" );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField( _spriteProp, _spriteLabel );
            EditorGUILayout.PropertyField( _colorProp );
            DrawPropertiesExcluding( serializedObject, "m_Script", "m_Sprite", "m_Color", "m_OnCullStateChanged" );

            serializedObject.ApplyModifiedProperties();
        }
    }
}