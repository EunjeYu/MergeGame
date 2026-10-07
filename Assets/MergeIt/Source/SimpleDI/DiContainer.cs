// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using MergeIt.SimpleDI.ReservedInterfaces;
using UnityEngine;

namespace MergeIt.SimpleDI
{
    public class DiContainer
    {
        private static readonly HashSet<Type> ReservedInterfaces = new()
        {
            typeof(IInitializable),
            typeof(IUpdatable),
            typeof(IDisposable)
        };

        internal static readonly HashSet<IUpdatable> UpdatableObjects = new();

        private static readonly Dictionary<Type, Dictionary<string, DiHandler>> TypesHandlers = new();
        
        internal static void Update()
        {
            foreach (IUpdatable updatable in UpdatableObjects)
            {
                updatable.Update();
            }
        }

        public static DiHandler Register<TInterface, TType>(string key = "")
            where TInterface : class
            where TType : TInterface
        {
            Type type = typeof(TInterface);
            var diHandler = new DiHandler();
            diHandler.AddDependency<TType>();

            return Bind<TType>(type, diHandler, key);
        }
        
        public static DiHandler Register<TType>(string key = "")
            where TType : class
        {
            Type type = typeof(TType);
            var diHandler = new DiHandler();
            diHandler.AddDependency<TType>();

            return Bind<TType>(type, diHandler, key);
        }

        public static DiHandler RegisterInterfacesFor<TType>(string key = "")
            where TType : class
        {
            Type type = typeof(TType);
            Type[] interfaces = type.GetInterfaces();

            var diHandler = new DiHandler();
            diHandler.AddDependency<TType>();

            for (int i = 0; i < interfaces.Length; i++)
            {
                Type interfaceType = interfaces[i];

                if (!ReservedInterfaces.Contains(interfaceType))
                {
                    Bind<TType>(interfaceType, diHandler, key);
                }
            }

            return diHandler;
        }

        public static TInterface Get<TInterface>(string key = "")
            where TInterface : class
        {
            return Get(typeof(TInterface), key) as TInterface;
        }

        public static void Drop<TInterface>(string key = "")
            where TInterface : class
        {
            Type type = typeof(TInterface);

            if (!TypesHandlers.TryGetValue(type, out Dictionary<string, DiHandler> dependencyHandlers))
            {
                Debug.LogWarning($"Binding for {type} was not found");
                return;
            }

            dependencyHandlers.Remove(key);
        }
        
        public static void Clear()
        {
            foreach (var typesHandler in TypesHandlers)
            {
                typesHandler.Value.Clear();
            }
            
            TypesHandlers.Clear();
            
            UpdatableObjects.Clear();
        }

        private static DiHandler Bind<TType>(Type type, DiHandler diHandler, string key = "")
        {
            if (!TypesHandlers.TryGetValue(type, out Dictionary<string, DiHandler> handlers))
            {
                handlers = new Dictionary<string, DiHandler> {{key, diHandler}};

                TypesHandlers.Add(type, handlers);
            }
            else
            {
                if (handlers.ContainsKey(key))
                {
                    throw new Exception(
                        $"Container already has dependency with key '{key}': {type} -> {typeof(TType)}");
                }

                handlers.Add(key, diHandler);
            }

            return diHandler;
        }

        internal static object Get(Type type, string key = "")
        {
            if (!TypesHandlers.TryGetValue(type, out Dictionary<string, DiHandler> dependencyHandlers) ||
                !dependencyHandlers.TryGetValue(key, out DiHandler handler))
            {
                Debug.LogError($"Bindings for {type} were not found");
                return default;
            }

            var instance = handler.Setup();

            return instance;
        }

        internal static void PostProcess()
        {
            foreach (var typesHandler in TypesHandlers)
            {
                foreach (var diHandler in typesHandler.Value)
                {
                    diHandler.Value.TrySetup();
                }
            }
        }
    }
}