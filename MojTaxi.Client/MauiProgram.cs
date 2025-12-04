using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.Client.Pages;
using MojTaxi.Client.Services;
using MojTaxi.Client.ViewModels;
using MojTaxi.Core.Abstractions;
using Polly;
using Polly.Extensions.Http;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;



namespace MojTaxi.Client
{
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
                BaseUrl = "https://ssl.irget.se",
                Version = "V5",
                BuildNumber = "185",
                AppVersion = "1.3.1"
            };
            builder.Services.AddSingleton(settings);

            // Storage & Core
            builder.Services.AddSingleton<ISessionStore, SecureStorageSessionStore>();
          //  builder.Services.AddMojTaxiCore();

            //Refit API clients
            builder.Services.AddRefitClient<IClientsApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.BaseUrl))
                .AddPolicyHandler(RetryPolicy());

            //Navigation Service
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<AppShell>();

            //All ViewModels as Transient
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<RideHistoryViewModel>();
            builder.Services.AddTransient<PaymentViewModel>();
            builder.Services.AddTransient<ChangePasswordViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<LegalViewModel>();

            //Pages as Transient
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

            //Register AppShell so DI can inject it if needed
            builder.Services.AddSingleton<AppShell>();

            return builder.Build();
        }

    }
}