using HealthModeApp.DataServices;

namespace HealthModeApp.Pages.ProfilePage;

public partial class UserProfile : ContentPage
{
    private readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;

    int userID;

    public UserProfile(IRestDataService dataService, ISQLiteDataService localData, ImageSource pfpSource)
	{
        Shell.SetTabBarIsVisible(this, false);
        InitializeComponent();
        _dataService = dataService;
        _localData = localData;
        ProfilePicture.Source = pfpSource;
        PopulateData();

	}

    async void PopulateData()
    {
        Username.Text = "      ";
        FlairLabel.TextColor = Colors.Transparent;
        FlairLabel.Text = "Flair";
        FlairBG.Background = Colors.Transparent;

        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);

        
        PfpHolder.Background = Color.FromHex(userInfo.PictureBGColor);


        FlairBG.Background = Color.FromHex(userInfo.FlairColor);

        FlairLabel.TextColor = userInfo.IsBlackText ? Colors.Black : Colors.White;
        FlairLabel.Text = userInfo.Flair.ToString();
        Username.Text = userInfo.Username.ToString();
    }

    void FlairTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {

    }
}
