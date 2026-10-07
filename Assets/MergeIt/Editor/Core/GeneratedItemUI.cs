// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MergeIt.Editor
{
    public class GeneratedItemUI : VisualElement, IDisposable
    {
        private static Action<GeneratableElement, int> _changedChanceEvent;
        private static Action<GeneratableElement, int> _changedIndexEvent;
        private static Action<GeneratableElement, ElementConfig> _changedElementEvent;

        public Action<int, GeneratableElement> RemoveGeneratedItemEvent { get; set; }

        public int Index
        {
            set => _changedIndexEvent?.Invoke(_target, value);
        }

        private readonly PopupField<ElementConfig> _elementsConfigPopup;
        private readonly ElementConfig _currentConfig;
        private readonly GeneratableElement _target;
        private readonly Label _indexLabel;
        private readonly IntegerField _chanceField;
        private readonly Button _removeButton;

        private int _index;
        private ElementConfig _selectedElement;
        private List<ElementConfig> _allConfigs;
        private List<EvolutionData> _evolutions;

        public GeneratedItemUI(GeneratableElement item, 
            ElementConfig currentConfig, 
            int index, 
            List<EvolutionData> evolutions,
            List<ElementConfig> configs)
        {
            _currentConfig = currentConfig;
            _allConfigs = configs;
            _evolutions = evolutions;
            
            _target = item;
            _index = index;

            var template = (VisualTreeAsset)EditorGUIUtility.Load(Path.Combine(Constants.InspectorResourcesRoot,
                "GeneratedItemTemplate.uxml"));

            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.paddingTop = new StyleLength(5);

            template.CloneTree(this);

            if (item.Element == null && !_allConfigs.Contains(_selectedElement))
            {
                _selectedElement = _allConfigs.FirstOrDefault(x => x != _currentConfig);
                _target.Element = _selectedElement;
            }
            else
            {
                _selectedElement = item.Element;
            }

            var container = this.Q<VisualElement>("ElementsPopup");
            if (_selectedElement != null)
            {
                _elementsConfigPopup = new PopupField<ElementConfig>("", _allConfigs, _selectedElement);

                _elementsConfigPopup.formatListItemCallback = FormatElementCallback;
                _elementsConfigPopup.formatSelectedValueCallback = FormatElementCallback;

                // _elementsConfigPopup.AddToClassList("genItemList");
                _elementsConfigPopup.RegisterValueChangedCallback(OnElementChanged);

                container.Add(_elementsConfigPopup);

                EditorUtility.SetDirty(_currentConfig);
            }
            else
            {
                container.Add(new Label("Add more configs!"));
            }

            _indexLabel = this.Q<Label>("ItemGenIndex");
            _indexLabel.text = $"{_index + 1}. ";

            _chanceField = this.Q<IntegerField>("ItemChance");
            _removeButton = this.Q<Button>("RemoveButton");

            _removeButton.clicked += OnRemoveClicked;

            _chanceField.value = _target.Possibility;
            _chanceField.RegisterValueChangedCallback(OnChanceChanged);

            _changedChanceEvent += ChanceChanged;
            _changedElementEvent += ElementChanged;
            _changedIndexEvent += IndexChanged;
        }

        private void OnChanceChanged(ChangeEvent<int> evt)
        {
            _changedChanceEvent?.Invoke(_target, evt.newValue);
        }

        private void OnRemoveClicked()
        {
            RemoveGeneratedItemEvent?.Invoke(_index, _target);
        }

        private void OnElementChanged(ChangeEvent<ElementConfig> evt)
        {
            _changedElementEvent?.Invoke(_target, evt.newValue);
        }

        private void IndexChanged(GeneratableElement target, int index)
        {
            if (target == _target)
            {
                _index = index;
                _indexLabel.text = (_index + 1).ToString();
            }
        }

        private string FormatElementCallback(ElementConfig arg)
        {
            List<ElementConfig> chain =
                _evolutions
                    .Where(x => x.Chain.Contains(arg))
                    .Select(x => x.Chain)
                    .FirstOrDefault();
            
            int index = -1;
            if (chain != null)
            {
                index = chain.IndexOf(arg);
            }
            
            string text = index != -1 ? $"{arg.CommonSettings.Name} level {index + 1}" : $"{arg.CommonSettings.Name}";
            return text;
        }

        private void ChanceChanged(GeneratableElement target, int chance)
        {
            if (target == _target)
            {
                _target.Possibility = chance;
                _chanceField.SetValueWithoutNotify(chance);

                EditorUtility.SetDirty(_currentConfig);
            }
        }

        private void ElementChanged(GeneratableElement target, ElementConfig elementConfig)
        {
            if (target == _target)
            {
                if (elementConfig == _currentConfig)
                {
                    EditorWindow.focusedWindow.ShowNotification(new GUIContent("The generator can't generate itself."));
                    _selectedElement = _allConfigs.FirstOrDefault(x => x != _currentConfig);
                }
                else
                {
                    _selectedElement = elementConfig;
                    _target.Element = _selectedElement;
                }

                _elementsConfigPopup.SetValueWithoutNotify(_selectedElement);
                EditorUtility.SetDirty(_currentConfig);
            }
        }

        public void Dispose()
        {
            _chanceField.UnregisterValueChangedCallback(OnChanceChanged);
            _removeButton.clicked -= OnRemoveClicked;

            _changedChanceEvent -= ChanceChanged;
            _changedElementEvent -= ElementChanged;
            _changedIndexEvent -= IndexChanged;
        }
    }
}