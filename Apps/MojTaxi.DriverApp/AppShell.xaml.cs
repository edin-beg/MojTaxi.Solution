namespace MojTaxi.DriverApp;

public partial class AppShell : Shell
{
    public AppShell(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        Routing.RegisterRoute("MainPage", typeof(MainPage));
    }
}