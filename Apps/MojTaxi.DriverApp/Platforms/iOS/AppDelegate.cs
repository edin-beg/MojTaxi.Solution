using Foundation;
using UIKit;

namespace MojTaxi.DriverApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnActivated(UIApplication uiApplication)
    {
        UIApplication.SharedApplication.StatusBarHidden = true;
        base.OnActivated(uiApplication);
    }
}