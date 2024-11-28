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
        private readonly Serilog.ILogger logger;
        private readonly ICryptoQuoteService cryptoQuoteService;
        private readonly ApiSettings settings;

        public ExchangeRateController(
            Serilog.ILogger logger,
            ICryptoQuoteService cryptoQuoteService,
            ApiSettings settings)
        {
            this.logger = logger;
            this.cryptoQuoteService = cryptoQuoteService;
            this.settings = settings;
        }

        [HttpGet]
        public async Task<IEnumerable<ExchangeRate>> Get()
        {
            logger.Information("Getting Exchange Rates ...");

            var rates = await cryptoQuoteService.GetAllCurrenciesRate(settings.Currencies);

            logger.Information("Exchange Rates are {@exchangeRates}", rates);

            return rates;
        }
    }
}