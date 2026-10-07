// Copyright (c) 2024, Awessets

using MergeIt.Core.Utils;
using UnityEngine;

namespace MergeIt.Game.Field.Elements.Generator
{
    public class FieldElementGeneratorView : FieldElementView
    {
        [SerializeField]
        private GeneratorTimerComponent _timer;
        
        public void SetTimer(Bindable<float> remainChargeTime, float fullRemainTime)
        { 
            _timer.StartTimer(remainChargeTime, fullRemainTime);
        }

        public void HideTimer()
        {
            _timer.gameObject.SetActive(false);
        }
    }
}