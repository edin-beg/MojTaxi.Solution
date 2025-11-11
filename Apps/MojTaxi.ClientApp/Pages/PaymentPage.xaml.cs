using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojTaxi.ClientApp.Pages;

using ViewModels;

public partial class PaymentPage : ContentPage {
    public PaymentPage(PaymentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }


}

