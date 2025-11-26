using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Maps;
using MojTaxi.Client.ViewModels;

namespace MojTaxi.Client.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _vm;
        private Pin? userPin;

        // MainViewModel sada dolazi iz DI
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();

#if IOS
        this.SafeAreaEdges = Microsoft.Maui.SafeAreaEdges.None;
#endif
            var insets = On<iOS>().SafeAreaInsets();
            BindingContext = _vm = vm;

            Loaded += async (_, __) =>
            {
                await _vm.LoadLocationCommand.ExecuteAsync(null);
                UpdateMap();
            };
        }

        private void UpdateMap()
        {
            if (_vm.CurrentLocation == null)
                return;

            var span = MapSpan.FromCenterAndRadius(_vm.CurrentLocation, Distance.FromKilometers(1));
            mapControl.MoveToRegion(span);

            if (userPin == null)
            {
                userPin = new Pin
                {
                    Label = "Moja lokacija",
                    Address = "Trenutna pozicija",
                    Type = PinType.Generic,
                    Location = _vm.CurrentLocation
                };
                mapControl.Pins.Add(userPin);
            }
            else
            {
                userPin.Location = _vm.CurrentLocation;
            }
        }
    }
}
