namespace CryptoQuote.Infra.HttpServices
{
    public interface IHttpService
    {
        Dictionary<string, string> Headers { get; set; }
        Task<T> GetAsync<T>(string path);
    }
}
