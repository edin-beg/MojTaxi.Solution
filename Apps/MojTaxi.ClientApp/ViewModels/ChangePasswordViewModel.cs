using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MojTaxi.ClientApp.ViewModels;

public partial class ChangePasswordViewModel : ObservableObject
{
    [ObservableProperty] private string currentPassword;
    [ObservableProperty] private string newPassword;
    [ObservableProperty] private string confirmPassword;

    [RelayCommand]
    private async Task ChangePassword()
    {
        if (NewPassword != ConfirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert("Greška", "Passwordi se ne poklapaju!", "OK");
            return;
        }

        // TODO: API call
        await Application.Current.MainPage.DisplayAlert("Uspješno", "Password promijenjen!", "OK");
    }
}
