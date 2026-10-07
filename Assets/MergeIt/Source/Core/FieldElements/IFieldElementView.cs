// Copyright (c) 2024, Awessets

using MergeIt.Core.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Core.FieldElements
{
    public interface IFieldElementView : IView
    {
        RectTransform RectTransform { get; }
        Canvas Canvas { get; }
        GraphicRaycaster GraphicRaycaster { get; }
        void Lock(bool isLocked);
        void ResetState();
    }
}