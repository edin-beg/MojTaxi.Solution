using MojTaxi.Client.Pages;

namespace MojTaxi.Client
{
    public partial class AppShell : Shell
    {
        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Registracija svih Shell ruta
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
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
            await Shell.Current.GoToAsync(route);
        }
    }
}
