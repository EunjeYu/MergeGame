// Copyright (c) 2024, Awessets

using MergeIt.Core.MVP;
using UnityEngine;

namespace MergeIt.Core.FieldElements
{
    public interface IFieldElementPresenter : IPresenter
    {
        FieldElementState State { get; }
        Transform Transform { get; }
        RectTransform RectTransform { get; }
        Canvas Canvas { get; }
        bool IsAvailable { get; }
        bool IsBusy { get; }
        bool IsLocked { get; }

        void Update(IFieldElement fieldElement);
        void Activate(bool isActive);
        void Release();
        void SetBusy(bool isBusy);
        // void UpdateInitialPosition();
        void Select(bool isSelected);
        void StartDrag();
        void EndDrag();
        void SetParent(Transform parent, bool resetPosition);
        void SetPoint(GridPoint point);
        void SetLock(bool block);
        void Remove();
        void ResetPosition();
        IFieldElementView GetView();
        IFieldElementModel GetModel();
        void SetState(FieldElementState state);
    }
}