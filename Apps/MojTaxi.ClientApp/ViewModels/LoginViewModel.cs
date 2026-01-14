using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.ClientApp.Services.Auth;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

/// <summary>
/// ViewModel responsible for handling login flow (SMS or Email) using OTP.
/// Implements MVVM pattern via CommunityToolkit.
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    /// <summary>
    /// List of supported countries used for phone number prefix selection.
    /// </summary>
    public ObservableCollection<CountryInfo> Countries { get; }

    /// <summary>
    /// Currently selected country for SMS login.
    /// </summary>
    [ObservableProperty]
    private CountryInfo selectedCountry;

    /// <summary>
    /// Phone number entered by the user (without country code).
    /// Triggers CanSendOtp recalculation when changed.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSendOtp))]
    private string phoneNumber = string.Empty;

    /// <summary>
    /// Email entered by the user.
    /// Triggers CanSendOtp recalculation when changed.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSendOtp))]
    private string email = string.Empty;

    /// <summary>
    /// OTP code entered by the user.
    /// </summary>
    [ObservableProperty]
    private string otpCode = string.Empty;

    /// <summary>
    /// Indicates whether an OTP has been sent.
    /// </summary>
    [ObservableProperty]
    private bool otpSent;

    /// <summary>
    /// Indicates ongoing background operation (API call, OTP verification, etc.).
    /// Used to disable UI interactions.
    /// </summary>
    [ObservableProperty]
    private bool isBusy;

    /// <summary>
    /// Error message displayed to the user.
    /// </summary>
    [ObservableProperty]
    private string error = string.Empty;

    /// <summary>
    /// Countdown timer (in seconds) before OTP can be resent.
    /// </summary>
    [ObservableProperty]
    private int otpCooldown;

    /// <summary>
    /// Currently selected login method (SMS or Email).
    /// </summary>
    [ObservableProperty]
    private LoginMethod selectedLoginMethod = LoginMethod.Sms;

    /// <summary>
    /// Helper flags for UI binding.
    /// </summary>
    public bool IsSms => SelectedLoginMethod == LoginMethod.Sms;
    public bool IsEmail => SelectedLoginMethod == LoginMethod.Email;

    /// <summary>
    /// Determines whether OTP can be sent based on input validity and busy state.
    /// </summary>
    public bool CanSendOtp =>
        !IsBusy &&
        (
            (IsSms && !string.IsNullOrWhiteSpace(PhoneNumber)) ||
            (IsEmail && !string.IsNullOrWhiteSpace(Email))
        );

    /// <summary>
    /// Determines whether OTP verification can be executed.
    /// </summary>
    public bool CanVerifyOtp => !IsBusy && !string.IsNullOrWhiteSpace(OtpCode);

    /// <summary>
    /// Determines whether OTP can be resent (cooldown expired).
    /// </summary>
    public bool CanResendOtp => OtpCooldown == 0;

    // External dependencies injected via constructor
    private readonly IClientsApi _clientsApi;
    private readonly ApiSettings _apiSettings;
    private readonly IAuthService _authService;
    private readonly IOtpSenderService _otpSenderService;

    /// <summary>
    /// Initializes the LoginViewModel and sets up default country list.
    /// </summary>
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

        // Static list of supported countries
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

        // Default country selection
        SelectedCountry = Countries.First(x => x.Code == "+387");
    }

    /// <summary>
    /// Triggered automatically when SelectedLoginMethod changes.
    /// Forces UI refresh of dependent properties.
    /// </summary>
    partial void OnSelectedLoginMethodChanged(LoginMethod value)
    {
        OnPropertyChanged(nameof(IsSms));
        OnPropertyChanged(nameof(IsEmail));
        OnPropertyChanged(nameof(CanSendOtp));
    }

    /// <summary>
    /// Switches login mode to SMS.
    /// </summary>
    [RelayCommand]
    private void SetSms() => SelectedLoginMethod = LoginMethod.Sms;

    /// <summary>
    /// Switches login mode to Email.
    /// </summary>
    [RelayCommand]
    private void SetEmail() => SelectedLoginMethod = LoginMethod.Email;

    /// <summary>
    /// Sends OTP via selected login method.
    /// </summary>
    [RelayCommand]
    private async Task SendOtp()
    {
        IsBusy = true;

        await _authService.LoginAsync(
               email: "edin.begovic@it-craft.ba",
               password: "lokaL993***");

        // Navigate to registration page after successful login
        IsBusy = false;
        await Shell.Current.GoToAsync("//RegistrationPage", animate: true);

        //if (!CanSendOtp) return;

        //IsBusy = true;
        //Error = string.Empty;

        //try
        //{
        //    // Planned OTP sending logic (currently disabled)
        //    // await _otpSenderService.SendOtpAsync(
        //    //     SelectedLoginMethod,
        //    //     IsSms ? $"{SelectedCountry.Code}{PhoneNumber}" : Email);

        //    // OtpSent = true;
        //    // await StartOtpCooldown();

        //    // Temporary shortcut for development/testing
        //    await VerifyOtp();
        //}
        //catch (Exception)
        //{
        //    Error = "Greška pri slanju OTP-a";
        //}
        //finally
        //{
        //    IsBusy = false;
        //}
    }

    /// <summary>
    /// Verifies OTP and performs login.
    /// </summary>
    [RelayCommand]
    private async Task VerifyOtp()
    {
        //if (!CanVerifyOtp) return;

        //IsBusy = true;
        //Error = string.Empty;

        //try
        //{
        //    // Temporary hardcoded login for testing purposes
        //    await _authService.LoginAsync(
        //        email: "edin.begovic@it-craft.ba",
        //        password: "lokaL993**");

        //    // Navigate to registration page after successful login
        //    await Shell.Current.GoToAsync("//RegistrationPage");
        //}
        //catch (Exception ex)
        //{
        //    Error = ex.Message;
        //}
        //finally
        //{
        //    IsBusy = false;
        //}
    }

    /// <summary>
    /// Starts OTP resend cooldown timer.
    /// </summary>
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

    /// <summary>
    /// Updates CanVerifyOtp when OTP code changes.
    /// </summary>
    partial void OnOtpCodeChanged(string value) =>
        OnPropertyChanged(nameof(CanVerifyOtp));

    /// <summary>
    /// Updates dependent UI state when busy flag changes.
    /// </summary>
    partial void OnIsBusyChanged(bool value)
    {
        OnPropertyChanged(nameof(CanVerifyOtp));
        OnPropertyChanged(nameof(CanSendOtp));
    }
}

/// <summary>
/// Supported login methods.
/// </summary>
public enum LoginMethod
{
    Sms,
    Email
}
