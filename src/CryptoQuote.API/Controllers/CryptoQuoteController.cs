using CryptoQuote.API.Models;
using CryptoQuote.Domain.Models;
using CryptoQuote.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuote.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoQuoteController : ControllerBase
    {
        private readonly ICryptoQuoteService cryptoQuoteService;
        private readonly ApiSettings settings;

        public CryptoQuoteController(ICryptoQuoteService cryptoQuoteService, ApiSettings settings)
        {
            this.cryptoQuoteService = cryptoQuoteService;
            this.settings = settings;
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoRate>> Get(string symbol)
        {
            var rates = await cryptoQuoteService.GetAllCryptoRates(symbol, settings.Currencies);
            return rates;
        }

        [HttpGet("details")]
        public async Task<CryptoRateExchangeDetails> GetWithExchangeDetails(string symbol)
        {
            var rates = await cryptoQuoteService.GetAllCryptoRatesWithDetails(symbol, settings.Currencies);
            return rates;
        }
    }
}