namespace CryptoQuote.Infra.HttpServices
{
    public class HttpClientService : IHttpService
    {
        private readonly HttpClient httpClient;

        public Dictionary<string, string> Headers { get; set; } = null!;

        public HttpClientService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));

                httpClient.DefaultRequestHeaders.Clear();

                if (Headers != null)
                {
                    foreach (var item in Headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                using var response = await httpClient.GetAsync(path);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);

                if (result == null)
                    throw new Exception($"Result of api could not deserialize to {typeof(T).FullName}");

                return result;
            }
            catch (Exception ex)
            {
                // TODO: Log Exceptions
                throw;
            }

        }
    }
}
