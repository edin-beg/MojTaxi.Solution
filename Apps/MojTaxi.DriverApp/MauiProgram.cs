using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MojTaxi.ApiClient;
using MojTaxi.Core;
using MojTaxi.Core.Abstractions;
using MojTaxi.DriverApp.Services;
using MojTaxi.DriverApp.Pages;
using MojTaxi.DriverApp.ViewModels;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using MojTaxi.ApiClient.Infrastructure;
using Refit;

namespace MojTaxi.DriverApp;

public static class MauiProgram
{
    static IAsyncPolicy<HttpResponseMessage> RetryPolicy() =>
        HttpPolicyExtensions.HandleTransientHttpError()
            .OrResult(m => (int)m.StatusCode == 429)
            .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(200 * (i + 1)));

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(f =>
            {
                f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                f.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var settings = new ApiSettings
        {
            BaseUrl    = "https://ssl.irget.se",
            Version    = "V5",
            BuildNumber= "185",
            AppVersion = "1.3.1"
        };
        builder.Services.AddSingleton(settings);

        builder.Services.AddSingleton<ISessionStore, SecureStorageSessionStore>();

        builder.Services.AddRefitClient<IDriversApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.BaseUrl))
            .AddPolicyHandler(RetryPolicy());

        builder.Services.AddMojTaxiCore();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        return builder.Build();
    }
}