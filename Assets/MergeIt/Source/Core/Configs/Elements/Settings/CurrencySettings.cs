// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Types;
using UnityEngine;

namespace MergeIt.Core.Configs.Elements
{
    [Serializable]
    public struct CurrencySettings
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private int _amount;

        public CurrencyType Currency
        {
            get => _currencyType;
        }
        
        public int Amount
        {
            get => _amount;
        }
    }
}