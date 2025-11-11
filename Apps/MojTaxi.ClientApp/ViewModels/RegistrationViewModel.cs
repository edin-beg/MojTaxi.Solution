using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.Core.Abstractions;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

using Pages;

public partial class RegistrationViewModel : ObservableObject
{
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

    public RegistrationViewModel() { }

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

        Application.Current.MainPage = new MainPage();

    }
}