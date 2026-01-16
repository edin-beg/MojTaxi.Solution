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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_initialized)
            return;

        _initialized = true;

        try
        {
            if (await _auth.TryRestoreAsync())
                await Shell.Current.GoToAsync("//MainPage", animate: true);
            else
                await Shell.Current.GoToAsync("//LoginPage", animate: true);
        }
        catch (Exception ex)
        {
            // fallback
            await Shell.Current.GoToAsync("//LoginPage", animate: true);
        }
        // await Task.Delay(8000); 
        // await Shell.Current.GoToAsync("//LoginPage");
    }
}