// Copyright (c) 2024, Awessets

using MergeIt.Core.Messages;
using MergeIt.Core.WindowSystem.Windows;

namespace MergeIt.Core.WindowSystem.Messages
{
    public class CloseWindowMessage : IMessage
    {
        public IWindowPresenter Presenter { get; set; }
    }
}