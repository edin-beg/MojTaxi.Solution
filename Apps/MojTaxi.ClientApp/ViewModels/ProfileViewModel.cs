using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.ClientApp.Pages;
using MojTaxi.ClientApp.Services;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Implementations;
using MojTaxi.Core.Services;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly INavigationService _nav;
    private readonly IClientSession _clientSession;
    private readonly IAuthService _authService;

    public ProfileViewModel(INavigationService nav, IClientSession clientSession, IAuthService authService)
    {
        _nav = nav;
        _clientSession = clientSession;
        LoadProfileFromSession();

        BuildMenuItems();

        _clientSession.SessionChanged += OnSessionChanged;
        _authService = authService;

    }

    [ObservableProperty] private string fullName = string.Empty;
    [ObservableProperty] private string phoneNumber = string.Empty;
    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private ImageSource? profileImage;

    public ObservableCollection<ProfileItem> Items { get; } = new();

    private void BuildMenuItems()
    {
        Items.Add(new ProfileItem("Historijat voznji", "\uE88F", nameof(RideHistoryPage)));
        Items.Add(new ProfileItem("Plaćanje", "\uE8CB", nameof(PaymentPage)));
        Items.Add(new ProfileItem("Valuta i jezik", "\uE894", nameof(SettingsPage)));
        Items.Add(new ProfileItem("Podrška", "\uE8F6", nameof(LegalPage)));
        Items.Add(new ProfileItem("Pravne informacije", "\uE88F", nameof(LegalPage)));
    }

    public async Task NavigateTo(ProfileItem item)
    {
        if (item is null) return;
        await _nav.GoToAsync(item.Route, animate: true);
    }

    [RelayCommand]
    private async Task DeleteAccount()
    {
        var page = Application.Current?.Windows[0].Page;
        if (page == null) return;

        bool confirm = await page.DisplayAlertAsync(
            "Potvrda",
            "Da li sigurno želiš obrisati svoj profil (GDPR)?",
            "Obriši",
            "Odustani");

        if (confirm)
        {
            // api call kasnije
            await page.DisplayAlertAsync("Obrisano", "Profil je obrisan.", "OK");
            await _nav.GoToAsync($"///{nameof(LoginPage)}", animate: true);
        }
    }
    private void LoadProfileFromSession()
    {
        var client = _clientSession.Client;

        if (client == null)
            return;

        FullName = $"{client.FirstName} {client.LastName}".Trim();
        PhoneNumber = client.Phone ?? string.Empty;
        Email = client.Email ?? string.Empty;

        ProfileImage = ImageSource.FromUri(new Uri("https://images.unsplash.com/photo-1728577740843-5f29c7586afe?auto=format&fit=crop&q=80&w=1160"));

    }
    private void OnSessionChanged()
    {
        MainThread.BeginInvokeOnMainThread(LoadProfileFromSession);
    }

    [RelayCommand]
    private async Task Logout()
    {
        await _authService.LogoutAsync();
    }

}

public record ProfileItem(string Title, string Icon, string Route);
