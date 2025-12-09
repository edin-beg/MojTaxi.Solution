using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.Core;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Implementations;
using MojTaxi.Notifications;
using MojTaxi.Settings.Services;
using MojTaxi.StaticClientApp.Pages;
using MojTaxi.StaticClientApp.Services;
using MojTaxi.StaticClientApp.ViewModels;
using Polly;
using Polly.Extensions.Http;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Net.Http;

namespace MojTaxi.StaticClientApp;

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
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
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

        builder.Services.AddSingleton<ILocalErrorStore, LocalErrorStore>();
        builder.Services.AddSingleton<IRemoteErrorSender, RemoteErrorSender>();
        builder.Services.AddSingleton<IErrorLogger, ErrorLogger>();

        builder.Services.AddHttpClient<IRemoteErrorSender, RemoteErrorSender>(client =>
        {
            client.BaseAddress = new Uri("https://tvoj-api.com");
        });

        // Background sync
        builder.Services.AddSingleton<ErrorSyncBackgroundService>();

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