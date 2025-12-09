using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MojTaxi.ClientApp.Services;
using MojTaxi.ClientApp.ViewModels;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;
using System.Diagnostics;

namespace MojTaxi.ClientApp.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private readonly IAppStatusService _status;

    private CancellationTokenSource? _statusAnimationToken;    

    public MainPage(MainViewModel vm, IAppStatusService status)
    {
        InitializeComponent();

        _vm = vm;
        _status = status;
        BindingContext = _vm;

        CheckInitialStatus();

        _status.InternetStatusChanged += OnInternetChanged;
        _status.GpsStatusChanged += OnGpsChanged;

        Loaded += async (_, __) =>
        {
            await _vm.LoadLocationCommand.ExecuteAsync(null);

            if (_vm.CurrentLocation != null)
            {
                mapControl.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        _vm.CurrentLocation,
                        Distance.FromKilometers(2)));
            }
        };
    }


    // ============================================================
    // OnAppearing — centriranje na user lokaciju
    // ============================================================
    protected override async void OnAppearing()
    {
        base.OnAppearing();

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _status.InternetStatusChanged -= OnInternetChanged;
        _status.GpsStatusChanged -= OnGpsChanged;

        StopPulseAnimation();
    }


    // ============================================================
    // STATUS HANDLER
    // ============================================================
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

    // ============================================================
    // STATUS ANIMACIJA (Pulse)
    // ============================================================
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
            StatusContainer.IsVisible = true;
            StatusLine.BackgroundColor = Colors.Red;
            _ = StartPulseAnimation();
        }
        else if (!_status.HasGps)
        {
            StatusText.Text = "GPS je isključen";
            StatusContainer.IsVisible = true;
            StatusLine.BackgroundColor = Colors.Orange;
            _ = StartPulseAnimation();
        }
        else
        {
            StatusContainer.IsVisible = false;
        }
    }

    // ============================================================
    // UI (Hamburger)
    // ============================================================
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
}
