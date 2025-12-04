using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojTaxi.Client.Pages;

public partial class LandingPage : ContentPage
{
    public LandingPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(5000); // 2s splash

        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

        await Shell.Current.GoToAsync("//LoginPage");
    }
}