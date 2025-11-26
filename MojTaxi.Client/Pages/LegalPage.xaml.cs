using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojTaxi.Client.Pages;

using ViewModels;

public partial class LegalPage : ContentPage {
    public LegalPage(LegalViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

}

