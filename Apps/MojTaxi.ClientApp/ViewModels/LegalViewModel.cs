using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MojTaxi.ClientApp.ViewModels;

public partial class LegalViewModel : ObservableObject
{
    [ObservableProperty] private string supportEmail = "support@mojtaxi.ba";

    [RelayCommand]
    private async Task OpenPrivacyPolicy()
        => await Browser.OpenAsync("https://mojtaxi.ba/privacy");

    [RelayCommand]
    private async Task ContactSupport()
        => await Launcher.OpenAsync($"mailto:{SupportEmail}");
}
