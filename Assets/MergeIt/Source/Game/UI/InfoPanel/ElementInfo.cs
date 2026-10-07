// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.UI.InfoPanel
{
    public abstract class ElementInfo : MonoBehaviour
    {
        protected IConfigsService ConfigsService;
        protected IMessageBus MessageBus;

        protected IFieldElement SelectedElement;
        public abstract ElementActionType ActionType { get; }

        private void Start()
        {
            MessageBus = DiContainer.Get<IMessageBus>();

            OnStart();
        }

        private void OnDisable()
        {
            Clear();
        }

        private void OnDestroy()
        {
            Destroy();
        }

        public void TrySetup(IFieldElement fieldElement)
        {
            ConfigsService = DiContainer.Get<IConfigsService>();
            SelectedElement = fieldElement;

            if (!OnTrySetup())
            {
                SelectedElement = null;
                gameObject.SetActive(false);
            }
        }

        protected abstract bool OnTrySetup();

        protected void Clear()
        {
            SelectedElement = null;

            OnClear();
        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnClear()
        {
        }

        protected virtual void Destroy()
        {

        }

        protected void ActionButtonClick()
        {
            OnActionButtonClick();
        }

        protected virtual void OnActionButtonClick()
        {
            SendActionMessage();
        }

        protected void SendActionMessage()
        {
            var message = new ElementActionMessage
            {
                Element = SelectedElement,
                ActionType = ActionType
            };

            MessageBus.Fire(message);
        }
    }
}