using MojTaxi.Client.ViewModels;

namespace MojTaxi.Client.Pages;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage(RegistrationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
