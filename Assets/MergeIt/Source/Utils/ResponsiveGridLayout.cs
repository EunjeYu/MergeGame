// Copyright (c) 2024, Awessets

using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Core.Utils
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class ResponsiveGridLayout : MonoBehaviour
    {
        private GridLayoutGroup _gridLayout;
        private RectTransform _parentRectTransform;

        private bool _initialized;
        private void Awake()
        {
            _gridLayout = GetComponent<GridLayoutGroup>();
            _parentRectTransform = _gridLayout.transform.parent.GetComponent<RectTransform>();

            _initialized = true;
            
            AdjustCellSize();
        }

        private void AdjustCellSize()
        {
            int columns = _gridLayout.constraintCount;
            float containerWidth = _parentRectTransform.rect.width - (_gridLayout.padding.left + _gridLayout.padding.right) -
                                    _gridLayout.spacing.x * (columns - 1);
            
            float cellSize = containerWidth / columns;

            _gridLayout.cellSize = new Vector2(cellSize, cellSize);
        }

        private void OnRectTransformDimensionsChange()
        {
            if (_initialized)
            {
                AdjustCellSize();
            }
        }
    }
}