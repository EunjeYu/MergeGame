// Copyright (c) 2024, Awessets

using MergeIt.Core.Helpers;
using MergeIt.Core.WindowSystem;
using MergeIt.Game.Enums;
using MergeIt.Game.Services;
using MergeIt.Game.Windows.UserProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.HUD
{
    public class ProgressComponent : HudPanelBase
    {
        [SerializeField]
        private TMP_Text _levelValue;

        [SerializeField]
        private SlicedFilledImage _progressImage;

        [SerializeField]
        private GameObject _giftImageContainer;

        [SerializeField]
        private Button _progressButton;

        public override HudPanelType Type
        {
            get => HudPanelType.UserProgress;
        }

        private int _nextExperience;
        private float _prevProgress;
        private float _nextProgress;
        private bool _animate;
        private float _tempProgress;

        private IWindowSystem _windowSystem;

        protected override void Start()
        {
            base.Start();
            
            _progressButton.onClick.AddListener(OnProgressButtonClick);
        }

        public void OnDestroy()
        {
            _progressButton.onClick.RemoveListener(OnProgressButtonClick);

            UserServiceModel.Level.Unsubscribe(OnLevelChanged);
            UserServiceModel.Experience.Unsubscribe(OnExperienceChanged);
        }

        public void Initialize(IWindowSystem windowSystem)
        {
            _windowSystem = windowSystem;
        }

        public void SetMaxProgress(int nextExperience)
        {
            _nextExperience = nextExperience;
        }

        public void UpdateProgress()
        {
            SetProgress(_nextExperience, UserServiceModel.Experience.Value, false);
        }

        protected override void OnApplyModel(UserServiceModel userServiceModel)
        {
            UserServiceModel.Level.Subscribe(OnLevelChanged, true);
            UserServiceModel.Experience.Subscribe(OnExperienceChanged, true);
        }
        

        private void OnProgressButtonClick()
        {
            _windowSystem.OpenWindow<UserInfoPresenter>(enableBlackout: true);
        }
        
        private void OnLevelChanged(int value)
        {
            _levelValue.text = $"{value}";
        }

        private void OnExperienceChanged(int prev, int current)
        {
            SetProgress(prev, current, true);
        }

        private void SetProgress(int prev, int current, bool animated)
        {
            if (animated)
            {
                _prevProgress = prev / (float)_nextExperience;
                _nextProgress = current / (float)_nextExperience;
                _tempProgress = _prevProgress;
                _animate = true;
            }
            else
            {
                _progressImage.fillAmount = current / (float)_nextExperience;
            }
        }

        private void Update()
        {
            if (_animate)
            {
                _tempProgress += Time.deltaTime;

                if (_tempProgress >= _nextProgress)
                {
                    _tempProgress = 0f;
                    _progressImage.fillAmount = _nextProgress;
                    _animate = false;
                }
                else
                {
                    _progressImage.fillAmount = _tempProgress;
                }
            }
        }
    }
}