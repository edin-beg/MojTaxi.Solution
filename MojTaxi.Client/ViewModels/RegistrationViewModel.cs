using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.Core.Abstractions;
using System.Collections.ObjectModel;

namespace MojTaxi.Client.ViewModels;

using Pages;
using Services;

public partial class RegistrationViewModel : ObservableObject
{
    private readonly INavigationService _nav;
    
    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string phoneNumber;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private ImageSource profileImage;

    public RegistrationViewModel(INavigationService nav)
    {
        _nav = nav;
    }

    [RelayCommand]
    private async Task PickImage()
    {
        FileResult photo = await MediaPicker.PickPhotoAsync();
        if (photo != null)
        {
            ProfileImage = ImageSource.FromFile(photo.FullPath);
        }
    }

    [RelayCommand]
    private async Task Submit()
    {
        // TODO: Validacija + API poziv

        await _nav.GoToAsync(nameof(MainPage));

    }
}