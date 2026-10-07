// Copyright (c) 2024, Awessets

using System;
using System.Linq;
using System.Reflection;
using MergeIt.SimpleDI.ReservedInterfaces;

namespace MergeIt.SimpleDI
{
    public class DiHandler
    {
        private Type _type;
        private object _instance;
        private bool _isSingleton;
        private bool _needSetup = true;

        internal bool NeedSetup
        {
            get => _needSetup;
            set => _needSetup = value;
        }
        
        internal void TrySetup()
        {
            Setup();
        }
        
        internal object Setup()
        {
            if (!_needSetup)
            {
                return _instance;
            }

            _needSetup = false;
            
            _instance = GetOrCreateInstance();
            
            var type = _instance.GetType();
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<IntroduceAttribute>() != null);

            var fields = type
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttribute<IntroduceAttribute>() != null);

            foreach (PropertyInfo propertyInfo in properties)
            {
                var attribute = propertyInfo.GetCustomAttribute<IntroduceAttribute>();
                var attributeKey = attribute.GetType().GetField("_key", BindingFlags.Instance | BindingFlags.NonPublic);
                string value = attributeKey.GetValue(attribute) as string;

                propertyInfo.SetValue(_instance, DiContainer.Get(propertyInfo.PropertyType, value));
            }
            
            foreach (FieldInfo fieldInfo in fields)
            {
                var attribute = fieldInfo.GetCustomAttribute<IntroduceAttribute>();
                var attributeKey = attribute.GetType().GetField("_key", BindingFlags.Instance | BindingFlags.NonPublic);
                string value = attributeKey.GetValue(attribute) as string;

                fieldInfo.SetValue(_instance, DiContainer.Get(fieldInfo.FieldType, value));
            }

            if (_instance is IInitializable initializable)
            {
                initializable.Initialize();
            }

            if (_instance is IUpdatable updatable)
            {
                DiContainer.UpdatableObjects.Add(updatable);
            }

            return _instance;
        }

        public void AddDependency<TType>()
        {
            _type = typeof(TType);
        }

        public DiHandler AsSingleton()
        {
            _isSingleton = true;
            
            GetOrCreateInstance();

            return this;
        }

        public DiHandler AsSingleton<TType>(TType instance) where TType : class
        {
            _isSingleton = true;
            _instance = instance;
            _needSetup = false;
            
            return this;
        }
        
        private object GetOrCreateInstance()
        {
            if (_isSingleton)
            {
                return _instance ??= Activator.CreateInstance(_type);
            }

            var instance = Activator.CreateInstance(_type);

            return instance;
        }
    }
}