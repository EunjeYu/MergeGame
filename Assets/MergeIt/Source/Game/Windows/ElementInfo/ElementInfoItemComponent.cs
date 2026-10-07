// Copyright (c) 2024, Awessets

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MergeIt.Game.Windows.ElementInfo
{
    public class ElementInfoItemComponent : MonoBehaviour
    {

        [SerializeField]
        private Button _button;

        [SerializeField]
        private Color _selectedColor;

        [SerializeField]
        private Image _backgroundImage;

        [SerializeField]
        private GameObject _generatorIcon;

        [SerializeField]
        private Transform _iconContainer;

        [SerializeField]
        private TMP_Text _numberText;

        [SerializeField]
        private GameObject _numberContainer;

        [SerializeField]
        private GameObject _unknownContainer;

        [SerializeField]
        private GameObject _lockerObject;

        [SerializeField]
        private GameObject _maxLevelIcon;

        public UnityAction<ElementInfoItemComponent> ClickEvent = delegate { };

        public RectTransform RectTransform { get; private set; }

        public void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _button.onClick.AddListener(OnClick);
        }

        public void OnDestroy()
        {
            ClickEvent = null;
            _button.onClick.RemoveListener(OnClick);
        }

        public void Setup(bool isGenerator, bool isSelected, int number = -1)
        {
            _backgroundImage.color = isSelected ? _selectedColor : Color.white;
            _generatorIcon.SetActive(isGenerator);

            if (number > -1 && _numberText)
            {
                _numberContainer.SetActive(true);
                _numberText.text = number.ToString();
            }
            else
            {
                _numberContainer.SetActive(false);
            }
        }

        public void SetIcon(Transform iconTransform, bool isLocked = false)
        {
            iconTransform.SetParent(_iconContainer);
            iconTransform.localScale = Vector3.one;

            if (_lockerObject)
            {
                _lockerObject.SetActive(isLocked);
            }
        }

        private void OnClick()
        {
            ClickEvent?.Invoke(this);
        }

        public void SetUnknown()
        {
            _unknownContainer.SetActive(true);
        }
    }

}