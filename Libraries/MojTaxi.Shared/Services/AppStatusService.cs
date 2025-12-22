using MojTaxi.Core.Abstractions;
using System;

namespace MojTaxi.Shared.Services
{
    public class AppStatusService : IAppStatusService
    {
        private readonly IGpsService _gpsService;
        private readonly ILocalNotificationService _notificationService;

        public bool HasInternet { get; private set; }
        public bool HasGps { get; private set; }

        public event Action<bool>? InternetStatusChanged;
        public event Action<bool>? GpsStatusChanged;

        public AppStatusService(
            IGpsService gpsService,
            ILocalNotificationService notificationService)
        {
            _gpsService = gpsService;
            _notificationService = notificationService;

            // početni internet status
            HasInternet = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }

        /// <summary>
        /// Pokreće monitoring GPS-a i Internet konekcije.
        /// Poziva se jednom u App.xaml.cs pri pokretanju aplikacije.
        /// </summary>
        public void StartMonitoring()
        {
            // Internet promjene
            Connectivity.ConnectivityChanged += HandleInternetChanged;

            // GPS timer (MAUI 10 proper)
            var dispatcher = Application.Current?.Dispatcher;

            if (dispatcher != null)
            {
                dispatcher.StartTimer(
                    TimeSpan.FromSeconds(10),
                    () =>
                    {
                        _ = CheckGpsAsync();
                        return true; // ponavljaj timer
                    });
            }
        }

        private void HandleInternetChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            bool newStatus = e.NetworkAccess == NetworkAccess.Internet;

            if (HasInternet != newStatus)
            {
                HasInternet = newStatus;
                InternetStatusChanged?.Invoke(HasInternet);

                if (!HasInternet)
                {
                    _notificationService.Show(
                        "Nema internet konekcije",
                        "Provjerite Wi-Fi ili mobilne podatke.");
                }
            }
        }

        private async Task CheckGpsAsync()
        {
            var gpsEnabled = await _gpsService.IsGpsEnabledAsync();

            if (gpsEnabled != HasGps)
            {
                HasGps = gpsEnabled;
                GpsStatusChanged?.Invoke(HasGps);

                if (!HasGps)
                {
                    _notificationService.Show(
                        "GPS je isključen",
                        "Molimo uključite lokaciju da bi aplikacija radila ispravno.");
                }
            }
        }
    }
}
