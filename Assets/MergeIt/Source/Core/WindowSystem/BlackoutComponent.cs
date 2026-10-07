// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.WindowSystem
{
    public class BlackoutComponent : MonoBehaviour
    {
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetLayer(string layer = null)
        {
            string layerName = string.IsNullOrEmpty(layer) ? Configs.Windows.SortingLayers.Default : layer;

            _canvas.overrideSorting = true;
            _canvas.sortingLayerName = layerName;
        }
    }
}