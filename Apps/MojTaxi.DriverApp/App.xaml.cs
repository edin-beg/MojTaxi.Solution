namespace MojTaxi.DriverApp;

public partial class App : Application
{
    public static IServiceProvider? Services { get; private set; }

    public App(IServiceProvider provider)
    {
        InitializeComponent();
        Services = provider;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell(Services!));
    }
}