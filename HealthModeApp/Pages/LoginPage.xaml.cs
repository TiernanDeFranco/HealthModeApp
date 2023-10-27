

using System.Data.SqlTypes;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages;

public partial class LoginPage : ContentPage
{


    public readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

    Dashboard _sender;
    int _userID;

    public LoginPage(Dashboard sender, IRestDataService dataService, ISQLiteDataService localData)
	{
		InitializeComponent();
        _localData = localData;
        _dataService = dataService;
        _sender = sender;
        NavigationPage.SetHasNavigationBar(this, false); // hide navigation bar
        NavigationPage.SetHasBackButton(this, false); // hide back button

        Logo.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .28;
        LoginFrame.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .3;
        EmailLogin.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .27;
        PasswordLogin.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .27;

    }

    public LoginPage(IRestDataService dataService, ISQLiteDataService localData)
    {
        InitializeComponent();
        _localData = localData;
        _dataService = dataService;
        NavigationPage.SetHasNavigationBar(this, false); // hide navigation bar
        NavigationPage.SetHasBackButton(this, false); // hide back button

        Logo.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .28;
        LoginFrame.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .3;
        EmailLogin.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .27;
        PasswordLogin.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .27;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);
        NavigationPage.SetHasNavigationBar(this, false);

    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    string cVersion;

    async void AutoLogin(string email, string hashedPassword)
    {
        int userID = await _dataService.GetUserIDByEmailAsync(email); Debug.WriteLine(userID);
        _userID = userID;
        (UserInfo userInfo, bool seesAds, string flair, string flairColor, bool isBlackText, string picturePath) = await _dataService.GetUserInfoOnLoginAsync(userID, email, hashedPassword);
        Debug.WriteLine(seesAds);
        Debug.WriteLine("Add User:");

        await _localData.AddUserAsync(userID, userInfo.Email, userInfo.Username, userInfo.Password, seesAds, (int)userInfo.WeightPlan, userInfo.MainGoals, userInfo.Units, (int)userInfo.Sex, (decimal)userInfo.HeightCm, (DateTime)userInfo.Birthday, (int)userInfo.Weight, (int)userInfo.GoalWeight, (int)userInfo.ActivityLevel, flair, flairColor, isBlackText, userInfo.PictureBGColor, picturePath, userInfo.Title);

        DateTime startDate = new DateTime(1900, 1, 1);
        Dictionary<string, int> nutrientGoals = await CalculateNutrientGoals((int)userInfo.CalorieGoal);
        bool hasOGGoals = await _localData.NutritionGoalDateExists(userID);
        bool hasWaterGoal = await _localData.DoesWaterGoalExist(userID);

        var waterGoal = CalculateWaterGoal(userInfo);

        if (!hasOGGoals)
        {


            await _localData.AddNutritionGoals(userID, startDate, (int)userInfo.CalorieGoal, nutrientGoals["carb"], nutrientGoals["fat"], nutrientGoals["protein"],
                nutrientGoals["satfat"], nutrientGoals["punsatfat"], nutrientGoals["munsatfat"], 0, nutrientGoals["sugar"], nutrientGoals["fiber"],
                nutrientGoals["iron"], nutrientGoals["calcium"], nutrientGoals["potassium"], nutrientGoals["sodium"], nutrientGoals["cholesterol"],
                nutrientGoals["vitaminA"], nutrientGoals["thiamin"], nutrientGoals["riboflavin"], nutrientGoals["niacin"], nutrientGoals["b5"],
                nutrientGoals["b6"], nutrientGoals["biotin"], nutrientGoals["cobalamine"], nutrientGoals["folicacid"],
                nutrientGoals["vitaminC"], nutrientGoals["vitaminD"], nutrientGoals["vitaminE"], nutrientGoals["vitaminK"]);
        }

        if (!hasWaterGoal)
        {
            if (userInfo.WaterGoal == null) userInfo.WaterGoal = 2200;
            await _localData.AddWaterGoal(userID, DateTime.Today, (int)userInfo.WaterGoal);
        }

        var weightEntry = await _localData.DoesWeightEntryExist(userID);

        if (weightEntry == false)
        {
            await _localData.AddWeightEntry(userID, DateTime.Today.AddDays(-1), (decimal)userInfo.Weight, null);
            await _localData.AddWeightEntry(userID, DateTime.Today, (decimal)userInfo.Weight, null);
        }
    }

