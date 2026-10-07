// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MergeIt.Core.Saves;

namespace MergeIt.Core.Helpers
{
    public static class SavesHelper
    {
        public static readonly Dictionary<Type, (string Name,  string Extension)> SavableData = new();

        static SavesHelper()
        {
            Type interfaceType = typeof(ISavable);
            IEnumerable<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && interfaceType.IsAssignableFrom(type));

            foreach (Type type in allTypes)
            {
                var savable = type.GetCustomAttribute<SavableAttribute>();
                if (savable != null)
                {
                    SavableData[type] = (savable.Name, savable.Extension);
                }
            }
        }

        public static string GetFileName<T>() where T : class, ISavable
        {
            Type type = typeof(T);

            if (SavableData.TryGetValue(type, out var data))
            {
                return data.Name;
            }

            return string.Empty;
        }
        
        public static string GetExtension<T>() where T : class, ISavable
        {
            Type type = typeof(T);
            
            if (SavableData.TryGetValue(type, out var data))
            {
                return data.Extension;
            }

            return string.Empty;
        }
        
        public static string GetFileNameWithExtension<T>() where T : class, ISavable
        {
            Type type = typeof(T);
            
            if (SavableData.TryGetValue(type, out var data))
            {
                return $"{data.Name}.{data.Extension}";
            }

            return string.Empty;
        }
    }
}