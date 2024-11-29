using CryptoQuote.Domain.Models;

namespace CryptoQuote.Domain.Business
{
    internal class ExchangeRateCalculator
    {
        public IEnumerable<ExchangeRate> CalculateAllRatesPerRootCurrencyRates(CurrencyRateResponse rootCurrencyRate, string[] currencies)
        {
            var rootCurrency = rootCurrencyRate.BaseCurrency;
            var exchangeRatesPerRootCurrency = rootCurrencyRate.GetExchangeRates();

            var unQuotedCurrencyPairs = ExchangeRate.GenerateUnquotedCurrencyPairs(currencies);
            foreach (var unQuotedCurrencyPair in unQuotedCurrencyPairs)
            {
                if (unQuotedCurrencyPair.BaseCurrency == rootCurrency)
                {
                    unQuotedCurrencyPair.Rate = exchangeRatesPerRootCurrency.Single(x => x.QuoteCurrency == unQuotedCurrencyPair.QuoteCurrency).Rate;
                }
                else if (unQuotedCurrencyPair.QuoteCurrency == rootCurrency)
                {
                    var foundQuotedExchangeRate = exchangeRatesPerRootCurrency.Single(x => x.QuoteCurrency == unQuotedCurrencyPair.BaseCurrency);
                    unQuotedCurrencyPair.Rate = foundQuotedExchangeRate.GetReverseRate();
                }
                else
                {
                    var rateOfBaseCurrencyPerRootCurrency = exchangeRatesPerRootCurrency.Single(x => x.QuoteCurrency == unQuotedCurrencyPair.BaseCurrency);
                    var rateOfQuoteCurrencyPerRootCurrency = exchangeRatesPerRootCurrency.Single(x => x.QuoteCurrency == unQuotedCurrencyPair.QuoteCurrency).Rate;

                    unQuotedCurrencyPair.Rate = Math.Round(rateOfBaseCurrencyPerRootCurrency.GetReverseRate() * rateOfQuoteCurrencyPerRootCurrency, 2);
                }
            }

            return unQuotedCurrencyPairs;
        }
    }
}