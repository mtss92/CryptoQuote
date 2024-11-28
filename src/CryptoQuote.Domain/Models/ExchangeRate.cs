namespace CryptoQuote.Domain.Models
{
    public class ExchangeRate
    {
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public decimal Rate { get; set; } = -1.0m;

        public ExchangeRate(string baseCurrency, string quoteCurrency)
        {
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
        }

        public decimal GetReverseRate()
        {
            return Math.Round(1 / Rate, 2);
        }

        public override string ToString()
        {
            return $"{BaseCurrency}/{QuoteCurrency} : {Rate}";
        }
    }

}
