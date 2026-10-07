// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Services
{
    public interface IResourcesLoaderService
    {
        T GetObject<T>(string path) where T : Object;
    }
}