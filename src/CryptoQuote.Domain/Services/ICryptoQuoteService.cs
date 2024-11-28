using CryptoQuote.Domain.Models;

namespace CryptoQuote.Domain.Services
{
    public interface ICryptoQuoteService
    {
        Task<IEnumerable<CryptoRate>> GetAllCryptoRates(string symbol, string[] quoteUnits);
        Task<IEnumerable<ExchangeRate>> GetAllCurrenciesRate(string[] currencies);
    }
}