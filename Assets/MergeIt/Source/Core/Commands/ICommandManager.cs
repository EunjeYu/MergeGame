// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Core.Commands
{
    public interface ICommandManager
    {
        event Action<ICommandManager> Finished;
        
        bool Executing { get; }

        void Run();
        void RunSimultaneously();
        void Add(ICommand command);
    }

}