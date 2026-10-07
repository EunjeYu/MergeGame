// Copyright (c) 2024, Awessets

using System;
using Cysharp.Threading.Tasks;

namespace MergeIt.Core.Commands
{
    public abstract class Command : ICommand, IDisposable
    {
        public event Action<ICommand> Finished;
        
        public virtual void Execute()
        {
            
        }

        public async virtual UniTask ExecuteAsync()
        {
#if !UNITY_WEBGL
            await UniTask.RunOnThreadPool(Execute);
#else
            await UniTask.Create(async ()=>
            {
                Execute();
                await UniTask.Yield();
            });
#endif
        }
        
        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void Finish()
        {
            Finished?.Invoke(this);
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}