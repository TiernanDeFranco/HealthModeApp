using HealthModeApp.DataServices;
using Mopups.Services;

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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        PopulateData();
    }

    public async void PopulateData()
    {
        Username.Text = "      ";
        FlairLabel.TextColor = Colors.Transparent;
        FlairLabel.Text = "Flair";
        FlairBG.Background = Colors.Transparent;

        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);

        string cdnUrl = "https://d2f1hfw011wycq.cloudfront.net";

        string pfpSource = cdnUrl + "/profile-pictures" + userInfo.PicturePath;
        ProfilePicture.Source = new UriImageSource { Uri = new System.Uri(pfpSource) };

        PfpHolder.Background = Color.FromHex(userInfo.PictureBGColor);


        FlairBG.Background = Color.FromHex(userInfo.FlairColor);

        FlairLabel.TextColor = userInfo.IsBlackText ? Colors.Black : Colors.White;
        FlairLabel.Text = userInfo.Flair.ToString();
        Username.Text = userInfo.Username.ToString();
    }

    

    async void FlairTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var flairPage = new ChangeFlair(this, _dataService, _localData);
        await MopupService.Instance.PushAsync(flairPage);
    }

    async void PasswordTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await MopupService.Instance.PushAsync(new PasswordChange(_dataService, _localData));
    }

    async void UsernameTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var userNameChangePage = new UsernameChange(this, _dataService, _localData);

        

        await MopupService.Instance.PushAsync(userNameChangePage);
    }



    async void PfpTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var pfpPage = new UpdateProfilePicture(this, _dataService, _localData, ProfilePicture.Source);
        await MopupService.Instance.PushAsync(pfpPage);
    }
}
