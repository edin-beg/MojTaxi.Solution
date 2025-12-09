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

        [ObservableProperty]
        private Location currentLocation;

        public MainViewModel(INavigationService nav)
        {
            _nav = nav;
        }


        // ============================================================
        //  1) LOADING USER LOCATION 
        // ============================================================
        [RelayCommand]
        private async Task LoadLocation()
        {
            try
            {
                var granted = await EnsureLocationPermission();
                if (!granted)
                {
                    Debug.WriteLine("Lokacija nije dozvoljena.");
                    return;
                }

                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    var request = new GeolocationRequest(
                        GeolocationAccuracy.High,
                        TimeSpan.FromSeconds(10));

                    location = await Geolocation.GetLocationAsync(request);
                }

                if (location != null)
                    currentLocation = new Location(location.Latitude, location.Longitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Greška u geolokaciji: {ex.Message}");
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
    }
}
