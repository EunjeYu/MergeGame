// Copyright (c) 2024, Awessets

using MergeIt.Core.Helpers;
using TMPro;
using UnityEngine;

namespace MergeIt.Game.Windows.UserProgress
{
    public class UserProgressComponent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _levelValue;

        [SerializeField]
        private TMP_Text _progressText;

        [SerializeField]
        private SlicedFilledImage _progressImage;

        public void UpdateProgress(int level, int currentExp, int nextExp)
        {
            _levelValue.text = $"{level}";
            _progressText.text = $"{currentExp}/{nextExp}";
            _progressImage.fillAmount = (float)currentExp / nextExp;
        }
    }
}