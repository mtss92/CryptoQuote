using CryptoQuote.Domain.Models;

namespace CryptoQuote.Domain.Services
{
    public interface ICryptoQuoteService
    {
        Task<IEnumerable<ExchangeRate>> GetAllCurrenciesRate(string[] currencies);
    }
}