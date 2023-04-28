using System.Diagnostics;
using System.Text.RegularExpressions;
using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages.FoodJournalPage;


public partial class FoodInfo : ContentPage
{
    public readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;
    public NutritionModel _food;
    public DateTime _date;
    public string _servingUnit;

    int _mealType;
    decimal servingSizeSelected;
    List<ServingSizeOption> servingSizeOptions;

    public FoodInfo(IRestDataService dataService, ISQLiteDataService localData, NutritionModel food, int mealType, DateTime date)
    {
        InitializeComponent();
        TimeSelect.Time = DateTime.Now.TimeOfDay;
        _localData = localData;
        _dataService = dataService;
        _mealType = mealType - 1;
        _food = food;
        _date = date;
        _servingUnit = _food.ServingUnit;


        switch (_servingUnit)
        {
            case "g":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize, Description = $"{Convert.ToInt32(_food.ServingSize)}g" },
                    new ServingSizeOption { Grams = 1, Description = "1g" }
                };
                break;
            case "mL":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize, Description = $"{Convert.ToInt32(_food.ServingSize)}mL" },
                    new ServingSizeOption { Grams = (decimal)food.Grams/food.ServingSize, Description = "1mL" },

                    new ServingSizeOption { Grams = (decimal)food.Grams, Description = $"{Convert.ToInt32(_food.Grams)}g" },
                    new ServingSizeOption { Grams = 1, Description = "1g" }
                };
                break;
            case "oz":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize*(decimal)28.3495, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize*(decimal)28.3495, Description = $"{Convert.ToInt32(_food.ServingSize)}oz" },
                    new ServingSizeOption { Grams = (decimal)28.3495, Description = "1oz" },

                    new ServingSizeOption { Grams = _food.ServingSize*(decimal)28.3495, Description = $"{Convert.ToInt32(_food.ServingSize*(decimal)28.3495)}g" },
                    new ServingSizeOption { Grams = 1, Description = "1g" }
                };
                break;
        }

        ServingSizePicker.ItemsSource = servingSizeOptions;
        ServingSizePicker.ItemDisplayBinding = new Binding("Description");
        ServingSizePicker.SelectedIndex = 0;

        PopulateFoodInfo(_food);
        GetMealOptions();  
    }


   

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);
        SeesAds();
    }

    async void SeesAds()
    {
        Ad.IsVisible = await _localData.GetSeesAds();

    }

    public class ServingSizeOption
    {
        public decimal Grams { get; set; }
        public string Description { get; set; }
    }

    decimal _factor = 1;

     async void PopulateFoodInfo(NutritionModel food)
    {
        int userID = await _localData.GetUserID();
        var loggedFoods = await _localData.GetLoggedFoods(userID, _date);

        // Initialize the totals to zero
        decimal loggedCalories = 0;
        decimal loggedCarbs = 0;
        decimal loggedFat = 0;
        decimal loggedProtein = 0;

        foreach (var foodItem in loggedFoods)
        {
            // Add the calories, carbs, fat, and protein of the current food item to the totals
            loggedCalories += foodItem.Calories;
            loggedCarbs += foodItem.Carbs;
            loggedFat += foodItem.Fat;
            loggedProtein += foodItem.Protein;
        }


        var selectedOption = ServingSizePicker.SelectedItem as ServingSizeOption;
        if (selectedOption != null)
        {
            servingSizeSelected = selectedOption.Grams;
        }


       var numberOfServings = decimal.Parse(ServingNumberEntry.Text);
       var totalGrams = servingSizeSelected * numberOfServings;

        var servingRatio = totalGrams / _food.ServingSize;
        var totalCalories = servingRatio * _food.Calories;
        var totalCarbs = servingRatio * _food.Carbs;
        var totalFat = servingRatio * _food.Fat;
        var totalProtein = servingRatio * _food.Protein;

        var totalSugar = servingRatio * _food.Sugar;
        var totalAddedSugar = servingRatio * _food.AddSugar;
        var totalSugarAlc = servingRatio * _food.SugarAlc;
        var totalFiber = servingRatio * _food.Fiber;
        var totalNetCarb = servingRatio * _food.NetCarb;

        var totalSatFat = servingRatio * _food.SatFat;
        var totalPUnSatFat = servingRatio * _food.PUnSatFat;
        var totalMUnSatFat = servingRatio * _food.MUnSatFat;
        var totalTransFat = servingRatio * _food.TransFat;

        var totalIron = servingRatio * _food.Iron;
        var totalCalcium = servingRatio * _food.Calcium;
        var totalPotassium = servingRatio * _food.Potassium;
        var totalSodium = servingRatio * _food.Sodium;
        var totalCholesterol = servingRatio * _food.Cholesterol;

        var totalVitaminA = servingRatio * _food.VitaminA;
        var totalThiamin = servingRatio * _food.Thiamin;
        var totalRiboflavin = servingRatio * _food.Riboflavin;
        var totalNiacin = servingRatio * _food.Niacin;
        var totalB5 = servingRatio * _food.B5;
        var totalB6 = servingRatio * _food.B6;
        var totalBiotin = servingRatio * _food.B7;
        var totalFolicAcid = servingRatio * _food.FolicAcid;
        var totalCobalamin = servingRatio * _food.B12;
        var totalVitaminC = servingRatio * _food.VitaminC;
        var totalVitaminD = servingRatio * _food.VitaminD;
        var totalVitaminE = servingRatio * _food.VitaminE;
        var totalVitaminK = servingRatio * _food.VitaminK;


        FoodName.Text = food.FoodName;
        BrandName.Text = food.Brand;

        string foodUnit = await _localData.GetFoodEnergyUnit();

        Debug.WriteLine(foodUnit);

       


    

        CarbLabel.Text = Math.Round(totalCarbs, 1).ToString("0.#" + "g");
        FatLabel.Text = Math.Round(totalFat, 1).ToString("0.#" + "g");
        ProteinLabel.Text = Math.Round(totalProtein, 1).ToString("0.#" + "g");

        decimal carbPercent = (totalCarbs * 4 / totalCalories) * 100;
        decimal fatPercent = (totalFat * 9 / totalCalories) * 100;
        decimal proteinPercent = (totalProtein * 4 / totalCalories) * 100;

        switch (foodUnit)
        {
            case "kCal":
                _factor = 1;
                break;

            case "cal":
                _factor = 1;
                break;

            case "kJ":
                totalCalories *= (decimal)4.184;
                _factor = (decimal)4.184;
                break;
        }

        CalorieLabel.Text = Math.Round(totalCalories).ToString();

        CarbPercent.Text = Math.Round(carbPercent, 1).ToString("0.#") + "%";
        FatPercent.Text = Math.Round(fatPercent, 1).ToString("0.#") + "%";
        ProteinPercent.Text = Math.Round(proteinPercent, 1).ToString("0.#") + "%";

        TotalCarbs.Text = CarbLabel.Text;
        SugarLabel.Text = (totalSugar.HasValue) ? totalSugar.Value.ToString("0.#g") : "-";
        AddedSugarLabel.Text = (totalAddedSugar.HasValue) ? totalAddedSugar.Value.ToString("0.#g") : "-";
        SugarAlcLabel.Text = (totalSugarAlc.HasValue) ? totalSugarAlc.Value.ToString("0.#g") : "-";
        FiberLabel.Text = (totalFiber.HasValue) ? totalFiber.Value.ToString("0.#g") : "-";
        NetCarbLabel.Text = (totalNetCarb.HasValue) ? totalNetCarb.Value.ToString("0.#g") : "-";

        TotalFat.Text = FatLabel.Text;
        SatFatLabel.Text = totalSatFat.HasValue ? totalSatFat.Value.ToString("0.#g") : "-";
        PUnSatFatLabel.Text = totalPUnSatFat.HasValue ? totalPUnSatFat.Value.ToString("0.#g") : "-";
        MUnSatFatLabel.Text = totalMUnSatFat.HasValue ? totalMUnSatFat.Value.ToString("0.#g") : "-";
        TransFatLabel.Text = totalTransFat.HasValue ? totalTransFat.Value.ToString("0.#g") : "-";

        TotalIron.Text = totalIron.HasValue ? totalIron.Value.ToString("0.#mg") : "-";
        CalciumLabel.Text = totalCalcium.HasValue ? totalCalcium.Value.ToString("0.#mg") : "-";
        PotassiumLabel.Text = totalPotassium.HasValue ? totalPotassium.Value.ToString("0.#mg") : "-";
        SodiumLabel.Text = totalSodium.HasValue ? totalSodium.Value.ToString("0.#mg") : "-";
        CholesterolLabel.Text = totalCholesterol.HasValue ? totalCholesterol.Value.ToString("0.#mg") : "-";

        VitaminALabel.Text = totalVitaminA.HasValue ? totalVitaminA.Value.ToString("0.#") + "µg" : "-";
        ThiaminLabel.Text = totalThiamin.HasValue ? totalThiamin.Value.ToString("0.#") + "mg" : "-";
        RiboflavinLabel.Text = totalRiboflavin.HasValue ? totalRiboflavin.Value.ToString("0.#") + "mg" : "-";
        NiacinLabel.Text = totalNiacin.HasValue ? totalNiacin.Value.ToString("0.#") + "mg" : "-";
        B5Label.Text = totalB5.HasValue ? totalB5.Value.ToString("0.#") + "mg" : "-";
        B6Label.Text = totalB6.HasValue ? totalB6.Value.ToString("0.#") + "mg" : "-";
        BiotinLabel.Text = totalBiotin.HasValue ? totalBiotin.Value.ToString("0.#") + "µg" : "-";
        FolateLabel.Text = totalFolicAcid.HasValue ? totalFolicAcid.Value.ToString("0.#") + "µg" : "-";
        B12Label.Text = totalCobalamin.HasValue ? totalCobalamin.Value.ToString("0.#") + "µg" : "-";
        VitaminCLabel.Text = totalVitaminC.HasValue ? totalVitaminC.Value.ToString("0.#") + "mg" : "-";
        VitaminDLabel.Text = totalVitaminD.HasValue ? totalVitaminD.Value.ToString("0.#") + "µg" : "-";
        VitaminELabel.Text = totalVitaminE.HasValue ? totalVitaminE.Value.ToString("0.#") + "mg" : "-";
        VitaminKLabel.Text = totalVitaminK.HasValue ? totalVitaminK.Value.ToString("0.#") + "µg" : "-";



        var goals = await _localData.GetNutritionGoals(userID, _date);

        


        CalorieBar.ProgressTo((double)(loggedCalories + totalCalories / (goals.CalorieGoal * _factor)), 1500, Easing.CubicOut);
        CarbBar.ProgressTo((double)(loggedCarbs + totalCarbs / (goals.CarbGoal)), 1500, Easing.CubicOut);
        FatBar.ProgressTo((double)(loggedFat + totalFat / (goals.FatGoal)), 1500, Easing.CubicOut);
        ProteinBar.ProgressTo((double)(loggedProtein + totalProtein / goals.ProteinGoal), 1500, Easing.CubicOut);

        CalorieGoal.Text = (loggedCalories + totalCalories / (goals.CalorieGoal *_factor)).ToString("0.#" + "%");
        CarbGoal.Text = (loggedCarbs + totalCarbs / (goals.CarbGoal)).ToString("0.#" + "%");
        FatGoal.Text = (loggedFat + totalFat / goals.FatGoal).ToString("0.#" + "%");
        ProteinGoal.Text = (loggedProtein + totalProtein / (goals.ProteinGoal)).ToString("0.#" + "%");


    }

    async void GetMealOptions()
    {
        var mealNames = await _localData.GetMealNames();
        MealPicker.ItemsSource = mealNames;
        MealPicker.SelectedIndex = _mealType;

    }


    void ServingSizeChanged(System.Object sender, System.EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServingNumberEntry.Text))
        {
            ServingNumberEntry.Text = "1";
        }
        else
        {
           PopulateFoodInfo(_food);
        }
         
    }



    void ServingNumberChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(ServingNumberEntry.Text) && decimal.TryParse(ServingNumberEntry.Text, out decimal numberOfServings) && numberOfServings > 0)
        { 
            PopulateFoodInfo(_food);  
        }
        else if (ServingNumberEntry.Text == "0")
        {
            ServingNumberEntry.Text = "1";
            DisplayAlert("Notice", "You can't have 0 servings", "OK");
        }

    }
}
