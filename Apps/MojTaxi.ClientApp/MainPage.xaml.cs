using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MojTaxi.ClientApp.ViewModels;
using Microsoft.Maui.Controls.Maps;   // Map + Pin
using Microsoft.Maui.Maps;            // MapSpan + Distance + Location
using Microsoft.Maui.Devices.Sensors; // Geolocation
using System.Diagnostics;


namespace MojTaxi.ClientApp.Pages;

public partial class MainPage : ContentPage
{
    Pin userPin;

    public MainPage()
    {
        InitializeComponent();
        _ = InitializeUserLocationAsync();
    }

    private async Task InitializeUserLocationAsync()
    {
        await CheckPermissionsAsync();
        await SetUserLocationAsync();
    }

    private async Task CheckPermissionsAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }
    }

    private async Task SetUserLocationAsync()
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync()
                           ?? await Geolocation.GetLocationAsync();

            if (location == null)
                return;

            var mapSpan = MapSpan.FromCenterAndRadius(
            new Location(location.Latitude, location.Longitude),
            Distance.FromKilometers(1)
            );

            mapControl.MoveToRegion(mapSpan);

            // CREATE OR UPDATE PIN
            if (userPin == null)
            {
                userPin = new Pin
                {
                    Label = "Moja lokacija",
                    Address = "Trenutna pozicija",
                    Type = PinType.Generic,
                    Location = new Location(location.Latitude, location.Longitude)
                };

               mapControl.Pins.Add(userPin);
            }
            else
            {
                userPin.Location = new Location(location.Latitude, location.Longitude);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Location error: {ex.Message}");
        }
    }
    
    private async void ToggleMenu(object sender, EventArgs e)
    {
        // Opciona animacija
        if (!TabContainer.IsVisible)
        {
            TabContainer.TranslationY = 20;
            TabContainer.Opacity = 0;
            TabContainer.IsVisible = true;

            await TabContainer.TranslateTo(0, 0, 200, Easing.CubicOut);
            await TabContainer.FadeTo(1, 200);
        }
        else
        {
            await TabContainer.FadeTo(0, 200);
            TabContainer.IsVisible = false;
        }
    }
    

    private async void OnTabClicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        await DisplayAlert("Tab klik", $"Otvorio si: {btn.Text}", "OK");
    }
}