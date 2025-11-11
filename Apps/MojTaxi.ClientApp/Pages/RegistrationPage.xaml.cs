using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojTaxi.ClientApp.Pages;

using ViewModels;

public partial class RegistrationPage : ContentPage {
    public RegistrationPage()
    {
        InitializeComponent();
        BindingContext = new RegistrationViewModel();
    }
}

