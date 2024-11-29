using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Domain.Models
{
    public class CryptoRateExchangeDetails
    {
        public IEnumerable<CryptoRate> CryptoRates { get; set; }
        public IEnumerable<ExchangeRate> ExchangeRate { get; set; }
    }
}
