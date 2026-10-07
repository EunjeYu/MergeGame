// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.Services;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Services
{
    public class CurrencyService : ICurrencyService
    {
        [Introduce]
        private UserServiceModel _userServiceModel;

        public bool TryPay(CurrencySettings currencySettings)
        {
            bool result = false;
            switch (currencySettings.Currency)
            {
                case CurrencyType.Soft:
                    if (_userServiceModel.SoftCurrency.Value > currencySettings.Amount)
                    {
                        _userServiceModel.SoftCurrency.Value -= currencySettings.Amount;
                        result = true;
                    }

                    break;

                case CurrencyType.Hard:
                    if (_userServiceModel.HardCurrency.Value > currencySettings.Amount)
                    {
                        _userServiceModel.HardCurrency.Value -= currencySettings.Amount;
                        result = true;
                    }

                    break;

                case CurrencyType.Splitter:
                    if (_userServiceModel.Splitters.Value > currencySettings.Amount)
                    {
                        _userServiceModel.Splitters.Value -= currencySettings.Amount;
                        result = true;
                    }

                    break;

                case CurrencyType.Energy:
                    if (_userServiceModel.Energy.Value > currencySettings.Amount)
                    {
                        _userServiceModel.Energy.Value -= currencySettings.Amount;
                        result = true;
                    }

                    break;
            }

            return result;
        }

        public void Sell(CurrencySettings currencySettings)
        {
            switch (currencySettings.Currency)
            {
                case CurrencyType.Soft:
                    _userServiceModel.SoftCurrency.Value += currencySettings.Amount;
                    break;

                case CurrencyType.Hard:
                    _userServiceModel.HardCurrency.Value += currencySettings.Amount;
                    break;

                case CurrencyType.Splitter:
                    _userServiceModel.Splitters.Value += currencySettings.Amount;
                    break;

                case CurrencyType.Energy:
                    _userServiceModel.Energy.Value += currencySettings.Amount;
                    break;
            }
        }
    }
}