// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MergeIt.Core.Commands
{
    public class CommandManager : ICommandManager
    {
        public event Action<ICommandManager> Finished;
        public bool Executing { get; private set; }

        private readonly Queue<ICommand> _commands = new Queue<ICommand>();
        private ICommand _currentCommand;

        public void Run()
        {
            Executing = true;
            RunNext();
        }

        public void RunSimultaneously()
        {
            Executing = true;
            if (_commands.Count > 0)
            {
                while (_commands.Count > 0)
                {
                    var currentCommand = _commands.Dequeue();
                    currentCommand.Finished += OnSimultaneousCommandFinished;
                    currentCommand.Execute();
                }
            }
            else
            {
                Finish();
            }
        }
        
        public async UniTask RunAsync()
        {
            Executing = true;
            foreach (ICommand command in _commands)
            {
                _currentCommand = command;
                await _currentCommand.ExecuteAsync();
            }

            Finish();
        }

        public void Add(ICommand command)
        {
            _commands.Enqueue(command);
        }

        private void OnCommandFinished(ICommand command)
        {
            command.Finished -= OnCommandFinished;

            if (_commands.Count == 0)
            {
                Finish();
            }
            else
            {
                RunNext();
            }
        }

        private void OnSimultaneousCommandFinished(ICommand command)
        {
            command.Finished -= OnSimultaneousCommandFinished;

            if (_commands.Count == 0)
            {
                Finish();
            }
        }

        private void RunNext()
        {
            if (_currentCommand != null)
            {
                Debug.Log($"Command manager is busy. Running command {_currentCommand.GetType()}");
                return;
            }
            
            if (_commands.Count > 0)
            {
                _currentCommand = _commands.Dequeue();
                _currentCommand.Finished += OnCommandFinished;
                _currentCommand.Execute();
            }
        }

        private void Finish()
        {
            Executing = false;
            Finished?.Invoke(this);
        }
    }
}