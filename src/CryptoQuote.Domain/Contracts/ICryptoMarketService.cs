using CryptoQuote.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Domain.Contracts
{
    public interface ICryptoMarketService
    {
        Task<IEnumerable<CryptoRate>> GetCryptoRate(string symbol, string quoteUnit = null!);
    }
}
