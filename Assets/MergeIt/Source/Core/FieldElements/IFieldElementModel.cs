// Copyright (c) 2024, Awessets

namespace MergeIt.Core.FieldElements
{
    public interface IFieldElementModel
    {
        bool IsBusy { get; set; }
        bool IsLocked { get; set; }
        bool Selected { get; set; }
        int ClicksCount { get; set; }
        GridPoint Point { get; set; }
    }
}