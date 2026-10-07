// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Data;

namespace MergeIt.Editor.LevelEditor.Commands
{
    public class ClearCellCommand : ActionCommandBase
    {
        private readonly LevelElementData _previousData;
        
        public ClearCellCommand(LevelEditorWindow window, LevelElementData previousData) 
            : base(window)
        {
            _previousData = previousData;
        }

        public override void Undo()
        {
            EvolutionSelection.UpdateCell(_previousData);
            Window.UndoClearCell(_previousData);
        }

        public override void Execute()
        {
            Window.ClearCell(_previousData.Position);
        }
    }
}