using Microsoft.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using MojTaxi.ClientApp.Pages;
using MojTaxi.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MojTaxi.ClientApp.ViewModels
{
    using Services;

    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _nav;

        [ObservableProperty] private Location currentLocation;

        public MainViewModel(INavigationService nav)
        {
            _nav = nav;
        }

        // ============================================================
        //  LOADING USER LOCATION (iOS-safe)
        // ============================================================
        [RelayCommand]
        private async Task LoadLocation()
        {
            try
            {
                // 1️⃣ Permissions (UI thread – OK)
                var granted = await EnsureLocationPermission();
                if (!granted)
                    return;

                // 2️⃣ Mali grace period za iOS
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                    await Task.Delay(300);

                // 3️⃣ Geolocation VAN UI threada
                var location = await Task.Run(async () =>
                {
                    var last = await Geolocation.GetLastKnownLocationAsync();
                    if (last != null)
                        return last;

                    var request = new GeolocationRequest(
                        GeolocationAccuracy.Medium, // High tek kad treba
                        TimeSpan.FromSeconds(10));

                    return await Geolocation.GetLocationAsync(request);
                });

                // 4️⃣ Update UI state
                if (location != null)
                {
                    CurrentLocation = new Location(
                        location.Latitude,
                        location.Longitude);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Greška u geolokaciji: {ex}");
            }
        }

        private async Task<bool> EnsureLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return true;

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status == PermissionStatus.Granted;
        }

        // ============================================================
        //  COMMANDS
        // ============================================================

        [RelayCommand]
        private Task CallTaxi()
        {
            var page = Application.Current?.Windows[0].Page;
            return page?.DisplayAlertAsync("Taxi", "Poziv taksija kliknut", "OK")
                   ?? Task.CompletedTask;
        }

        [RelayCommand]
        private Task Profile()
            => _nav.GoToAsync(nameof(ProfilePage));

        [RelayCommand]
        private Task TabClicked(string tabName)
        {
            var page = Application.Current?.Windows[0].Page;
            return page?.DisplayAlertAsync("Tab", $"Otvorio si: {tabName}", "OK")
                   ?? Task.CompletedTask;
        }
    }
}
