// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.FieldElements;
using MergeIt.Game.Effects.Parameters;

namespace MergeIt.Game.Effects.Controllers
{
    public class MoveToUiEffectWithPresenter : BaseEffectWithPresenter
    {
        private readonly MoveToUiEffect _effect = new();

        public override void Start()
        {
            base.Start();

            _effect.Start();
        }

        public override void Setup(IFieldElementPresenter presenter, IEffectParameters effectParameters = null, Action finishedCallback = null)
        {
            base.Setup(presenter, effectParameters, finishedCallback);

            _effect.Setup(presenter.RectTransform, effectParameters, FinishCallbackHandler);
        }

        public override void Update()
        {
            if (_effect.Started)
            {
                _effect.Update();
            }
        }

        protected override void OnStarted()
        {
            base.OnStarted();

            Presenter.Canvas.sortingOrder = 2;
            Presenter.SetBusy(true);
        }
        
        // protected void OnFinishedSubEffect()
        // {
        //     Presenter.Canvas.sortingOrder = 1;
        //     // _presenter.UpdateInitialPosition();
        //     Presenter.SetBusy(false);
        //     
        //     Finish();
        // }
    }
}