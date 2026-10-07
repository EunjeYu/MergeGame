// Copyright (c) 2024, Awessets

using MergeIt.Game.Services;

namespace MergeIt.Game.HUD
{
    public interface IUserListener
    {
        void ApplyModel(UserServiceModel userServiceModel);
    }
}