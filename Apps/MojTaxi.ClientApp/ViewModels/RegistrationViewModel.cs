using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.Core.Abstractions;

namespace MojTaxi.ClientApp.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using Models;


public class RegistrationViewModel : ObservableObject
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; } // set from previous screen
    public string Email { get; set; }
    public ImageSource ProfileImage { get; set; }

    public ICommand PickImageCommand { get; }
    public ICommand SubmitRegistrationCommand { get; }

    public RegistrationViewModel()
    {
        PickImageCommand = new Command(async () => await PickImage());
        SubmitRegistrationCommand = new Command(async () => await Submit());
    }

    private async Task PickImage()
    {
        FileResult photo = await MediaPicker.PickPhotoAsync();
        if (photo != null)
            ProfileImage = ImageSource.FromFile(photo.FullPath);

        OnPropertyChanged(nameof(ProfileImage));
    }

    private async Task Submit()
    {
        // Validation + API poziv za kreiranje usera
        await App.Current.MainPage.DisplayAlert("OK", "Registracija uspješna", "OK");

        await Shell.Current.GoToAsync("//MainPage");

    }
}
