using CryptoQuote.Infra.CurrencyServices;
using CryptoQuote.Infra.HttpServices;
using CryptoQuote.Infra.Test;
using Microsoft.Extensions.Configuration;
using Shouldly;

namespace CryptoQuote.Infra.Test
{
    [TestClass]
    public class CoinMarketApiServiceTest
    {
        private IConfigurationRoot config = null!;

        [TestInitialize]
        public void Initialize()
        {
            config = Configuration.InitConfiguration();
        }

        [TestMethod]
        [DynamicData(nameof(GetCryptoRateTestData), DynamicDataSourceType.Method)]
        public async Task GetCryptoRate_ResultShouldNotBeNullTest(string symbol, string currency)
        {
            var settings = config.GetCoinMarketCapApiSettings();

            var apiService = new CoinMarketCapApiService(new HttpClientService(new HttpClient()), settings);

            var response = await apiService.GetCryptoRate(symbol, currency);

            response.ShouldNotBeNull();
        }

        [TestMethod]
        [DynamicData(nameof(GetCryptoRateTestData), DynamicDataSourceType.Method)]
        public async Task GetCryptoRate_QuoteUnitShouldBeOneTest(string symbol, string currency)
        {
            var settings = config.GetCoinMarketCapApiSettings();

            var apiService = new CoinMarketCapApiService(new HttpClientService(new HttpClient()), settings);

            var response = await apiService.GetCryptoRate(symbol, currency);

            response.GroupBy(x => x.Unit).Count().ShouldBe(1);
        }

        private static IEnumerable<object[]> GetCryptoRateTestData()
        {
            yield return new object[] { "BTC", "USD" };
            yield return new object[] { "BTC", "EUR" };
        }
    }
}