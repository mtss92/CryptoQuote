namespace CryptoQuote.Infra.Models
{
    public class CoinMarketCapApiSettings
    {
        public string BaseUrl { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
