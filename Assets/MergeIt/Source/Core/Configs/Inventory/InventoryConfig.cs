// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using UnityEngine;

namespace MergeIt.Core.Configs.Inventory
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "Merge Toolkit/Inventory config")]
    public class InventoryConfig : ScriptableObject
    {
        public int InitialCapacity;

        public CurrencySettings[] PaidCells;
    }
}