// Copyright (c) 2024, Awessets

using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MergeIt.Editor.Helpers
{
    public class SavesHelper : UnityEditor.Editor
    {
        [MenuItem("Tools/Merge Toolkit/Clear saves", false, 4)]
        public static void ClearSaves()
        {
            string path = Path.Combine(Application.persistentDataPath, "Saves");
            string oldPath = Path.Combine(Application.streamingAssetsPath, "Saves");

            if (Directory.Exists(path))
            {
                ClearDirectory(path);
            }
            
            if (Directory.Exists(oldPath))
            {
                ClearDirectory(oldPath);
                AssetDatabase.Refresh();
            }
        }

        private static void ClearDirectory(string path)
        {
            Directory.Delete(path, true);
            string meta = Path.ChangeExtension(path, "meta");

            if (File.Exists(meta))
            {
                File.Delete(meta);
            }
        }
    }
}