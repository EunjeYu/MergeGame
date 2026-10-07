// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;

namespace MergeIt.Core.Services
{
    public interface ICurrencyService
    {
        bool TryPay(CurrencySettings currencySettings);
        void Sell(CurrencySettings currencySettings);
    }
}