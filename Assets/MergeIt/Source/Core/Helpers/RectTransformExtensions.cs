// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Helpers
{
    public static class RectTransformExtensions
    {
        public static void Stretch(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
        }
    }
}