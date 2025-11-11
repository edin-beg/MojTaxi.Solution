using MojTaxi.ClientApp.ViewModels;

namespace MojTaxi.ClientApp.Pages;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage(RegistrationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
