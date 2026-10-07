// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Data;

namespace MergeIt.Editor.LevelEditor.Commands
{
    public class ApplyCellCommand : ActionCommandBase
    {
        private readonly LevelElementData _newData;
        public ApplyCellCommand(LevelEditorWindow window, LevelElementData newData) 
            : base(window)
        {
            _newData = newData;
        }
        
        public override void Execute()
        {
            Window.ApplyCell(_newData, false);
        }

        public override void Undo()
        {
            Window.UndoApplyCell(null, _newData);
        }
    }
}