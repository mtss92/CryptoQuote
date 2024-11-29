namespace CryptoQuote.Domain.Models
{
    public class CurrencyRateResponse
    {
        public string BaseCurrency { get; set; } = null!;
        public DateOnly Date { get; set; }
        public Dictionary<string, decimal> CurrenciesRate { get; set; } = null!;

        internal IEnumerable<ExchangeRate> GetExchangeRates()
        {
            var rates = CurrenciesRate
                .Where(x => x.Key != BaseCurrency)
                .Select(x => new ExchangeRate(BaseCurrency, x.Key) { Rate = x.Value });

            return rates;
        }
    }

}
