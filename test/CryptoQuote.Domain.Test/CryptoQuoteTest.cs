using CryptoQuote.Domain.Contracts;
using CryptoQuote.Domain.Models;
using CryptoQuote.Domain.Services;
using Moq;
using Shouldly;

namespace CryptoQuote.Domain.Test
{
    [TestClass]
    public class CryptoQuoteTest
    {
        [TestMethod]
        [DynamicData(nameof(CountTestData), DynamicDataSourceType.Method)]
        public async Task GetAllCurrenciesRate_CountTest(string[] validCurrencies, Func<Task<CurrencyRateResponse>> resultOfMokService)
        {
            var currencyRateService = new Mock<ICurrencyRateService>();
            currencyRateService.Setup(x => x.GetLatestRates(validCurrencies)).Returns(resultOfMokService);


            var cryptoQuoteService = new CryptoQuoteService(currencyRateService.Object);

            var result = await cryptoQuoteService.GetAllCurrenciesRate(validCurrencies);

            var countOfCurrencies = validCurrencies.Length;
            result.Count().ShouldBeEquivalentTo(countOfCurrencies * (countOfCurrencies - 1));
        }

        [TestMethod]
        [DynamicData(nameof(RateOfOtherCurrencyTestData), DynamicDataSourceType.Method)]
        public async Task GetAllCurrenciesRate_RateOfOtherCurrencyEqualityTest(string[] validCurrencies,
            Func<Task<CurrencyRateResponse>> resultOfMokService, string baseCurrency, string quoteCurrency, decimal rate)
        {
            var currencyRateService = new Mock<ICurrencyRateService>();
            currencyRateService.Setup(x => x.GetLatestRates(validCurrencies)).Returns(resultOfMokService);

            var cryptoQuoteService = new CryptoQuoteService(currencyRateService.Object);

            var result = await cryptoQuoteService.GetAllCurrenciesRate(validCurrencies);

            result
                .Single(x => x.BaseCurrency == baseCurrency && x.QuoteCurrency == quoteCurrency)
                .Rate
                .ShouldBe(rate);
        }

        [TestMethod]
        [DynamicData(nameof(RateOfOtherCurrencyTestData), DynamicDataSourceType.Method)]
        public async Task GetAllCurrenciesRate_RateOfOtherCurrencyUnEqualityTest(string[] validCurrencies,
            Func<Task<CurrencyRateResponse>> resultOfMokService, string baseCurrency, string quoteCurrency, decimal rate)
        {
            var currencyRateService = new Mock<ICurrencyRateService>();
            currencyRateService.Setup(x => x.GetLatestRates(validCurrencies)).Returns(resultOfMokService);


            var cryptoQuoteService = new CryptoQuoteService(currencyRateService.Object);

            var result = await cryptoQuoteService.GetAllCurrenciesRate(validCurrencies);

            result
                .Single(x => x.BaseCurrency == baseCurrency && x.QuoteCurrency == quoteCurrency)
                .Rate
                .ShouldNotBe(rate + 0.01m);

            result
                .Single(x => x.BaseCurrency == baseCurrency && x.QuoteCurrency == quoteCurrency)
                .Rate
                .ShouldNotBe(rate - 0.01m);
        }

        #region Data
        private static string[] Currencies_USD_EUR = new string[] { "USD", "EUR" };
        private static string[] Currencies_USD_EUR_BRL = new string[] { "USD", "EUR", "BRL" };
        private static string[] Currencies_USD_EUR_BRL_GBP = new string[] { "USD", "EUR", "BRL", "GBP" };
        private static string[] Currencies_USD_EUR_BRL_GBP_AUD = new string[] { "USD", "EUR", "BRL", "GBP", "AUD" };

        private static new Func<Task<CurrencyRateResponse>> Func_ResultForUSD_EUR = new Func<Task<CurrencyRateResponse>>(ResultForUSD_EUR);
        private static new Func<Task<CurrencyRateResponse>> Func_ResultForUSD_EUR_BRL = new Func<Task<CurrencyRateResponse>>(ResultForUSD_EUR_BRL);
        private static new Func<Task<CurrencyRateResponse>> Func_ResultForUSD_EUR_BRL_GBP = new Func<Task<CurrencyRateResponse>>(ResultForUSD_EUR_BRL_GBP);
        private static new Func<Task<CurrencyRateResponse>> Func_ResultForUSD_EUR_BRL_GBP_AUD = new Func<Task<CurrencyRateResponse>>(ResultForUSD_EUR_BRL_GBP_AUD);


        private static IEnumerable<object[]> CountTestData()
        {
            yield return new object[] { Currencies_USD_EUR, Func_ResultForUSD_EUR };
            yield return new object[] { Currencies_USD_EUR_BRL, Func_ResultForUSD_EUR_BRL };
            yield return new object[] { Currencies_USD_EUR_BRL_GBP, Func_ResultForUSD_EUR_BRL_GBP };
            yield return new object[] { Currencies_USD_EUR_BRL_GBP_AUD, Func_ResultForUSD_EUR_BRL_GBP_AUD };
        }

        private static IEnumerable<object[]> RateOfOtherCurrencyTestData()
        {
            yield return new object[] { Currencies_USD_EUR, Func_ResultForUSD_EUR, "EUR", "USD", 1.05m };
            yield return new object[] { Currencies_USD_EUR_BRL, Func_ResultForUSD_EUR_BRL, "EUR", "BRL", 6.28m };
            yield return new object[] { Currencies_USD_EUR_BRL, Func_ResultForUSD_EUR_BRL, "BRL", "USD", 0.17m };
            yield return new object[] { Currencies_USD_EUR_BRL_GBP, Func_ResultForUSD_EUR_BRL_GBP, "EUR", "GBP", 0.87m };
            yield return new object[] { Currencies_USD_EUR_BRL_GBP, Func_ResultForUSD_EUR_BRL_GBP, "GBP", "USD", 1.20m };
            yield return new object[] { Currencies_USD_EUR_BRL_GBP_AUD, Func_ResultForUSD_EUR_BRL_GBP_AUD, "GBP", "AUD", 1.94m };
            yield return new object[] { Currencies_USD_EUR_BRL_GBP_AUD, Func_ResultForUSD_EUR_BRL_GBP_AUD, "AUD", "USD", 0.62m };
        }

        private static async Task<CurrencyRateResponse> ResultForUSD_EUR()
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

        private static async Task<CurrencyRateResponse> ResultForUSD_EUR_BRL()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "USD",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "EUR", 0.95m },
                     { "BRL", 5.98m }
                }
            };
        }

        private static async Task<CurrencyRateResponse> ResultForUSD_EUR_BRL_GBP()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "USD",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "EUR", 0.95m },
                     { "BRL", 5.98m },
                     { "GBP", 0.83m }
                }
            };
        }

        private static async Task<CurrencyRateResponse> ResultForUSD_EUR_BRL_GBP_AUD()
        {
            return new CurrencyRateResponse
            {
                BaseCurrency = "USD",
                Date = DateOnly.FromDateTime(DateTime.Now),
                CurrenciesRate = new Dictionary<string, decimal>
                {
                     { "EUR", 0.95m },
                     { "BRL", 5.98m },
                     { "GBP", 0.83m },
                     { "AUD", 1.62m }
                }
            };
        }
        #endregion
    }
}