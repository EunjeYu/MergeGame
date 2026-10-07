// Copyright (c) 2024, Awessets

using MergeIt.Core.User;

namespace MergeIt.Core.Services
{
    public interface IUserService
    {
        void CreateUser();
        void SetupUser(IUserData userData);
    }
}