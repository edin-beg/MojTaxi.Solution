using MojTaxi.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojTaxi.ClientApp.Pages;

public partial class LandingPage : ContentPage
{
    private readonly IAuthService _auth;
    private bool _initialized;
    public LandingPage(IAuthService auth)
    {
        _auth = auth;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_initialized)
            return;

        _initialized = true;
        _ = InitAndNavigateAsync();
    }

    private async Task InitAndNavigateAsync()
    {
        try
        {
            var authTask = _auth.TryRestoreAsync();
            var splashDelayTask = Task.Delay(5000);

            await Task.WhenAll(authTask, splashDelayTask);

            var ok = authTask.Result;

            await MainThread.InvokeOnMainThreadAsync(() =>
                Shell.Current.GoToAsync(
                    ok ? "//MainPage" : "//LoginPage",
                    animate: true
                )
            );
        }
        catch
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
                Shell.Current.GoToAsync("//LoginPage", animate: true)
            );
        }
    }

}