using CryptoQuote.Domain.Business;
using CryptoQuote.Domain.Contracts;
using CryptoQuote.Domain.Models;
using CryptoQuote.Domain.Services;
using Moq;
using Shouldly;

namespace CryptoQuote.Domain.Test
{
    [TestClass]
    public class ExchangeRateCalculatorTest
    {
        [TestMethod]
        [DynamicData(nameof(GetAllCryptoRatesTestDataForCountTest), DynamicDataSourceType.Method)]
        public void CalculateAllRatesPerRootCurrencyRates_CountTest(string[] currencies,
            Func<CurrencyRateResponse> getCurrencyRateReponse,
            int resultCount)
        {
            var exchangeRateCalculator = new ExchangeRateCalculator();

            var result = exchangeRateCalculator.CalculateAllRatesPerRootCurrencyRates(getCurrencyRateReponse.Invoke(), currencies);
            var cnt = result.Count();
            cnt.ShouldBe(resultCount);
        }

        [TestMethod]
        [DynamicData(nameof(GetAllCryptoRatesTestDataForReverseRateTest), DynamicDataSourceType.Method)]
        public void CalculateAllRatesPerRootCurrencyRates_ReverseRateTest(string[] currencies,
            Func<CurrencyRateResponse> getCurrencyRateReponse,
            List<Tuple<string, string, decimal>> correctResult)
        {
            var exchangeRateCalculator = new ExchangeRateCalculator();

            var rootCurrencyRate = getCurrencyRateReponse.Invoke();
            var result = exchangeRateCalculator.CalculateAllRatesPerRootCurrencyRates(rootCurrencyRate, currencies);

            foreach (var item in correctResult)
            {
                result.SingleOrDefault(x => x.BaseCurrency == item.Item1 && x.QuoteCurrency == item.Item2 && x.Rate == item.Item3).ShouldNotBeNull();
            }
        }


        #region Data
        private static IEnumerable<object[]> GetAllCryptoRatesTestDataForCountTest()
        {
            yield return new object[] { Currencies_USD_EUR, new Func<CurrencyRateResponse>(ResultForUSDAsRootAndEURAsQuoteCurrency), 2 };
            yield return new object[] { Currencies_USD_EUR, new Func<CurrencyRateResponse>(ResultForEURAsRootAndUSDAsQuoteCurrency), 2 };
            yield return new object[] { Currencies_USD_EUR_BRL, new Func<CurrencyRateResponse>(ResultForUSDAsRootAndEUR_BRLAsQuoteCurrency), 6 };
            yield return new object[] { Currencies_USD_EUR_BRL, new Func<CurrencyRateResponse>(ResultForEURAsRootAndUSD_BRLAsQuoteCurrency), 6 };
            yield return new object[] { Currencies_USD_EUR_BRL_GBP, new Func<CurrencyRateResponse>(ResultForUSDAsRootAndEUR_BRL_GBPAsQuoteCurrency), 12 };
            yield return new object[] { Currencies_USD_EUR_BRL_GBP, new Func<CurrencyRateResponse>(ResultForEURAsRootAndUSD_BRL_GBPAsQuoteCurrency), 12 };
        }
        private static IEnumerable<object[]> GetAllCryptoRatesTestDataForReverseRateTest()
        {
            yield return new object[] { Currencies_USD_EUR, new Func<CurrencyRateResponse>(ResultForUSDAsRootAndEURAsQuoteCurrency),
                new List<Tuple<string, string, decimal>> { new Tuple<string, string, decimal>("EUR", "USD", 1.05m) } };
            yield return new object[] { Currencies_USD_EUR, new Func<CurrencyRateResponse>(ResultForEURAsRootAndUSDAsQuoteCurrency),
                new List<Tuple<string, string, decimal>> { new Tuple<string, string, decimal>("USD", "EUR", 0.94m) } };
            yield return new object[] { Currencies_USD_EUR_BRL, new Func<CurrencyRateResponse>(ResultForUSDAsRootAndEUR_BRLAsQuoteCurrency),
                new List<Tuple<string, string, decimal>> { 
                    new Tuple<string, string, decimal>("EUR", "USD", Math.Round(1 / 0.75m, 2)),
                    new Tuple<string, string, decimal>("BRL", "USD", Math.Round(1 / 0.36m, 2)),
                    new Tuple<string, string, decimal>("EUR", "BRL", Math.Round(Math.Round(1 / 0.75m, 2) * 0.36m, 2)),
                    new Tuple<string, string, decimal>("BRL", "EUR", Math.Round(Math.Round(1 / 0.36m, 2) * 0.75m, 2))
                } };
            yield return new object[] { Currencies_USD_EUR_BRL, new Func<CurrencyRateResponse>(ResultForEURAsRootAndUSD_BRLAsQuoteCurrency),
                new List<Tuple<string, string, decimal>> {
                    new Tuple<string, string, decimal>("USD", "EUR", Math.Round(1 / 0.72m, 2)),
                    new Tuple<string, string, decimal>("BRL", "EUR", Math.Round(1 / 0.36m, 2)),
                    new Tuple<string, string, decimal>("USD", "BRL", Math.Round(Math.Round(1 / 0.72m, 2) * 0.36m, 2)),
                    new Tuple<string, string, decimal>("BRL", "USD", Math.Round(Math.Round(1 / 0.36m, 2) * 0.72m, 2))
                } };
        }

        private static string[] Currencies_USD_EUR = new string[] { "USD", "EUR" };
        private static string[] Currencies_USD_EUR_BRL = new string[] { "USD", "EUR", "BRL" };
        private static string[] Currencies_USD_EUR_BRL_GBP = new string[] { "USD", "EUR", "BRL", "GBP" };

        private static CurrencyRateResponse ResultForUSDAsRootAndEURAsQuoteCurrency()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "USD",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "EUR", 0.95m }
                }
            };
        }
        private static CurrencyRateResponse ResultForUSDAsRootAndEUR_BRLAsQuoteCurrency()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "USD",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "EUR", 0.75m },
                     { "BRL", 0.36m }
                }
            };
        }
        private static CurrencyRateResponse ResultForUSDAsRootAndEUR_BRL_GBPAsQuoteCurrency()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "USD",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "EUR", 0.75m },
                     { "BRL", 0.36m },
                     { "GBP", 0.2m }
                }
            };
        }

        private static CurrencyRateResponse ResultForEURAsRootAndUSDAsQuoteCurrency()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "EUR",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "USD", 1.06m }
                }
            };
        }
        private static CurrencyRateResponse ResultForEURAsRootAndUSD_BRLAsQuoteCurrency()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "EUR",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "USD", 0.72m },
                     { "BRL", 0.36m }
                }
            };
        }
        private static CurrencyRateResponse ResultForEURAsRootAndUSD_BRL_GBPAsQuoteCurrency()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "EUR",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "USD", 0.75m },
                     { "BRL", 0.26m },
                     { "GBP", 0.23m }
                }
            };
        }

        #endregion
    }
}