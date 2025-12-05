using MojTaxi.Core.Abstractions;
using MojTaxi.DriverApp.Helpers;

namespace MojTaxi.DriverApp;

public partial class App : Application
{
    public static IServiceProvider? Services { get; private set; }

    public App(IServiceProvider provider)
    {
        InitializeComponent();
        Services = provider;

        AppDomain.CurrentDomain.UnhandledException += async (s, e) =>
        {
            try
            {
                var logger = ServiceHelper.Get<IErrorLogger>();

                if (e.ExceptionObject is Exception ex)
                {
                    await logger!.LogAsync(ex);
                }
                else
                {
                    await logger!.LogAsync(new Exception($"Unhandled non-CLR exception: {e.ExceptionObject}"));
                }
            }
            catch
            {
               
            }
        };

        TaskScheduler.UnobservedTaskException += async (s, e) =>
        {
            try
            {
                var logger = ServiceHelper.Get<IErrorLogger>();
                await logger!.LogAsync(e.Exception);
            }
            catch
            {

            }
        };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell(Services!));
    }
}