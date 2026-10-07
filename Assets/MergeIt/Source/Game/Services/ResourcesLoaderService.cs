// Copyright (c) 2024, Awessets

using MergeIt.Core.Services;
using UnityEngine;

namespace MergeIt.Game.Services
{
    public class ResourcesLoaderService : IResourcesLoaderService
    {
        public TResource GetObject<TResource>(string path) where TResource : Object
        {
            var resource = Resources.Load<TResource>(path);

            return resource;
        }
    }
}