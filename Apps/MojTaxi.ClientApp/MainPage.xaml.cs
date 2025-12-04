using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Maps;
using MojTaxi.ClientApp.Services;   
using MojTaxi.ClientApp.ViewModels;
using MojTaxi.Core;
using MojTaxi.Core.Abstractions;               

namespace MojTaxi.ClientApp.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private readonly IAppStatusService _status; 
    private Pin? userPin;

    public MainPage(MainViewModel vm, IAppStatusService status)
    {
        InitializeComponent();

        _vm = vm;
        _status = status;
        BindingContext = _vm;

        CheckInitialStatus();

        //PRETPLATA NA EVENTE
        _status.InternetStatusChanged += OnInternetChanged;
        _status.GpsStatusChanged += OnGpsChanged;

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

            await TabContainer.TranslateToAsync(0, 0, 200, Easing.CubicOut);
            await TabContainer.FadeToAsync(1, 200);
        }
        else
        {
            await TabContainer.FadeToAsync(0, 200);
            TabContainer.IsVisible = false;
        }
    }

    // -------------------------
    // INTERNET HANDLER
    // -------------------------
    private void OnInternetChanged(bool hasInternet)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (!hasInternet)
            {
                StatusText.Text = "Nema internet konekcije";
                StatusContainer.IsVisible = true;
                StatusLine.BackgroundColor = Colors.Red;
                await StartPulseAnimation();
            }
            else
            {
                StatusContainer.IsVisible = false;
                StopPulseAnimation();
            }
        });
    }


    // -------------------------
    // GPS HANDLER
    // -------------------------
    private void OnGpsChanged(bool hasGps)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (!hasGps)
            {
                StatusText.Text = "GPS je isključen";
                StatusContainer.IsVisible = true;
                StatusLine.BackgroundColor = Colors.Orange;
                await StartPulseAnimation();
            }
            else
            {
                StatusContainer.IsVisible = false;
                StopPulseAnimation();
            }
        });
    }

    /*   private void UpdateStatusLine()
       {
           // Crvena = nema internet
           // Narandžasta = nema GPS

           if (!_status.HasInternet)
           {
               StatusLine.IsVisible = true;
               StatusLine.BackgroundColor = Colors.Red;
           }
           else if (!_status.HasGps)
           {
               StatusLine.IsVisible = true;
               StatusLine.BackgroundColor = Colors.Orange;
           }
           else
           {
               StatusLine.IsVisible = false;
           }

           // Reset pozicije za animaciju
           StatusLine.TranslationX = -400;
       }
    */
    private CancellationTokenSource? _statusAnimationToken;

    private async Task StartPulseAnimation()
    {
        _statusAnimationToken?.Cancel();
        _statusAnimationToken = new CancellationTokenSource();

        var token = _statusAnimationToken.Token;

        while (!token.IsCancellationRequested)
        {
            await StatusLine.FadeToAsync(0.2, 800, Easing.CubicInOut);
            await StatusLine.FadeToAsync(1.0, 800, Easing.CubicInOut);
        }
    }

    private void StopPulseAnimation()
    {
        _statusAnimationToken?.Cancel();
        StatusLine.Opacity = 1;
    }

    private void CheckInitialStatus()
    {
        if (!_status.HasInternet)
        {
            StatusText.Text = "Nema internet konekcije";
            StatusText.TextColor = Colors.Red;
            StatusContainer.IsVisible = true;
            StatusLine.BackgroundColor = Colors.Red;
            _ = StartPulseAnimation();
        }
        else if (!_status.HasGps)
        {
            StatusText.Text = "GPS je isključen";
            StatusText.TextColor = Colors.Orange;
            StatusContainer.IsVisible = true;
            StatusLine.BackgroundColor = Colors.Orange;
            _ = StartPulseAnimation();
        }
        else
        {
            StatusContainer.IsVisible = false;
        }
    }
}
