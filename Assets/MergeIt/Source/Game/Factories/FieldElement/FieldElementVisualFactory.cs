// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.ComponentModel;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Services;
using MergeIt.Game.Factories.Icons;
using MergeIt.Game.Field.Elements;
using MergeIt.Game.Field.Elements.Generator;
using MergeIt.SimpleDI;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace MergeIt.Game.Factories.FieldElement
{
    public class FieldElementVisualFactory : IFieldElementVisualFactory
    {
        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IIconFactory _iconFactory;

        private readonly Dictionary<ElementConfig, IObjectPool<IFieldElementPresenter>> _pools = new();

        private IFieldElement _actualElement;

        public IFieldElementPresenter CreateFieldElement(IFieldElement element)
        {
            ElementConfig config = element.ConfigParameters.ElementConfig;

            _actualElement = element;

            if (!_pools.TryGetValue(config, out IObjectPool<IFieldElementPresenter> pool))
            {
                pool = new ObjectPool<IFieldElementPresenter>(
                    () =>
                    {
                        ElementType type = _actualElement.InfoParameters.Type;
                        IFieldElementView objectPrototype = _configsService.GetElementPrefab(type);
                        GameObject fieldObject = Object.Instantiate(objectPrototype.GameObject);

                        _iconFactory.CreateIcon(config, fieldObject.transform);
                        
                        var view = fieldObject.GetComponent<IFieldElementView>();

                        IFieldElementPresenter elementPresenter = null;
                        switch (type)
                        {
                            case ElementType.Regular:
                                elementPresenter = new FieldElementPresenter();
                                break;

                            case ElementType.Generator:
                                elementPresenter = new FieldElementGeneratorPresenter();
                                break;
                            
                            default:
                                throw new InvalidEnumArgumentException($"Unknown item type: {type}");
                        }

                        if (elementPresenter != null)
                        {
                            elementPresenter.Initialize(view);
                            elementPresenter.Update(_actualElement);

                            if (view != null)
                            {
                                view.Initialize();
                            }
                        }
                        
                        return elementPresenter;
                    },
                    pooledElement =>
                    {
                        pooledElement.Update(_actualElement);
                        pooledElement.Activate(true);
                    },
                    pooledElement =>
                    {
                        pooledElement.Release();
                        pooledElement.Activate(false);
                    },
                    pooledElement =>
                    {
                        pooledElement.Remove();
                    },
                    defaultCapacity: 2,
                    maxSize: 20);

                _pools[config] = pool;
            }

            IFieldElementPresenter presenter = SetupElement(pool);
            return presenter;
        }

        private IFieldElementPresenter SetupElement(IObjectPool<IFieldElementPresenter> pool)
        {
            IFieldElementPresenter presenter = pool.Get();

            return presenter;
        }
    }
}