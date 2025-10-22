using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MojTaxi.ApiClient;
using MojTaxi.Core;
using MojTaxi.Core.Abstractions;
using MojTaxi.ClientApp.Services;
using MojTaxi.ClientApp.Pages;
using MojTaxi.ClientApp.ViewModels;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using MojTaxi.ApiClient.Infrastructure;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MojTaxi.ClientApp;

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
            .UseSkiaSharp()
            .ConfigureFonts(f =>
            {
                f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                f.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // === API settings (prilagodi!) ===
        var settings = new ApiSettings
        {
            BaseUrl    = "https://ssl.irget.se", // <-- promijeni ako treba
            Version    = "V5",
            BuildNumber= "185",
            AppVersion = "1.3.1"
        };
        builder.Services.AddSingleton(settings);

        // Storage
        builder.Services.AddSingleton<ISessionStore, SecureStorageSessionStore>();

        // Refit klijenti
        builder.Services.AddRefitClient<IClientsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.BaseUrl))
            .AddPolicyHandler(RetryPolicy());

        // Core servisi
        builder.Services.AddMojTaxiCore();

        // Pages + VM (za DI)
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        return builder.Build();
    }
}