    async void LoginButton_Clicked(System.Object sender, System.EventArgs e)
    {
        LoginButton.IsVisible = false;
        loadingIndicator.IsRunning = true;
        loadingIndicator.IsVisible = true;
        
        
        if (!string.IsNullOrWhiteSpace(EmailLogin.Text) && !string.IsNullOrWhiteSpace(PasswordLogin.Text))
        {
            var email = EmailLogin.Text.Trim().ToLower();
            var password = PasswordLogin.Text.Trim();

            string salt = await _dataService.GetSaltByEmailAsync(email);


            if (salt != null)
            {
                salt = salt.Replace("\"", "");
                string hashedPassword = HashAndSaltPassword(password, salt);
                Debug.WriteLine(hashedPassword);
                Debug.WriteLine(salt);
                (string success, cVersion) = await _dataService.LoginAsync(email, hashedPassword);
                if (success == "AllClear")
                {

                    AutoLogin(email, hashedPassword);
                    if (_sender != null)
                    {
                        _sender.DashboardAppear();
                    }
                    await Navigation.PopModalAsync();
                    
                    
                }
                else if (success == "Maintenance")
                {
                    Debug.WriteLine("Maintenance");
                    AutoLogin(email, hashedPassword);
                    if (_userID == 12)
                    {
                        await Navigation.PopModalAsync();
                        DisplayAlert("Maintenance", "Maintenance mode is active", "Got it");
                    }
                    else
                    {
                        await Navigation.PushModalAsync(new MaintenancePage());
                    }


                }
                else if (success == "Update")
                {
                    AutoLogin(email, hashedPassword);
                    await Navigation.PushModalAsync(new UpdatePage(cVersion));

                }

                else await DisplayAlert("Notice", "Invalid password", "OK");
            }
            else 
            {
                await DisplayAlert("Notice", "No account found with the specified email address.", "OK");
            }
        }
        else if (string.IsNullOrWhiteSpace(EmailLogin.Text))
        {
            await DisplayAlert("Notice", "Email field is empty", "OK");
        }
        else if (string.IsNullOrWhiteSpace(PasswordLogin.Text))
        {
            await DisplayAlert("Notice", "Password field is empty", "OK");
        }

        loadingIndicator.IsRunning = false;
        loadingIndicator.IsVisible = false;
        LoginButton.IsVisible = true;

    }

    async void RegisterButton_Clicked(System.Object sender, System.EventArgs e)
    { 
        await Navigation.PushModalAsync(new SignUpPage(_localData, _dataService)); 
    }


    public int CalculateWaterGoal(UserInfo userInfo)
    {
        int waterGoal;

        if (userInfo.ActivityLevel == 0)
        {
            waterGoal = (userInfo.Sex == 0) ? (int)userInfo.Weight * 15 : (int)userInfo.Weight * 13;
        }
        else if (userInfo.ActivityLevel == 1)
        {
            waterGoal = (userInfo.Sex == 0) ? (int)userInfo.Weight * 17 : (int)userInfo.Weight * 15;
        }
        else if (userInfo.ActivityLevel == 2)
        {
            waterGoal = (userInfo.Sex == 0) ? (int)userInfo.Weight * 20 : (int)userInfo.Weight * 17;
        }
        else
        {
            waterGoal = (userInfo.Sex == 0) ? (int)userInfo.Weight * 22 : (int)userInfo.Weight * 20;
        }

        Debug.WriteLine($"Water Goal: {waterGoal}");
        
        return waterGoal;
    }

