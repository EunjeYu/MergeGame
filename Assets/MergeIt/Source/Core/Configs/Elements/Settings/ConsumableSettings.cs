// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Types;
using UnityEngine;

namespace MergeIt.Core.Configs.Elements
{
    [Serializable]
    public struct ConsumableSettings
    {
        [SerializeField] private ConsumableTargetType _target;
        [SerializeField] private ConsumableOperationType _operation;
        [SerializeField] private int _value;

        public ConsumableTargetType Target
        {
            get => _target;
        }

        public ConsumableOperationType Operation
        {
            get => _operation;
        }

        public int Value
        {
            get => _value;
        }
    }

}