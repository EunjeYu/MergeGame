// Copyright (c) 2024, Awessets

using System.IO;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Helpers;
using Newtonsoft.Json;
using UnityEngine;

namespace MergeIt.Game.Services.Saves.Strategies
{
    public class JsonSerializeStrategy : BaseSerializeStrategy
    {
        public async override UniTask Save<T>(T data)
        {
            string fileName = SavesHelper.GetFileNameWithExtension<T>();
            string serialized = JsonConvert.SerializeObject(data);
            
            string path = SaveDirectoryPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, fileName);

            await File.WriteAllTextAsync(path, serialized);
        }
        
        public override T Load<T>()
        {
            string fileName = SavesHelper.GetFileNameWithExtension<T>();
            string loadPath = Path.Combine(SaveDirectoryPath, fileName);
            if (File.Exists(loadPath))
            {
                string fileStream = File.ReadAllText(loadPath);

                var loadedData = JsonConvert.DeserializeObject<T>(fileStream);

                return loadedData;
            }
            
            Debug.Log($"There is no saved {fileName} found.");

            return null;
        }
    }

}