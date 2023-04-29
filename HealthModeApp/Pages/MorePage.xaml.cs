using System.Diagnostics;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using HealthModeApp.Models;
using HealthModeApp.Pages.FoodJournalPage;

namespace HealthModeApp.Pages;

public partial class MorePage : ContentPage
{
    private readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;

    UserInfo updatedUserInfo = new UserInfo
    {
        Email = "newemail@example.com"
    };

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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SeesAds();

    }

    async void SeesAds()
    {
        Ad.IsVisible = await _localData.GetSeesAds();
    }

    async void NoAdsClicked(System.Object sender, System.EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmation", "Do you want to disable ads for 30 minutes?", "Yes", "No");
        if (answer)
        {
            var userID = await _localData.GetUserID();
            await _dataService.UpdateExpDateAsync(userID, 0.5);
            bool seesAds = await _dataService.GetSeesAdsAsync(userID);
            await _localData.UpdateSeesAdsAsync(seesAds);
            SeesAds();
        }

    }

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmation", "Are you sure you want to logout?", "Yes", "No");
       
        if (answer)
        {
         
            await _localData.DeleteUser();
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                await Navigation.PushAsync(new LoginPage(_dataService, _localData));
            }
            else { await Navigation.PushModalAsync(new LoginPage(_dataService, _localData)); }

            
            // In your page code behind, you can access the TabBar like this:
            var shell = Shell.Current;
            var tabBar = shell.FindByName<TabBar>("TabBar");

            // To change the selected tab, you can set the CurrentItem property of the TabBar:
            tabBar.CurrentItem = tabBar.Items[2];
           
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
}
