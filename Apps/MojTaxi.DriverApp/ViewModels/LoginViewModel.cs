using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.Core.Models;
using System.Collections.ObjectModel;

namespace MojTaxi.DriverApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    public ObservableCollection<CountryInfo> Countries { get; set; }

    [ObservableProperty]
    private CountryInfo selectedCountry;

    [ObservableProperty]
    private string phoneNumber = string.Empty;

    [ObservableProperty]
    private string otpCode = string.Empty;

    [ObservableProperty]
    private bool otpSent;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string error = string.Empty;

    [ObservableProperty]
    private int otpCooldown = 60;

    public bool CanSendOtp => !IsBusy && !string.IsNullOrWhiteSpace(PhoneNumber);
    public bool CanVerifyOtp => !IsBusy && !string.IsNullOrWhiteSpace(OtpCode);
    public bool CanResendOtp => OtpCooldown == 0;

    public LoginViewModel()
    {
        Countries = new ObservableCollection<CountryInfo>
        {
            new CountryInfo { Name = "Bosna i Hercegovina", Code = "+387", Flag = "🇧🇦" },
            new CountryInfo { Name = "Srbija", Code = "+381", Flag = "🇷🇸" },
            new CountryInfo { Name = "Hrvatska", Code = "+385", Flag = "🇭🇷" },
            new CountryInfo { Name = "Crna Gora", Code = "+382", Flag = "🇲🇪" },
            new CountryInfo { Name = "Slovenija", Code = "+386", Flag = "🇸🇮" },
            new CountryInfo { Name = "Sjeverna Makedonija", Code = "+389", Flag = "🇲🇰" },
            new CountryInfo { Name = "Germany", Code = "+49", Flag = "🇩🇪" },
            new CountryInfo { Name = "United States", Code = "+1", Flag = "🇺🇸" }
        };

        SelectedCountry = Countries.First(x => x.Code == "+387");
    }

    [RelayCommand]
    private async Task SendOtp()
    {
        if (!CanSendOtp) return;

        IsBusy = true;
        Error = string.Empty;

        try
        {
            // TODO: Pošalji OTP preko API-a
            await Task.Delay(500); // simulacija API poziva

            OtpSent = true;
            await StartOtpCooldown();
        }
        catch
        {
            Error = "Greška pri slanju OTP-a";
        }

        IsBusy = false;
    }

    [RelayCommand]
    private async Task VerifyOtp()
    {
        // ✅ Navigiraj na RegistrationPage
        await Shell.Current.GoToAsync("//RegistrationPage");
    /*    if (!CanVerifyOtp) return;

        IsBusy = true;
        Error = null;

        try
        {
            // TODO: Validacija OTP-a (API)
            await Task.Delay(500);

            bool otpIsValid = true; // simulacija - zamijeniti realnom provjerom

            if (otpIsValid)
            {
                // ✅ Navigiraj na RegistrationPage
                await Shell.Current.GoToAsync("//RegistrationPage");
            }
            else
            {
                Error = "OTP nije ispravan";
            }
        }
        catch
        {
            Error = "Greška pri verifikaciji OTP-a";
        }
        */

        IsBusy = false;
    }

    private async Task StartOtpCooldown()
    {
        OtpCooldown = 60;
        OnPropertyChanged(nameof(OtpCooldown));
        OnPropertyChanged(nameof(CanResendOtp));

        while (OtpCooldown > 0)
        {
            await Task.Delay(1000);
            OtpCooldown--;
            OnPropertyChanged(nameof(OtpCooldown));
            OnPropertyChanged(nameof(CanResendOtp));
        }
    }
    
    partial void OnOtpCodeChanged(string value)
    {
        OnPropertyChanged(nameof(CanVerifyOtp));
    }

    partial void OnIsBusyChanged(bool value)
    {
        OnPropertyChanged(nameof(CanVerifyOtp));
    }

}
