using CryptoQuote.Domain.Contracts;
using CryptoQuote.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Domain.Services
{
    public class CryptoQuoteService : ICryptoQuoteService
    {
        private readonly ICurrencyRateService currencyRateService;

        public CryptoQuoteService(ICurrencyRateService currencyRateService)
        {
            this.currencyRateService = currencyRateService;
        }

        public async Task<IEnumerable<ExchangeRate>> GetAllCurrenciesRate(string[] currencies)
        {
            if (currencies == null || currencies.Length == 0)
                return Enumerable.Empty<ExchangeRate>();

            var currencyRate = await currencyRateService.GetLatestRates(currencies);
            if (!currencyRate.CurrenciesRate.Keys.All(x => currencies.Contains(x)))
                throw new Exception("Some currencies are not in result of CurrencyRateService");

            var baseCurrency = currencyRate.BaseCurrency;

            var exchangeRatesPerBaseCurrency = currencyRate.GetExchangeRates();

            var numberOfCurrencies = exchangeRatesPerBaseCurrency.Count();

            var currenciesPair = AllCurrenciesPair(currencies);
            foreach (var pair in currenciesPair)
            {
                var rateIsUpdated = TryToUpdateRateIfPairContainsBaseCurrency(pair, baseCurrency, exchangeRatesPerBaseCurrency);
                if (!rateIsUpdated)
                {
                    var basePairRate = exchangeRatesPerBaseCurrency.Single(x => x.QuoteCurrency == pair.BaseCurrency).GetReverseRate();
                    var quotePairRate = exchangeRatesPerBaseCurrency.Single(x => x.QuoteCurrency == pair.QuoteCurrency).Rate;

                    pair.Rate = Math.Round(basePairRate * quotePairRate, 2);
                }
            }

            return currenciesPair;
        }

        private bool TryToUpdateRateIfPairContainsBaseCurrency(ExchangeRate pair,
            string baseCurrency,
            IEnumerable<ExchangeRate> exchangeRatesPerBaseCurrency)
        {
            if (pair.BaseCurrency == baseCurrency)
            {
                pair.Rate = exchangeRatesPerBaseCurrency.Single(x => x.QuoteCurrency == pair.QuoteCurrency).Rate;
            }
            else if (pair.QuoteCurrency == baseCurrency)
            {
                var foundRate = exchangeRatesPerBaseCurrency.Single(x => x.QuoteCurrency == pair.BaseCurrency);
                pair.Rate = foundRate.GetReverseRate();
            }

            return pair.Rate > 0;
        }

        private IEnumerable<ExchangeRate> AllCurrenciesPair(string[] currencies)
        {
            var pair = new List<ExchangeRate>();
            for (int i = 0; i < currencies.Length; i++)
            {
                for (int j = 0; j < currencies.Length; j++)
                {
                    if (i == j)
                        continue;

                    pair.Add(new ExchangeRate(currencies[i], currencies[j]));
                }
            }
            return pair;
        }
    }
}
