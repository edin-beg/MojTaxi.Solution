using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.Core.Abstractions;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

using Pages;
using Services;

public partial class RegistrationViewModel : ObservableObject
{
    private readonly INavigationService _nav;
    
    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private string phoneNumber = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private ImageSource? profileImage = null;

    public RegistrationViewModel(INavigationService nav)
    {
        _nav = nav;
    }

    [RelayCommand]
    private async Task PickImage()
    {
        List<FileResult> photo = await MediaPicker.PickPhotosAsync();
        if (photo != null)
        {
            ProfileImage = ImageSource.FromFile(photo[0].FullPath);
        }
    }

    [RelayCommand]
    private async Task Submit()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}