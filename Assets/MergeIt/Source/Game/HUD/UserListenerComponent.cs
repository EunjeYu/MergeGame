// Copyright (c) 2024, Awessets

using MergeIt.Game.Services;
using UnityEngine;

namespace MergeIt.Game.HUD
{
    public abstract class UserListenerComponent : MonoBehaviour, IUserListener
    {
        protected UserServiceModel UserServiceModel { get; private set; }

        public void ApplyModel(UserServiceModel userServiceModel)
        {
            UserServiceModel = userServiceModel;
            
            OnApplyModel(userServiceModel);
        }

        protected abstract void OnApplyModel(UserServiceModel userServiceModel);
    }
}