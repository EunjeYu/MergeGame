// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using MergeIt.Core.Services;
using MergeIt.Game.Field;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Factories.Field
{
    public class FieldFactory : IFieldFactory
    {
        public const string FieldGameObjectPath = "Prefabs/Field/FieldContainer";
        public const string FieldCellObjectOddPath = "Prefabs/Field/FieldCellOdd";
        public const string FieldCellObjectEvenPath = "Prefabs/Field/FieldCellEven";
        
        [Introduce]
        private IResourcesLoaderService _resourcesLoaderService;

        public FieldPresenter CreateField(Transform parent)
        {
            var fieldViewPrefab = _resourcesLoaderService.GetObject<FieldView>(FieldGameObjectPath);
            var fieldViewObject = Object.Instantiate(fieldViewPrefab.gameObject, parent);
            
            fieldViewObject.TryGetComponent(out FieldView fieldView);

            if (fieldView)
            {
                var presenter = new FieldPresenter();
                presenter.Initialize(fieldView);
                
                fieldView.Initialize();

                return presenter;
            }

            return null;
        }
        
        public FieldCellComponent CreateFieldCell(Transform parent, GridPoint gridPoint)
        {
            int sum = (gridPoint.X + gridPoint.Y) % 2;
            var fieldCellPrefab = sum == 0 ? 
                    _resourcesLoaderService.GetObject<FieldCellComponent>(FieldCellObjectEvenPath) :
                    _resourcesLoaderService.GetObject<FieldCellComponent>(FieldCellObjectOddPath) ;
            var fieldCellObject = Object.Instantiate(fieldCellPrefab.gameObject, parent);

            fieldCellObject.TryGetComponent(out FieldCellComponent fieldCell);

            if (fieldCell)
            {
                fieldCell.Initialize(gridPoint);
            }
            
            return fieldCell;
        }
    }
}