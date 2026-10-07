// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Field.Actions
{
    public abstract class FieldActionProcessorBase : IFieldActionProcessor
    {
        protected readonly IMessageBus MessageBus = DiContainer.Get<IMessageBus>();
        protected readonly IGameSaveService SaveService = DiContainer.Get<IGameSaveService>();
        protected readonly FieldLogicModel FieldLogicModel = DiContainer.Get<FieldLogicModel>();

        public virtual void ProcessClick(FieldCellComponent cellComponent)
        {
            
        }
        
        public virtual void ProcessEndDrag(GridPoint fromPoint, GameObject toGameObject)
        {
            
        }
    }
}