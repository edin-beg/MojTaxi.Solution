using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.ClientApp.Services.Auth;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    public ObservableCollection<CountryInfo> Countries { get; }

    [ObservableProperty]
    private CountryInfo selectedCountry;

    [ObservableProperty]
    private string phoneNumber = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string otpCode = string.Empty;

    [ObservableProperty]
    private bool otpSent;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string error = string.Empty;

    [ObservableProperty]
    private int otpCooldown;

    [ObservableProperty]
    private LoginMethod selectedLoginMethod = LoginMethod.Sms;

    public bool IsSms => SelectedLoginMethod == LoginMethod.Sms;
    public bool IsEmail => SelectedLoginMethod == LoginMethod.Email;

    public bool CanSendOtp =>
        !IsBusy &&
        (
            (IsSms && !string.IsNullOrWhiteSpace(PhoneNumber)) ||
            (IsEmail && !string.IsNullOrWhiteSpace(Email))
        );

    public bool CanVerifyOtp => !IsBusy && !string.IsNullOrWhiteSpace(OtpCode);
    public bool CanResendOtp => OtpCooldown == 0;

    private readonly IClientsApi _clientsApi;
    private readonly ApiSettings _apiSettings;
    private readonly IAuthService _authService;
    private readonly IOtpSenderService _otpSenderService;   

    public LoginViewModel(
        IClientsApi clientsApi,
        ApiSettings apiSettings,
        IAuthService authService,
        IOtpSenderService otpSenderService)
    {
        _clientsApi = clientsApi;
        _apiSettings = apiSettings;
        _authService = authService;
        _otpSenderService = otpSenderService;

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

    partial void OnSelectedLoginMethodChanged(LoginMethod value)
    {
        OnPropertyChanged(nameof(IsSms));
        OnPropertyChanged(nameof(IsEmail));
        OnPropertyChanged(nameof(CanSendOtp));
    }

    [RelayCommand]
    private void SetSms() => SelectedLoginMethod = LoginMethod.Sms;

    [RelayCommand]
    private void SetEmail() => SelectedLoginMethod = LoginMethod.Email;

    [RelayCommand]
    private async Task SendOtp()
    {
        if (!CanSendOtp) return;

        IsBusy = true;
        Error = string.Empty;

        try
        {
            await _otpSenderService.SendOtpAsync(
    SelectedLoginMethod,
    IsSms
        ? $"{SelectedCountry.Code}{PhoneNumber}"
        : Email);
            await Task.Delay(500);

            OtpSent = true;
            await StartOtpCooldown();
        }
        catch
        {
            Error = "Greška pri slanju OTP-a";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task VerifyOtp()
    {
        if (!CanVerifyOtp) return;

        IsBusy = true;
        Error = string.Empty;

        try
        {
            await _authService.LoginAsync(
                email: "edin.begovic@it-craft.ba",
                password: "lokaL993**");

            await Shell.Current.GoToAsync("//RegistrationPage");
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task StartOtpCooldown()
    {
        OtpCooldown = 60;
        OnPropertyChanged(nameof(CanResendOtp));

        while (OtpCooldown > 0)
        {
            await Task.Delay(1000);
            OtpCooldown--;
            OnPropertyChanged(nameof(OtpCooldown));
            OnPropertyChanged(nameof(CanResendOtp));
        }
    }

    partial void OnOtpCodeChanged(string value) =>
        OnPropertyChanged(nameof(CanVerifyOtp));

    partial void OnIsBusyChanged(bool value)
    {
        OnPropertyChanged(nameof(CanVerifyOtp));
        OnPropertyChanged(nameof(CanSendOtp));
    }
}

public enum LoginMethod
{
    Sms,
    Email
}
