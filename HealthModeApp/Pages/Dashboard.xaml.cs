using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using static SQLite.SQLite3;

namespace HealthModeApp.Pages;

public partial class Dashboard : ContentPage
{
    public readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;
    public string success;
    int _userID;
    public Dashboard(IRestDataService dataService, ISQLiteDataService localData)
    {
        _dataService = dataService;
        _localData = localData;
        Shell.SetTabBarIsVisible(this, false);

        TryLogin();
        InitializeComponent();


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
            Debug.WriteLine("1st Else");
            await Navigation.PopAsync();
            await Navigation.PushModalAsync(new LoginPage(_dataService, _localData));
            LoadingPage.IsVisible = false;
            dashboardGrid.IsVisible = true;
            Shell.SetTabBarIsVisible(this, true);
            try
            {
                await _localData.DeleteUser();
            }
            catch
            {
                Debug.WriteLine("No User Table");
            }
            return;
        }
        if (success == "AllClear")
        {
            Debug.WriteLine("AutoLogin");
            int userID = await _dataService.GetUserIDByEmailAsync(email);
            (var userInfo, bool seesAds) = await _dataService.GetUserInfoOnLoginAsync(userID);
            _userID = userID;
            await _localData.UpdateUserAsync(userID, userInfo.Email, userInfo.Username, userInfo.Password, seesAds, userInfo.WeightPlan, userInfo.MainGoals, userInfo.Units, userInfo.Sex, userInfo.HeightCm, userInfo.Birthday, userInfo.Weight, userInfo.GoalWeight, userInfo.ActivityLevel);
            Dictionary<string, int> nutrientGoals = await CalculateNutrientGoals((int)userInfo.CalorieGoal);
            await _localData.UpdateNutritionGoals(userID, DateTime.Today, userInfo.CalorieGoal, null, null, null,
                            nutrientGoals["satfat"], nutrientGoals["punsatfat"], nutrientGoals["munsatfat"], 0, nutrientGoals["sugar"],
                            nutrientGoals["iron"], nutrientGoals["calcium"], nutrientGoals["potassium"], nutrientGoals["sodium"], nutrientGoals["cholesterol"],
                            nutrientGoals["vitaminA"], nutrientGoals["thiamin"], nutrientGoals["riboflavin"], nutrientGoals["niacin"], nutrientGoals["b5"],
                            nutrientGoals["b6"], nutrientGoals["biotin"], nutrientGoals["cobalamine"], nutrientGoals["folicacid"],
                            nutrientGoals["vitaminC"], nutrientGoals["vitaminD"], nutrientGoals["vitaminE"], nutrientGoals["vitaminK"]);

            LoadingPage.IsVisible = false;
            dashboardGrid.IsVisible = true;
            Shell.SetTabBarIsVisible(this, true);

        }
        else
        {
            Debug.WriteLine("2nd Else");
            await Navigation.PopAsync();
            await Navigation.PushModalAsync(new LoginPage(_dataService, _localData));
            LoadingPage.IsVisible = false;
            dashboardGrid.IsVisible = true;
            Shell.SetTabBarIsVisible(this, true);
            try
            {
                await _localData.DeleteUser();
            }
            catch
            {
                Debug.WriteLine("No User Table");
            }
            return;
        }



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

}
