// Copyright (c) 2024, Awessets

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Helpers;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Factories.Field;
using MergeIt.Game.Factories.FieldElement;
using MergeIt.Game.Field;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine;

namespace MergeIt.Game.Services
{
    public class GameFieldService : IGameFieldService, IInitializable, IDisposable
    {
        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IFieldElementFactory _fieldElementFactory;

        [Introduce]
        private IFieldFactory _fieldFactory;

        [Introduce]
        private FieldLogicModel _fieldLogicModel;

        [Introduce]
        private GameServiceModel _gameServiceModel;

        [Introduce]
        private IMessageBus _messageBus;

        public void Dispose()
        {
            _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
        }

        public GridPoint? GetFreeCell()
        {
            int fieldHeight = _fieldLogicModel.FieldHeight;
            int fieldWidth = _fieldLogicModel.FieldWidth;

            var randomHeight = ListExtensions.GenerateShuffledArray(fieldHeight);
            var randomWidth = ListExtensions.GenerateShuffledArray(fieldWidth);

            for (int i = 0; i < randomHeight.Count; i++)
            {
                for (int j = 0; j < randomWidth.Count; j++)
                {
                    int row = randomHeight[i];
                    int column = randomWidth[j];

                    var point = GridPoint.Create(row, column);
                    if (!_fieldLogicModel.FieldElements.ContainsKey(point))
                    {
                        return point;
                    }
                }
            }

            return null;
        }

        public GridPoint? GetNearCell(GridPoint generator)
        {
            //y축과 관련된것이 width , x축과 관련된것이 height
            int a = Mathf.Abs(generator.Y - _fieldLogicModel.FieldWidth);
            int b = Mathf.Abs(generator.X - _fieldLogicModel.FieldHeight);
            int round = a > b ? a : b;

            int x = generator.X;
            int y = generator.Y;

            List<GridPoint> pntList = new List<GridPoint>();

            for (int i = 1; i < round; i++)
            {
                for (int j = x - i; j <= x + i; j++)
                {
                    for (int k = y - i; k <= y + i; k++)
                    {
                        if (Mathf.Abs(j - x) > (i - 1) || Mathf.Abs(k - y) > i - 1)
                        {
                            GridPoint gridPoint = new GridPoint(j, k);
                            pntList.Add(gridPoint);
                        }
                    }
                }

                pntList.Shuffle();

                for (int l = 0; l < pntList.Count; l++)
                {
                    if (IsAvailablePoint(pntList[l]))
                        return pntList[l];
                }

                pntList.Clear();
            }

            return null;
        }
        
        bool IsAvailablePoint(GridPoint pnt)
        {
            return pnt.X >= 0 &&
           pnt.X < _fieldLogicModel.FieldHeight &&
           pnt.Y >= 0 &&
           pnt.Y < _fieldLogicModel.FieldWidth &&
           !_fieldLogicModel.FieldElements.ContainsKey(pnt);
        }

        public IFieldElement CreateNewElement(ElementConfig config, GridPoint point, bool blocked = false)
        {
            IFieldElement newElement =
                _fieldElementFactory.CreateFieldElement(config, point, blocked);

            return newElement;
        }

        public void Initialize()
        {
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
        }

        private void OnLoadedGameMessageHandler(LoadedGameMessage message)
        {
            FieldPresenter field = _fieldFactory.CreateField(_gameServiceModel.MainCanvas.transform);
            field.Initialize();
        }
    }
}