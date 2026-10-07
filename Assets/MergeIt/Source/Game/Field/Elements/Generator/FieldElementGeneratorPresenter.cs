// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Field.Elements.Generator
{
    public class FieldElementGeneratorPresenter : FieldElementPresenterBase<FieldElementGeneratorView, FieldElementGeneratorModel>
    {
        private readonly IMessageBus _messageBus;
        private IGeneratorParameters _generatorParameters;
        private IGeneratorOpenParameters _generatorOpenParameters;
        
        public FieldElementGeneratorPresenter()
        {
            _messageBus = DiContainer.Get<IMessageBus>();
            _messageBus.AddListener<CheckGeneratorMessage>(CheckGeneratorMessageHandler);
            _messageBus.AddListener<GeneratorOpenStartMessage>(GeneratorOpenStartMessageHandler);
        }

        public override void Update(IFieldElement fieldElement)
        {
            base.Update(fieldElement);
            
            _generatorParameters = FieldElement.GeneratorParameters;
            _generatorOpenParameters = FieldElement.GeneratorOpenParameters;
            
            Check();
            CheckOpening();
        }

        public override void Release()
        {
            base.Release();
            
            View.HideTimer();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _messageBus.RemoveListener<CheckGeneratorMessage>(CheckGeneratorMessageHandler);
            _messageBus.RemoveListener<GeneratorOpenStartMessage>(GeneratorOpenStartMessageHandler);
        }
        
        private void CheckGeneratorMessageHandler(CheckGeneratorMessage message)
        {
            if (Model.Point == message.GeneratorPoint)
            {
                Check();
            }
        }
        
        private void GeneratorOpenStartMessageHandler(GeneratorOpenStartMessage message)
        {
            if (Model.Point == message.GeneratorPoint)
            {
                CheckOpening();
            }
        }

        private void Check()
        {
            if (_generatorParameters.AvailableToDrop == 0)
            {
                View.SetTimer(_generatorParameters.MinDropChargeTime, _generatorParameters.MinDropFullChargeTime);
            }
        }
        
        private void CheckOpening()
        {
            if (_generatorOpenParameters?.IsOpening == true)
            {
                var fullOpenTime = FieldElement.ConfigParameters.ElementConfig.GeneratorSettings.OpenTime;
                View.SetTimer(_generatorOpenParameters.RemainingTime, fullOpenTime);
            }
        }
    }
}