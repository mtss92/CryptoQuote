using CryptoQuote.Domain.Contracts;
using CryptoQuote.Domain.Models;
using CryptoQuote.Infra.HttpServices;
using CryptoQuote.Infra.Models;
using Newtonsoft.Json.Linq;

namespace CryptoQuote.Infra.CurrencyServices
{
    public class CoinMarketCapApiService : ICryptoMarketService
    {
        private readonly IHttpService httpService;
        private readonly CoinMarketCapApiSettings apiSettings;

        public CoinMarketCapApiService(
            IHttpService httpService,
            CoinMarketCapApiSettings apiSettings)
        {
            this.httpService = httpService;
            this.apiSettings = apiSettings;
        }

        public async Task<IEnumerable<CryptoRate>> GetCryptoRate(string symbol, string quoteUnit = null!)
        {
            var query = $"symbol={symbol}" + (quoteUnit != null ? $"&convert={quoteUnit}" : "");

            var builder = new UriBuilder(apiSettings.BaseUrl);
            builder.Path = "/v2/cryptocurrency/quotes/latest";
            builder.Query = query;

            httpService.Headers = new Dictionary<string, string>
            {
                { "Accepts", "application/json"},
                { "X-CMC_PRO_API_KEY", apiSettings.Token}
            };

            var response = await httpService.GetAsync<CoinMarketCapApiResponse>(builder.Uri.ToString());

            if (!response.Data.ContainsKey(symbol.ToUpper()) || response.Data[symbol.ToUpper()] == null)
                throw new Exception($"{symbol} not found in CoinMarketCap response");

            var foundSymbols = (JArray)response.Data[symbol.ToUpper()]!;

            var result = new List<CryptoRate>(foundSymbols.Count);

            foreach (var foundSymbol in foundSymbols)
            {
                if (foundSymbol == null)
                    continue;

                try
                {
                    var cryptoRate = new CryptoRate();
                    cryptoRate.Id = foundSymbol["id"]!.ToObject<int>();
                    cryptoRate.LastUpdated = foundSymbol["last_updated"]!.ToObject<DateTime>();
                    cryptoRate.Name = foundSymbol["name"]!.ToString();
                    cryptoRate.Symbol = foundSymbol["symbol"]!.ToString();
                    cryptoRate.Unit = JObject.Parse(foundSymbol["quote"]!.ToString()).Properties().First().Name;
                    cryptoRate.Price = foundSymbol["quote"]![cryptoRate.Unit]!["price"]!.ToObject<decimal>();
                    result.Add(cryptoRate);
                }
                catch (Exception ex)
                {
                    throw new Exception("Structure of CoinMarketCap response changed and this service could not parse it", ex);
                }
            }

            return result;
        }
    }
}
