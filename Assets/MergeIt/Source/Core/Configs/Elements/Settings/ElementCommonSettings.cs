// Copyright (c) 2024, Awessets

using System;
using UnityEngine;

namespace MergeIt.Core.Configs.Elements
{
    [Serializable]
    public class ElementCommonSettings
    {
        [SerializeField, HideInInspector]
        private bool _expanded;
        
        [SerializeField] private string _description;
        [SerializeField] private CurrencySettings _splitCostSettings;
        [SerializeField] private CurrencySettings _sellCostSettings;
        [SerializeField] private CurrencySettings _unlockCostSettings;
        [SerializeField] private string _name;
        [SerializeField] private FieldElementIconComponent _icon;
        [SerializeField] private bool _isConsumable;
        [SerializeField] private ConsumableSettings _consumableSettings;
        
        public bool Expanded
        {
            get => _expanded;
            set => _expanded = value;
        }

        public string Description
        {
            get => _description;
        }

        public CurrencySettings SplitCostSettings
        {
            get => _splitCostSettings;
        }

        public CurrencySettings SellCostSettings
        {
            get => _sellCostSettings;
        }

        public CurrencySettings UnlockCostSettings
        {
            get => _unlockCostSettings;
        }

        public ConsumableSettings ConsumableSettings
        {
            get => _consumableSettings;
        }
        
        public bool IsConsumable
        {
            get => _isConsumable;
        }

        public string Name
        {
            get => _name;
        } 

        public FieldElementIconComponent Icon
        {
            get => _icon;
        }
    }
}