using System.Diagnostics;
using System.Text.RegularExpressions;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.FoodJournalPage;


public partial class FoodInfo : ContentPage
{
    public readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;
    public NutritionModel _food;
    public CustomFoods _customFood;
    public DateTime _date;
    public string _servingUnit;
    public bool _custom;
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
        _servingUnit = food.ServingUnit;
        _custom = false;


        switch (_servingUnit)
        {
            case "g":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize, Description = $"{Convert.ToInt32(food.ServingSize)}g" },
                    new ServingSizeOption { Grams = 1, Description = "1g" }
                };
                break;
            case "mL":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize, Description = $"{Convert.ToInt32(food.ServingSize)}mL" },
                    new ServingSizeOption { Grams = (decimal)food.Grams/food.ServingSize, Description = "1mL" },

                    new ServingSizeOption { Grams = (decimal)food.Grams, Description = $"{Convert.ToInt32(food.Grams)}g" },
                    new ServingSizeOption { Grams = 1, Description = "1g" }
                };
                break;
            case "oz":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize*(decimal)28.3495, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize*(decimal)28.3495, Description = $"{Convert.ToInt32(food.ServingSize)}oz" },
                    new ServingSizeOption { Grams = (decimal)28.3495, Description = "1oz" },

                    new ServingSizeOption { Grams = food.ServingSize*(decimal)28.3495, Description = $"{Convert.ToInt32(food.ServingSize*(decimal)28.3495)}g" },
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

    public FoodInfo(IRestDataService dataService, ISQLiteDataService localData, CustomFoods food, int mealType, DateTime date)
    {
        InitializeComponent();
        TimeSelect.Time = DateTime.Now.TimeOfDay;
        _localData = localData;
        _dataService = dataService;
        
        _mealType = mealType - 1;
        _customFood = food;
        _date = date;
        _servingUnit = food.ServingUnit;
        _custom = true;

        switch (_servingUnit)
        {
            case "g":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize, Description = $"{Convert.ToInt32(food.ServingSize)}g" },
                    new ServingSizeOption { Grams = 1, Description = "1g" }
                };
                break;
            case "mL":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize, Description = $"{Convert.ToInt32(food.ServingSize)}mL" },
                    new ServingSizeOption { Grams = (decimal)food.Grams/food.ServingSize, Description = "1mL" },

                    new ServingSizeOption { Grams = (decimal)food.Grams, Description = $"{Convert.ToInt32(food.Grams)}g" },
                    new ServingSizeOption { Grams = 1, Description = "1g" }
                };
                break;
            case "oz":
                servingSizeOptions = new List<ServingSizeOption>
                {
                    new ServingSizeOption { Grams = food.ServingSize*(decimal)28.3495, Description = food.ServingName },
                    new ServingSizeOption { Grams = food.ServingSize*(decimal)28.3495, Description = $"{Convert.ToInt32(food.ServingSize)}oz" },
                    new ServingSizeOption { Grams = (decimal)28.3495, Description = "1oz" },

                    new ServingSizeOption { Grams = food.ServingSize*(decimal)28.3495, Description = $"{Convert.ToInt32(food.ServingSize*(decimal)28.3495)}g" },
                    new ServingSizeOption { Grams = 1, Description = "1g" }
                };
                break;
        }

        ServingSizePicker.ItemsSource = servingSizeOptions;
        ServingSizePicker.ItemDisplayBinding = new Binding("Description");
        ServingSizePicker.SelectedIndex = 0;

        PopulateCustomFoodInfo(_customFood);
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


    decimal numberOfServings;
    decimal totalGrams;


    decimal totalCalories;
    decimal totalCarbs;
    decimal totalFat;
    decimal totalProtein;

    decimal? totalSugar;
    decimal? totalAddedSugar;
    decimal? totalSugarAlc;
    decimal? totalFiber;
    decimal? totalNetCarb;

    decimal? totalSatFat;
    decimal? totalPUnSatFat;
    decimal? totalMUnSatFat;
    decimal? totalTransFat;

    decimal? totalIron;
    decimal? totalCalcium;
    decimal? totalPotassium;
    decimal? totalSodium;
    decimal? totalCholesterol;

    decimal? totalVitaminA;
    decimal? totalThiamin;
    decimal? totalRiboflavin;
    decimal? totalNiacin;
    decimal? totalB5;
    decimal? totalB6;
    decimal? totalBiotin;
    decimal? totalFolicAcid;
    decimal? totalCobalamin;
    decimal? totalVitaminC;
    decimal? totalVitaminD;
    decimal? totalVitaminE;
    decimal? totalVitaminK;

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
            loggedCalories += (decimal)foodItem.Calories;
            loggedCarbs += (decimal)foodItem.Carbs;
            loggedFat += (decimal)foodItem.Fat;
            loggedProtein += (decimal)foodItem.Protein;
        }


        var selectedOption = ServingSizePicker.SelectedItem as ServingSizeOption;
        if (selectedOption != null)
        {
            servingSizeSelected = selectedOption.Grams;
        }


       numberOfServings = decimal.Parse(ServingNumberEntry.Text);
       totalGrams = servingSizeSelected * numberOfServings;

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

        decimal carbPercent = 0;
        decimal fatPercent = 0;
        decimal proteinPercent = 0;

        if (totalCalories == 0)
        {
            carbPercent = 0;
            fatPercent = 0;
            proteinPercent = 0;
        }
        else
        {
            carbPercent = (totalCarbs * 4 / totalCalories) * 100;
            fatPercent = (totalFat * 9 / totalCalories) * 100;
            proteinPercent = (totalProtein * 4 / totalCalories) * 100;
        }

        switch (foodUnit)
        {
            case "kCal":
                _factor = 1;
                break;

            case "cal":
                _factor = 1;
                break;

            case "kJ":
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

        

        
        CalorieBar.ProgressTo((double)(((loggedCalories * _factor) + totalCalories) / (goals.CalorieGoal * _factor)), 1500, Easing.CubicOut);
        CarbBar.ProgressTo((double)((loggedCarbs + totalCarbs) / (goals.CarbGoal)), 1500, Easing.CubicOut);
        FatBar.ProgressTo((double)((loggedFat + totalFat) / (goals.FatGoal)), 1500, Easing.CubicOut);
        ProteinBar.ProgressTo((double)((loggedProtein + totalProtein) / goals.ProteinGoal), 1500, Easing.CubicOut);

        CalorieGoal.Text = (((loggedCalories * _factor) + totalCalories) / (goals.CalorieGoal * _factor)).ToString("0.#" + "%");
        CarbGoal.Text = ((loggedCarbs + totalCarbs) / (goals.CarbGoal)).ToString("0.#" + "%");
        FatGoal.Text = ((loggedFat + totalFat) / goals.FatGoal).ToString("0.#" + "%");
        ProteinGoal.Text = ((loggedProtein + totalProtein) / (goals.ProteinGoal)).ToString("0.#" + "%");


    }

    async void PopulateCustomFoodInfo(CustomFoods food)
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
            loggedCalories += (decimal)foodItem.Calories;
            loggedCarbs += (decimal)foodItem.Carbs;
            loggedFat += (decimal)foodItem.Fat;
            loggedProtein += (decimal)foodItem.Protein;
        }


        var selectedOption = ServingSizePicker.SelectedItem as ServingSizeOption;
        if (selectedOption != null)
        {
            servingSizeSelected = selectedOption.Grams;
        }


        numberOfServings = decimal.Parse(ServingNumberEntry.Text);
        totalGrams = servingSizeSelected * numberOfServings;

        var servingRatio = totalGrams / _customFood.ServingSize;
        var totalCalories = servingRatio * _customFood.Calories;
        var totalCarbs = servingRatio * _customFood.Carbs;
        var totalFat = servingRatio * _customFood.Fat;
        var totalProtein = servingRatio * _customFood.Protein;

        var totalSugar = servingRatio * _customFood.Sugar;
        var totalAddedSugar = servingRatio * _customFood.AddSugar;
        var totalSugarAlc = servingRatio * _customFood.SugarAlc;
        var totalFiber = servingRatio * _customFood.Fiber;
        var totalNetCarb = servingRatio * _customFood.NetCarb;

        var totalSatFat = servingRatio * _customFood.SatFat;
        var totalPUnSatFat = servingRatio * _customFood.PUnSatFat;
        var totalMUnSatFat = servingRatio * _customFood.MUnSatFat;
        var totalTransFat = servingRatio * _customFood.TransFat;

        var totalIron = servingRatio * _customFood.Iron;
        var totalCalcium = servingRatio * _customFood.Calcium;
        var totalPotassium = servingRatio * _customFood.Potassium;
        var totalSodium = servingRatio * _customFood.Sodium;
        var totalCholesterol = servingRatio * _customFood.Cholesterol;

        var totalVitaminA = servingRatio * _customFood.VitaminA;
        var totalThiamin = servingRatio * _customFood.Thiamin;
        var totalRiboflavin = servingRatio * _customFood.Riboflavin;
        var totalNiacin = servingRatio * _customFood.Niacin;
        var totalB5 = servingRatio * _customFood.B5;
        var totalB6 = servingRatio * _customFood.B6;
        var totalBiotin = servingRatio * _customFood.B7;
        var totalFolicAcid = servingRatio * _customFood.FolicAcid;
        var totalCobalamin = servingRatio * _customFood.B12;
        var totalVitaminC = servingRatio * _customFood.VitaminC;
        var totalVitaminD = servingRatio * _customFood.VitaminD;
        var totalVitaminE = servingRatio * _customFood.VitaminE;
        var totalVitaminK = servingRatio * _customFood.VitaminK;


        FoodName.Text = food.FoodName;
        BrandName.Text = food.Brand;

        string foodUnit = await _localData.GetFoodEnergyUnit();

        Debug.WriteLine(foodUnit);






        CarbLabel.Text = Math.Round(totalCarbs, 1).ToString("0.#" + "g");
        FatLabel.Text = Math.Round(totalFat, 1).ToString("0.#" + "g");
        ProteinLabel.Text = Math.Round(totalProtein, 1).ToString("0.#" + "g");

        decimal carbPercent = 0;
        decimal fatPercent = 0;
        decimal proteinPercent = 0;

        if (totalCalories == 0)
        {
            carbPercent = 0;
            fatPercent = 0;
            proteinPercent = 0;
        }
        else
        {
            carbPercent = (totalCarbs * 4 / totalCalories) * 100;
            fatPercent = (totalFat * 9 / totalCalories) * 100;
            proteinPercent = (totalProtein * 4 / totalCalories) * 100;
        }

        switch (foodUnit)
        {
            case "kCal":
                _factor = 1;
                break;

            case "cal":
                _factor = 1;
                break;

            case "kJ":
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




        CalorieBar.ProgressTo((double)(((loggedCalories * _factor) + totalCalories) / (goals.CalorieGoal * _factor)), 1500, Easing.CubicOut);
        CarbBar.ProgressTo((double)((loggedCarbs + totalCarbs) / (goals.CarbGoal)), 1500, Easing.CubicOut);
        FatBar.ProgressTo((double)((loggedFat + totalFat) / (goals.FatGoal)), 1500, Easing.CubicOut);
        ProteinBar.ProgressTo((double)((loggedProtein + totalProtein) / goals.ProteinGoal), 1500, Easing.CubicOut);

        CalorieGoal.Text = (((loggedCalories * _factor) + totalCalories) / (goals.CalorieGoal * _factor)).ToString("0.#" + "%");
        CarbGoal.Text = ((loggedCarbs + totalCarbs) / (goals.CarbGoal)).ToString("0.#" + "%");
        FatGoal.Text = ((loggedFat + totalFat) / goals.FatGoal).ToString("0.#" + "%");
        ProteinGoal.Text = ((loggedProtein + totalProtein) / (goals.ProteinGoal)).ToString("0.#" + "%");


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
            if (_custom)
            {
                PopulateCustomFoodInfo(_customFood);
            }
            else
            {
                PopulateFoodInfo(_food);
            }
        }
         
    }



    void ServingNumberChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(ServingNumberEntry.Text) && decimal.TryParse(ServingNumberEntry.Text, out decimal numberOfServings) && numberOfServings >= 0 && ServingNumberEntry.Text != ".")
        {
            if (_custom)
            {
                PopulateCustomFoodInfo(_customFood);
            }
            else
            {
                PopulateFoodInfo(_food);
            }
        }
        

    }

    async void LogFoodClicked(System.Object sender, System.EventArgs e)
    {
        if (_custom)
        {
            numberOfServings = decimal.Parse(ServingNumberEntry.Text);
            totalGrams = servingSizeSelected * numberOfServings;
            var servingRatio = totalGrams / _customFood.ServingSize;
            totalCalories = (servingRatio * _customFood.Calories);
            totalCarbs = (servingRatio * _customFood.Carbs);
            totalFat = (servingRatio * _customFood.Fat);
            totalProtein = (servingRatio * _customFood.Protein);

            totalSugar = (servingRatio * _customFood.Sugar);
            totalAddedSugar = (servingRatio * _customFood.AddSugar);
            totalSugarAlc = (servingRatio * _customFood.SugarAlc);
            totalFiber = (servingRatio * _customFood.Fiber);
            totalNetCarb = (servingRatio * _customFood.NetCarb);

            totalSatFat = (servingRatio * _customFood.SatFat);
            totalPUnSatFat = (servingRatio * _customFood.PUnSatFat);
            totalMUnSatFat = (servingRatio * _customFood.MUnSatFat);
            totalTransFat = (servingRatio * _customFood.TransFat);

            totalIron = (servingRatio * _customFood.Iron);
            totalCalcium = (servingRatio * _customFood.Calcium);
            totalPotassium = (servingRatio * _customFood.Potassium);
            totalSodium = (servingRatio * _customFood.Sodium);
            totalCholesterol = (servingRatio * _customFood.Cholesterol);

            totalVitaminA = (servingRatio * _customFood.VitaminA);
            totalThiamin = (servingRatio * _customFood.Thiamin);
            totalRiboflavin = (servingRatio * _customFood.Riboflavin);
            totalNiacin = (servingRatio * _customFood.Niacin);
            totalB5 = (servingRatio * _customFood.B5);
            totalB6 = (servingRatio * _customFood.B6);
            totalBiotin = (servingRatio * _customFood.B7);
            totalFolicAcid = (servingRatio * _customFood.FolicAcid);
            totalCobalamin = (servingRatio * _customFood.B12);
            totalVitaminC = (servingRatio * _customFood.VitaminC);
            totalVitaminD = (servingRatio * _customFood.VitaminD);
            totalVitaminE = (servingRatio * _customFood.VitaminE);
            totalVitaminK = (servingRatio * _customFood.VitaminK);
        }
        else
        {
            numberOfServings = decimal.Parse(ServingNumberEntry.Text);
            totalGrams = servingSizeSelected * numberOfServings;
            var servingRatio = totalGrams / _food.ServingSize;
            totalCalories = (servingRatio * _food.Calories);
            totalCarbs = (servingRatio * _food.Carbs);
            totalFat = (servingRatio * _food.Fat);
            totalProtein = (servingRatio * _food.Protein);

            totalSugar = (servingRatio * _food.Sugar);
            totalAddedSugar = (servingRatio * _food.AddSugar);
            totalSugarAlc = (servingRatio * _food.SugarAlc);
            totalFiber = (servingRatio * _food.Fiber);
            totalNetCarb = (servingRatio * _food.NetCarb);

            totalSatFat = (servingRatio * _food.SatFat);
            totalPUnSatFat = (servingRatio * _food.PUnSatFat);
            totalMUnSatFat = (servingRatio * _food.MUnSatFat);
            totalTransFat = (servingRatio * _food.TransFat);

            totalIron = (servingRatio * _food.Iron);
            totalCalcium = (servingRatio * _food.Calcium);
            totalPotassium = (servingRatio * _food.Potassium);
            totalSodium = (servingRatio * _food.Sodium);
            totalCholesterol = (servingRatio * _food.Cholesterol);

            totalVitaminA = (servingRatio * _food.VitaminA);
            totalThiamin = (servingRatio * _food.Thiamin);
            totalRiboflavin = (servingRatio * _food.Riboflavin);
            totalNiacin = (servingRatio * _food.Niacin);
            totalB5 = (servingRatio * _food.B5);
            totalB6 = (servingRatio * _food.B6);
            totalBiotin = (servingRatio * _food.B7);
            totalFolicAcid = (servingRatio * _food.FolicAcid);
            totalCobalamin = (servingRatio * _food.B12);
            totalVitaminC = (servingRatio * _food.VitaminC);
            totalVitaminD = (servingRatio * _food.VitaminD);
            totalVitaminE = (servingRatio * _food.VitaminE);
            totalVitaminK = (servingRatio * _food.VitaminK);
        }

        var userID = await _localData.GetUserID();
        if (numberOfServings > 0)
        {
            if (!_custom)
            {
                await _localData.AddLoggedFood(userID, _date, (MealPicker.SelectedIndex + 1), TimeSelect.Time, ServingSizePicker.SelectedIndex, numberOfServings, totalGrams, _food.FoodName, _food.Brand,
                    _food.ServingSize, _food.ServingUnit, (decimal)_food.Grams, _food.ServingName,
                    (totalCalories / _factor), totalCarbs, totalSugar, totalAddedSugar, totalSugarAlc, totalFiber, totalNetCarb,
                    totalFat, totalSatFat, totalPUnSatFat, totalMUnSatFat, totalTransFat, totalProtein,
                    totalIron, totalCalcium, totalPotassium, totalSodium, totalCholesterol,
                    totalVitaminA, totalThiamin, totalRiboflavin, totalNiacin, totalB5, totalB6, totalBiotin, totalFolicAcid, totalCobalamin,
                    totalVitaminC, totalVitaminD, totalVitaminE, totalVitaminK);
            }
            else
            {
                await _localData.AddLoggedFood(userID, _date, (MealPicker.SelectedIndex + 1), TimeSelect.Time, ServingSizePicker.SelectedIndex, numberOfServings, totalGrams, _customFood.FoodName, _customFood.Brand,
                    _customFood.ServingSize, _customFood.ServingUnit, (decimal)_customFood.Grams, _customFood.ServingName,
                    (totalCalories / _factor), totalCarbs, totalSugar, totalAddedSugar, totalSugarAlc, totalFiber, totalNetCarb,
                    totalFat, totalSatFat, totalPUnSatFat, totalMUnSatFat, totalTransFat, totalProtein,
                    totalIron, totalCalcium, totalPotassium, totalSodium, totalCholesterol,
                    totalVitaminA, totalThiamin, totalRiboflavin, totalNiacin, totalB5, totalB6, totalBiotin, totalFolicAcid, totalCobalamin,
                    totalVitaminC, totalVitaminD, totalVitaminE, totalVitaminK);

            }
            await Navigation.PopToRootAsync();
        }
        else
        {
            await DisplayAlert("Notice", "Enter a valid number of servings", "OK");
        }
    }
}
