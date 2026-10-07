// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Editor.Configs;
using UnityEditor;
using UnityEngine.UIElements;
using VisualElement = UnityEngine.UIElements.VisualElement;

namespace MergeIt.Editor
{
    public class GeneratedItemsCreator : IDisposable
    {
        private static Action<ElementConfig, int> _clickAddNewGeneratedItem;
        private static Action<ElementConfig, int, GeneratableElement> _clickRemoveGeneratedItem;
        private static Action<ElementConfig> _clickClearAllGeneratedItems;

        private readonly VisualElement _itemsContainer;
        private readonly ElementGeneratorSettings _settings;
        private readonly ElementConfig _config;
        private readonly List<EvolutionData> _evolutions;
        private readonly List<ElementConfig> _configs;
        private readonly List<GeneratedItemUI> _generatedItemUis;
        private readonly Button _addButton;
        private readonly Button _clearAllButton;
        
        public GeneratedItemsCreator(VisualElement genItemsControls, VisualElement genItemsContainer, ElementConfig config,
            List<EvolutionData> evolutions, List<ElementConfig> configs)
        {
            _config = config;
            _evolutions = evolutions;
            _configs = configs;
            _itemsContainer = genItemsContainer;
            _settings = config.GeneratorSettings;
            _generatedItemUis = new List<GeneratedItemUI>();

            _addButton = genItemsControls.Q<Button>("CreateGenerateItem");
            _addButton.clicked += OnClickAddNewGeneratedItem;

            _clearAllButton = genItemsControls.Q<Button>("ClearGenerateItem");
            _clearAllButton.clicked += OnClickClearAllGeneratedItems;

            _clickAddNewGeneratedItem += CreateNewGeneratedElementUI;
            _clickRemoveGeneratedItem += RemoveGeneratedItemUI;
            _clickClearAllGeneratedItems += ClearAllGeneratedItems;
            
            if (_configs.Count == 0)
            {
                var helpBox = new HelpBox("There are no saved evolutions.",
                    HelpBoxMessageType.Warning);
                
                _itemsContainer.Add(helpBox);

                _addButton.style.display = StylesConstants.DisplayNone;
                _clearAllButton.style.display = StylesConstants.DisplayNone;
            }
        }

        public void Generate()
        {
            var generateItems = _settings.GenerateItems;
            if (generateItems?.Count > 0)
            {
                for (int i = 0; i < generateItems.Count; i++)
                {
                    CreateGeneratedElementUI(i, generateItems[i]);
                }
            }
        }

        private void OnClickAddNewGeneratedItem()
        {
            var newItem = new GeneratableElement();

            _settings.GenerateItems.Add(newItem);
            EditorUtility.SetDirty(_config);

            _clickAddNewGeneratedItem?.Invoke(_config, _settings.GenerateItems.Count - 1);
        }

        private void OnClickRemoveGeneratedItem(int index, GeneratableElement element)
        {
            _settings.GenerateItems.Remove(element);
            EditorUtility.SetDirty(_config);

            _clickRemoveGeneratedItem?.Invoke(_config, index, element);
        }

        private void OnClickClearAllGeneratedItems()
        {
            _settings.GenerateItems?.Clear();
            _clickClearAllGeneratedItems?.Invoke(_config);
        }

        private void CreateGeneratedElementUI(int index, GeneratableElement generatableElement)
        {
            if (_configs.Count == 1 &&
                _configs.First() == _config)
            {
                var helpBox = new HelpBox($"Evolutions have only this element {_config.name}",
                    HelpBoxMessageType.Warning);
                
                _itemsContainer.Add(helpBox);
                return;    
            }

            if (generatableElement.Element && !_configs.Contains(generatableElement.Element))
            {
                var helpBox = new HelpBox($"Add {generatableElement.Element.name} to the current evolution scheme and save it.",
                    HelpBoxMessageType.Warning);
                
                _itemsContainer.Add(helpBox);
                return;
            }
            
            var generatableUI = new GeneratedItemUI(generatableElement, _config, index, _evolutions, _configs);
            generatableUI.RemoveGeneratedItemEvent += OnClickRemoveGeneratedItem;

            _generatedItemUis.Add(generatableUI);
            _itemsContainer.Add(generatableUI);
        }

        private void CreateNewGeneratedElementUI(ElementConfig config, int index)
        {
            if (_config == config)
            {
                CreateGeneratedElementUI(index, _settings.GenerateItems.Last());
            }
        }

        private void RemoveGeneratedItemUI(ElementConfig config, int index, GeneratableElement element)
        {
            if (_config == config)
            {
                _generatedItemUis[index].RemoveGeneratedItemEvent -= OnClickRemoveGeneratedItem;
                _generatedItemUis.RemoveAt(index);

                _itemsContainer.RemoveAt(index);

                for (int i = 0; i < _generatedItemUis.Count; i++)
                {
                    var genItem = _generatedItemUis[i];
                    genItem.Index = i;
                }
            }
        }

        private void ClearAllGeneratedItems(ElementConfig config)
        {
            if (_config == config)
            {
                ClearVisualItems();
            }
        }

        private void ClearVisualItems()
        {
            for (int i = 0; i < _generatedItemUis.Count; i++)
            {
                _generatedItemUis[i].RemoveGeneratedItemEvent -= OnClickRemoveGeneratedItem;
            }

            _generatedItemUis.Clear();
            _itemsContainer.Clear();
        }

        public void Clear()
        {
            ClearVisualItems();

            _clearAllButton.clicked -= OnClickClearAllGeneratedItems;
            _addButton.clicked -= OnClickAddNewGeneratedItem;

            _clickAddNewGeneratedItem -= CreateNewGeneratedElementUI;
            _clickRemoveGeneratedItem -= RemoveGeneratedItemUI;
            _clickClearAllGeneratedItems -= ClearAllGeneratedItems;
        }

        public void Dispose()
        {
            Clear();
        }
    }
}