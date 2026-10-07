// Copyright (c) 2024, Awessets

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.Windows.ElementInfo
{
    public class ElementInfoItemsPanelComponent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _panelTitle;

        [SerializeField]
        private GridLayoutGroup _itemsGrid;

        private VerticalLayoutGroup _layoutGroup;
        private RectTransform _rectTransform;

        public GridLayoutGroup ItemsGrid
        {
            get => _itemsGrid;
        }

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        public TMP_Text PanelTitle
        {
            get => _panelTitle;
        }

        private void Awake()
        {
            _layoutGroup = GetComponent<VerticalLayoutGroup>();
        }

        public void SetSpacing(float spacing)
        {
            _layoutGroup.spacing = spacing;
        }

        public void SetTitle(string titleText)
        {
            _panelTitle.text = titleText;
        }

        public void SetItem(Transform item)
        {
            item.SetParent(_itemsGrid.transform);
            item.localScale = Vector3.one;
        }
    }
}