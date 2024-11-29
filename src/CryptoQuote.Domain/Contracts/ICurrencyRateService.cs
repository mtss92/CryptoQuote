using CryptoQuote.Domain.Models;

namespace CryptoQuote.Domain.Contracts
{
    public interface ICurrencyRateService
    {
        Task<CurrencyRateResponse> GetLatestRates(IEnumerable<string> currencies);
    }
}
