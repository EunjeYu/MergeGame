// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Commands;
using MergeIt.Core.Services;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Commands
{
    public class LoadConfigsCommand : Command
    {
        private readonly IConfigsService _configsService = DiContainer.Get<IConfigsService>();

        public async override UniTask ExecuteAsync()
        {
            await _configsService.Load();
        }
    }
}