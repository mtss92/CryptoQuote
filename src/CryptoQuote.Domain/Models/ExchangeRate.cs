namespace CryptoQuote.Domain.Models
{
    public class ExchangeRate
    {
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public decimal Rate { get; set; } = -1.0m;

        public ExchangeRate(string baseCurrency, string quoteCurrency)
        {
            if (baseCurrency == quoteCurrency)
                throw new ArgumentException("currencies could not be same for ExchangeRate creation");

            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
        }

        public decimal GetReverseRate()
        {
            if(Rate == 0) return 0;

            return Math.Round(1 / Rate, 2);
        }

        public static IEnumerable<ExchangeRate> GenerateForCurrencies(string[] currencies)
        {
            var pair = new List<ExchangeRate>();
            for (int i = 0; i < currencies.Length; i++)
            {
                for (int j = 0; j < currencies.Length; j++)
                {
                    if (i == j)
                        continue;

                    pair.Add(new ExchangeRate(currencies[i], currencies[j]));
                }
            }
            return pair;
        }

        public override string ToString()
        {
            return $"{BaseCurrency}/{QuoteCurrency} : {Rate}";
        }
    }

}
