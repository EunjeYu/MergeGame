// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Schemes;
using MergeIt.Core.Schemes.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MergeIt.Editor.EvolutionsEditor
{
    public class EvolutionsGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<EvolutionsGraphView, EvolutionsGraphViewTraits>
        {
        }

        protected override bool canCopySelection
        {
            get => selection.TrueForAll(x => x is IEvolutionNode);
        }
        
        protected override bool canDuplicateSelection
        {
            get => selection.TrueForAll(x => x is IEvolutionNode);
        }

        private EventBase _currentEvent;
        private List<EvolutionData> _evolutionsData;
        private List<ElementConfig> _elementConfigs = new();

        internal List<EvolutionData> EvolutionsData
        {
            get => _evolutionsData;
        }

        internal List<ElementConfig> Configs
        {
            get => _elementConfigs;
        }

        public EvolutionsGraphView()
        {
            serializeGraphElements = OnSerializeGraphElements;
            canPasteSerializedData = OnCanPasteSerializedData;
            unserializeAndPaste = OnUnserializeAndPaste;
            deleteSelection = OnDeleteSelection;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is GraphView)
            {
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Paste", _ => PasteCallback(),
                    _ => canPaste ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendAction("Create evolution root", CreateEvolutionRoot);
            }

            if (evt.target is ISaveNode)
            {
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Copy", _ => CopySelectionCallback(),
                    _ => canCopySelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendAction("Duplicate", a => DuplicateSelectionCallback(),
                    a =>
                        canDuplicateSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendAction("Delete", _ => DeleteSelectionCallback(AskUser.DontAskUser),
                    _ => canDeleteSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendSeparator();
            }
        }

        public void Validate()
        {
            nodes.ForEach(x =>
            {
                if (x is IElementNode elementNode)
                {
                    elementNode.Validate();
                }
            });
        }

        private void CreateEvolutionRoot(DropdownMenuAction action)
        {
            var evolutionNodeData = new EvolutionNodeData();
            Vector2 position = GetLocalMousePosition(action.eventInfo.mousePosition);
            var rect = new Rect(position.x, position.y, 160f, 50f);
            evolutionNodeData.Position = rect;

            CreateEvolutionNode(evolutionNodeData);
        }

        private void CreateEvolutionNode(EvolutionNodeData data)
        {
            var evolutionNode = new EvolutionNode(data);
            evolutionNode.SetPosition(data.Position);

            AddElement(evolutionNode);
        }

        private Vector2 GetLocalMousePosition(Vector2 mousePosition)
        {
            var worldPosition = (mousePosition - (Vector2)viewTransform.position) * (1 / scale);
            return worldPosition;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatibleAnchors = new List<Port>();

            foreach (Port candidateAnchor in ports.ToList())
            {
                if (startPort == candidateAnchor ||
                    startPort.node == candidateAnchor.node ||
                    startPort.direction == candidateAnchor.direction ||
                    startPort.portType != candidateAnchor.portType)
                {
                    continue;
                }

                compatibleAnchors.Add(candidateAnchor);
            }

            return compatibleAnchors;
        }

        public SchemeData Save()
        {
            var allElements = graphElements.ToList();

            var allEdges = allElements.OfType<Edge>();
            var allNodes = allElements.OfType<ISaveNode>();

            var elementsToSave = new List<ElementNodeData>();
            var evolutionsToSave = new List<EvolutionNodeData>();
            var edgesToSave = new List<EdgeData>();

            var brokenNodes = new List<ElementNode>();

            foreach (ISaveNode saveNode in allNodes)
            {
                if (saveNode is ElementNode elementNode)
                {
                    if (!elementNode.Validate())
                    {
                        brokenNodes.Add(elementNode);
                    }
                }
            }

            if (brokenNodes.Count > 0)
            {
                ClearSelection();

                foreach (ElementNode brokenNode in brokenNodes)
                {
                    AddToSelection(brokenNode);
                }

                EditorWindow.focusedWindow.ShowNotification(new GUIContent($"Can't save scheme with broken node. Broken nodes was selected."));
                return null;
            }

            foreach (ISaveNode saveNode in allNodes)
            {
                saveNode.SaveData();
                switch (saveNode)
                {
                    case IElementNode elementNode:
                        elementsToSave.Add(elementNode.Data as ElementNodeData);
                        break;

                    case IEvolutionNode evolutionNode:
                        evolutionsToSave.Add(evolutionNode.Data as EvolutionNodeData);
                        break;
                }
            }

            foreach (Edge edge in allEdges)
            {
                var edgeData = new EdgeData();
                edgeData.InputPortName = edge.input.portName;
                edgeData.OutputPortName = edge.output.portName;

                if (edge.input.node is ISaveNode saveNode)
                {
                    edgeData.InputNodeID = saveNode.Id;
                }

                if (edge.output.node is ISaveNode outputNode)
                {
                    edgeData.OutputNodeID = outputNode.Id;
                }

                edgesToSave.Add(edgeData);
            }

            List<EvolutionData> evolutionsData = BakeEvolutionsData();
            var saveData = new SchemeData
            {
                ElementsNodesData = elementsToSave,
                EvolutionsNodesData = evolutionsToSave,
                Edges = edgesToSave,
                EvolutionsData = evolutionsData
            };

            return saveData;
        }

        public List<EvolutionData> BakeEvolutionsData()
        {
            var allElements = graphElements.ToList();
            var evolutionsNodes = allElements.OfType<IEvolutionNode>();
            _evolutionsData = new List<EvolutionData>();

            foreach (IEvolutionNode itemNode in evolutionsNodes)
            {
                Port startPort = itemNode.GetPort(Constants.EvolutionRootPort);
                if (startPort.connected)
                {
                    var evoData = new EvolutionData(itemNode.Data.Id,
                        itemNode.Data.Name,
                        itemNode.Data.Description,
                        itemNode.Data.Discovered);

                    IElementNode firstNode = startPort.connections.First().input.node as IElementNode;
                    FillChain(evoData.Chain, firstNode);

                    _evolutionsData.Add(evoData);
                }
            }

            ExtractConfigs();

            return _evolutionsData;
        }

        private void FillChain(List<ElementConfig> chain, IElementNode node)
        {
            chain.Add(node.Config);

            Port evolvePort = node.GetPort(Constants.EvolvePort);
            if (evolvePort.connected)
            {
                var nextNode = evolvePort.connections.First().input.node as IElementNode;

                FillChain(chain, nextNode);
            }
        }

        public void Restore(SchemeData savedGraph)
        {
            _evolutionsData = savedGraph.EvolutionsData;

            ExtractConfigs();

            DeleteElements(nodes.ToList(), edges.ToList());

            List<ElementNodeData> elementsNodesData = savedGraph.ElementsNodesData;
            List<EvolutionNodeData> evolutionNodesData = savedGraph.EvolutionsNodesData;
            List<EdgeData> edgesData = savedGraph.Edges;

            if (elementsNodesData != null)
            {
                CreateNodes(elementsNodesData);
            }

            if (evolutionNodesData != null)
            {
                CreateEvolutionsNodes(evolutionNodesData, CreateEvolutionNode);
            }

            if (edgesData != null)
            {
                var allNodes = nodes.ToList().OfType<ISaveNode>();

                foreach (EdgeData edgeData in edgesData)
                {
                    ISaveNode inputNode = null;
                    ISaveNode outputNode = null;
                    foreach (ISaveNode itemNode in allNodes)
                    {
                        if (itemNode.Id == edgeData.InputNodeID)
                        {
                            inputNode = itemNode;
                        }

                        if (itemNode.Id == edgeData.OutputNodeID)
                        {
                            outputNode = itemNode;
                        }

                        if (outputNode != null && inputNode != null)
                        {
                            break;
                        }
                    }

                    if (outputNode != null && inputNode != null)
                    {
                        var outputPort = outputNode.GetPort(edgeData.OutputPortName);
                        var inputPort = inputNode.GetPort(edgeData.InputPortName);

                        if (outputPort != null && inputPort != null)
                        {
                            Edge newEdge = inputPort.ConnectTo(outputPort);
                            inputPort.Connect(newEdge);
                            outputPort.Connect(newEdge);

                            AddElement(newEdge);
                        }
                    }
                }
            }
        }

        private void ExtractConfigs()
        {
            _elementConfigs.Clear();
            List<ElementConfig> configs = _evolutionsData.SelectMany(x => x.Chain).ToList();

            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];
                if (config)
                {
                    _elementConfigs.Add(config);
                }
            }
        }

        private string OnSerializeGraphElements(IEnumerable<GraphElement> elements)
        {
            var selectedEvolutions = elements.OfType<IEvolutionNode>();
            var evolutionsToSave = new List<EvolutionNodeData>();

            foreach (IEvolutionNode selectedEvolution in selectedEvolutions)
            {
                selectedEvolution.SaveData();
                evolutionsToSave.Add(selectedEvolution.Data.Copy<EvolutionNodeData>());
            }

            var saveData = new SerializedElementNodes
            {
                EvolutionNodesData = evolutionsToSave,
            };

            string json = JsonUtility.ToJson(saveData, false);

            return json;
        }

        private bool OnCanPasteSerializedData(string data)
        {
            try
            {
                JsonUtility.FromJson<SerializedElementNodes>(data);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void OnUnserializeAndPaste(string operationName, string copyPasteData)
        {
            var deserializedData = JsonUtility.FromJson<SerializedElementNodes>(copyPasteData);
            
            if (deserializedData != null)
            {
                if (deserializedData.EvolutionNodesData != null)
                {
                    CreateEvolutionsNodes(deserializedData.EvolutionNodesData, CreateEvolutionNode, 50f);
                }
            }
        }

        private void OnDeleteSelection(string operationName, AskUser askUser)
        {
            var selectedNodes = selection.OfType<Node>().ToList();
            var selectedEdges = selection.OfType<Edge>().ToList();

            DeleteElements(selectedNodes, selectedEdges);
        }

        private void DeleteElements(List<Node> nodesToDelete, List<Edge> edgesToDelete)
        {
            if (nodesToDelete.Count > 0)
            {
                var allEdges = edges.ToList();
                for (int i = nodesToDelete.Count - 1; i >= 0; i--)
                {
                    Node node = nodesToDelete[i];

                    var nodeEdgesToDelete = allEdges
                        .Select(x => x)
                        .Where(x => x.input.node == node || x.output.node == node)
                        .ToList();

                    for (int j = nodeEdgesToDelete.Count - 1; j >= 0; j--)
                    {
                        var edge = nodeEdgesToDelete[j];
                        edge.input.Disconnect(edge);
                        edge.output.Disconnect(edge);
                        
                        RemoveElement(edge);
                    }

                    if (node is BaseNode baseNode)
                    {
                        baseNode.Dispose();
                    }
                    
                    RemoveElement(node);
                }
            }

            if (edgesToDelete.Count > 0)
            {
                for (int i = edgesToDelete.Count - 1; i >= 0; i--)
                {
                    var edge = edgesToDelete[i];
                    var input = edge.input;
                    var output = edge.output;

                    input.Disconnect(edge);
                    output.Disconnect(edge);

                    RemoveElement(edge);
                }
            }
        }

        private void CreateEvolutionsNodes(List<EvolutionNodeData> elements, Action<EvolutionNodeData> createCallback, float positionOffset = 0f)
        {
            foreach (EvolutionNodeData noteData in elements)
            {
                EvolutionNodeData newData = noteData;
                newData.Position =
                    new Rect(newData.Position.position + new Vector2(positionOffset, positionOffset), newData.Position.size);

                createCallback(newData);
            }
        }

        private void CreateNodes(List<ElementNodeData> elements, Vector2 positionOffset = default)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                var nodeData = elements[i];

                CreateNode(nodeData, positionOffset * (i + 1));
            }
        }

        private void CreateNode(ElementNodeData data, Vector2 positionOffset = default)
        {
            var newNode = new ElementNode(data, positionOffset, this);

            AddElement(newNode);
            AddToSelection(newNode);
        }
        
        protected override void ExecuteDefaultAction(EventBase evt)
        {
            base.ExecuteDefaultAction(evt);

            if (_currentEvent is DragPerformEvent)
            {
                _currentEvent = evt;
                return;
            }

            _currentEvent = evt;

            switch (evt)
            {
                case DragUpdatedEvent _:
                    var dragDrop = DragAndDrop.objectReferences;
                    if (dragDrop.All(x => x is ElementConfig))
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    }

                    break;

                case DragPerformEvent dragPerformEvent:

                    var type = dragPerformEvent.target.GetType();
                    var typeName = type.Name;

                    if (typeName == "ObjectFieldDisplay")
                    {
                        return;
                    }

                    var mousePosition = GetLocalMousePosition(dragPerformEvent.localMousePosition);

                    var rect = new Rect(mousePosition.x, mousePosition.y, 100, 100);
                    var dragDropObjects = DragAndDrop.objectReferences;
                    if (dragDropObjects.All(x => x is ElementConfig))
                    {
                        List<ElementNodeData> elementNodesData =
                            dragDropObjects.Where(x => x is ElementConfig).Select(x =>
                            {
                                var config = x as ElementConfig;
                                var nodeData = new ElementNodeData
                                {
                                    ElementConfig = config,
                                    Position = rect
                                };

                                return nodeData;
                            }).ToList();

                        if (elementNodesData.Count > 0)
                        {
                            CreateNodes(elementNodesData, Constants.NodeCreationOffset);
                        }
                    }

                    break;
            }
        }
        
        public void ClearNodes()
        {
            foreach (Node node in nodes)
            {
                if (node is BaseNode saveNode)
                {
                    saveNode.Dispose();
                }
            }
        }
    }
}