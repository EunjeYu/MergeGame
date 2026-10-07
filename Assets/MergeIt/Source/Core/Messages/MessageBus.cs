// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Linq;
using MergeIt.SimpleDI.ReservedInterfaces;
using UnityEngine;

namespace MergeIt.Core.Messages
{
    public class MessageBus : IMessageBus, IInitializable
    {
        private static readonly Dictionary<Type, List<object>> Messages = new Dictionary<Type, List<object>>();

        public void Initialize()
        {
            Type interfaceType = typeof(IMessage);
            IEnumerable<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && interfaceType.IsAssignableFrom(type));

            foreach (Type type in allTypes)
            {
                Messages[type] = new List<object>();
            }
        }

        public void DefineMessage<T>() where T : IMessage
        {
            Type type = typeof(T);

            if (!Messages.ContainsKey(type))
            {
                Messages.Add(type, new List<object>());
            }
        }

        public void AddListener<T>(Action<T> callback) where T : IMessage
        {
            Type type = typeof(T);

            if (!Messages.ContainsKey(type))
            {
                Debug.LogWarning($"Message with type {type} was not defined and will added to cache.");

                Messages[type] = new List<object>();
            }

            Messages[type].Add(callback);
        }

        public void RemoveListener<T>(Action<T> callback) where T : IMessage
        {
            if (Messages.TryGetValue(typeof(T), out List<object> callbacks))
            {
                callbacks.Remove(callback);
            }
        }

        public void Fire<T>(T messageData) where T : IMessage
        {
            Type type = typeof(T);

            if (Messages.TryGetValue(type, out var message))
            {
                for (var index = 0; index < message.Count;)
                {
                    var callback = message[index];
                    Action<T> action = callback as Action<T>;

                    if (action == null)
                    {
                        message.RemoveAt(index);

                        continue;
                    }

                    action(messageData);

                    index++;
                }
            }
        } 
        
        public void Fire<T>() where T : IMessage, new()
        {
            Type type = typeof(T);

            if (Messages.TryGetValue(type, out var message))
            {
                for (var index = 0; index < message.Count;)
                {
                    var callback = message[index];
                    Action<T> action = callback as Action<T>;

                    if (action == null)
                    {
                        message.RemoveAt(index);

                        continue;
                    }

                    action(new T());

                    index++;
                }
            }
        }
    }
}