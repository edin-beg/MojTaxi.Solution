namespace MojTaxi.ClientApp;

using Pages;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public App(IServiceProvider provider)
    {
        InitializeComponent();
        MainPage = new AppShell(provider);
        Services = provider;
    }
}