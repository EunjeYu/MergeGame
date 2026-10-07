// Copyright (c) 2024, Awessets

using System.Collections;
using MergeIt.Core.Helpers;
using MergeIt.Core.Messages;
using MergeIt.Game.Enums;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;
using TMPro;
using UnityEngine;

namespace MergeIt.Game.HUD
{
    public class EnergyComponent : HudPanelBase
    {
        [SerializeField]
        private TMP_Text _value;

        [SerializeField]
        private TMP_Text _timerLabel;
        
        public override HudPanelType Type
        {
            get => HudPanelType.Energy;
        }

        private IMessageBus _messageBus;
        private long _remainingTime;
        private Coroutine _timerUpdate;

        protected override void Start()
        {
            base.Start();
            
            _messageBus = DiContainer.Get<IMessageBus>();
            _messageBus.AddListener<StartChargingMessage>(OnStartChargingMessageHandler);
        }

        private void OnDestroy()
        {
            UserServiceModel.Energy.Unsubscribe(OnEnergyChanged);
            _messageBus.RemoveListener<StartChargingMessage>(OnStartChargingMessageHandler);
        }

        private void OnStartChargingMessageHandler(StartChargingMessage message)
        {
            if (message.RemainingTime != -1)
            {
                _timerLabel.gameObject.SetActive(true);

                _remainingTime = message.RemainingTime;

                if (_timerUpdate != null)
                {
                    StopCoroutine(_timerUpdate);
                }

                _timerLabel.text = _remainingTime.FormatTime();
                _timerUpdate = StartCoroutine(UpdateTimer());
            }
            else
            {
                if (_timerUpdate != null)
                {
                    StopCoroutine(_timerUpdate);
                }
                
                _timerLabel.gameObject.SetActive(false);
            }
        }

        protected override void OnApplyModel(UserServiceModel userServiceModel)
        {
            UserServiceModel.Energy.Subscribe(OnEnergyChanged, true);
        }

        private void OnEnergyChanged(int current)
        {
            _value.text = $"{current}";
        }

        private IEnumerator UpdateTimer()
        {
            while (_remainingTime > 0)
            {
                yield return new WaitForSecondsRealtime(1f);

                _remainingTime -= 1;
                _timerLabel.text = _remainingTime.FormatTime();
            }

            _messageBus.Fire<EnergyRestoredMessage>();
        }
    }
}