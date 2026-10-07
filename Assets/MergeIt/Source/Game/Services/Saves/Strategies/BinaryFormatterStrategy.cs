// Copyright (c) 2024, Awessets

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Helpers;
using UnityEngine;

namespace MergeIt.Game.Services.Saves.Strategies
{
    public class BinaryFormatterStrategy : BaseSerializeStrategy
    {
        private readonly BinaryFormatter _formatter = new();

        public async override UniTask Save<T>(T data)
        {
            using var memoryStream = new MemoryStream();

            _formatter.Serialize(memoryStream, data);

            byte[] serializedData = memoryStream.ToArray();
            string fileName = SavesHelper.GetFileNameWithExtension<T>();

            await WriteSerializedDataAsync(serializedData, fileName);
        }
        
        public override T Load<T>()
        {
            string fileName = SavesHelper.GetFileNameWithExtension<T>();
            string loadPath = Path.Combine(SaveDirectoryPath, fileName);
            if (File.Exists(loadPath))
            {
                FileStream fileStream = File.Open(loadPath, FileMode.Open);

                var loadedData = _formatter.Deserialize(fileStream) as T;

                fileStream.Close();

                return loadedData;
            }
            
            Debug.Log($"There is no saved {fileName} found.");

            return null;
        }
        
        private async UniTask WriteSerializedDataAsync(byte[] bytes, string fileName)
        {
            string path = SaveDirectoryPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, fileName);

            await using var fileStream =
                new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);

            await fileStream.WriteAsync(bytes, 0, bytes.Length);
        }
    }

}