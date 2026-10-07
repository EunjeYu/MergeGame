// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.FieldElements;
using MergeIt.Game.Effects.Parameters;

namespace MergeIt.Game.Effects.Controllers
{
    public interface IEffectWithPresenter
    {
        void Setup(IFieldElementPresenter presenter, IEffectParameters effectParameters = null, Action finishedCallback = null);

        IFieldElementPresenter GetPresenter();
    }
}