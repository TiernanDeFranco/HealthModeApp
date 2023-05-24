using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using HealthModeApp.Pages.Progress;
using static SQLite.SQLite3;

using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using LiveChartsCore;

using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.Defaults;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using LiveChartsCore.Kernel.Sketches;
using System.Text.Json;
using HealthModeApp.Pages.FoodJournalPage;

namespace HealthModeApp.Pages;

public partial class Dashboard : ContentPage
{
    public readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;
    public string success;
    int _userID;
    string _dateFormat;

    public Dashboard(IRestDataService dataService, ISQLiteDataService localData)
    {
        _dataService = dataService;
        _localData = localData;
        Shell.SetTabBarIsVisible(this, false);

        TryLogin();
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var userID = await _localData.GetUserID();

        if (userID != -1)
        {
            var exists = await _localData.DoesWeightEntryExist(userID);
            var userInfo = await _localData.GetUserAsync(userID);
            var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
            var dateFormat = unitList[4];


            switch (dateFormat)
            {
                case "MM/dd/yy":
                    TodaysDate.Text = DateTime.Today.ToString("dddd, MMMM d, yyyy");
                    break;

                case "dd/MM/yy":
                    TodaysDate.Text = DateTime.Today.ToString("dddd, d MMMM, yyyy");
                    break;

                case "yy/MM/dd":
                    TodaysDate.Text = DateTime.Today.ToString("dddd, MMMM d, yyyy");
                    break;
            }

            SeesAds();
            PopulateFoodInfo();
            PopulateMealName();
            if (exists)
            {
                FillGraph();
            }
        }
        

    }

    async void SeesAds()
    {

       

    }

    async void PopulateMealName()
    {
        var mealNames = await _localData.GetMealNames();

        if (mealNames == null || mealNames.Count == 0)
        {
            // Create default meal names
            var defaultMealNames = new List<string> { "Meal 1", "Meal 2", "Meal 3", "Meal 4", "Meal 5", "Meal 6" };

            // Add the default meal names to the MealNames table
            foreach (var name in defaultMealNames)
            {
                var mealName = new MealNames { MealName = name };
                await _localData.AddMealName(mealName);
            }


        }
    }

    async void TryLogin()
    {
        bool doesTableExist = await _localData.DoesUserTableExist();
        string email = null;
        string password = null;
        if (doesTableExist)
        {
            (email, password) = await _localData.GetUserCredentials();
        }
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
            return;
        }
        if (success == "AllClear")
        {
            NavigationPage.SetHasNavigationBar(this, true);
            Debug.WriteLine("AutoLogin");
            int userID = await _dataService.GetUserIDByEmailAsync(email);
            (var userInfo, bool seesAds) = await _dataService.GetUserInfoOnLoginAsync(userID, email, password);
            _userID = userID;
            await _localData.UpdateUserAsync(userID, userInfo.Email, userInfo.Username, userInfo.Password, seesAds, userInfo.WeightPlan, userInfo.MainGoals, userInfo.Units, userInfo.Sex, userInfo.HeightCm, userInfo.Birthday, (decimal)userInfo.Weight, (decimal)userInfo.GoalWeight, userInfo.ActivityLevel);
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


    //Made login and signup nonmodal


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



    public ObservableCollection<DateTimePoint> ChartValues { get; set; }

    public async void FillGraph()
    {

        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[4];
        var weightUnit = unitList[0];

        switch (dateFormat)
        {
            case "MM/dd/yy":
                _dateFormat = "M/d";
                break;

            case "dd/MM/yy":
                _dateFormat = "d/M";
                break;

            case "yy/MM/dd":
                _dateFormat = "M/d";
                break;
        }

        var weights = await _localData.GetWeightsIndex(userID, 0);
        WeightValue.IsVisible = true;
        var weightUnitFormat = "lbs";
        foreach (var weight in weights)
        {
            if (weightUnit == "kg")
            {
                weight.Weight = Math.Round(weight.Weight / (decimal)2.2, 1);
                weightUnitFormat = "kg";
            }
            else
            {
                weightUnitFormat = "lbs";
            }
        }

        ChartValues = new ObservableCollection<DateTimePoint>();
        if (weights.Count > 0)
        {
            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Weight));
            }

