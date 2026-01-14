using CommunityToolkit.Maui;
using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.ClientApp.Pages;
using MojTaxi.ClientApp.Services;
using MojTaxi.ClientApp.Services.Auth;
using MojTaxi.ClientApp.ViewModels;
using MojTaxi.Core;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Implementations;
using MojTaxi.Core.Services;
using MojTaxi.Notifications;
using MojTaxi.Shared.Services;
using Polly;
using Polly.Extensions.Http;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Net.Http;

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
            .UseMauiCommunityToolkit()
            .UseMauiMaps()
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
            BaseUrl    = "https://staging.mojtaxi.ba",
            Version    = "V5",
            BuildNumber= "185",
            AppVersion = "1.3.1"
        };
        builder.Services.AddSingleton(settings);

        // Storage & Core
        builder.Services.AddSingleton<BusyService>();

        builder.Services.AddSingleton<ILocalNotificationService, LocalNotificationService>();
        builder.Services.AddSingleton<IGpsService, GpsService>();
        builder.Services.AddSingleton<IAppStatusService, AppStatusService>();

        builder.Services.AddSingleton<ILocalErrorStore, LocalErrorStore>();
        builder.Services.AddSingleton<IRemoteErrorSender, RemoteErrorSender>();
        builder.Services.AddSingleton<IErrorLogger, ErrorLogger>();

        builder.Services.AddSingleton<IClientSession, ClientSession>();

        builder.Services.AddSingleton<ISessionStore, SecureSessionStore>();
        builder.Services.AddSingleton<ICredentialStore, SecureCredentialStore>();

        builder.Services.AddSingleton<IAuthService, AuthService>();

        builder.Services.AddHttpClient<InfobipSmsSender>();
        builder.Services.AddSingleton<EmailOtpSender>();
        builder.Services.AddSingleton<IOtpSenderService, OtpSenderService>();


        builder.Services.AddHttpClient<IRemoteErrorSender, RemoteErrorSender>(client =>
        {
            client.BaseAddress = new Uri("https://tvoj-api.com");
        });

        // Background sync
        builder.Services.AddSingleton<ErrorSyncBackgroundService>();

        // Refit API clients
        builder.Services.AddRefitClient<IClientsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.BaseUrl+"/API"))
            .AddPolicyHandler(RetryPolicy());

        // Navigation Service
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<AppShell>();

        // All ViewModels as Transient
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegistrationViewModel>();
        builder.Services.AddTransient<RideHistoryViewModel>();
        builder.Services.AddTransient<PaymentViewModel>();
        builder.Services.AddTransient<ChangePasswordViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<LegalViewModel>();

        // Pages as Transient
        builder.Services.AddTransient<LandingPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<RideHistoryPage>();
        builder.Services.AddTransient<PaymentPage>();
        builder.Services.AddTransient<ChangePasswordPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<LegalPage>();

        // Register AppShell so DI can inject it if needed
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}
