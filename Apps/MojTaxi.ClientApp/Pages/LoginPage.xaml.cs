using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MojTaxi.ClientApp.ViewModels;

namespace MojTaxi.ClientApp.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is LoginViewModel vm)
        {
            vm.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(vm.OtpSent) && vm.OtpSent)
                {
                    await Task.Delay(200);
                    OtpEntry.Focus();
                }
            };
        }
    }

}