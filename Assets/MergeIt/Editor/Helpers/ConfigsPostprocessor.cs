// Copyright (c) 2024, Awessets

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MergeIt.Core.Configs.Elements;
using UnityEditor;

namespace MergeIt.Editor.Helpers
{
    public class ConfigsPostprocessor : AssetModificationProcessor
    {
        private async static void OnWillCreateAsset(string assetName)
        {
            string ext = Path.GetExtension(assetName);
            if (ext == ".asset")
            {
                await WaitCreation(assetName);
            }
        }
        
        private async static Task WaitCreation(string path)
        {
            var configType = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (configType != null)
            {
                CheckConfig(configType, path);
            }
            else
            {
                await Task.Yield();

                configType = AssetDatabase.GetMainAssetTypeAtPath(path);
                CheckConfig(configType, path);
            }
        }
        
        private static void CheckConfig(Type configType, string path)
        {
            if (configType != null && configType == typeof(ElementConfig))
            {
                var config = AssetDatabase.LoadAssetAtPath<ElementConfig>(path);

                MethodInfo methodInfo = configType.GetMethod("GenerateGuid", BindingFlags.NonPublic | BindingFlags.Instance);

                methodInfo?.Invoke(config, null);

                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}