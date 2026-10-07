// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using UnityEngine;

namespace MergeIt.Core.Helpers
{
    public class MonoEventsListener : MonoBehaviour
    {
        private static MonoEventsListener _instance;
        public static MonoEventsListener Instance
        {
            get
            {
                if (!_instance)
                {
                    var updater = new GameObject($"{nameof(MonoEventsListener)}");
                    _instance = updater.AddComponent<MonoEventsListener>();
                    DontDestroyOnLoad(updater);
                }

                return _instance;
            }
        }

        private static readonly List<IMonoUpdateHandler> UpdatableHandlers = new List<IMonoUpdateHandler>();
        private static readonly List<IMonoApplicationQuitHandler> ApplicationQuitHandlers = new List<IMonoApplicationQuitHandler>();

        public void SubscribeOnUpdate(IMonoUpdateHandler monoUpdateHandler)
        {
            UpdatableHandlers.Add(monoUpdateHandler);
        }

        public void UnsubscribeFromUpdate(IMonoUpdateHandler monoUpdateHandler)
        {
            UpdatableHandlers.Remove(monoUpdateHandler);
        }
        
        public void SubscribeOnApplicationQuit(IMonoApplicationQuitHandler monoApplicationQuitHandler)
        {
            ApplicationQuitHandlers.Add(monoApplicationQuitHandler);
        }
        
        public void UnsubscribeFromApplicationQuit(IMonoApplicationQuitHandler monoApplicationQuitHandler)
        {
            ApplicationQuitHandlers.Remove(monoApplicationQuitHandler);
        }

        private void Update()
        {
            for (int i = 0; i < UpdatableHandlers.Count; i++)
            {
                UpdatableHandlers[i].Update();
            }
        }

        private void OnApplicationQuit()
        {
            for (int i = 0; i < ApplicationQuitHandlers.Count; i++)
            {
                ApplicationQuitHandlers[i].OnApplicationQuit();
            }
        }
    }
}