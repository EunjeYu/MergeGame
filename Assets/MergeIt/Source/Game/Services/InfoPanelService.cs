// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Messages;
using MergeIt.Game.UI.InfoPanel;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;

namespace MergeIt.Game.Services
{
    public class InfoPanelService : IInfoPanelService, IInitializable, IDisposable
    {
        [Introduce]
        private IElementService _elementService;

        [Introduce]
        private IGeneratorsService _generatorsService;

        [Introduce]
        private IMessageBus _messageBus;

        public void Dispose()
        {
            _messageBus.RemoveListener<ElementActionMessage>(OnElementActionMessageHandler);
        }

        public void Initialize()
        {
            _messageBus.AddListener<ElementActionMessage>(OnElementActionMessageHandler);
        }

        private void OnElementActionMessageHandler(ElementActionMessage message)
        {
            IFieldElement fieldElement = message.Element;
            switch (message.ActionType)
            {
                case ElementActionType.SkipCharging:
                    _generatorsService.TrySkipCharging(fieldElement);
                    break;

                case ElementActionType.SkipOpening:
                    _generatorsService.TrySkipOpening(fieldElement);
                    break;

                case ElementActionType.Sell:
                    _elementService.TrySell(fieldElement);
                    break;

                case ElementActionType.Split:
                    _elementService.TrySplit(fieldElement);
                    break;

                case ElementActionType.Open:
                    _generatorsService.TryOpen(fieldElement);
                    break;

                case ElementActionType.Unlock:
                    _elementService.TryUnlock(fieldElement);
                    break;
            }
        }
    }
}