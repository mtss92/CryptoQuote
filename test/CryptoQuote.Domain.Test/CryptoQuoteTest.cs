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
        [DynamicData(nameof(GetAllCryptoRatesWithDetailsTestDataOnBTC), DynamicDataSourceType.Method)]
        [DynamicData(nameof(GetAllCryptoRatesWithDetailsTestDataOnBTC_ETH), DynamicDataSourceType.Method)]
        public async Task GetAllCryptoRatesWithDetails_CountTest(string symbol,
            Dictionary<string, Func<Task<IEnumerable<CryptoRate>>>> pairOfCurrencyAndFuncToGetCryptoQuote,
            Func<Task<CurrencyRateResponse>> resultOfMokCurrencyRateService)
        {
            var cryptoMarketService = new Mock<ICryptoMarketService>();
            foreach (var item in pairOfCurrencyAndFuncToGetCryptoQuote)
            {
                cryptoMarketService.Setup(x => x.GetCryptoRate(symbol, item.Key)).Returns(item.Value);
            }

            var quoteUnits = pairOfCurrencyAndFuncToGetCryptoQuote.Keys.ToArray();

            var currencyRateService = new Mock<ICurrencyRateService>();
            currencyRateService.Setup(x => x.GetLatestRates(quoteUnits)).Returns(resultOfMokCurrencyRateService);

            var cryptoQuoteService = new CryptoQuoteService(currencyRateService.Object, cryptoMarketService.Object);

            var result = await cryptoQuoteService.GetAllCryptoRatesWithDetails(symbol, quoteUnits);

            result.CryptoRates.Count().ShouldBe(quoteUnits.Length * 2);
        }

        [TestMethod]
        [DynamicData(nameof(GetAllCryptoRatesTestData), DynamicDataSourceType.Method)]
        public async Task GetAllCryptoRates_CountTest(string symbol, string quoteUnit, Func<Task<IEnumerable<CryptoRate>>> resultOfMokService)
        {
            var cryptoMarketService = new Mock<ICryptoMarketService>();
            cryptoMarketService.Setup(x => x.GetCryptoRate(symbol, quoteUnit)).Returns(resultOfMokService);


            var cryptoQuoteService = new CryptoQuoteService(null!, cryptoMarketService.Object);

            var result = await cryptoQuoteService.GetAllCryptoRates(symbol, new string[] { quoteUnit });

            result.Count().ShouldBe(2);
        }

        [TestMethod]
        [DynamicData(nameof(CountTestData), DynamicDataSourceType.Method)]
        public async Task GetAllCurrenciesRate_CountTest(string[] validCurrencies, Func<Task<CurrencyRateResponse>> resultOfMokService)
        {
            var currencyRateService = new Mock<ICurrencyRateService>();
            currencyRateService.Setup(x => x.GetLatestRates(validCurrencies)).Returns(resultOfMokService);


            var cryptoQuoteService = new CryptoQuoteService(currencyRateService.Object, null!);

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

            var cryptoQuoteService = new CryptoQuoteService(currencyRateService.Object, null!);

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


            var cryptoQuoteService = new CryptoQuoteService(currencyRateService.Object, null!);

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

        private static new Func<Task<IEnumerable<CryptoRate>>> Func_ResultForBTC_USD = new Func<Task<IEnumerable<CryptoRate>>>(ResultForBTC_USD);
        private static new Func<Task<IEnumerable<CryptoRate>>> Func_ResultForBTC_EUR = new Func<Task<IEnumerable<CryptoRate>>>(ResultForBTC_EUR);
        private static new Func<Task<IEnumerable<CryptoRate>>> Func_ResultForBTC_BRL = new Func<Task<IEnumerable<CryptoRate>>>(ResultForBTC_BRL);

        private static new Func<Task<IEnumerable<CryptoRate>>> Func_ResultForETH_USD = new Func<Task<IEnumerable<CryptoRate>>>(ResultForETH_USD);
        private static new Func<Task<IEnumerable<CryptoRate>>> Func_ResultForETH_EUR = new Func<Task<IEnumerable<CryptoRate>>>(ResultForETH_EUR);
        private static new Func<Task<IEnumerable<CryptoRate>>> Func_ResultForETH_BRL = new Func<Task<IEnumerable<CryptoRate>>>(ResultForETH_BRL);


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

        private static IEnumerable<object[]> GetAllCryptoRatesWithDetailsTestDataOnBTC()
        {
            yield return new object[] 
            {
                "BTC", 
                new Dictionary<string, Func<Task<IEnumerable<CryptoRate>>>>
                {
                    { "USD",  Func_ResultForBTC_USD},
                    { "EUR",  Func_ResultForBTC_EUR},
                },
                Func_ResultForUSD_EUR
            };
        }

        private static IEnumerable<object[]> GetAllCryptoRatesWithDetailsTestDataOnBTC_ETH()
        {
            yield return new object[]
            {
                "BTC",
                new Dictionary<string, Func<Task<IEnumerable<CryptoRate>>>>
                {
                    { "USD",  Func_ResultForBTC_USD},
                    { "EUR",  Func_ResultForBTC_EUR},
                    { "BRL",  Func_ResultForBTC_BRL},

                },
                Func_ResultForUSD_EUR
            };
            yield return new object[]
            {
                "ETH",
                new Dictionary<string, Func<Task<IEnumerable<CryptoRate>>>>
                {
                    { "USD",  Func_ResultForETH_USD},
                    { "EUR",  Func_ResultForETH_EUR},
                    { "BRL",  Func_ResultForETH_BRL},

                },
                Func_ResultForUSD_EUR
            };
        }

        private static IEnumerable<object[]> GetAllCryptoRatesTestData()
        {
            yield return new object[] { "BTC", "USD", Func_ResultForBTC_USD };
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

        private static async Task<IEnumerable<CryptoRate>> ResultForBTC_USD()
        {
            return new List<CryptoRate>
            {
                new CryptoRate
                {
                    Id = 1,
                    LastUpdated = DateTime.Now,
                    Name = "bitcoin",
                    Price = 69740m,
                    Symbol = "BTC",
                    Unit = "USD"
                },
                new CryptoRate
                {
                    Id = 2,
                    LastUpdated = DateTime.Now,
                    Name = "pitcoin",
                    Price = 0.01m,
                    Symbol = "BTC",
                    Unit = "USD"
                }
            };
        }

        private static async Task<IEnumerable<CryptoRate>> ResultForBTC_EUR()
        {
            return new List<CryptoRate>
            {
                new CryptoRate
                {
                    Id = 1,
                    LastUpdated = DateTime.Now,
                    Name = "bitcoin",
                    Price = 64740m,
                    Symbol = "BTC",
                    Unit = "EUR"
                },
                new CryptoRate
                {
                    Id = 2,
                    LastUpdated = DateTime.Now,
                    Name = "pitcoin",
                    Price = 0.009m,
                    Symbol = "BTC",
                    Unit = "EUR"
                }
            };
        }

        private static async Task<IEnumerable<CryptoRate>> ResultForBTC_BRL()
        {
            return new List<CryptoRate>
            {
                new CryptoRate
                {
                    Id = 1,
                    LastUpdated = DateTime.Now,
                    Name = "bitcoin",
                    Price = 250m,
                    Symbol = "BTC",
                    Unit = "BRL"
                },
                new CryptoRate
                {
                    Id = 2,
                    LastUpdated = DateTime.Now,
                    Name = "pitcoin",
                    Price = 0.01m,
                    Symbol = "BTC",
                    Unit = "BRL"
                }
            };
        }

        private static async Task<IEnumerable<CryptoRate>> ResultForETH_USD()
        {
            return new List<CryptoRate>
            {
                new CryptoRate
                {
                    Id = 1,
                    LastUpdated = DateTime.Now,
                    Name = "etherum",
                    Price = 69740m,
                    Symbol = "ETH",
                    Unit = "USD"
                },
                new CryptoRate
                {
                    Id = 2,
                    LastUpdated = DateTime.Now,
                    Name = "sheterum",
                    Price = 0.01m,
                    Symbol = "ETH",
                    Unit = "USD"
                }
            };
        }

        private static async Task<IEnumerable<CryptoRate>> ResultForETH_EUR()
        {
            return new List<CryptoRate>
            {
                new CryptoRate
                {
                    Id = 1,
                    LastUpdated = DateTime.Now,
                    Name = "etherum",
                    Price = 64740m,
                    Symbol = "ETH",
                    Unit = "EUR"
                },
                new CryptoRate
                {
                    Id = 2,
                    LastUpdated = DateTime.Now,
                    Name = "sheterum",
                    Price = 0.009m,
                    Symbol = "ETH",
                    Unit = "EUR"
                }
            };
        }

        private static async Task<IEnumerable<CryptoRate>> ResultForETH_BRL()
        {
            return new List<CryptoRate>
            {
                new CryptoRate
                {
                    Id = 1,
                    LastUpdated = DateTime.Now,
                    Name = "etherum",
                    Price = 250m,
                    Symbol = "ETH",
                    Unit = "BRL"
                },
                new CryptoRate
                {
                    Id = 2,
                    LastUpdated = DateTime.Now,
                    Name = "sheterum",
                    Price = 0.01m,
                    Symbol = "ETH",
                    Unit = "BRL"
                }
            };
        }
        #endregion
    }
}