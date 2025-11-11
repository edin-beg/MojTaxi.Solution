using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

public partial class RideHistoryViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<string> rides = new();

    [RelayCommand]
    private async Task LoadRides()
    {
        // Mock podaci (kasnije API)
        Rides.Clear();
        Rides.Add("Sarajevo → Ilidža | 12.09.2025 | 8.50 KM");
        Rides.Add("Baščaršija → Grbavica | 11.09.2025 | 6.00 KM");
        Rides.Add("Otoka → Aerodrom | 10.09.2025 | 14.00 KM");
    }
}
