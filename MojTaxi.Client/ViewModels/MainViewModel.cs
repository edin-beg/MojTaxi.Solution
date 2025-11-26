using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using MojTaxi.Client.Pages;

namespace MojTaxi.Client.ViewModels
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
                await CheckPermissions();

                var location = await Geolocation.GetLastKnownLocationAsync()
                               ?? await Geolocation.GetLocationAsync(new GeolocationRequest(
                                    GeolocationAccuracy.High, 
                                    TimeSpan.FromSeconds(10)));

                if (location != null)
                {
                    CurrentLocation = new Location(location.Latitude, location.Longitude);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Location error: {ex.Message}");
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
    }
}
