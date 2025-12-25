namespace MojTaxi.ClientApp;

using MojTaxi.ClientApp.Helpers;
using MojTaxi.Core.Abstractions;
using Pages;

public partial class App : Application
{
    public static IServiceProvider? Services { get; private set; }

    public App(IServiceProvider provider)
    {
        InitializeComponent();
        Services = provider;

        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            try
            {
                var logger = ServiceHelper.Get<IErrorLogger>();

                if (e.ExceptionObject is Exception ex)
                    _ = Task.Run(() => logger!.LogAsync(ex));
            }
            catch { }
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            try
            {
                var logger = ServiceHelper.Get<IErrorLogger>();
                _ = Task.Run(() => logger!.LogAsync(e.Exception));
            }
            catch { }
        };

    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell(Services!));
    }
}