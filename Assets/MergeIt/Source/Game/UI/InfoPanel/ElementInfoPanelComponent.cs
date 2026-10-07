// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.WindowSystem;
using MergeIt.Game.Field;
using MergeIt.Game.Messages;
using MergeIt.Game.Windows.ElementInfo;
using MergeIt.SimpleDI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.UI.InfoPanel
{
    public class ElementInfoPanelComponent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _commonLabel;

        [SerializeField]
        private ElementInfo[] _elementInfos;

        [SerializeField]
        private Button _infoButton;

        private Dictionary<ElementActionType, ElementInfo> _elementInfoDict = new();
        private FieldLogicModel _fieldLogicModel;
        private IMessageBus _messageBus;
        private IFieldElement _selectedElement;
        private IWindowSystem _windowSystem;

        private void Start()
        {
            _infoButton.onClick.AddListener(OnItemInfoClicked);

            _elementInfoDict = _elementInfos.ToDictionary(el => el.ActionType, el => el);
            _commonLabel.gameObject.SetActive(true);

            _windowSystem = DiContainer.Get<IWindowSystem>();
            _messageBus = DiContainer.Get<IMessageBus>();
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.AddListener<SelectedElementMessage>(OnSelectedElementMessageHandler);
            _messageBus.AddListener<ResetSelectionMessage>(OnResetSelectionMessageHandler);
            _messageBus.AddListener<RemoveElementMessage>(OnRemoveElementMessageHandler);
            _messageBus.AddListener<GeneratorOpenStartMessage>(OnGeneratorOpenStartMessageHandler);
            _messageBus.AddListener<GeneratorOpenedMessage>(OnGeneratorOpenedMessageHandler);
            _messageBus.AddListener<GeneratorRestoredMessage>(OnGeneratorRestoredMessageHandler);
            _messageBus.AddListener<CheckGeneratorMessage>(OnCheckGeneratorMessageHandler);
            _messageBus.AddListener<UnlockElementMessage>(OnUnlockElementMessageHandler);
        }

        private void OnDestroy()
        {
            _infoButton.onClick.RemoveListener(OnItemInfoClicked);

            _messageBus?.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus?.RemoveListener<SelectedElementMessage>(OnSelectedElementMessageHandler);
            _messageBus?.RemoveListener<ResetSelectionMessage>(OnResetSelectionMessageHandler);
            _messageBus?.RemoveListener<RemoveElementMessage>(OnRemoveElementMessageHandler);
            _messageBus?.RemoveListener<GeneratorOpenStartMessage>(OnGeneratorOpenStartMessageHandler);
            _messageBus?.RemoveListener<GeneratorOpenedMessage>(OnGeneratorOpenedMessageHandler);
            _messageBus?.RemoveListener<GeneratorRestoredMessage>(OnGeneratorRestoredMessageHandler);
            _messageBus?.RemoveListener<CheckGeneratorMessage>(OnCheckGeneratorMessageHandler);
            _messageBus?.RemoveListener<UnlockElementMessage>(OnUnlockElementMessageHandler);
        }

        private void OnLoadedGameMessageHandler(LoadedGameMessage message)
        {
            _fieldLogicModel = DiContainer.Get<FieldLogicModel>();
        }

        private void DefineInfoTypes(IFieldElement fieldElement)
        {
            foreach (var elementInfo in _elementInfoDict)
            {
                elementInfo.Value.TrySetup(fieldElement);
            }
        }

        private void OnSelectedElementMessageHandler(SelectedElementMessage message)
        {
            _selectedElement = _fieldLogicModel.FieldElements[message.Point];
            UpdatePanel(message.Point);
        }

        private void OnResetSelectionMessageHandler(ResetSelectionMessage message)
        {
            ResetSelection();
        }

        private void OnRemoveElementMessageHandler(RemoveElementMessage message)
        {
            if (_selectedElement != null &&
                _selectedElement.InfoParameters.LogicPosition == message.RemoveAtPoint)
            {
                ResetSelection();
            }
        }

        private void OnGeneratorOpenStartMessageHandler(GeneratorOpenStartMessage message)
        {
            UpdatePanel(message.GeneratorPoint);
        }

        private void OnGeneratorOpenedMessageHandler(GeneratorOpenedMessage message)
        {
            UpdatePanel(message.GeneratorPoint);
        }

        private void OnGeneratorRestoredMessageHandler(GeneratorRestoredMessage message)
        {
            UpdatePanel(message.GeneratorPoint);
        }

        private void OnCheckGeneratorMessageHandler(CheckGeneratorMessage message)
        {
            UpdatePanel(message.GeneratorPoint);
        }

        private void OnUnlockElementMessageHandler(UnlockElementMessage message)
        {
            UpdatePanel(message.Element.InfoParameters.LogicPosition);
        }

        private void ResetSelection()
        {
            _infoButton.gameObject.SetActive(false);
            _selectedElement = null;
            _commonLabel.gameObject.SetActive(true);

            foreach (var elementInfo in _elementInfoDict)
            {
                elementInfo.Value.gameObject.SetActive(false);
            }
        }

        private void UpdatePanel(GridPoint point)
        {
            if (_selectedElement == null)
            {
                return;
            }

            _infoButton.gameObject.SetActive(true);

            IFieldElement element = _fieldLogicModel.FieldElements[point];

            if (element != _selectedElement)
            {
                return;
            }

            _commonLabel.gameObject.SetActive(false);

            DefineInfoTypes(_selectedElement);
        }

        private void OnItemInfoClicked()
        {
            if (_selectedElement != null)
            {
                var infoArgs = new ElementInfoArgs {ElementConfig = _selectedElement?.ConfigParameters.ElementConfig};

                _windowSystem.OpenWindow<ElementInfoPresenter>(enableBlackout: true, args: infoArgs);
            }
        }
    }
}