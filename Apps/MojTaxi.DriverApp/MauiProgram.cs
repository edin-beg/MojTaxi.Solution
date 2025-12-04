using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.Core;
using MojTaxi.Core.Abstractions;
using MojTaxi.DriverApp.Pages;
using MojTaxi.DriverApp.Services;
using MojTaxi.DriverApp.ViewModels;
using MojTaxi.Notifications;
using MojTaxi.Settings.Services;
using Polly;
using Polly.Extensions.Http;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Net.Http;

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
            .UseSkiaSharp()
            .ConfigureFonts(f =>
            {
                f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                f.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                f.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });

        // === API settings ===
        var settings = new ApiSettings
        {
            BaseUrl = "https://ssl.irget.se",
            Version = "V5",
            BuildNumber = "185",
            AppVersion = "1.3.1"
        };
        builder.Services.AddSingleton(settings);

        // Storage & Core
        builder.Services.AddSingleton<ISessionStore, SecureStorageSessionStore>();
        builder.Services.AddMojTaxiCore();
        builder.Services.AddSingleton<ILocalNotificationService, LocalNotificationService>();
        builder.Services.AddSingleton<IGpsService, GpsService>();
        builder.Services.AddSingleton<IAppStatusService, AppStatusService>();

        // Refit API clients
        builder.Services.AddRefitClient<IClientsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.BaseUrl))
            .AddPolicyHandler(RetryPolicy());

        // Navigation Service
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<AppShell>();

        // All ViewModels as Transient
        builder.Services.AddTransient<LoginViewModel>();

        // Pages as Transient
        builder.Services.AddTransient<LandingPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();

        // Register AppShell so DI can inject it if needed
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}