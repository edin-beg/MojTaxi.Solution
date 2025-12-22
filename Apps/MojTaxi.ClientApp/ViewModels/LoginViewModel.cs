using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

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

    private readonly IClientsApi _clientsApi;
    private readonly ApiSettings _apiSettings;
    private readonly IAuthService _authService; 

    public LoginViewModel(IClientsApi clientsApi, ApiSettings apiSettings, IAuthService authService)
    {
        _clientsApi = clientsApi;
        _apiSettings = apiSettings;
        _authService = authService;
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
        if (IsBusy) return;

        IsBusy = true;
        Error = string.Empty;

        try
        {
            await Task.Run(async () =>
            {
                await _authService.LoginAsync(
                    email: "edin.begovic@it-craft.ba",
                    password: "lokaL993**");
            });

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync("//RegistrationPage");
            });
        }
        catch (InvalidOperationException ex)
        {
            Error = ex.Message; // "Invalid email/password combination..."
        }
        catch (Exception)
        {
            Error = "Unexpected error occurred. Try again.";
        }
        finally
        {
            IsBusy = false;
        }
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
