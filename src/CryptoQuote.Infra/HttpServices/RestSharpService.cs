namespace CryptoQuote.Infra.HttpServices
{
    public class RestSharpService : IHttpService
    {
        public Dictionary<string, string> Headers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<T> GetAsync<T>(string path)
        {
            throw new NotImplementedException();
        }
    }
}
