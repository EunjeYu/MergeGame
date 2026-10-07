// Copyright (c) 2024, Awessets

namespace MergeIt.Editor.LevelEditor.Commands
{
    public interface IActionCommandManager
    {
        void ExecuteCommand(IActionCommand command);
        void Undo();
        void Redo();
    }
}