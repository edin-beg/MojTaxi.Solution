using MojTaxi.ClientApp.ViewModels;

namespace MojTaxi.ClientApp.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ProfileItem item)
            await ((ProfileViewModel)BindingContext).NavigateTo(item);

        ((CollectionView)sender).SelectedItem = null; // remove highlight
    }
}
