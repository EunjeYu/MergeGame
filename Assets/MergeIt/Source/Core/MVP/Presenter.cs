// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.MVP
{
    public abstract class Presenter<TView, TModel> : IPresenter
        where TView : Component, IView
        where TModel : IModel, new()
    {
        public TView View { get; private set; }
        public TModel Model { get; } = new();

        public void Initialize(IView view)
        {
            View = view as TView;
            View.DestroyEvent += Dispose;
            
            OnInitialize(View);
        }

        public void Dispose()
        {
            if (View)
            {
                View.DestroyEvent -= Dispose;
            }
            
            OnDispose();
        }
        
        protected virtual void OnInitialize(TView view)
        {
            
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}