using System.Diagnostics;
using System.Text.RegularExpressions;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.FoodJournalPage;


public partial class FoodUpdate : ContentPage
{

    public readonly ISQLiteDataService _localData;
    public LoggedFoodTable _food;
    public string _servingUnit;
    DateTime _date;
    int _mealType;
    decimal servingSizeSelected;
    List<ServingSizeOption> servingSizeOptions;
    int _loggedFoodID;
    decimal baseServings;
    decimal baseGrams;

    public FoodUpdate(ISQLiteDataService localData, LoggedFoodTable food)
    {
        InitializeComponent();
        
        _localData = localData;
        _food = food;
        TimeSelect.Time = _food.Time;
        _date = _food.Date;
        _mealType = _food.MealType;
        baseServings = _food.ServingAmount;
        baseGrams = _food.TotalGrams / baseServings;
        _servingUnit = _food.ServingUnit;
        _loggedFoodID = _food.LoggedFoodID;
        ServingNumberEntry.Text = baseServings.ToString();

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
        ServingSizePicker.SelectedIndex = _food.ServingSizeSelected;

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


    decimal numberOfServings;
    decimal totalGrams;


    decimal baseCalories;
    decimal baseCarbs;
    decimal baseFat;
    decimal baseProtein;

    decimal? baseSugar = 0;
    decimal? baseAddedSugar = 0;
    decimal? baseSugarAlc = 0;
    decimal? baseFiber = 0;
    decimal? baseNetCarb = 0;

    decimal? baseSatFat = 0;
    decimal? basePUnSatFat = 0;
    decimal? baseMUnSatFat = 0;
    decimal? baseTransFat = 0;

    decimal? baseIron = 0;
    decimal? baseCalcium = 0;
    decimal? basePotassium = 0;
    decimal? baseSodium = 0;
    decimal? baseCholesterol = 0;

    decimal? baseVitaminA = 0;
    decimal? baseThiamin = 0;
    decimal? baseRiboflavin = 0;
    decimal? baseNiacin = 0;
    decimal? baseB5 = 0;
    decimal? baseB6 = 0;
    decimal? baseBiotin = 0;
    decimal? baseFolicAcid = 0;
    decimal? baseCobalamin = 0;
    decimal? baseVitaminC = 0;
    decimal? baseVitaminD = 0;
    decimal? baseVitaminE = 0;
    decimal? baseVitaminK = 0;

    async void PopulateFoodInfo(LoggedFoodTable food)
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

        //have to set the base values by getting their _food._____ counterparts and dividing by servings sort of. so if its 100 cal for 1 serving,
        //if its 4 servings its _food.Calories / servings, but need to make sure that will be true for the math when the serving size is different than base
        Debug.WriteLine(baseGrams);
        baseCalories = (decimal)(_food.Calories / baseGrams);
        baseCarbs = (decimal)(_food.Carbs / baseGrams);
        baseFat = (decimal)(_food.Fat / baseGrams);
        baseProtein = (decimal)(_food.Protein / baseGrams);

        baseAddedSugar = _food.AddSugar != null ? (decimal)(_food.AddSugar / baseGrams) : null;
        baseSugarAlc = _food.SugarAlc != null ? (decimal)(_food.SugarAlc / baseGrams) : null;
        baseFiber = _food.Fiber != null ? (decimal)(_food.Fiber / baseGrams) : null;
        baseNetCarb = _food.NetCarb != null ? (decimal)(_food.NetCarb / baseGrams) : null;

        baseSatFat = _food.SatFat != null ? (decimal)(_food.SatFat / baseGrams) : null;
        basePUnSatFat = _food.PUnSatFat != null ? (decimal)(_food.PUnSatFat / baseGrams) : null;
        baseMUnSatFat = _food.MUnSatFat != null ? (decimal)(_food.MUnSatFat / baseGrams) : null;
        baseTransFat = _food.TransFat != null ? (decimal)(_food.TransFat / baseGrams) : null;

        baseIron = _food.Iron != null ? (decimal)(_food.Iron / baseGrams) : null;
        baseCalcium = _food.Calcium != null ? (decimal)(_food.Calcium / baseGrams) : null;
        basePotassium = _food.Potassium != null ? (decimal)(_food.Potassium / baseGrams) : null;
        baseSodium = _food.Sodium != null ? (decimal)(_food.Sodium / baseGrams) : null;
        baseCholesterol = _food.Cholesterol != null ? (decimal)(_food.Cholesterol / baseGrams) : null;

        baseVitaminA = _food.VitaminA != null ? (decimal)(_food.VitaminA / baseGrams) : null;
        baseThiamin = _food.Thiamin != null ? (decimal)(_food.Thiamin / baseGrams) : null;
        baseRiboflavin = _food.Riboflavin != null ? (decimal)(_food.Riboflavin / baseGrams) : null;
        baseNiacin = _food.Niacin != null ? (decimal)(_food.Niacin / baseGrams) : null;
        baseB5 = _food.B5 != null ? (decimal)(_food.B5 / baseGrams) : null;
        baseB6 = _food.B6 != null ? (decimal)(_food.B6 / baseGrams) : null;
        baseBiotin = _food.B7 != null ? (decimal)(_food.B7 / baseGrams) : null;
        baseFolicAcid = _food.FolicAcid != null ? (decimal)(_food.FolicAcid / baseGrams) : null;
        baseCobalamin = _food.B12 != null ? (decimal)(_food.B12 / baseGrams) : null;
        baseVitaminC = _food.VitaminC != null ? (decimal)(_food.VitaminC / baseGrams) : null;
        baseVitaminD = _food.VitaminD != null ? (decimal)(_food.VitaminD / baseGrams) : null;
        baseVitaminE = _food.VitaminE != null ? (decimal)(_food.VitaminE / baseGrams) : null;
        baseVitaminK = _food.VitaminK != null ? (decimal)(_food.VitaminK / baseGrams) : null;




        var servingRatio = totalGrams / baseServings;
        var totalCalories = servingRatio * baseCalories;
        var totalCarbs = servingRatio * baseCarbs;
        var totalFat = servingRatio * baseFat;
        var totalProtein = servingRatio * baseProtein;

        var totalSugar = servingRatio * baseSugar;
        var totalAddedSugar = servingRatio * baseAddedSugar;
        var totalSugarAlc = servingRatio * baseSugarAlc;
        var totalFiber = servingRatio * baseFiber;
        var totalNetCarb = servingRatio * baseNetCarb;

        var totalSatFat = servingRatio * baseSatFat;
        var totalPUnSatFat = servingRatio * basePUnSatFat;
        var totalMUnSatFat = servingRatio * baseMUnSatFat;
        var totalTransFat = servingRatio * baseTransFat;

        var totalIron = servingRatio * baseIron;
        var totalCalcium = servingRatio * baseCalcium;
        var totalPotassium = servingRatio * basePotassium;
        var totalSodium = servingRatio * baseSodium;
        var totalCholesterol = servingRatio * baseCholesterol;

        var totalVitaminA = servingRatio * baseVitaminA;
        var totalThiamin = servingRatio * baseThiamin;
        var totalRiboflavin = servingRatio * baseRiboflavin;
        var totalNiacin = servingRatio * baseNiacin;
        var totalB5 = servingRatio * baseB5;
        var totalB6 = servingRatio * baseB6;
        var totalBiotin = servingRatio * baseBiotin;
        var totalFolicAcid = servingRatio * baseFolicAcid;
        var totalCobalamin = servingRatio * baseCobalamin;
        var totalVitaminC = servingRatio * baseVitaminC;
        var totalVitaminD = servingRatio * baseVitaminD;
        var totalVitaminE = servingRatio * baseVitaminE;
        var totalVitaminK = servingRatio * baseVitaminK;


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
            Debug.WriteLine("0");
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

        decimal calNumber = (decimal)(((loggedCalories - food.Calories) * _factor) + totalCalories) / (goals.CalorieGoal * _factor);
        decimal carbNumber = (decimal)((loggedCarbs - food.Carbs) + totalCarbs) / (goals.CarbGoal);
        decimal fatNumber = (decimal)((loggedFat - food.Fat) + totalFat) / (goals.FatGoal);
        decimal proteinNumber = (decimal)((loggedProtein - food.Protein) + totalProtein) / (goals.ProteinGoal);

        CalorieBar.ProgressTo((double)calNumber, 1500, Easing.CubicOut);
        CarbBar.ProgressTo((double)(carbNumber), 1500, Easing.CubicOut);
        FatBar.ProgressTo((double)(fatNumber), 1500, Easing.CubicOut);
        ProteinBar.ProgressTo((double)(proteinNumber), 1500, Easing.CubicOut);

        CalorieGoal.Text = (calNumber).ToString("0.#" + "%");
        CarbGoal.Text = (carbNumber).ToString("0.#" + "%");
        FatGoal.Text = (fatNumber).ToString("0.#" + "%");
        ProteinGoal.Text = (proteinNumber).ToString("0.#" + "%");


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

        if (!string.IsNullOrWhiteSpace(ServingNumberEntry.Text) && decimal.TryParse(ServingNumberEntry.Text, out decimal numberOfServings) && numberOfServings >= 0 && ServingNumberEntry.Text != ".")
        {
            PopulateFoodInfo(_food);
        }
        

    }

