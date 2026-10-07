// Copyright (c) 2024, Awessets

using MergeIt.Game.Field;
using UnityEngine;

namespace MergeIt.Game.Services
{
    public class GameServiceModel
    {
        private Camera _mainCamera;
        private Canvas _mainCanvas;

        public FieldData LoadedLevel { get; set; }

        public Camera MainCamera
        {
            get
            {
                if (!_mainCamera)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

        public Canvas MainCanvas
        {
            get
            {
                if (!_mainCanvas)
                {
                    _mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
                }

                return _mainCanvas;
            }
        }
    }
}