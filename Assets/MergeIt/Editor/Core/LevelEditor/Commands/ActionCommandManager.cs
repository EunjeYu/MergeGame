// Copyright (c) 2024, Awessets

using MergeIt.Editor.Core.LevelEditor.Commands;

namespace MergeIt.Editor.LevelEditor.Commands
{
    public class ActionCommandManager : IActionCommandManager
    {
        private readonly LimitedStack<IActionCommand> _undoStack = new(10);
        private readonly LimitedStack<IActionCommand> _redoStack = new(10);

        public void ExecuteCommand(IActionCommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
        }

        public void Undo()
        {
            if (_undoStack.Any())
            {
                var command = _undoStack.Pop();
                command.Undo();
                
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoStack.Any())
            {
                var command = _redoStack.Pop();
                command.Execute();
                
                _undoStack.Push(command);
            }
        }
    }
}