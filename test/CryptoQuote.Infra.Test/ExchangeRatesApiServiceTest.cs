using CryptoQuote.Infra.CurrencyServices;
using CryptoQuote.Infra.HttpServices;
using CryptoQuote.Infra.Test;
using Microsoft.Extensions.Configuration;
using Shouldly;

namespace CryptoQuote.Infra.Test
{
    [TestClass]
    public class ExchangeRatesApiServiceTest
    {
        private IConfigurationRoot config = null!;

        [TestInitialize]
        public void Initialize()
        {
            config = Configuration.InitConfiguration();
        }

        [TestMethod]
        [DynamicData(nameof(GetLatestRatesTestData), DynamicDataSourceType.Method)]
        [DataRow(new string[] { "USD", "EUR", "BRL", "GBP", "AUD" })]
        public async Task GetLatestRates_ResultShouldNotBeNullTest(IEnumerable<string> currencies)
        {
            var settings = config.GetExchangeRatesApiSettings();

            var apiService = new ExchangeRatesApiService(new HttpClientService(new HttpClient()), settings);

            var response = await apiService.GetLatestRates(currencies);

            response.ShouldNotBeNull();
        }

        [TestMethod]
        [DynamicData(nameof(GetLatestRatesTestData), DynamicDataSourceType.Method)]
        [DataRow(new string[] { "USD", "EUR", "BRL", "GBP", "AUD" })]
        public async Task GetLatestRates_CurrenciesRateCountTest(IEnumerable<string> currencies)
        {
            var settings = config.GetExchangeRatesApiSettings();

            var apiService = new ExchangeRatesApiService(new HttpClientService(new HttpClient()), settings);

            var response = await apiService.GetLatestRates(currencies);

            response.CurrenciesRate.Count.ShouldBe(currencies.Count());
        }

        [TestMethod]
        [DynamicData(nameof(GetLatestRatesTestData), DynamicDataSourceType.Method)]
        [DataRow(new string[] { "USD", "EUR", "BRL", "GBP", "AUD" })]
        public async Task GetLatestRates_BaseCurrencyRateShouldBeOneTest(IEnumerable<string> currencies)
        {
            var settings = config.GetExchangeRatesApiSettings();

            var apiService = new ExchangeRatesApiService(new HttpClientService(new HttpClient()), settings);

            var response = await apiService.GetLatestRates(currencies);

            if(response.CurrenciesRate.TryGetValue(response.BaseCurrency, out decimal rate))
                rate.ShouldBe(1);
            rate.ShouldBe(0);
        }

        private static IEnumerable<object[]> GetLatestRatesTestData()
        {
            yield return new object[] { new string[] { "USD"} };
            yield return new object[] { new string[] { "USD", "EUR" } };
            yield return new object[] { new string[] { "USD", "EUR", "BRL" } };
            yield return new object[] { new string[] { "USD", "EUR", "BRL", "GBP" } };
            yield return new object[] { new string[] { "USD", "EUR", "BRL", "GBP", "AUD" } };
        }
    }
}