// Copyright (c) 2024, Awessets

namespace MergeIt.Editor.LevelEditor.Commands
{
    public abstract class ActionCommandBase : IActionCommand
    {
        protected LevelEditorWindow Window;
        
        public ActionCommandBase(LevelEditorWindow window)
        {
            Window = window;
        }
        
        public abstract void Execute();

        public abstract void Undo();
    }
}