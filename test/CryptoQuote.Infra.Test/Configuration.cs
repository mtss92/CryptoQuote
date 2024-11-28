using CryptoQuote.Infra.Models;
using Microsoft.Extensions.Configuration;

namespace CryptoQuote.Infra.Test
{
    internal static class Configuration
    {
        public static IConfigurationRoot InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
               .AddEnvironmentVariables()
               .Build();

            return config;
        }

        public static ExchangeRatesApiSettings GetExchangeRatesApiSettings(this IConfigurationRoot config)
        {
            var settingsRoot = new ExchangeRatesApiSettings();
            
            config.GetSection("ExchangeRatesApi").Bind(settingsRoot);

            return settingsRoot;
        }

        public static CoinMarketCapApiSettings GetCoinMarketCapApiSettings(this IConfigurationRoot config)
        {
            var settingsRoot = new CoinMarketCapApiSettings();

            config.GetSection("CoinMarketCapApi").Bind(settingsRoot);

            return settingsRoot;
        }
    }
}
