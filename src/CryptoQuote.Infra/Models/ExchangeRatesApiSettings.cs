namespace CryptoQuote.Infra.Models
{
    public class ExchangeRatesApiSettings
    {
        public string BaseUrl { get; set; } = null!;
        public string Token { get; set; } = null!;
    }

}
