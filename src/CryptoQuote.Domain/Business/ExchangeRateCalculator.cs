using CryptoQuote.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Domain.Business
{
    internal class ExchangeRateCalculator
    {
        public IEnumerable<ExchangeRate> CalculateAllRatesPerBaseCurrencyRates(CurrencyRateResponse currencyRate, string[] currencies)
        {
            var baseCurrency = currencyRate.BaseCurrency;

            var exchangeRatesPerBaseCurrency = currencyRate.GetExchangeRates();

            var numberOfCurrencies = exchangeRatesPerBaseCurrency.Count();

            var emptyExchangeRates = ExchangeRate.GenerateForCurrencies(currencies);
            foreach (var emptyExchangeRate in emptyExchangeRates)
            {
                var rateIsUpdated = TryToUpdateRateIfPairContainsBaseCurrency(emptyExchangeRate, baseCurrency, exchangeRatesPerBaseCurrency);
                if (!rateIsUpdated)
                {
                    var basePairRate = exchangeRatesPerBaseCurrency.Single(x => x.QuoteCurrency == emptyExchangeRate.BaseCurrency).GetReverseRate();
                    var quotePairRate = exchangeRatesPerBaseCurrency.Single(x => x.QuoteCurrency == emptyExchangeRate.QuoteCurrency).Rate;

                    emptyExchangeRate.Rate = Math.Round(basePairRate * quotePairRate, 2);
                }
            }

            return emptyExchangeRates;
        }

        private bool TryToUpdateRateIfPairContainsBaseCurrency(ExchangeRate emptyExchangeRate,
            string baseCurrency,
            IEnumerable<ExchangeRate> exchangeRatesPerBaseCurrency)
        {
            if (emptyExchangeRate.BaseCurrency == baseCurrency)
            {
                emptyExchangeRate.Rate = exchangeRatesPerBaseCurrency.Single(x => x.QuoteCurrency == emptyExchangeRate.QuoteCurrency).Rate;
            }
            else if (emptyExchangeRate.QuoteCurrency == baseCurrency)
            {
                var foundRate = exchangeRatesPerBaseCurrency.Single(x => x.QuoteCurrency == emptyExchangeRate.BaseCurrency);
                emptyExchangeRate.Rate = foundRate.GetReverseRate();
            }

            return emptyExchangeRate.Rate >= 0;
        }
    }
}
