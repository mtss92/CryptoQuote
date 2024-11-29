namespace CryptoQuote.Domain.Models
{
    public class CryptoRateExchangeDetails
    {
        public IEnumerable<CryptoRate> CryptoRates { get; set; }
        public IEnumerable<ExchangeRate> ExchangeRate { get; set; }
    }
}
