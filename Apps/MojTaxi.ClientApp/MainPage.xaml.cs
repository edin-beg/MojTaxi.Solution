using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MojTaxi.ClientApp.ViewModels;

namespace MojTaxi.ClientApp.Pages;
using Microsoft.Maui.Maps;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        SetUserLocation();
    }

    private async void SetUserLocation()
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location != null)
        {
            mapControl.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Location(location.Latitude, location.Longitude),
                Distance.FromKilometers(1)
            ));
        }
    }

    private void OnTabClicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        string param = btn.CommandParameter?.ToString();
        // handle tab click...
    }
}