    public static string HashAndSaltPassword(string password, string salt)
    {
        string saltedPassword = password + salt; Debug.WriteLine(saltedPassword);
        var sha256 = SHA256.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword); Debug.WriteLine(passwordBytes);
        byte[] hashedBytes = sha256.ComputeHash(passwordBytes); Debug.WriteLine(hashedBytes);
        string hashedPassword = Convert.ToBase64String(hashedBytes); Debug.WriteLine(hashedPassword);
        hashedPassword = hashedPassword.Replace("+", "");
        hashedPassword = hashedPassword.Replace("/", "");
        return hashedPassword;
    }

    public (double carbsPercent, double fatPercent, double proteinPercent) GetMacronutrientPercentages(int weightPlan)
    {
        switch (weightPlan)
        {
            case -1:
                return (0.5, 0.2, 0.3);
            case 0:
                return (0.5, 0.3, 0.2);
            case 1:
                return (0.5, 0.28, 0.21);
            default:
                return (0.5, 0.3, 0.2); // Default to maintaining weight
        }
    }

    public async Task<Dictionary<string, int>> CalculateNutrientGoals(int calorieGoal)
    {
        var userInfo = await _localData.GetUserAsync(_userID);
        var (carbsPercent, fatPercent, proteinPercent) = GetMacronutrientPercentages(userInfo.WeightPlan);


        // Convert macronutrient calorie goals to gram goals using standard calorie/gram ratios
        int carbGrams = (int)Math.Round((calorieGoal * carbsPercent) / 4.0);
        int fatGrams = (int)Math.Round((calorieGoal * fatPercent) / 9.0);
        int proteinGrams = (int)Math.Round((calorieGoal * proteinPercent) / 4.0);

        // Calculate recommended daily values (RDVs) for micronutrients based on a 2000 calorie diet
        int saturatedFatRDV = (int)Math.Round(20.0 / 2000.0 * calorieGoal);
        int polyunsaturatedFatRDV = (int)Math.Round(22.0 / 2000.0 * calorieGoal);
        int monounsaturatedFatRDV = (int)Math.Round(33.0 / 2000.0 * calorieGoal);
        int sugarRDV = (int)Math.Round(50.0 / 2000.0 * calorieGoal);
        int fiberRDV = (int)Math.Round(28.0 / 2000.0 * calorieGoal);
        int ironRDV = (int)Math.Round(30.0 / 2000.0 * calorieGoal);
        int calciumRDV = (int)Math.Round(1300.0 / 2000.0 * calorieGoal);
        int potassiumRDV = (int)Math.Round(4700.0 / 2000.0 * calorieGoal);
        int sodiumRDV = (int)Math.Round(2300.0 / 2000.0 * calorieGoal);
        int cholesterolRDV = (int)Math.Round(300.0 / 2000.0 * calorieGoal);
        int vitaminARDV = (int)Math.Round(900.0 / 2000.0 * calorieGoal);
        int thiaminRDV = (int)Math.Round(1.2 / 2000.0 * calorieGoal);
        int riboflavinRDV = (int)Math.Round(1.3 / 2000.0 * calorieGoal);
        int niacinRDV = (int)Math.Round(16.0 / 2000.0 * calorieGoal);
        int vitaminB5RDV = (int)Math.Round(5.0 / 2000.0 * calorieGoal);
        int vitaminB6RDV = (int)Math.Round(1.7 / 2000.0 * calorieGoal);
        int biotinRDV = (int)Math.Round(30.0 / 2000.0 * calorieGoal);
        int cobalamineRDV = (int)Math.Round(2.4 / 2000.0 * calorieGoal);
        int folicAcidRDV = (int)Math.Round(400.0 / 2000.0 * calorieGoal);
        int vitaminCRDV = (int)Math.Round(90.0 / 2000.0 * calorieGoal);
        int vitaminDRDV = (int)Math.Round(15.0 / 2000.0 * calorieGoal);
        int vitaminERDV = (int)Math.Round(15.0 / 2000.0 * calorieGoal);
        int vitaminKRDV = (int)Math.Round(120.0 / 2000.0 * calorieGoal);

        // Create dictionary to store nutrient goals and return it
        Dictionary<string, int> nutrientGoals = new Dictionary<string, int>
    {
        { "cal", calorieGoal },
        { "carb", carbGrams },
        { "fat", fatGrams },
        { "protein", proteinGrams },
        { "satfat", saturatedFatRDV },
        { "punsatfat", polyunsaturatedFatRDV },
        { "munsatfat", monounsaturatedFatRDV },
        { "sugar", sugarRDV },
        { "fiber", fiberRDV },
        { "iron", ironRDV },
        { "calcium", calciumRDV },
        { "potassium", potassiumRDV },
        { "sodium", sodiumRDV },
        { "cholesterol", cholesterolRDV },
        { "vitaminA", vitaminARDV },
        { "thiamin", thiaminRDV },
        { "riboflavin", riboflavinRDV },
        { "niacin", niacinRDV },
        { "b5", vitaminB5RDV },
        { "b6", vitaminB6RDV },
        { "biotin", biotinRDV },
        { "cobalamine", cobalamineRDV },
        { "folicacid", folicAcidRDV },
        { "vitaminC", vitaminCRDV },
        { "vitaminD", vitaminDRDV },
        { "vitaminE", vitaminERDV },
        { "vitaminK", vitaminKRDV }
    };
        return nutrientGoals;
    }

    async void ForgotPassword(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPassword(_dataService));
    }

    






}
