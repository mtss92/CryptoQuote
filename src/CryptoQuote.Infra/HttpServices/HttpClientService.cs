using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Infra.HttpServices
{
    public class HttpClientService : IHttpService
    {
        private readonly HttpClient httpClient;

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

                using var response = await httpClient.GetAsync(path);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);

                if(result == null)
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
