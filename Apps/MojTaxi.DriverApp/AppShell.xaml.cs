namespace MojTaxi.DriverApp;

public partial class AppShell : Shell
{
    public AppShell(IServiceProvider serviceProvider)
    {
        InitializeComponent();

    }

    //Čista Shell navigacija po ruti
    public async Task GoToPageAsync<T>() where T : Page
    {
        var route = typeof(T).Name;
        await Shell.Current.GoToAsync(route, animate: true);
    }
}