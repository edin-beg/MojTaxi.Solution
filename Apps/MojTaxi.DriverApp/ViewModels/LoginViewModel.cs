using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.Core.Abstractions;
using MojTaxi.ApiClient.Dtos;

namespace MojTaxi.DriverApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IDriverAuthService _auth;
    private readonly MojTaxi.ApiClient.ApiSettings _settings;

    [ObservableProperty] 
    List<OrganisationDto> organisations = new();

    [ObservableProperty] 
    OrganisationDto? selectedOrganisation;

    [ObservableProperty] 
    string taxiId = "";

    [ObservableProperty] 
    string password = "";

    [ObservableProperty] 
    bool isBusy;

    [ObservableProperty] 
    string? error;

    public LoginViewModel(IDriverAuthService auth, MojTaxi.ApiClient.ApiSettings settings)
    {
        _auth = auth;
        _settings = settings;
        _ = LoadOrganisationsAsync();
    }

    private async Task LoadOrganisationsAsync()
    {
        try
        {
            IsBusy = true;
            var orgs = await _auth.GetOrganisationsAsync();
            Organisations = orgs.ToList();
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            IsBusy = true;
            Error = null;

            if (SelectedOrganisation == null)
            {
                Error = "Odaberi organizaciju.";
                return;
            }

            var deviceId = "drv-" + Guid.NewGuid().ToString("N").Substring(0, 6);
            var res = await _auth.LoginAsync(
                TaxiId, Password, deviceId,
                SelectedOrganisation.OrganisationId,
                _settings.AppVersion, _settings.BuildNumber);

            await Application.Current!.MainPage!.DisplayAlert("OK", $"Driver session: {res.SessionId}", "Super");
            await Shell.Current?.Navigation.PopToRootAsync()!;
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }
}
