// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.Core.Messages
{
    public interface IMessageBus
    {
        void DefineMessage<T>() where T : IMessage;
        void AddListener<T>(Action<T> callback) where T : IMessage;
        void RemoveListener<T>(Action<T> callback) where T : IMessage;
        void Fire<T>(T messageData) where T : IMessage;
        void Fire<T>() where T : IMessage, new();
    }
}