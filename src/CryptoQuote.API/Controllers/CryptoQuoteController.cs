using CryptoQuote.API.Models;
using CryptoQuote.Domain.Models;
using CryptoQuote.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuote.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ICryptoQuoteService cryptoQuoteService;
        private readonly ApiSettings settings;

        public ExchangeRateController(ICryptoQuoteService cryptoQuoteService, ApiSettings settings)
        {
            this.cryptoQuoteService = cryptoQuoteService;
            this.settings = settings;
        }

        [HttpGet]
        public async Task<IEnumerable<ExchangeRate>> Get()
        {
            var rates = await cryptoQuoteService.GetAllCurrenciesRate(settings.Currencies);
            return rates;
        }
    }
}