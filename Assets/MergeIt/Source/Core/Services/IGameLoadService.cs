// Copyright (c) 2024, Awessets

using MergeIt.Core.Saves;

namespace MergeIt.Core.Services
{
    public interface IGameLoadService
    {
        T Load<T>() where T : class, ISavable;
    }
}