            var lastWeight = weights.Last();
            if (weights.All(w => w.Date != DateTime.Today))
            {
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeight.Weight));
            }
            WeightValue.Text = lastWeight.Weight.ToString($"0.# {weightUnitFormat}");
        }
        else
        {
            var weightsAll = await _localData.GetWeightsIndex(userID, 6);
            if (weightsAll.Count > 0)
            {
                var lastWeight = weightsAll.Last();
                WeightValue.Text = lastWeight.ToString();
                ChartValues.Add(new DateTimePoint(lastWeight.Date, (double)lastWeight.Weight));
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeight.Weight));
            }
            else
            {
                WeightValue.IsVisible = false;
            }
        }

        var series = new LineSeries<DateTimePoint>()
        {
            Values = ChartValues,
            LineSmoothness = 0.1f, // Optional: adjust the line smoothness to your liking

            TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):MMMM d}: {chartPoint.PrimaryValue}",
            GeometryFill = new SolidColorPaint(SKColor.Parse("#67ade6")),
            Stroke = new SolidColorPaint(SKColor.Parse("#4b9fe3")) { StrokeThickness = 10 },
            Fill = null,
            GeometryStroke = null,

            GeometrySize = 3
        };

        WeightChart.Series = new ISeries[] { series };

        AppTheme currentTheme = Application.Current.RequestedTheme;
        var colorMode = SKColors.CornflowerBlue;
        switch (currentTheme)
        {
            case AppTheme.Light:
                colorMode = SKColors.DarkSlateGray;
                break;
            case AppTheme.Dark:
                colorMode = SKColors.White;
                break;
            case AppTheme.Unspecified:
                colorMode = SKColors.DarkSlateGray;
                break;

        }

        decimal smallestWeight = (decimal)weights.OrderBy(w => w.Weight).FirstOrDefault()?.Weight;
        decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Weight).FirstOrDefault()?.Weight;

        WeightChart.XAxes = new[]
            {
                new Axis
                {
                     Labeler = value => new DateTime((long) value).ToString($"{_dateFormat}"),
                     UnitWidth = TimeSpan.FromDays(1).Ticks,
                     MinStep = TimeSpan.FromDays(1).Ticks,
                     TextSize = 20,
                     LabelsPaint = new SolidColorPaint(colorMode)



                }
             };

        WeightChart.YAxes = new[]
           {
                new Axis
                {
                     TextSize = 25,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - 5,
                     MaxLimit = (double)largestWeight + 5,
                }
           };



    }

    void WeightProgressClicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new WeightProgress(_dataService, _localData));
    }


    int totalCal;
    int totalCarb;
    int totalFat;
    int totalProtein;
    public LoggedFoodTable food;

    public int goalCal = 0;
    public string energyUnit;
    async void PopulateFoodInfo()
    {
        totalCal = 0;
        totalCarb = 0;
        totalFat = 0;
        totalProtein = 0;
        goalCal = 0;

        var userID = await _localData.GetUserID();


        var foods = new List<LoggedFoodTable>();

        var loggedFoods = await _localData.GetLoggedFoods(userID, DateTime.Today);
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        energyUnit = unitList[2];



        var mealFoodIDs = loggedFoods.Select(f => f.LoggedFoodID).ToList();

        foreach (int foodID in mealFoodIDs)
        {
            food = await _localData.GetLoggedFoodDetails(userID, foodID);

            foods.Add(food);


        }

        var nutritionGoals = await _localData.GetNutritionGoals(userID, DateTime.Today);
        if (nutritionGoals != null)
        {
            switch (energyUnit)
            {
                case "kCal":
                    goalCal = nutritionGoals.CalorieGoal;
                    break;
                case "cal":
                    goalCal = nutritionGoals.CalorieGoal;
                    break;
                case "kJ":
                    goalCal = (int)Math.Round(nutritionGoals.CalorieGoal * 4.184);
                    break;
            }

            foreach (var food in foods)
            {
                totalCal += (int)Math.Round((decimal)food.Calories);
                totalCarb += (int)Math.Round((decimal)food.Carbs);
                totalFat += (int)Math.Round((decimal)food.Fat);
                totalProtein += (int)Math.Round((decimal)food.Protein);
            }


            var (carbPercent, fatPercent, proteinPercent) = CalculateMacrosPercentage(totalCal, totalCarb, totalFat, totalProtein);

            switch (energyUnit)
            {
                case "kCal":
                    totalCal = ((int)Math.Round((decimal)totalCal));
                    break;
                case "cal":
                    totalCal = ((int)Math.Round((decimal)totalCal));
                    break;
                case "kJ":
                    totalCal = ((int)Math.Round((decimal)totalCal * (decimal)4.184));
                    break;
            }
            double frameWidth;
            double innerRadius;

            frameWidth = DeviceDisplay.MainDisplayInfo.Width * .35;


            innerRadius = frameWidth * .5;
            if (innerRadius >= 170) innerRadius = frameWidth * .52;
            if (innerRadius < 170 && innerRadius >= 150) innerRadius = frameWidth * .38;
            if (frameWidth < 150) innerRadius = frameWidth * .3;

            Debug.WriteLine(frameWidth);
            PieChart.WidthRequest = frameWidth *.5;
            PieChart.HeightRequest = frameWidth *.5;

            ISeries[] SeriesCollection = new ISeries[]
                {


                    new PieSeries<double>
                    {
                        Values = new List<double> { carbPercent },
                        Name = "Carbohydrate %",
                        Fill = new SolidColorPaint(SKColor.Parse("#F5A623")),
                        InnerRadius = innerRadius

                    },
                    new PieSeries<double>
                    {
                        Values = new List<double> { fatPercent },
                        Name = "Fat %",
                        Fill = new SolidColorPaint(SKColor.Parse("#4A90E2")),
                        InnerRadius = innerRadius
                    },
                     new PieSeries<double>
                    {
                        Values = new List<double> { proteinPercent },
                        Name = "Protein %",
                        Fill = new SolidColorPaint(SKColor.Parse("#7ED321")),
                        InnerRadius = innerRadius
                    },




                };

            RemainingCalLabel.Text = (goalCal - totalCal).ToString("0\nRemaining");
            CalorieLabel.Text = totalCal.ToString();

            if (totalCal < 100) { CalorieLabel.FontSize = 32; CalIcon.WidthRequest = 30; CalIcon.HeightRequest = 30; }
            else if (totalCal < 1000 && totalCal >= 100) { CalorieLabel.FontSize = 30; CalIcon.WidthRequest = 25; CalIcon.HeightRequest = 25; }
            else if (totalCal < 10000 && totalCal >= 1000) { CalorieLabel.FontSize = 28; CalIcon.WidthRequest = 23; CalIcon.HeightRequest = 23; }
            else if (totalCal >= 10000) { CalorieLabel.FontSize = 25; CalIcon.WidthRequest = 21; CalIcon.HeightRequest = 21; }

            PieChart.InitialRotation = 80;
            PieChart.Series = SeriesCollection;

            CalorieBar.ProgressTo(((double)totalCal / goalCal) + .02, 1200, Easing.CubicOut);

            CalorieBar.WidthRequest = frameWidth / 5;
        }


    }

    public static (double carbPercent, double fatPercent, double proteinPercent) CalculateMacrosPercentage(int totalCalories = 0, int totalCarbs = 0, int totalFat = 0, int totalProtein = 0)
    {
        // Calculate the percentage of calories from each macro
        double carbCalories = totalCarbs * 4;
        double proteinCalories = totalProtein * 4;
        double fatCalories = totalFat * 9;
        double carbPercentage = carbCalories / totalCalories * 100;
        double proteinPercentage = proteinCalories / totalCalories * 100;
        double fatPercentage = fatCalories / totalCalories * 100;

        // Return a Tuple containing the macro percentages
        return (carbPercentage, fatPercentage, proteinPercentage);
    }

    void OpenNutritionBreakdown(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new NutritionalBreakdown(_localData, DateTime.Today));
    }

    void LogFoodClicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new FoodSearch(_localData, _dataService, DateTime.Today));

    }
}
