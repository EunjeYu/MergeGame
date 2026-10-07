// Copyright (c) 2024, Awessets

using System;
using System.Diagnostics;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;

namespace MergeIt.Core.Configs.Data
{
    [Serializable, DebuggerDisplay("Position={Position}, Element={Element?.Id}, Evo={Evolution?.Name}")]
    public class LevelElementData
    {
        public string EvolutionId;
        public ElementConfig Element;
        public bool IsBlocked;
        public GridPoint Position;

        public void CopyFrom(LevelElementData itemData, bool copyPosition = true)
        {
            EvolutionId = itemData.EvolutionId;
            Element = itemData.Element;
            IsBlocked = itemData.IsBlocked;

            if (copyPosition)
            {
                Position = itemData.Position.Copy();
            }
        }

        public LevelElementData GetClone(bool copyPosition = true)
        {
            var fieldCellData = new LevelElementData();
            fieldCellData.CopyFrom(this, copyPosition);

            return fieldCellData;
        }
    }
}