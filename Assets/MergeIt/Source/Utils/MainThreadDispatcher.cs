// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MergeIt.Core.Helpers
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static MainThreadDispatcher _instance;
        public static MainThreadDispatcher Instance
        {
            get
            {
                if (!_instance)
                {
                    var updater = new GameObject($"{nameof(MainThreadDispatcher)}");
                    _instance = updater.AddComponent<MainThreadDispatcher>();
                    DontDestroyOnLoad(updater);
                }

                return _instance;
            }
        }
        
        private static int _mainThreadId;
        private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();

        private void Awake()
        {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }
        
        private void Update()
        {
            lock (ExecutionQueue)
            {
                while (ExecutionQueue.Count > 0)
                {
                    ExecutionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(Action action)
        {
            if (action == null)
            {
                Debug.LogError("No action to enqueue.");
                return;
            }

            lock (ExecutionQueue)
            {
                ExecutionQueue.Enqueue(action);
            }
        }

        public void RunOnMainThread(Action action)
        {
            if (action == null)
            {
                Debug.LogError("No action to run on main thread.");
                return;
            }

            if (IsMainThread())
            {
                action();
            }
            else
            {
                Enqueue(action);
            }
        }

        public bool IsMainThread()
        {
            return Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        }
    }
}