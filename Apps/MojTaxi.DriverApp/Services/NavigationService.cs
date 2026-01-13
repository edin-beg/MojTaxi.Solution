using System.Diagnostics;

namespace MojTaxi.DriverApp.Services
{
    public class NavigationService : INavigationService
    {
        public async Task GoToAsync(string route, bool animate = true)
        {
            if (Shell.Current == null)
            {
                Debug.WriteLine("Shell.Current je null - Navigacija nije moguća!");
                return;
            }

            await Shell.Current.GoToAsync(route, animate);
        }

        public async Task GoToAsync(string route, IDictionary<string, object>? parameters, bool animate = true)
        {
            if (Shell.Current == null)
            {
                Debug.WriteLine("Shell.Current je null - Navigacija nije moguća!");
                return;
            }

            await Shell.Current.GoToAsync(route, animate, parameters ?? new Dictionary<string, object>());
        }

        public async Task GoBackAsync()
        {
            if (Shell.Current?.Navigation == null)
                return;

            await Shell.Current.Navigation.PopAsync();
        }

        public async Task GoToRootAsync()
        {
            if (Shell.Current == null) return;
            await Shell.Current.GoToAsync("//", animate: true);
        }
    }
}
