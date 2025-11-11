using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MojTaxi.ClientApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty] private bool isDarkMode = true;
    [ObservableProperty] private string selectedLanguage = "Bosanski";

    public string[] Languages { get; } = { "Bosanski", "English", "Deutsch" };

    [RelayCommand]
    private void SaveSettings()
    {
        // TODO: Spremiti u Preferences / API
    }
}
