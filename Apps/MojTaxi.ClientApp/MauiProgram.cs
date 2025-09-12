using Microsoft.Extensions.Logging;
using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace MojTaxi.ClientApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        var settings = new ApiSettings { BaseUrl = "https://ssl.irget.se", Version = "V5", BuildNumber = "185", AppVersion = "1.3.1" };
        builder.Services.AddSingleton(settings);

        static IAsyncPolicy<HttpResponseMessage> RetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => (int)msg.StatusCode == 429)
                .WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200 * (retry + 1)));

        builder.Services
            .AddRefitClient<IClientsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{settings.BaseUrl}"))
            .AddPolicyHandler(RetryPolicy());

        builder.Services
            .AddRefitClient<IDriversApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{settings.BaseUrl}"))
            .AddPolicyHandler(RetryPolicy());

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}