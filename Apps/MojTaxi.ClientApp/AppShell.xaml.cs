namespace MojTaxi.ClientApp;
using Microsoft.Maui;
using MojTaxi.Core.Abstractions;
using Pages;

public partial class AppShell : Shell
{
    private readonly IAuthService _auth;
    private bool _initialized;
    public AppShell(IAuthService auth)
    {
        _auth = auth;
        InitializeComponent();

        // Registracija svih Shell ruta
        //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(RideHistoryPage), typeof(RideHistoryPage));
        Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
        Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(LegalPage), typeof(LegalPage));
        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
    }

    public async Task GoToPageAsync<T>() where T : Page
    {
        var route = typeof(T).Name;
        await Shell.Current.GoToAsync(route, animate: true);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_initialized)
            return;

        _initialized = true;

        try
        {
            if (await _auth.TryRestoreAsync())
                await GoToAsync("//MainPage", animate:true);
            else
                await GoToAsync("//LoginPage", animate: true);
        }
        catch (Exception ex)
        {
            // fallback
            await GoToAsync("//LoginPage", animate: true);
        }
    }
}
