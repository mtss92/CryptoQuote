using CryptoQuote.API.Middlewares;
using CryptoQuote.API.Models;
using CryptoQuote.Domain.Contracts;
using CryptoQuote.Domain.Services;
using CryptoQuote.Infra.CurrencyServices;
using CryptoQuote.Infra.HttpServices;
using CryptoQuote.Infra.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var apiSettings = new ApiSettings();
config.GetSection("ApiSettings").Bind(apiSettings);

var exchangeRatesApiSettings = new ExchangeRatesApiSettings();
config.GetSection("ExchangeRatesApi").Bind(exchangeRatesApiSettings);

var coinMarketCapApiSettings = new CoinMarketCapApiSettings();
config.GetSection("CoinMarketCapApi").Bind(coinMarketCapApiSettings);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ApiSettings>(apiSettings);
builder.Services.AddSingleton<ExchangeRatesApiSettings>(exchangeRatesApiSettings);
builder.Services.AddSingleton<CoinMarketCapApiSettings>(coinMarketCapApiSettings);

builder.Services.AddScoped<ICryptoQuoteService, CryptoQuoteService>();
builder.Services.AddScoped<ICryptoMarketService, CoinMarketCapApiService>();
builder.Services.AddScoped<ICurrencyRateService, ExchangeRatesApiService>();
builder.Services.AddScoped<IHttpService, HttpClientService>();
builder.Services.AddHttpClient();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(builder.Configuration["Seq:Url"])
                .CreateLogger();
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",

        builder =>
        {
            builder.AllowAnyOrigin()
                   .WithMethods("GET");
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.UseCors("AllowAnyOrigin");


app.Run();

