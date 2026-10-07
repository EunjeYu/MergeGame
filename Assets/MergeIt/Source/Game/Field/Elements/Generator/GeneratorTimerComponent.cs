// Copyright (c) 2024, Awessets

using MergeIt.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.Field.Elements.Generator
{
    public class GeneratorTimerComponent : MonoBehaviour
    {
        [SerializeField]
        private Image _fill;

        private float _remainFullTime;
        private Bindable<float> _remainChargeTime;
        
        public void StartTimer(Bindable<float> remainChargeTime, float fullRemainTime)
        {
            if (remainChargeTime.Value > 0f)
            {
                gameObject.SetActive(true);
                
                _remainFullTime = fullRemainTime;

                _remainChargeTime?.Unsubscribe(OnRemainTimeChanged);

                _remainChargeTime = remainChargeTime;
                _remainChargeTime.Subscribe(OnRemainTimeChanged, true);
            }
        }

        private void OnRemainTimeChanged(float newValue)
        {
            if (newValue <= 0f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                var fill = newValue / _remainFullTime;
                _fill.fillAmount = 1f - fill;
            }
        }

        private void OnDisable()
        {
            _remainChargeTime.Unsubscribe(OnRemainTimeChanged);
            _remainChargeTime = null;
            _remainFullTime = 0f;
        }
    }
}