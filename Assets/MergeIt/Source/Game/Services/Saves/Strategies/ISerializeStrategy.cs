// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Saves;

namespace MergeIt.Game.Services.Saves.Strategies
{
    public interface ISerializeStrategy
    {
        string SaveDir { get; }
        
        UniTask Save<T>(T data) where T : class, ISavable;
        T Load<T>() where T : class, ISavable;
    }
}