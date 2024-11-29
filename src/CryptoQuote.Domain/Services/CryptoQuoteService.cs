using CryptoQuote.Domain.Business;
using CryptoQuote.Domain.Contracts;
using CryptoQuote.Domain.Models;

namespace CryptoQuote.Domain.Services
{
    public class CryptoQuoteService : ICryptoQuoteService
    {
        private readonly ICurrencyRateService currencyRateService;
        private readonly ICryptoMarketService cryptoMarketService;

        public CryptoQuoteService(ICurrencyRateService currencyRateService, ICryptoMarketService cryptoMarketService)
        {
            this.currencyRateService = currencyRateService;
            this.cryptoMarketService = cryptoMarketService;
        }

        public async Task<CryptoRateExchangeDetails> GetAllCryptoRatesWithDetails(string symbol, string[] quoteUnits)
        {
            var result = new CryptoRateExchangeDetails
            {
                CryptoRates = await GetAllCryptoRates(symbol, quoteUnits),
                ExchangeRate = await GetCurrenciesRate(quoteUnits)
            };

            return result;
        }

        public async Task<IEnumerable<CryptoRate>> GetAllCryptoRates(string symbol, string[] quoteUnits)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentNullException(nameof(symbol));

            if (quoteUnits == null || quoteUnits.Length == 0)
            {
                return await cryptoMarketService.GetCryptoRate(symbol);
            }

            var allCryptoRates = new List<CryptoRate>();
            var tasks = new List<Task<IEnumerable<CryptoRate>>>();

            foreach (var quoteUnit in quoteUnits)
            {
                tasks.Add(cryptoMarketService.GetCryptoRate(symbol, quoteUnit));
            }

            var results = await Task.WhenAll(tasks);

            foreach (var cryptoRates in results)
            {
                allCryptoRates.AddRange(cryptoRates);
            }

            return allCryptoRates;
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrenciesRate(string[] currencies)
        {
            if (currencies == null || currencies.Length == 0)
                return null!;

            var currencyRate = await currencyRateService.GetLatestRates(currencies);
            if (!currencyRate.CurrenciesRate.Keys.All(x => currencies.Contains(x)))
                throw new Exception("Some currencies are not in result of CurrencyRateService");

            return currencyRate.GetExchangeRates();
        }

        public async Task<IEnumerable<ExchangeRate>> GetAllCurrenciesRate(string[] currencies)
        {
            if (currencies == null || currencies.Length == 0)
                return Enumerable.Empty<ExchangeRate>();

            var currencyRate = await currencyRateService.GetLatestRates(currencies);
            if (!currencyRate.CurrenciesRate.Keys.All(x => currencies.Contains(x)))
                throw new Exception("Some currencies are not in result of CurrencyRateService");

            var exchangeRateGenerator = new ExchangeRateCalculator();

            var currenciesPair = exchangeRateGenerator.CalculateAllRatesPerRootCurrencyRates(currencyRate, currencies);

            return currenciesPair;
        }
    }
}
