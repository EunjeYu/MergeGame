// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Commands;
using MergeIt.Core.Configs.Windows;
using MergeIt.Core.Messages;
using MergeIt.Core.WindowSystem.Commands;
using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Factory;
using MergeIt.Core.WindowSystem.Messages;
using MergeIt.Core.WindowSystem.Windows;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;
using UnityEngine;

namespace MergeIt.Core.WindowSystem
{
    public class WindowsSystem : IWindowSystem, IInitializable, IDisposable
    {
        private const string WindowsLayersConfigPath = "Configs/Windows/LayersConfig";

        private readonly Dictionary<string, LinkedList<IWindowPresenter>> _layersWindows = new();
        private readonly Dictionary<IWindowPresenter, IWindowOpenParameters> _openedWindows = new();
        private readonly Queue<ICommandManager> _commandsQueue = new();

        [Introduce]
        private IWindowFactory _windowFactory;

        [Introduce]
        private IMessageBus _messageBus;

        private LayersConfig _layersConfig;
        private RectTransform _root;
        private BlackoutComponent _blackout;
        private ICommandManager _currentCommands;
        private string[] _layers;

        public RectTransform Root
        {
            get
            {
                if (!_root)
                {
                    _root = _windowFactory.GetRoot();
                }

                return _root;
            }
        }

        public BlackoutComponent Blackout
        {
            get
            {
                if (!_blackout)
                {
                    _blackout = _windowFactory.GetBlackout(Root);
                }

                return _blackout;
            }
        }

        public void Initialize()
        {
            _messageBus.AddListener<CloseWindowMessage>(OnCloseWindowMessageHandler);
            _layersConfig = Resources.Load<LayersConfig>(WindowsLayersConfigPath);

            var sortedLayers = _layersConfig.Layers.OrderBy(x => x.Order).ToArray();
            _layers = new string[sortedLayers.Length];

            int index = 0;
            foreach (LayerParameters layerParameters in sortedLayers)
            {
                _layersWindows[layerParameters.Name] = new LinkedList<IWindowPresenter>();
                _layers[index++] = layerParameters.Name;
            }
        }

        public void Dispose()
        {
            _messageBus.RemoveListener<CloseWindowMessage>(OnCloseWindowMessageHandler);
        }

        public void OpenWindow<TPresenter>(bool closePrevious = false, bool enableBlackout = false, IWindowArgs args = null)
            where TPresenter : class, IWindowPresenter, new()
        {
            WindowCreateInfo createInfo = WindowsStorage.GetWindowInfo<TPresenter>();
            TPresenter windowPresenter = _windowFactory.CreateWindow<TPresenter>(createInfo, Root.transform, args);
            if (windowPresenter != null)
            {
                var openParameters = new WindowOpenParameters
                {
                    ClosePrevious = closePrevious,
                    NeedBlackout = enableBlackout,
                    Presenter = windowPresenter
                };

                _openedWindows[windowPresenter] = openParameters;

                ICommandManager commands = RequestOpenWindow(openParameters);
                AddToQueue(commands);
            }
        }

        public void CloseWindow(IWindowPresenter presenter)
        {
            ICommandManager commands = RequestCloseWindow(presenter);
            AddToQueue(commands);
        }

        private ICommandManager RequestOpenWindow(WindowOpenParameters parameters)
        {
            var commandManager = new CommandManager();
            string layer = parameters.Presenter.Layer;

            int index = Array.IndexOf(_layers, layer);
            if (index == -1)
            {
                Debug.LogError($"Couldn't find layer with name ({layer})");

                return null;
            }

            for (int i = index; i < _layers.Length; i++)
            {
                string layerName = _layers[i];
                LinkedList<IWindowPresenter> list = _layersWindows[layerName];

                var node = list.Last;

                while (node != null)
                {
                    WindowState state = node.Value.State;
                    LinkedListNode<IWindowPresenter> prev = node.Previous;
                    
                    switch (state)
                    {
                        case WindowState.Opened:
                            if (parameters.ClosePrevious)
                            {
                                commandManager.Add(new WindowCloseCommand(node.Value));
                                Remove(list, node);
                            }
                            else
                            {
                                commandManager.Add(new WindowHideCommand(node.Value));
                            }
                            break;

                        case WindowState.None:
                        case WindowState.Hidden:
                        case WindowState.Closed:
                            if (parameters.ClosePrevious)
                            {
                                Remove(list, node);
                            }
                            break;
                    }
                    
                    node = prev;
                }
            }
            
            if (parameters.NeedBlackout)
            {
                Blackout.SetActive(true);
                Blackout.SetLayer(layer);
            }
            
            commandManager.Add(new WindowOpenCommand(parameters.Presenter));
            _layersWindows[layer].AddLast(parameters.Presenter);
            
            return commandManager;
        }
        
        private ICommandManager RequestCloseWindow(IWindowPresenter presenter)
        {
            CommandManager commandManager = new CommandManager();
            string layer = presenter.Layer;

            int index = Array.IndexOf(_layers, layer);
            if (index == -1)
            {
                Debug.LogError($"Couldn't find layer with name ({layer})");

                return null;
            }

            for (int i = index; i < _layers.Length; i++)
            {
                string layerName = _layers[i];
                LinkedList<IWindowPresenter> list = _layersWindows[layerName];

                var node = list.Last;

                if (i == index)
                {
                    IWindowPresenter value = node?.Value;

                    if (value == presenter)
                    {
                        commandManager.Add(new WindowCloseCommand(value));
                        IWindowPresenter prevWindow = node.Previous?.Value;

                        if (prevWindow != null)
                        {
                            commandManager.Add(new WindowOpenCommand(prevWindow));
                        }

                        Remove(list, node);
                    }
                }
                else
                {
                    while (node != null)
                    {
                        WindowState state = node.Value.State;
                        switch (state)
                        {
                            case WindowState.Opened:
                                commandManager.Add(new WindowCloseCommand(node.Value));
                                break;
                        }

                        LinkedListNode<IWindowPresenter> prev = node.Previous;

                        Remove(list, node);

                        node = prev;
                    }
                }
            }

            return commandManager;
        }

        private void Remove(LinkedList<IWindowPresenter> list, LinkedListNode<IWindowPresenter> node)
        {
            list.Remove(node);
            _openedWindows.Remove(node.Value);
        }
        
        private void AddToQueue(ICommandManager commands)
        {
            if (commands != null)
            {
                commands.Finished += OnBatchCommandFinished;
                _commandsQueue.Enqueue(commands);
                UpdateQueue();
            }
        }

        private void UpdateQueue()
        {
            if (_currentCommands is not {Executing: true} && _commandsQueue.Any())
            {
                _currentCommands = _commandsQueue.Dequeue();
                _currentCommands.RunSimultaneously();
            }
            else if (!_commandsQueue.Any())
            {
                bool needShowBlackout = false;
                IWindowPresenter lastPresenter = null;
                
                foreach (var openedWindow in _openedWindows)
                {
                    IWindowOpenParameters parameters = openedWindow.Value;
                    if (parameters.NeedBlackout)
                    {
                        needShowBlackout = true;
                        lastPresenter = openedWindow.Key;
                    }
                }

                Blackout.SetActive(needShowBlackout);
                Blackout.SetLayer(lastPresenter?.Layer);
            }
        }

        private void OnBatchCommandFinished(ICommandManager commandManager)
        {
            commandManager.Finished -= OnBatchCommandFinished;
            UpdateQueue();
        }

        private void OnCloseWindowMessageHandler(CloseWindowMessage message)
        {
            CloseWindow(message.Presenter);
        }
    }
}