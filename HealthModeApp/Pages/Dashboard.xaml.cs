using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages;

public partial class Dashboard : ContentPage
{
    public readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;
    public string success;

    public Dashboard(IRestDataService dataService, ISQLiteDataService localData)
    {

        InitializeComponent();
        _dataService = dataService;
        _localData = localData;
        TryLogin();
    }

    async void TryLogin()
    {
        (string email, string password) = await _localData.GetUserCredentials();
        if (email != null && password != null)
        {
            success = await _dataService.LoginAsync(email, password);
        }
        else
        {
            await Navigation.PopAsync();
            await Navigation.PushModalAsync(new LoginPage(_dataService, _localData));
            await _localData.DeleteUser();
            return;
        }
        if (success == "AllClear")
        {
            Debug.WriteLine("AutoLogin");
            int userID = await _dataService.GetUserIDByEmailAsync(email);
            (var userInfo, bool seesAds) = await _dataService.GetUserInfoOnLoginAsync(userID);
            await _localData.UpdateUserAsync(userID, userInfo.Email, userInfo.Username, userInfo.Password, seesAds, userInfo.WeightPlan, userInfo.MainGoals, userInfo.Units, userInfo.Sex, userInfo.HeightCm, userInfo.Birthday, userInfo.Weight, userInfo.GoalWeight, userInfo.ActivityLevel);
        }
        else
        {
            await Navigation.PopAsync();
            await Navigation.PushModalAsync(new LoginPage(_dataService, _localData));
            await _localData.DeleteUser();
            return;
        }

    }


}
