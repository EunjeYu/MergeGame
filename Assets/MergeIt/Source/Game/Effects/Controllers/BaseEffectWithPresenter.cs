// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.FieldElements;
using MergeIt.Game.Effects.Parameters;

namespace MergeIt.Game.Effects.Controllers
{
    public abstract class BaseEffectWithPresenter : BaseEffect, IEffectWithPresenter
    {
        protected IFieldElementPresenter Presenter;
        
        public virtual void Setup(IFieldElementPresenter presenter, IEffectParameters effectParameters = null, Action finishedCallback = null)
        {
            base.Setup(presenter.RectTransform, effectParameters, finishedCallback);

            Presenter = presenter;
        }

        public IFieldElementPresenter GetPresenter()
        {
            return Presenter;
        }

        protected virtual void FinishCallbackHandler()
        {
            Presenter.Canvas.sortingOrder = 1;
            // Presenter.UpdateInitialPosition();
            Presenter.SetBusy(false);
            
            Finish();
        }
    }
}