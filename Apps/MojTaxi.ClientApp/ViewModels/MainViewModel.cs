using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using MojTaxi.ClientApp.Pages;

namespace MojTaxi.ClientApp.ViewModels
{
    using Services;

    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _nav;

        [ObservableProperty]
        private Location? currentLocation;

        public MainViewModel(INavigationService nav)
        {
            _nav = nav;
        }

        [RelayCommand]
        private async Task LoadLocation()
        {
            try
            {
                // 1) provjera i zahtjev za lokacijom
                var granted = await EnsureLocationPermission();
                if (!granted)
                {
                    Debug.WriteLine("Lokacija nije dozvoljena.");
                    return;
                }

                // 2) probaj prvo last-known
                var location = await Geolocation.GetLastKnownLocationAsync();

                // 3) ako nema — idi na live GPS
                if (location == null)
                {
                    var request = new GeolocationRequest(
                        GeolocationAccuracy.High,
                        TimeSpan.FromSeconds(10));

                    location = await Geolocation.GetLocationAsync(request);
                }

                // 4) postavi ako postoji
                if (location != null)
                {
                    CurrentLocation = new Location(location.Latitude, location.Longitude);
                }
                else
                {
                    Debug.WriteLine("GPS nije vratio lokaciju.");
                }
            }
            catch (FeatureNotEnabledException)
            {
                Debug.WriteLine("GPS isključen na uređaju.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Greška u geolokaciji: {ex.Message}");
            }
        }


        [RelayCommand]
        private Task CallTaxi()
        {
            var page = Application.Current?.Windows[0].Page;
            return page?.DisplayAlertAsync("Taxi", "Poziv taksija kliknut", "OK")
                   ?? Task.CompletedTask;
        }

        [RelayCommand]
        private Task Profile()
        {
            return _nav.GoToAsync(nameof(ProfilePage));
        }

        [RelayCommand]
        private Task TabClicked(string tabName)
        {
            var page = Application.Current?.Windows[0].Page;
            return page?.DisplayAlertAsync("Tab", $"Otvorio si: {tabName}", "OK")
                   ?? Task.CompletedTask;
        }

        private async Task CheckPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
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

    }
}
