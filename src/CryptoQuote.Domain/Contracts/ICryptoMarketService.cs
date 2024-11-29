using CryptoQuote.Domain.Models;

namespace CryptoQuote.Domain.Contracts
{
    public interface ICryptoMarketService
    {
        Task<IEnumerable<CryptoRate>> GetCryptoRate(string symbol, string quoteUnit = null!);
    }
}
