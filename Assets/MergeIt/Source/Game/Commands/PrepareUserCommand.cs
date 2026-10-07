// Copyright (c) 2024, Awessets

using MergeIt.Core.Commands;
using MergeIt.Core.Services;
using MergeIt.Game.User;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Commands
{
    public class PrepareUserCommand : Command
    {
        private readonly IGameLoadService _gameLoadService = DiContainer.Get<IGameLoadService>();
        private readonly IUserService _userService = DiContainer.Get<IUserService>();
        
        public override void Execute()
        {
            var userData = _gameLoadService.Load<UserData>();

            if (userData == null)
            {
                _userService.CreateUser();
            }
            else
            {
                _userService.SetupUser(userData);
            }
        }
    }
}