using Microsoft.Extensions.Logging;

namespace MojTaxi.StaticClientApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>().
        ConfigureMauiHandlers(handlers =>
            {
#if IOS
                Microsoft.Maui.Handlers.PageHandler.Mapper.AppendToMapping("SafeArea", (handler, view) =>
                {
                    handler.PlatformView.InsetsLayoutMarginsFromSafeArea = false;
                });
#endif
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}