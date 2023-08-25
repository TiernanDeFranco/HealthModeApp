using System.Diagnostics;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using HealthModeApp.Models;
using HealthModeApp.Pages.FoodJournalPage;
using HealthModeApp.Pages.WorkoutPages;
using HealthModeApp.Pages.ProfilePage;
using System.Text.Json;
using System.Text;

namespace HealthModeApp.Pages;

public partial class MorePage : ContentPage
{
    private readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;

    int userID;
    UserData userInfo;


    public MorePage()
    {
        InitializeComponent();

    }

    public MorePage(IRestDataService dataService, ISQLiteDataService localData)
    {
        InitializeComponent();
        _dataService = dataService;
        _localData = localData;



        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        string cdnUrl = "https://d2f1hfw011wycq.cloudfront.net";
        

        

        Username.Text = "      ";
        FlairLabel.TextColor = Colors.Transparent;
        FlairLabel.Text = "Flair";
        FlairBG.Background = Colors.Transparent;

        userID = await _localData.GetUserID();
        userInfo = await _localData.GetUserAsync(userID);

        string pfpSource = cdnUrl + "/profile-pictures" + userInfo.PicturePath;
        ProfilePicture.Source = new UriImageSource { Uri = new System.Uri(pfpSource) };
        PfpHolder.Background = Color.FromHex(userInfo.PictureBGColor);

        if (userInfo.Title != null)
        {
            string titleSource = cdnUrl + "/titles" + userInfo.Title;
            TitleImage.Source = new UriImageSource { Uri = new System.Uri(titleSource) };
        }

        FlairBG.Background = Color.FromHex(userInfo.FlairColor);

        FlairLabel.TextColor = userInfo.IsBlackText ? Colors.Black : Colors.White;
        FlairLabel.Text = userInfo.Flair.ToString();
        Username.Text = userInfo.Username.ToString();

        
        CreateList();

        SeesAds();

        
    }

    async void SeesAds()
    {
       
    }

    async void NoAdsClicked(System.Object sender, System.EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmation", "Do you want to disable ads for 30 minutes?", "Yes", "No");
        if (answer)
        {
            var userID = await _localData.GetUserID();
            (string email, string password) = await _localData.GetUserCredentials();
            await _dataService.UpdateExpDateAsync(email, userID, 0.5);
            bool seesAds = await _dataService.GetSeesAdsAsync(userID);
            await _localData.UpdateSeesAdsAsync(seesAds);
            SeesAds();
        }

    }

   
    async void CaloriesMacrosTapped(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new NutritionGoalsPage(_localData, _dataService));
    }

    async void UnitsTapped(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new UnitPage(_localData, _dataService));
    }

    async void ProgressHubTapped(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new ProgressHub(_dataService, _localData));
    }

    async void NutritionBreakdownTapped(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new NutritionalBreakdown(_localData, DateTime.Today));
    }

    void BugReportClicked(System.Object sender, System.EventArgs e)
    {
        string url = "https://forms.gle/uZfeZYEe38C66LJh6";
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            Task.Run(async () =>
            {
                await Launcher.OpenAsync(url);
            });
        }
        else
        {
            Launcher.TryOpenAsync(url);
        }
    }

    void DiscordClicked(System.Object sender, System.EventArgs e)
    {
        string url = "https://discord.gg/htb7An6SGE";
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            Task.Run(async () =>
            {
                await Launcher.OpenAsync(url);
            });
        }
        else
        {
            Launcher.TryOpenAsync(url);
        }
    }

    void RedditClicked(System.Object sender, System.EventArgs e)
    {
        string url = "https://www.reddit.com/r/HealthModeApp/";
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            Task.Run(async () =>
            {
                await Launcher.OpenAsync(url);
            });
        }
        else
        {
            Launcher.TryOpenAsync(url);
        }
    }

    void InstagramClicked(System.Object sender, System.EventArgs e)
    {
        string url = "https://www.instagram.com/healthmodeapp/";
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            Task.Run(async () =>
            {
                await Launcher.OpenAsync(url);
            });
        }
        else
        {
            Launcher.TryOpenAsync(url);
        }
    }

    void TwitterClicked(System.Object sender, System.EventArgs e)
    {
        string url = "https://twitter.com/HealthModeApp";
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            Task.Run(async () =>
            {
                await Launcher.OpenAsync(url);
            });
        }
        else
        {
            Launcher.TryOpenAsync(url);
        }
    }

    void YoutubeClicked(System.Object sender, System.EventArgs e)
    {
        string url = "https://www.youtube.com/@healthmodeapp";
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            Task.Run(async () =>
            {
                await Launcher.OpenAsync(url);
            });
        }
        else
        {
            Launcher.TryOpenAsync(url);
        }
    }

    async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmation", "Are you sure you want to logout?", "Yes", "No");

        if (answer)
        {

            await _localData.DeleteUser();
            await Navigation.PushModalAsync(new LoginPage(_dataService, _localData));


            // In your page code behind, you can access the TabBar like this:
            var shell = Shell.Current;
            var tabBar = shell.FindByName<TabBar>("TabBar");

            // To change the selected tab, you can set the CurrentItem property of the TabBar:
            tabBar.CurrentItem = tabBar.Items[2];

        }



    }

 
    void CreateList()
    {
     
                    var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
                    var energyUnit = unitList[2];

                    switch (energyUnit)
                    {
                        case "cal":
                            CalMac.Text = "Calories & Macros";
                            break;

                        case "kCal":
                            CalMac.Text = "Kilocalories & Macros";
                            break;

                        case "kJ":
                            CalMac.Text = "Kilojoules & Macros";
                            break;
                    }

    }

    async void NotImplemented(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await DisplayAlert("Sorry", "This feature is currently not implemented but will be coming soon", "Ok");
    }

    async void MyProfileTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var pfpSource = ProfilePicture.Source;

        await Navigation.PushAsync(new UserProfile(_dataService, _localData, pfpSource));
    }




}
