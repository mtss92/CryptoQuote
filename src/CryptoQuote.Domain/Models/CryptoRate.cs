namespace CryptoQuote.Domain.Models
{
    public class CryptoRate
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
