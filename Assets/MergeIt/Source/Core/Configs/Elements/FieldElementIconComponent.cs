// Copyright (c) 2024, Awessets

using MergeIt.Core.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Core.Configs.Elements
{
    public class FieldElementIconComponent : MonoBehaviour
    {
        [SerializeField]
        private Material _blockedMaterial;
        
        [SerializeField]
        private Image _image;
        
        [SerializeField]
        private RectTransform _rectTransform;

        private static Material _defaultMaterial;

        public RectTransform RectTransform
        {
            get => _rectTransform;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
           
            if (!_defaultMaterial)
            {
                _defaultMaterial = new(Shader.Find("UI/Default"));
            }
        }

        private void Start()
        {
            _rectTransform.Stretch();
        }

        public void SetBlocked(bool locked)
        {
            if (locked)
            {
                _image.material = _blockedMaterial;
            }
            else
            {
                _image.material = _defaultMaterial;
            }
        }
        
        public Image GetImage()
        {
            return _image;
        }
    }
}