using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.ClientApp.Services;
using MojTaxi.ClientApp.Pages;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly INavigationService _nav;

    public ProfileViewModel(INavigationService nav)
    {
        _nav = nav;
        LoadProfileAsync();
        BuildMenuItems();
    }

    // ⬇️ Bindable UI fields
    [ObservableProperty] private string fullName;
    [ObservableProperty] private string phoneNumber;
    [ObservableProperty] private string email;
    [ObservableProperty] private ImageSource profileImage;

    // ⬇️ Data za CollectionView listu
    public ObservableCollection<ProfileItem> Items { get; } = new();

    private void BuildMenuItems()
    {
        Items.Add(new ProfileItem("Plaćanje", "\uE8CB", nameof(PaymentPage)));
        Items.Add(new ProfileItem("Valuta i jezik", "\uE894", nameof(SettingsPage)));
        Items.Add(new ProfileItem("Postavke", "\uE8B8", nameof(SettingsPage)));
        Items.Add(new ProfileItem("Podrška", "\uE8F6", nameof(LegalPage)));
        Items.Add(new ProfileItem("Pravne informacije", "\uE88F", nameof(LegalPage)));
    }

    // ✅ Binding koji poziva navigaciju kad klikneš item
    public async Task NavigateTo(ProfileItem item)
    {
        if (item is null) return;
        await _nav.GoToAsync(item.Route);
    }

    // ✅ Logout
    [RelayCommand]
    private async Task Logout()
    {
        // kasnije brisanje session tokena
        await _nav.GoToAsync($"///{nameof(LoginPage)}");
    }

    // ✅ Delete profile / GDPR
    [RelayCommand]
    private async Task DeleteAccount()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Potvrda",
            "Da li sigurno želiš obrisati svoj profil (GDPR)?",
            "Obriši",
            "Odustani"
        );

        if (confirm)
        {
            // TODO: API DELETE poziv
            await Shell.Current.DisplayAlert("Obrisano", "Profil je obrisan.", "OK");
            await _nav.GoToAsync($"///{nameof(LoginPage)}");
        }
    }

    // ✅ Placeholder loading (kasnije iz API-a)
    public async Task LoadProfileAsync()
    {
        FullName = "Edin Begović";
        PhoneNumber = "+387 61 000 000";
        Email = "edin@example.com";

        ProfileImage = ImageSource.FromUri(new Uri("https://images.unsplash.com/photo-1728577740843-5f29c7586afe?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1160"));
    }
}

public record ProfileItem(string Title, string Icon, string Route);
