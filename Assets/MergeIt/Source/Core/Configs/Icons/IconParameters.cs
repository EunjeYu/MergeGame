// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Types;
using UnityEngine;

namespace MergeIt.Core.Configs.Icons
{
    [Serializable]
    public class IconParameters
    {
        [SerializeField]
        private CurrencyType _currencyType;

        [SerializeField]
        private Sprite _currencyIcon;

        public CurrencyType CurrencyType
        {
            get => _currencyType;
        }

        public Sprite CurrencyIcon
        {
            get => _currencyIcon;
        }
    }
}