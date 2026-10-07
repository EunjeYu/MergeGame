// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Saves;

namespace MergeIt.Core.Services
{
    public interface IGameSaveService
    {
        UniTask Save(GameSaveType saveType);
    }
}