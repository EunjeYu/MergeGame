// Copyright (c) 2024, Awessets

using MergeIt.Core.WindowSystem.Windows;
using TMPro;
using UnityEngine;

namespace MergeIt.Game.Windows.ElementInfo
{
    public class ElementInfoWindow : WindowBase
    {
        [SerializeField]
        private TMP_Text _windowTitleText;

        [SerializeField]
        private float _spacingBetweenPanels;

        [SerializeField]
        private float _spacingInsidePanels;

        [SerializeField]
        private ElementInfoItemsPanelComponent _evolutionPanel;

        [SerializeField]
        private ElementInfoItemsPanelComponent _generatesPanel;

        [SerializeField]
        private ElementInfoItemsPanelComponent _createdFromPanel;

        [SerializeField]
        private RectTransform _scrollContent;

        public TMP_Text WindowTitleText
        {
            get => _windowTitleText;
        }

        public float SpacingBetweenPanels
        {
            get => _spacingBetweenPanels;
        }

        public float SpacingInsidePanels
        {
            get => _spacingInsidePanels;
        }

        public ElementInfoItemsPanelComponent EvolutionPanel
        {
            get => _evolutionPanel;
        }

        public ElementInfoItemsPanelComponent GeneratesPanel
        {
            get => _generatesPanel;
        }

        public ElementInfoItemsPanelComponent CreatedFromPanel
        {
            get => _createdFromPanel;
        }

        public RectTransform ScrollContent
        {
            get => _scrollContent;
        }
    }
}