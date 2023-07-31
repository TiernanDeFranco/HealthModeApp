using System.Diagnostics;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using HealthModeApp.Models;
using HealthModeApp.Pages.FoodJournalPage;
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

        string titleSource = cdnUrl + "/titles" + userInfo.Title;
        TitleImage.Source = new UriImageSource { Uri = new System.Uri(titleSource) };

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

   
    async void GoalButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushModalAsync(new NutritionGoalsPage(_localData, _dataService));
    }

    async void UnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushModalAsync(new UnitPage(_localData, _dataService));
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

    List<string> options = new List<string>
{
    "Remove Ads",
    "My Profile",
    "My Medals",
    "Water Goal",
    "CalMac",
    "Micronutrients",
    "Units",
    "Progress",
    "Nutrition Breakdown",
    "Workouts"
};

    List<string> images = new List<string>
{
    "crownicon",
    "usericon",
    "medalicon",
    "watericon",
    "calicon",
    "pillicon",
    "rulericon",
    "charticon",
    "piechart",
    "dumbbellicon2"

    // ... Add more image file names as needed
};

    int imageIndex = 0; // Variable to keep track of the image index

    void CreateList()
    {
        var firstRow = OptionsGrid.RowDefinitions[0];

        // Clear all existing rows in the OptionsGrid
        OptionsGrid.RowDefinitions.Clear();

        // Add the first row back to the OptionsGrid
        OptionsGrid.RowDefinitions.Add(firstRow);

        foreach (string option in options)
        {
            // Create a new row definition
            RowDefinition row = new RowDefinition();
            row.Height = GridLength.Auto; // Set the row height as needed
            OptionsGrid.RowDefinitions.Add(row);

            // Create the content for the new row
            Label label = new Label();
            label.Text = option;
            label.FontSize = 17;
            label.VerticalOptions = LayoutOptions.Center;

            if (label.Text == "CalMac")
            {
                
                var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
                var energyUnit = unitList[2];


                switch (energyUnit)
                {
                    case "cal":
                        label.Text = "Calories & Macros";
                        break;

                    case "kCal":
                        label.Text = "Kilocalories & Macros";
                        break;

                    case "kJ":
                        label.Text = "Kilojoules & Macros";
                        break;
                }
            }

            // Get the image for the current index
            string image = images[imageIndex];

            // Increment the image index
            imageIndex++;
            if (imageIndex >= images.Count)
            {
                imageIndex = 0; // Reset the index if it exceeds the image list size
            }

            Image img = new Image();
            img.Source = image;
            img.Aspect = Aspect.AspectFit;
            img.WidthRequest = 25;
            img.HeightRequest = 25;
            img.Margin = new Thickness(12, 0, 0, 0);
            img.VerticalOptions = LayoutOptions.Center;


            BoxView separator = new BoxView();
            separator.Color = Colors.LightGray;
            separator.HeightRequest = 1;

            // Add the content to the grid
            OptionsGrid.SetRow(label, OptionsGrid.RowDefinitions.Count - 1);
            OptionsGrid.SetColumn(label, 1);
            OptionsGrid.Children.Add(label);

            // Add the content to the grid
            OptionsGrid.SetRow(img, OptionsGrid.RowDefinitions.Count - 1);
            OptionsGrid.SetColumn(img, 0);
            OptionsGrid.Children.Add(img);

            RowDefinition row2 = new RowDefinition();
            row2.Height = GridLength.Auto; // Set the row height as needed
            OptionsGrid.RowDefinitions.Add(row2);

            OptionsGrid.SetRow(separator, OptionsGrid.RowDefinitions.Count - 1);
            OptionsGrid.SetColumnSpan(separator, 3);
            OptionsGrid.Children.Add(separator);

           
        }
    }


}
