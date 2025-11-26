using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojTaxi.Client.Pages;

using ViewModels;

public partial class RideHistoryPage : ContentPage {
    public RideHistoryPage()
    {
        InitializeComponent();
        BindingContext = new RideHistoryViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as RideHistoryViewModel)?.LoadRidesCommand.Execute(null);
    }

}