    async void UpdateFoodClicked(System.Object sender, System.EventArgs e)
    {
        numberOfServings = decimal.Parse(ServingNumberEntry.Text);
        totalGrams = servingSizeSelected * numberOfServings;

        var servingRatio = totalGrams / baseServings;
        var totalCalories = servingRatio * baseCalories;
        var totalCarbs = servingRatio * baseCarbs;
        var totalFat = servingRatio * baseFat;
        var totalProtein = servingRatio * baseProtein;

        var totalSugar = servingRatio * baseSugar;
        var totalAddedSugar = servingRatio * baseAddedSugar;
        var totalSugarAlc = servingRatio * baseSugarAlc;
        var totalFiber = servingRatio * baseFiber;
        var totalNetCarb = servingRatio * baseNetCarb;

        var totalSatFat = servingRatio * baseSatFat;
        var totalPUnSatFat = servingRatio * basePUnSatFat;
        var totalMUnSatFat = servingRatio * baseMUnSatFat;
        var totalTransFat = servingRatio * baseTransFat;

        var totalIron = servingRatio * baseIron;
        var totalCalcium = servingRatio * baseCalcium;
        var totalPotassium = servingRatio * basePotassium;
        var totalSodium = servingRatio * baseSodium;
        var totalCholesterol = servingRatio * baseCholesterol;

        var totalVitaminA = servingRatio * baseVitaminA;
        var totalThiamin = servingRatio * baseThiamin;
        var totalRiboflavin = servingRatio * baseRiboflavin;
        var totalNiacin = servingRatio * baseNiacin;
        var totalB5 = servingRatio * baseB5;
        var totalB6 = servingRatio * baseB6;
        var totalBiotin = servingRatio * baseBiotin;
        var totalFolicAcid = servingRatio * baseFolicAcid;
        var totalCobalamin = servingRatio * baseCobalamin;
        var totalVitaminC = servingRatio * baseVitaminC;
        var totalVitaminD = servingRatio * baseVitaminD;
        var totalVitaminE = servingRatio * baseVitaminE;
        var totalVitaminK = servingRatio * baseVitaminK;

        var userID = await _localData.GetUserID();
        if (numberOfServings > 0)
        {
            await _localData.UpdateLoggedFood(_loggedFoodID, userID, _date, _food.MealType, TimeSelect.Time, ServingSizePicker.SelectedIndex, numberOfServings, totalGrams, _food.FoodName, _food.Brand,
                 _food.ServingSize, _food.ServingUnit, (decimal)_food.Grams, _food.ServingName,
                 (totalCalories / _factor), totalCarbs, totalSugar, totalAddedSugar, totalSugarAlc, totalFiber, totalNetCarb,
                 totalFat, totalSatFat, totalPUnSatFat, totalMUnSatFat, totalTransFat, totalProtein,
                 totalIron, totalCalcium, totalPotassium, totalSodium, totalCholesterol,
                 totalVitaminA, totalThiamin, totalRiboflavin, totalNiacin, totalB5, totalB6, totalBiotin, totalFolicAcid, totalCobalamin,
                 totalVitaminC, totalVitaminD, totalVitaminE, totalVitaminK);
            await Navigation.PopToRootAsync();
        }
        else
        {
            await DisplayAlert("Notice", "Enter a valid number of servings", "OK");
        }
    }

    async void RemoveButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await _localData.RemoveLoggedFood(_loggedFoodID);
        await Navigation.PopToRootAsync();
    }
}
