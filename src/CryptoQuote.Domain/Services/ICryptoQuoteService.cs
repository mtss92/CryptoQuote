using CryptoQuote.Domain.Models;

namespace CryptoQuote.Domain.Services
{
    public interface ICryptoQuoteService
    {
        Task<CryptoRateExchangeDetails> GetAllCryptoRatesWithDetails(string symbol, string[] quoteUnits);
        Task<IEnumerable<CryptoRate>> GetAllCryptoRates(string symbol, string[] quoteUnits);
        Task<IEnumerable<ExchangeRate>> GetCurrenciesRate(string[] currencies);
        Task<IEnumerable<ExchangeRate>> GetAllCurrenciesRate(string[] currencies);
    }
}