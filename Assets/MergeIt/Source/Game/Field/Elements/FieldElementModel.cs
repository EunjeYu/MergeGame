// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.MVP;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Field.Elements
{

    public class FieldElementModel : IModel, IFieldElementModel
    {
        private readonly IMessageBus _messageBus = DiContainer.Get<IMessageBus>();
        
        private bool _selected;
        private Vector3 _initialPosition;
        private GridPoint _point;
        private int _clicksCount;
        
        public bool IsBusy { get; set; }
        public bool IsLocked { get; set; }

        public bool Selected
        {
            get => _selected;
            set
            {
                if (value != _selected)
                {
                    _selected = value;

                    if (_selected)
                    {
                        SendSelected();
                    }
                    else
                    {
                        _clicksCount = 0;
                    }
                }
            }
        }

        public int ClicksCount
        {
            get => _clicksCount;
            set => _clicksCount = value;
        }

        public GridPoint Point
        {
            get => _point;
            set => _point = value;
        }

        private void SendSelected()
        {
            var message = new SelectedElementMessage
            {
                Point = Point
            };

            _messageBus.Fire(message);
        }
    }

}