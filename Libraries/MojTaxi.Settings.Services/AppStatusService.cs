using MojTaxi.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Settings.Services
{


    public class AppStatusService : IAppStatusService
    {
        private readonly GpsService _gpsService;
        private readonly ILocalNotificationService _notificationService;

        public bool HasInternet { get; private set; }
        public bool HasGps { get; private set; }

        public event Action<bool>? InternetStatusChanged;
        public event Action<bool>? GpsStatusChanged;

        public AppStatusService(
            GpsService gpsService,
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
            // sluša internet promjene
            Connectivity.ConnectivityChanged += HandleInternetChanged;

            // periodična provjera GPS-a
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                _ = CheckGpsAsync();
                return true; // ponavljaj timer
            });
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
