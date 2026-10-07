// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Helpers;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Effects;
using MergeIt.Game.Effects.Controllers;
using MergeIt.Game.Effects.Parameters;
using MergeIt.Game.Enums;
using MergeIt.Game.Messages;
using MergeIt.Game.Services;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Field.Actions
{
    public class FieldConsumableProcessor : FieldActionProcessorBase
    {
        private readonly UserServiceModel _userServiceModel = DiContainer.Get<UserServiceModel>();
        private readonly IEffectsFactory _effectsFactory = DiContainer.Get<IEffectsFactory>();
        private readonly IGameSaveService _saveService = DiContainer.Get<IGameSaveService>();

        public override void ProcessClick(FieldCellComponent cellComponent)
        {
            base.ProcessClick(cellComponent);

            if (cellComponent.FieldElementPresenter?.GetModel().ClicksCount < 2)
            {
                return;
            }

            if (!FieldLogicModel.FieldElements.TryGetValue(cellComponent.Point, out IFieldElement fieldElement))
            {
                return;
            }

            if (fieldElement.InfoParameters.IsBlocked)
            {
                return;
            }

            ElementCommonSettings settings = fieldElement.ConfigParameters.ElementConfig.CommonSettings;

            if (!settings.IsConsumable)
            {
                return;
            }

            ConsumableSettings consumableSettings = settings.ConsumableSettings;
            Transform transform = cellComponent.FieldElementPresenter.Transform;
            MoveEffectParameters parameters = null;
            
            MessageBus.Fire<ResetHintsMessage>();

            switch (consumableSettings.Target)
            {
                case ConsumableTargetType.SoftCurrency:
                    _userServiceModel.SoftCurrency.ApplyOperation(consumableSettings.Operation, consumableSettings.Value, false);
                    _saveService.Save(GameSaveType.User);
                    parameters = new MoveEffectParameters(transform.position, Vector3.zero, HudPanelType.SoftCurrency);
                    _effectsFactory.CreateEffect<MoveToUiEffectWithPresenter>(cellComponent.FieldElementPresenter, parameters, OnEffectFinished);
                    break;

                case ConsumableTargetType.HardCurrency:
                    _userServiceModel.HardCurrency.ApplyOperation(consumableSettings.Operation, consumableSettings.Value, false);
                    _saveService.Save(GameSaveType.User);
                    parameters = new MoveEffectParameters(transform.position, Vector3.zero, HudPanelType.HardCurrency);
                    _effectsFactory.CreateEffect<MoveToUiEffectWithPresenter>(cellComponent.FieldElementPresenter, parameters, OnEffectFinished);
                    break;

                case ConsumableTargetType.Time:
                    RemoveItem(cellComponent);
                    MessageBus.Fire(new SkipTimeMessage {Seconds = consumableSettings.Value});
                    break;

                case ConsumableTargetType.Energy:
                    _userServiceModel.Energy.ApplyOperation(consumableSettings.Operation, consumableSettings.Value, false);
                    _saveService.Save(GameSaveType.User);
                    parameters = new MoveEffectParameters(transform.position, Vector3.zero, HudPanelType.Energy);
                    _effectsFactory.CreateEffect<MoveToUiEffectWithPresenter>(cellComponent.FieldElementPresenter, parameters, OnEffectFinished);
                    break;

                case ConsumableTargetType.Experience:
                    _userServiceModel.Experience.ApplyOperation(consumableSettings.Operation, consumableSettings.Value, false);
                    _saveService.Save(GameSaveType.User);
                    parameters = new MoveEffectParameters(transform.position, Vector3.zero, HudPanelType.UserProgress);
                    _effectsFactory.CreateEffect<MoveToUiEffectWithPresenter>(cellComponent.FieldElementPresenter, parameters, OnEffectFinished);
                    break;

                case ConsumableTargetType.Splitter:
                    _userServiceModel.Splitters.ApplyOperation(consumableSettings.Operation, consumableSettings.Value, false);
                    _saveService.Save(GameSaveType.User);
                    parameters = new MoveEffectParameters(transform.position, Vector3.zero, HudPanelType.UserProgress);
                    _effectsFactory.CreateEffect<MoveToUiEffectWithPresenter>(cellComponent.FieldElementPresenter, parameters, OnEffectFinished);
                    break;
            }

            void OnEffectFinished()
            {
                RemoveItem(cellComponent);
            }
        }

        private void RemoveItem(FieldCellComponent cellComponent)
        {
            var removeMessage = new RemoveElementMessage
            {
                RemoveAtPoint = cellComponent.Point
            };

            MessageBus.Fire(removeMessage);

            SendResetMessage();
        }

        private void SendResetMessage()
        {
            var resetMessage = new ResetSelectionMessage();

            MessageBus.Fire(resetMessage);
        }
    }
}