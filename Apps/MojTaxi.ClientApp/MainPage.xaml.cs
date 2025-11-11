namespace MojTaxi.ClientApp.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        SetActiveTab(BtnProfile); // default
    }

    private void OnTabClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        SetActiveTab(button);

        string tab = button.CommandParameter.ToString();

        switch (tab)
        {
            case "Profile":
                DisplayAlert("TAB", "Profil otvoren", "OK");
                break;
            case "Pets":
                DisplayAlert("TAB", "Ljubimci otvoreni", "OK");
                break;
            case "Kids":
                DisplayAlert("TAB", "Djeca otvorena", "OK");
                break;
            case "Special":
                DisplayAlert("TAB", "Posebno otvoreno", "OK");
                break;
        }
    }

    private void SetActiveTab(Button active)
    {
        // reset svih buttona
        BtnProfile.TextColor = Colors.Gray;
        BtnPets.TextColor = Colors.Gray;
        BtnKids.TextColor = Colors.Gray;
        BtnSpecial.TextColor = Colors.Gray;

        BtnProfile.BackgroundColor = Color.FromHex("#222222");
        BtnPets.BackgroundColor = Color.FromHex("#222222");
        BtnKids.BackgroundColor = Color.FromHex("#222222");
        BtnSpecial.BackgroundColor = Color.FromHex("#222222");

        // aktivni tab
        active.TextColor = Colors.Black;
        active.BackgroundColor = Colors.Yellow;
    }
}
