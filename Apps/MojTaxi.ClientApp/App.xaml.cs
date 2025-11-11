namespace MojTaxi.ClientApp;

using Pages;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell(); 

    }
}