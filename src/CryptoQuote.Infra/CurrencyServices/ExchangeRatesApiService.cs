using AutoMapper;
using CryptoQuote.Domain.Contracts;
using CryptoQuote.Domain.Models;
using CryptoQuote.Infra.HttpServices;
using CryptoQuote.Infra.Models;

namespace CryptoQuote.Infra.CurrencyServices
{
    public class ExchangeRatesApiService : ICurrencyRateService
    {
        private readonly IMapper mapper;
        private readonly IHttpService httpService;
        private readonly ExchangeRatesApiSettings apiSettings;

        public ExchangeRatesApiService(
            IHttpService httpService,
            ExchangeRatesApiSettings apiSettings)
        {
            this.httpService = httpService;
            this.apiSettings = apiSettings;

            this.mapper = CreateMaps();
        }

        public async Task<CurrencyRateResponse> GetLatestRates(IEnumerable<string> currencies)
        {
            var symbols = string.Join(",", currencies);

            var builder = new UriBuilder(apiSettings.BaseUrl);
            builder.Path = "/v1/latest";
            builder.Query = $"access_key={apiSettings.Token}&symbols={symbols}";

            var response = await httpService.GetAsync<ExchangeRatesApiResponse>(builder.Uri.ToString());

            var result = mapper.Map<CurrencyRateResponse>(response);

            return result;
        }

        private IMapper CreateMaps()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ExchangeRatesApiResponse, CurrencyRateResponse>()
                .ForMember(cr => cr.Date, er => er.MapFrom(p => DateOnly.Parse(p.Date)))
                .ForMember(cr => cr.BaseCurrency, er => er.MapFrom(p => p.Base))
                .ForMember(cr => cr.CurrenciesRate, er => er.MapFrom(p => p.Rates))
                ;

            }).CreateMapper();
        }
    }
}
