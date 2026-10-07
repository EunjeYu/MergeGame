// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Data;

namespace MergeIt.Editor.LevelEditor.Commands
{
    public class ChangeCellCommand : ActionCommandBase
    {
        private readonly LevelElementData _previousData;
        private readonly LevelElementData _newData;

        public ChangeCellCommand(LevelEditorWindow window, LevelElementData previousData, LevelElementData newData) 
            : base(window)
        {
            _previousData = previousData.GetClone();
            _newData = newData.GetClone();
        }
        
        public override void Execute()
        {
            Window.ApplyCell(_newData, true);
        }

        public override void Undo()
        {
            Window.UndoApplyCell(_previousData, _newData);
        }
    }
}