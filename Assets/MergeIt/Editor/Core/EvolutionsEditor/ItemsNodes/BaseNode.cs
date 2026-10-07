// Copyright (c) 2024, Awessets

using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace MergeIt.Editor.EvolutionsEditor
{
    public abstract class BaseNode : Node, ISaveNode, IDisposable
    {
        public abstract string Id { get; }
        public abstract void SaveData();
        
        public Port GetPort(string portName)
        {
            var port = inputContainer.Children()
                .OfType<Port>()
                .FirstOrDefault(x => x.portName.Equals(portName));

            if (port != null)
            {
                return port;
            }

            port = outputContainer.Children()
                .OfType<Port>()
                .FirstOrDefault(x => x.portName.Equals(portName));

            return port;
        }
        
        public virtual void Dispose()
        {
        }
    }
}