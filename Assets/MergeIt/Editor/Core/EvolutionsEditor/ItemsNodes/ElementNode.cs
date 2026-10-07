// Copyright (c) 2024, Awessets

using System;
using System.IO;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.Schemes.Data;
using MergeIt.Editor.Configs;
using MergeIt.Editor.Configs.ItemsDrawers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VisualElement = UnityEngine.UIElements.VisualElement;

namespace MergeIt.Editor.EvolutionsEditor
{
    public class ElementNode : BaseNode, IElementNode
    {
        private readonly EvolutionsGraphView _graphView;
        private readonly VisualElement _root;
        private readonly ElementConfig _config;
        private readonly string _configName;

        private CommonSettingsDrawer _commonSettingsDrawer;
        private GeneratorSettingsEvolutionDrawer _generatorSettingsDrawer;

        public override string Id
        {
            get => Data?.Id;
        }

        public IElementNodeData Data { get; private set; }

        public ElementConfig Config
        {
            get => _config;
        }

        public ElementNode(ElementNodeData data, Vector2 positionOffset, EvolutionsGraphView graphView)
        {
            _graphView = graphView;
            Data = data;
            _config = data.ElementConfig;
            
            if (_config)
            {
                var style1 = (StyleSheet)EditorGUIUtility.Load(Path.Combine(Constants.EvolutionsEditorResourcesRoot,
                    "GeneratorStyle.uss"));
                
                var titleLabel = titleContainer.Q<Label>("title-label");
                titleLabel.bindingPath = "_commonSettings._name";
                Port evolvedFromPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                    typeof(string));

                evolvedFromPort.portName = Constants.EvolvedFromPort;
                inputContainer.Add(evolvedFromPort);

                Port evolvePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                    typeof(string));
                evolvePort.portName = Constants.EvolvePort;
                outputContainer.Add(evolvePort);

                _root = new VisualElement();

                mainContainer.AddToClassList(StylesConstants.NodeStyles[ElementType.Regular]);
                mainContainer.AddToClassList(StylesConstants.NodeStyles[ElementType.Generator]);

                var typeEnum = new EnumField(_config.Type);
                typeEnum.bindingPath = "_type";
                typeEnum.RegisterValueChangedCallback(OnElementTypeChanged);

                _root.Add(typeEnum);

                Refresh(_config.Type);

                mainContainer.Add(_root);

                Data.Position = new Rect(
                    data.Position.x + positionOffset.x,
                    data.Position.y + positionOffset.y,
                    data.Position.width,
                    data.Position.height);
                
                mainContainer.Bind(new SerializedObject(_config));

                _configName = _config.name;
                _root.styleSheets.Add(style1);
            }
            else
            {
                mainContainer.AddToClassList(StylesConstants.InvalidNode);
                title = "Config was missing";
            }
            
            SetPosition(Data.Position);
        }

        public override void SaveData()
        {
            Data.Position = GetPosition();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            evt.menu.AppendAction("Select in Project View", SelectAction);
            evt.menu.AppendSeparator();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            _generatorSettingsDrawer?.Dispose();
            _commonSettingsDrawer?.Dispose();
            
            mainContainer.Unbind();
        }
        
        public bool Validate()
        {
            if (Data.ElementConfig)
            {
                return true;
            }

            title = $"Lost config: {_configName}";
            mainContainer.AddToClassList(StylesConstants.InvalidNode);
            mainContainer.EnableInClassList(StylesConstants.NodeStyles[ElementType.Regular], false);
            mainContainer.EnableInClassList(StylesConstants.NodeStyles[ElementType.Generator], false);

            mainContainer.SetEnabled(false);
            return false;
        }

        private void OnElementTypeChanged(ChangeEvent<Enum> changeEvent)
        {
            if (changeEvent.newValue == null)
            {
                return;
            }

            if (Enum.TryParse(changeEvent.newValue.ToString(), out ElementType type))
            {
                Debug.Log($"Value changed to ({type}), Target type now is {_config.Type}");

                Refresh(type);
            }
        }

        private void Refresh(ElementType newType)
        {
            if (_commonSettingsDrawer == null)
            {
                _commonSettingsDrawer = new CommonSettingsDrawer(_root, _config);
                _commonSettingsDrawer.Draw();
            }

            switch (newType)
            {
                case ElementType.Regular:
                    mainContainer.EnableInClassList(StylesConstants.NodeStyles[ElementType.Regular], true);
                    mainContainer.EnableInClassList(StylesConstants.NodeStyles[ElementType.Generator], false);

                    _generatorSettingsDrawer?.Hide();

                    break;

                case ElementType.Generator:
                    mainContainer.EnableInClassList(StylesConstants.NodeStyles[ElementType.Regular], false);
                    mainContainer.EnableInClassList(StylesConstants.NodeStyles[ElementType.Generator], true);

                    if (_generatorSettingsDrawer == null)
                    {
                        _generatorSettingsDrawer = new GeneratorSettingsEvolutionDrawer(_root, _config, _graphView);
                        _generatorSettingsDrawer.Draw();
                    }

                    _generatorSettingsDrawer.Show();
                    break;
            }
        }

        private void SelectAction(DropdownMenuAction dropdownMenuAction)
        {
            if (_config != null)
            {
                EditorGUIUtility.PingObject(_config);
            }
        }
    }
}