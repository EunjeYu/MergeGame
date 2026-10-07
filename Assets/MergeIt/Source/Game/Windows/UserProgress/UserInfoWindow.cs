// Copyright (c) 2024, Awessets

using MergeIt.Core.WindowSystem.Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MergeIt.Game.Windows.UserProgress
{
    public class UserInfoWindow : WindowBase
    {

        [SerializeField]
        private Button _levelUpButton;

        [SerializeField]
        private RectTransform _prizesContainer;

        [SerializeField]
        private UserProgressComponent _userProgressComponent;

        public UnityAction LevelUpEvent;

        public RectTransform PrizesContainer
        {
            get => _prizesContainer;
        }

        public override void OnOpenFinished()
        {
            base.OnOpenFinished();

            _levelUpButton.onClick.AddListener(LevelUpEvent);
        }

        public override void Close()
        {
            base.Close();

            _levelUpButton.onClick.RemoveListener(LevelUpEvent);
        }

        public void EnableLevelUpButton(bool enable)
        {
            _levelUpButton.interactable = enable;
        }

        public void UpdateProgress(int level, int currentExp, int nextExp)
        {
            _userProgressComponent.UpdateProgress(level, currentExp, nextExp);
        }
    }
}