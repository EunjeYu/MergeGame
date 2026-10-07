// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using UnityEngine;

namespace MergeIt.Editor.LevelEditor
{
    public class EvolutionSelection
    {
        public static EvolutionData Evolution => _evolution;
        public static ElementConfig EvoItem => _elementData.Element;
        public static LevelElementData ElementData => _elementData;

        private static LevelElementData _elementData;
        private static List<EvolutionData> _evolutions;
        private static EvolutionData _evolution;

        public static void Init(List<EvolutionData> evolutions)
        {
            _evolutions = evolutions;

            var firstEvo = _evolutions.First();
            var firstEvoItem = firstEvo.Chain.First();

            _evolution = firstEvo;

            _elementData = new LevelElementData
            {
                EvolutionId = firstEvo.Id,
                Element = firstEvoItem,
            };
        }

        public static void SelectCell(int row, int column, LevelElementData itemData = null)
        {
            UpdateCell(itemData, GridPoint.Create(row, column));
        }

        public static void LockCell(bool isLocked)
        {
            _elementData.IsBlocked = isLocked;
        }

        public static void UpdateCell(LevelElementData itemData, GridPoint newPosition = default, bool copyPosition = true)
        {
            if (itemData != null)
            {
                _elementData.CopyFrom(itemData, copyPosition);
                _evolution = _evolutions.FirstOrDefault(x => x.Id == itemData.EvolutionId);
            }
            else
            {
                _elementData.Position = newPosition;
            }
        }

        public static void SelectEvolution(EvolutionData evolutionData)
        {
            _evolution = evolutionData;

            _elementData.EvolutionId = _evolution.Id;

            if (!_evolution.Chain.Contains(_elementData.Element))
            {
                _elementData.Element = _evolution.Chain.FirstOrDefault();
            }
        }

        public static void SelectElement(ElementConfig evolutionElement)
        {
            var evolution = Evolution;

            if (evolution.Chain.Contains(evolutionElement))
            {
                _elementData.Element = evolutionElement;
            }
            else
            {
                Debug.LogWarning($"Evolution doesn't contain item with id: ({evolutionElement})!");
            }
        }

        public static void ClearSelection(bool force = false)
        {
            if (force)
            {
                _elementData = null;
            }
        }
    }
}