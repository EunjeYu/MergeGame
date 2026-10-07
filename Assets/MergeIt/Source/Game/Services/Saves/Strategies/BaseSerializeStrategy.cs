// Copyright (c) 2024, Awessets

using System.IO;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Saves;
using MergeIt.SimpleDI.ReservedInterfaces;
using UnityEngine;

namespace MergeIt.Game.Services.Saves.Strategies
{
    public abstract class BaseSerializeStrategy : ISerializeStrategy, IInitializable
    {
        public virtual string SaveDir => "Saves";

        protected string SaveDirectoryPath;
        
        public void Initialize()
        {
            SaveDirectoryPath = Path.Combine(Application.persistentDataPath, SaveDir);
        }

        public abstract UniTask Save<T>(T data) where T : class, ISavable;

        public abstract T Load<T>() where T : class, ISavable;
        
    }
}