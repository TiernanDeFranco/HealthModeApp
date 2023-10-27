using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class NutritionGoalsPage : ContentPage
{
    private readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;
    public NutritionGoalsPage(ISQLiteDataService localData, IRestDataService dataService)
    {
        InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
        _localData = localData;
        _dataService = dataService;
        
        CalorieGoalEntry.Text = "0";
        SetDefaults();


    }

    async void SetDefaults()
    {
        var userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var goals = await _localData.GetNutritionGoals(userID, DateTime.Today);
        var calGoal = goals.CalorieGoal;

        string foodUnit = await _localData.GetFoodEnergyUnit();
        switch (foodUnit)
        {
            case "kCal":
                break;

            case "cal":
                break;

            case "kJ":
                calGoal = (int)Math.Round(calGoal * 4.184);
                break;
        }

        int calorieGoal = calGoal;
        int carbGrams = goals.CarbGoal;
        int fatGrams = goals.FatGoal;
        int proteinGrams = goals.ProteinGoal;
        int totalGrams = carbGrams + fatGrams + proteinGrams;

        double carbCalories = carbGrams * 4.0; // 4 calories per gram of carb
        double fatCalories = fatGrams * 9.0; // 9 calories per gram of fat
        double proteinCalories = proteinGrams * 4.0; // 4 calories per gram of protein
        double totalCalories = carbCalories + fatCalories + proteinCalories;

        double carbPercent = carbCalories / totalCalories;
        double fatPercent = fatCalories / totalCalories;
        double proteinPercent = proteinCalories / totalCalories;

        int carbIndex = (int)Math.Round(carbPercent * 100);
        int fatIndex = (int)Math.Round(fatPercent * 100);
        int proteinIndex = (int)Math.Round(proteinPercent * 100);

        
        await Task.Delay(100);
        CalorieGoalEntry.Text = calGoal.ToString();
        

        
    }


    int _userID;
    int carbPercent;
    int fatPercent;
    int proteinPercent;
   

    public (int carbs, int fat, int protein) ConvertToGrams(int carbPercent, int fatPercent, int proteinPercent, int calories)
    {
        int carbCal = (carbPercent * calories) / 100;
        int fatCal = (fatPercent * calories) / 100;
        int proteinCal = (proteinPercent * calories) / 100;

        int carbGrams = carbCal / 4;
        int fatGrams = fatCal / 9;
        int proteinGrams = proteinCal / 4;

        return (carbGrams, fatGrams, proteinGrams);
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
                return (0.5, 0.3, 0.2);
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




   


}
