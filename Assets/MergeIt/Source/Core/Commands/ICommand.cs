// Copyright (c) 2024, Awessets

using System;
using Cysharp.Threading.Tasks;

namespace MergeIt.Core.Commands
{
    public interface ICommand
    {
        event Action<ICommand> Finished; 
        void Execute();
        UniTask ExecuteAsync();
    }
}