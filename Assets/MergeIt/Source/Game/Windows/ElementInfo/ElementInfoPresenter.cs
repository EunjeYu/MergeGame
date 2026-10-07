// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.Schemes;
using MergeIt.Core.Services;
using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Windows;
using MergeIt.Game.Factories.ElementInfo;
using MergeIt.SimpleDI;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.Windows.ElementInfo
{
    public class ElementInfoPresenter : WindowPresenter<ElementInfoWindow, ElementInfoModel>
    {

        private IConfigsService _configsService;

        private readonly Dictionary<ElementInfoItemComponent, ElementConfig> _elementConfigs = new();
        private ElementInfoArgs _elementInfoArgs;
        private IElementInfoFactory _elementInfoFactory;
        private IEvolutionsService _evolutionsService;
        private RectTransform _prevSetUpPanel;
        private ElementConfig _selectedConfig;

        protected override void OnInitialize(IWindowArgs args = null)
        {
            base.OnInitialize(args);

            _elementInfoFactory = DiContainer.Get<IElementInfoFactory>();
            _configsService = DiContainer.Get<IConfigsService>();
            _evolutionsService = DiContainer.Get<IEvolutionsService>();

            _elementInfoArgs = args as ElementInfoArgs;

            if (_elementInfoArgs != null)
            {
                _selectedConfig = _elementInfoArgs.ElementConfig;
                SchemeObject evolutionConfig = _configsService.LevelConfig.EvolutionsScheme;
                EvolutionData evolutionData = evolutionConfig.Evolution.FirstOrDefault(x => x.Chain.Contains(_selectedConfig));

                View.WindowTitleText.text = evolutionData?.Name;

                var elementChain = _evolutionsService.GetEvolutionChain(evolutionData);
                var generatorsChain = _evolutionsService.GetGeneratedBy(_selectedConfig);
                var generates = _evolutionsService.GetGenerates(_selectedConfig);

                bool lockedElements = _selectedConfig.Type == ElementType.Generator && _selectedConfig.GeneratorSettings.GenerateBlocked;

                SetupPanel(View.EvolutionPanel, elementChain, _prevSetUpPanel, evolutionData.Description, evolutionData, true);
                SetupPanel(View.GeneratesPanel, generates, _prevSetUpPanel, "Produce:", lockedItems: lockedElements);
                SetupPanel(View.CreatedFromPanel, generatorsChain, _prevSetUpPanel, "Created from:");

                CalculateScrollSize(View.EvolutionPanel.RectTransform, View.GeneratesPanel.RectTransform, View.CreatedFromPanel.RectTransform);
            }
        }

        protected override void OnDestroyWindow()
        {
            base.OnDestroyWindow();

            foreach (var elementConfig in _elementConfigs)
            {
                if (elementConfig.Key)
                {
                    elementConfig.Key.ClickEvent -= OnClickElement;
                }
            }

            _elementConfigs.Clear();
        }

        private void SetupPanel(ElementInfoItemsPanelComponent panelComponent, List<ElementConfig> elementConfigs,
            RectTransform prevPanelTransform, string panelTitle, EvolutionData evolutionData = null, bool showNumbers = false,
            bool lockedItems = false)
        {
            if (elementConfigs.Count > 0)
            {
                panelComponent.gameObject.SetActive(true);
                panelComponent.SetSpacing(View.SpacingInsidePanels);
                panelComponent.SetTitle(panelTitle);

                SetupPanelSize(panelComponent, elementConfigs.Count, prevPanelTransform);

                for (int i = 0; i < elementConfigs.Count; i++)
                {
                    ElementConfig elementConfig = elementConfigs[i];
                    ElementInfoItemComponent item = null;
                    bool isDiscovered = true;

                    if (evolutionData != null)
                    {
                        int index = evolutionData.Chain.IndexOf(elementConfig) + 1;
                        int progress = _evolutionsService.GetEvolutionProgress(evolutionData.Id);
                        isDiscovered = index <= progress;

                        if (isDiscovered)
                        {
                            item = CreateComponent(elementConfig, lockedItems);
                        }
                        else
                        {
                            item = _elementInfoFactory.CreateUnknownElementWindowItem();
                        }
                    }
                    else
                    {
                        item = CreateComponent(elementConfig, lockedItems);
                    }

                    if (item)
                    {
                        bool isGenerator = isDiscovered && elementConfig.Type == ElementType.Generator;
                        panelComponent.SetItem(item.transform);

                        item.Setup(isGenerator, elementConfig == _selectedConfig, showNumbers ? i + 1 : -1);
                    }
                }

                _prevSetUpPanel = panelComponent.RectTransform;
            }
            else
            {
                panelComponent.gameObject.SetActive(false);
            }
        }

        private ElementInfoItemComponent CreateComponent(ElementConfig elementConfig, bool isLocked = false)
        {
            ElementInfoItemComponent item = _elementInfoFactory.CreateElementWindowItem(elementConfig, isLocked: isLocked);
            item.ClickEvent += OnClickElement;

            _elementConfigs[item] = elementConfig;

            return item;
        }

        private void SetupPanelSize(ElementInfoItemsPanelComponent panel, int itemsCount, RectTransform prevPanelTransform)
        {
            GridLayoutGroup grid = panel.ItemsGrid;
            float cellHeight = grid.cellSize.y;
            int rows = Mathf.CeilToInt((float)itemsCount / grid.constraintCount);

            float resultSize = 0f;
            resultSize += panel.PanelTitle.textInfo.textComponent.rectTransform.rect.height;
            resultSize += View.SpacingInsidePanels;

            float gridSize = 0f;
            gridSize += cellHeight * rows;
            gridSize += grid.spacing.y * (rows - 1);
            gridSize += grid.padding.top + grid.padding.bottom;

            var gridRect = panel.ItemsGrid.GetComponent<RectTransform>();
            Vector2 panelSize = panel.RectTransform.sizeDelta;
            Vector2 gridLayoutSize = gridRect.sizeDelta;

            panelSize.y = resultSize + gridSize;
            gridLayoutSize.y = gridSize;

            gridRect.sizeDelta = gridLayoutSize;
            panel.RectTransform.sizeDelta = panelSize;

            if (prevPanelTransform != default)
            {
                float newPosition = prevPanelTransform.anchoredPosition.y - View.SpacingBetweenPanels - prevPanelTransform.sizeDelta.y;
                panel.RectTransform.anchoredPosition = new Vector2(panel.RectTransform.anchoredPosition.x, newPosition);
            }
        }

        private void CalculateScrollSize(params RectTransform[] panels)
        {
            float sumHeight =
                panels.Sum(x => x.gameObject.activeSelf ? x.sizeDelta.y : 0f) + panels.Length * View.SpacingBetweenPanels;

            View.ScrollContent.sizeDelta = new Vector2(View.ScrollContent.sizeDelta.x, sumHeight);
        }

        private void OnClickElement(ElementInfoItemComponent component)
        {
            if (_elementConfigs.TryGetValue(component, out ElementConfig config) &&
                config != _selectedConfig)
            {
                var args = new ElementInfoArgs
                {
                    ElementConfig = config
                };

                WindowSystem.OpenWindow<ElementInfoPresenter>(true, true, args);
            }
        }
    }
}