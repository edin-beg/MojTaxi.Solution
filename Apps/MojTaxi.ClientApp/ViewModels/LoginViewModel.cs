using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.Core.Abstractions;

namespace MojTaxi.ClientApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IClientAuthService _auth;
    private readonly MojTaxi.ApiClient.ApiSettings _settings;

    [ObservableProperty] string email = "";
    [ObservableProperty] string password = "";
    [ObservableProperty] bool isBusy;
    [ObservableProperty] string? error;

    public LoginViewModel(IClientAuthService auth, MojTaxi.ApiClient.ApiSettings settings)
    {
        _auth = auth;
        _settings = settings;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            IsBusy = true;
            Error = null;

            var deviceId = DeviceInfo.Current.Idiom.ToString() + "-" + Guid.NewGuid().ToString("N").Substring(0, 6);
            var result = await _auth.LoginAsync(Email, Password, deviceId, _settings.BuildNumber);

            // TODO: nakon uspješnog login-a, navigacija na HomePage/Shell
            await Application.Current!.MainPage!.DisplayAlert("OK", $"Session: {result.SessionId}", "Super");
            await Shell.Current?.Navigation.PopToRootAsync()!;
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally { IsBusy = false; }
    }
}