using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MojTaxi.ClientApp.ViewModels;

namespace MojTaxi.ClientApp.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private Pin? userPin;

    // ✅ MainViewModel sada dolazi iz DI
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;

        Loaded += async (_, __) =>
        {
            await _vm.LoadLocationCommand.ExecuteAsync(null);
            UpdateMap();
        };
    }

    private void UpdateMap()
    {
        if (_vm.CurrentLocation == null)
            return;

        var span = MapSpan.FromCenterAndRadius(_vm.CurrentLocation, Distance.FromKilometers(1));
        mapControl.MoveToRegion(span);

        if (userPin == null)
        {
            userPin = new Pin
            {
                Label = "Moja lokacija",
                Address = "Trenutna pozicija",
                Type = PinType.Generic,
                Location = _vm.CurrentLocation
            };
            mapControl.Pins.Add(userPin);
        }
        else
        {
            userPin.Location = _vm.CurrentLocation;
        }
    }

    private async void ToggleMenu(object sender, EventArgs e)
    {
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
}
