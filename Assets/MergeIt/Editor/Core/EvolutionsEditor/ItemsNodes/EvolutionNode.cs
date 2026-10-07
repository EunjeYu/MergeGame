// Copyright (c) 2024, Awessets

using System.IO;
using MergeIt.Core.Schemes.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace MergeIt.Editor.EvolutionsEditor
{
    public class EvolutionNode : BaseNode, IEvolutionNode
    {
        public string Name { get; }
        public string Description { get; }
        public bool Discovered { get; }

        public override string Id
        {
            get => Data?.Id;
        }
        
        public IEvolutionNodeData Data { get; } 

        private readonly Port _evolutionRootPort;
        private TextField _nameField;
        private TextField _descField;
        private Toggle _discoveredToggle;

        public EvolutionNode(EvolutionNodeData data)
        {
            Data = data;
            
            var visualTreeAsset =
                (VisualTreeAsset)EditorGUIUtility.Load(Path.Combine(Constants.EvolutionsEditorResourcesRoot, "EvolutionNode.uxml"));

            mainContainer.AddToClassList("nodeBorderEvolution");

            var root = visualTreeAsset.CloneTree();
            _nameField = root.Q<TextField>("Name");
            _descField = root.Q<TextField>("Description");
            _discoveredToggle = root.Q<Toggle>("Discovered");

            title = Data.Name;
            _nameField.SetValueWithoutNotify(Data.Name);
            _descField.SetValueWithoutNotify(Data.Description);
            _discoveredToggle.SetValueWithoutNotify(Data.Discovered);
            
            _nameField.RegisterValueChangedCallback(OnEvoNameTextChanged);
            _descField.RegisterValueChangedCallback(OnEvoDescriptionTextChanged);
            _discoveredToggle.RegisterValueChangedCallback(OnDiscoveredSelected);
            
            mainContainer.Add(root);
            
            topContainer.Remove(inputContainer);
            _evolutionRootPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string));
            _evolutionRootPort.portName = Constants.EvolutionRootPort;
            outputContainer.Add(_evolutionRootPort);
        }

        private void OnEvoNameTextChanged(ChangeEvent<string> evt)
        {
            Data.Name = evt.newValue;
            title = evt.newValue;
        }

        private void OnEvoDescriptionTextChanged(ChangeEvent<string> evt)
        {
            Data.Description = evt.newValue;
        }

        private void OnDiscoveredSelected(ChangeEvent<bool> evt)
        {
            Data.Discovered = evt.newValue;
            
        }

        public override void SaveData()
        {
            Data.Position = GetPosition();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            _nameField.UnregisterValueChangedCallback(OnEvoNameTextChanged);
            _descField.UnregisterValueChangedCallback(OnEvoDescriptionTextChanged);
            _discoveredToggle.UnregisterValueChangedCallback(OnDiscoveredSelected);
        }
    }
}