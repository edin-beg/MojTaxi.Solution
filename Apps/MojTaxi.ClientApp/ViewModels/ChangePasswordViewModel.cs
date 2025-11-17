using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MojTaxi.ClientApp.ViewModels;

public partial class ChangePasswordViewModel : ObservableObject
{
    [ObservableProperty] private string currentPassword = string.Empty;
    [ObservableProperty] private string newPassword = string.Empty;
    [ObservableProperty] private string confirmPassword = string.Empty;

    [RelayCommand]
    private async Task ChangePassword()
    {
        if (NewPassword != ConfirmPassword)
        {
            var page = Application.Current?.Windows[0].Page;
            if (page != null)
                await page.DisplayAlertAsync("Greška", "Passwordi se ne poklapaju!", "OK");

            return;
        }

        // TODO: API call

        var successPage = Application.Current?.Windows[0].Page;
        if (successPage != null)
            await successPage.DisplayAlertAsync("Uspješno", "Password promijenjen!", "OK");
    }
}