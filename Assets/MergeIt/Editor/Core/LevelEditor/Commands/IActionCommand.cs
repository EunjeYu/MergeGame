// Copyright (c) 2024, Awessets

namespace MergeIt.Editor.LevelEditor.Commands
{
    public interface IActionCommand
    {
        void Execute();
        void Undo();
    }
}