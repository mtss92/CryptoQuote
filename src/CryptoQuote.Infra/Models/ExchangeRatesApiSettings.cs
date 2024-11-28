using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Infra.Models
{
    public class ExchangeRatesApiSettings
    {
        public string BaseUrl { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
