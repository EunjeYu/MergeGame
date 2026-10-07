// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Types;
using MergeIt.Core.Utils;
using UnityEngine;

namespace MergeIt.Core.Helpers
{
    public static class NumericExtensions
    {
        public static void ApplyOperation(this Bindable<int> target, ConsumableOperationType opType, int value, bool holdValue)
        {
            if (holdValue)
            {
                target.Hold();
            }
            
            switch (opType)
            {
                case ConsumableOperationType.Multiply:
                    target.Value *= value;
                    break;
                
                case ConsumableOperationType.Divide:
                    target.Value /= value;
                    break;
                
                case ConsumableOperationType.Add:
                    target.Value += value;
                    break;
                
                case ConsumableOperationType.Subtract:
                    int targetValue = target.Value;
                    targetValue = Mathf.Clamp(targetValue - value, 0, int.MaxValue);
                    target.Value = targetValue;
                    break;
            }
        }
    }
}