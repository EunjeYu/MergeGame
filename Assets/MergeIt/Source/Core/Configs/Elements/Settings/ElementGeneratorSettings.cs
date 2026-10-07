// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MergeIt.Core.Configs.Elements
{
    [Serializable]
    public class ElementGeneratorSettings
    {
        [SerializeField, HideInInspector]
        private bool _expanded;
        
        [SerializeField] private int _stack;
        [SerializeField] private int _minDrop;
        [SerializeField] private int _maxDrop;
        [SerializeField] private int _chargeTime;
        [SerializeField] private int _dropCount;
        [SerializeField] private CurrencySettings _skipChargeCostSettings;
        [SerializeField] private bool _adSkip;
        [SerializeField] private bool _charged;
        [SerializeField] private int _energyCost;
        [SerializeField] private bool _isUnlimited;
        [SerializeField] private bool _needOpen;
        [SerializeField] private int _openTime;
        [SerializeField] private CurrencySettings _skipOpenCostSettings;
        [SerializeField] private bool _generateBlocked;
        [SerializeField] private List<GeneratableElement> _generateItems;
        
        public bool Expanded
        {
            get => _expanded;
            set => _expanded = value;
        }

        public int Stack
        {
            get => _stack;
        }

        public int MinDrop
        {
            get => _minDrop;
        }

        public int MaxDrop
        {
            get => _maxDrop;
        }

        public int ChargeTime
        {
            get => _chargeTime;
        }

        public int DropCount
        {
            get => _dropCount;
        }

        public CurrencySettings SkipChargeCostSettings
        {
            get => _skipChargeCostSettings;
        }

        public bool AdSkip
        {
            get => _adSkip;
        }

        public bool Charged
        {
            get => _charged;
        }

        public int EnergyCost
        {
            get => _energyCost;
        }

        public bool IsUnlimited
        {
            get => _isUnlimited;
        }

        public bool NeedOpen
        {
            get => _needOpen;
        }
        
        public int OpenTime
        {
            get => _openTime;
        }
        
        public CurrencySettings SkipOpenCostSettings
        {
            get => _skipOpenCostSettings;
        }

        public bool GenerateBlocked
        {
            get => _generateBlocked;
        }

        public List<GeneratableElement> GenerateItems
        {
            get => _generateItems;
        }
    }
}