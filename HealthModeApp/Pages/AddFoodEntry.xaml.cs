﻿using System.ComponentModel;
using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using HealthModeApp.Pages.FoodJournalPage;

namespace HealthModeApp.Pages;

[QueryProperty(nameof(Food), "Food")]
public partial class AddFoodEntry : ContentPage
{
	public readonly IRestDataService _dataService;
	public readonly ISQLiteDataService _localData;
	NutritionModel _nutritionModel;
    CustomFoods _customFood;
	public string _barcode;
    DateTime _date;
    int _mealType;

    public AddFoodEntry(IRestDataService dataService, ISQLiteDataService localData, string barcode, int mealType, DateTime date)
    {

        InitializeComponent();
        _nutritionModel = new NutritionModel();
        _customFood = new CustomFoods();
        _dataService = dataService;
		_localData = localData;
        _date = date;
        _mealType = mealType;

        BindingContext = this;
		_barcode = barcode;
        BarcodeEntry.Text = barcode;

        var servingUnits = new List<string>();
        servingUnits.Add("g");
        servingUnits.Add("mL");
        servingUnits.Add("oz");

        ServingUnitPicker.ItemsSource = servingUnits;
        ServingUnitPicker.SelectedIndex = 0;

        var mealList = new List<string>();
        mealList.Add("Select a meal");
        mealList.Add("Breakfast");
        mealList.Add("Lunch");
        mealList.Add("Dinner");
        mealList.Add("Snack");
        mealList.Add("N/A");


        mealPicker.ItemsSource = mealList;
        mealPicker.SelectedIndex = 0;

        var primaryList = new List<string>();
        primaryList.Add("Select a category");
        primaryList.Add("Fruits");
        primaryList.Add("Vegetables");
        primaryList.Add("Grains");
        primaryList.Add("Meats");
        primaryList.Add("Dairy");
        primaryList.Add("Beverage");
        primaryList.Add("Processed/Packaged");
        primaryList.Add("N/A");



        CategoryPicker.ItemsSource = primaryList;
        CategoryPicker.SelectedIndex = 0;

        VitaminUnitButton.Text = "Units";

        VitaminA.Text = "Vitamin A (μg/mcg):";
        Thiamin.Text = "Vitamin B1 [Thiamin] (mg):";
        Riboflavin.Text = "Vitamin B2 [Riboflavin] (mg):";
        Niacin.Text = "Vitamin B3 [Niacin] (mg):";
        PanthoAcid.Text = "Vitamin B5 [Panthothenic Acid] (mg):";
        Pyridoxin.Text = "Vitamin B6 [Pyridoxin] (mg):";
        Biotin.Text = "Vitamin B7 [Biotin] (μg/mcg):";
        FolicAcid.Text = "Vitamin B9 [Folic Acid] (μg/mcg):";
        Cobalamin.Text = "Vitamin B12 [Cobalamin] (μg/mcg):";
        VitaminC.Text = "Vitamin C (mg):";
        VitaminD.Text = "Vitamin D (μg/mcg):";
        VitaminE.Text = "Vitamin E (mg):";
        VitaminK.Text = "Vitamin K (μg/mcg):";

    }


    public NutritionModel Food
	{
		get => _nutritionModel;
		set
		{
			_nutritionModel = value;
			OnPropertyChanged(nameof(Food));
		}
	}

    public CustomFoods CustomFood
    {
        get => _customFood;
        set
        {
            _customFood = value;
            OnPropertyChanged(nameof(Food));
        }
    }


	protected override async void OnAppearing()
	{
		base.OnAppearing();

        // Hide the TabBar on this page
        Shell.SetTabBarIsVisible(this, false);

		string foodUnit = await _localData.GetFoodEnergyUnit();
		switch (foodUnit)
		{
			case "kCal":
				EnergyLabel.Text = "kCal:";
				EnergyMacrosLabel.Text = "Kilocalories & Macros";
				break;

            case "cal":
                EnergyLabel.Text = "Calories:";
                EnergyMacrosLabel.Text = "Calories & Macros";
                break;

            case "kJ":
                EnergyLabel.Text = "Kilojoules (kJ):";
                EnergyMacrosLabel.Text = "Kilojoules & Macros";
                break;
        }
    }




    bool uploading = false;
	async void OnUploadEntryClicked(object sender, EventArgs e)
	{
		Debug.WriteLine("Uploading data");
		if (BarcodeEntry.Text == null)
		{
			Food.Barcode = Food.FoodName;
            CustomFood.Barcode = CustomFood.FoodName;
		}
		if (ServingName.Text == null)
		{
			Food.ServingName = "1 Serving";
            CustomFood.ServingName = "1 Serving";
		}

        if (!uploading)
        {

            if ((!string.IsNullOrWhiteSpace(FoodEntry.Text)) && (!string.IsNullOrWhiteSpace(BrandEntry.Text)) && (!string.IsNullOrWhiteSpace(ServingSizeEntry.Text)) && (!string.IsNullOrWhiteSpace(EnergyEntry.Text)) && (!string.IsNullOrWhiteSpace(CarbEntry.Text)) && (!string.IsNullOrWhiteSpace(FatEntry.Text)) && (!string.IsNullOrWhiteSpace(ProteinEntry.Text)))
            {
                if ((mealPicker.SelectedItem.ToString() != "Select a meal") && (CategoryPicker.SelectedItem.ToString() != "Select a category"))
                {

                    bool terms = await DisplayAlert("Notice", "Does the nutrition label match the information you provided here?", "Yes", "No");
                    if (terms)
                    {
                        AddFood.IsVisible = false;
                        Loading.IsVisible = true;
 

                        uploading = true;
                        var foodInfo = await _dataService.GetNutritionInfoBarcodeAsync(_barcode);
                        var uploadFoodInfo = await _dataService.GetFoodUploadExistsAlready(_barcode);
                        if (foodInfo == null && uploadFoodInfo == null)
                        {
                            if (!_isUnitMin)
                            {
                                _isUnitMin = true;
                                // Assuming the weight of the food item is measured in grams
                                // and the recommended daily intake is in milligrams or micrograms, as appropriate.

                                string ironText = IronEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(ironText))
                                {
                                    if (decimal.TryParse(ironText, out decimal iron))
                                    {
                                        decimal ironMilligrams = iron / 100m * 30m; // Iron - RDI: 30mg

                                        IronEntry.Text = ironMilligrams.ToString();
                                    }
                                }

                                string calciumText = CalciumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(calciumText))
                                {
                                    if (decimal.TryParse(calciumText, out decimal calcium))
                                    {
                                        decimal calciumMilligrams = calcium / 100m * 1300m; // Calcium - RDI: 1300mg

                                        CalciumEntry.Text = calciumMilligrams.ToString();
                                    }
                                }

                                string sodiumText = SodiumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(sodiumText))
                                {
                                    if (decimal.TryParse(sodiumText, out decimal sodium))
                                    {
                                        decimal sodiumMilligrams = sodium / 100m * 2300m; // Sodium - RDI: 2300mg

                                        SodiumEntry.Text = sodiumMilligrams.ToString();
                                    }
                                }

                                string potassiumText = PotassiumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(potassiumText))
                                {
                                    if (decimal.TryParse(potassiumText, out decimal potassium))
                                    {
                                        decimal potassiumMilligrams = potassium / 100m * 4700m; // Potassium - RDI: 4700mg

                                        PotassiumEntry.Text = potassiumMilligrams.ToString();
                                    }
                                }

                                string cholesterolText = CholesterolEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(cholesterolText))
                                {
                                    if (decimal.TryParse(cholesterolText, out decimal cholesterol))
                                    {
                                        decimal cholesterolMilligrams = cholesterol / 100m * 300m; // Cholesterol - RDI: 300mg

                                        CholesterolEntry.Text = cholesterolMilligrams.ToString();
                                    }
                                }

                            }

                            if (!_isUnitVit)
                            {
                                _isUnitVit = true;
                                string vitaminAText = VitaminAEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminAText))
                                {
                                    if (decimal.TryParse(vitaminAText, out decimal vitaminA))
                                    {
                                        decimal vitaminAMicrograms = vitaminA / 100m * 900m; // Vitamin A - RDI: 900mcg

                                        VitaminAEntry.Text = vitaminAMicrograms.ToString();
                                    }
                                }

                                string vitaminCText = VitaminCEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminCText))
                                {
                                    if (decimal.TryParse(vitaminCText, out decimal vitaminC))
                                    {
                                        decimal vitaminCMilligrams = vitaminC / 100m * 90m; // Vitamin C - RDI: 90mg

                                        VitaminCEntry.Text = vitaminCMilligrams.ToString();
                                    }
                                }

                                string vitaminDText = VitaminDEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminDText))
                                {
                                    if (decimal.TryParse(vitaminDText, out decimal vitaminD))
                                    {
                                        decimal vitaminDMicrograms = vitaminD / 100m * 15m; // Vitamin D - RDI: 15mcg

                                        VitaminDEntry.Text = vitaminDMicrograms.ToString();
                                    }
                                }

                                string vitaminEText = VitaminEEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminEText))
                                {
                                    if (decimal.TryParse(vitaminEText, out decimal vitaminE))
                                    {
                                        decimal vitaminEMilligrams = vitaminE / 100m * 15m; // Vitamin E - RDI: 15mg

                                        VitaminEEntry.Text = vitaminEMilligrams.ToString();
                                    }
                                }

                                string vitaminKText = VitaminKEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminKText))
                                {
                                    if (decimal.TryParse(vitaminKText, out decimal vitaminK))
                                    {
                                        decimal vitaminKMicrograms = vitaminK / 100m * 120m; // Vitamin K - RDI: 120mcg

                                        VitaminKEntry.Text = vitaminKMicrograms.ToString();
                                    }
                                }

                                string thiaminText = ThiaminEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(thiaminText))
                                {
                                    if (decimal.TryParse(thiaminText, out decimal thiamin))
                                    {
                                        decimal thiaminMilligrams = thiamin / 100m * 1.2m; // Thiamin (Vitamin B1) - RDI: 1.2mg

                                        ThiaminEntry.Text = thiaminMilligrams.ToString();
                                    }
                                }

                                string riboflavinText = RiboflavinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(riboflavinText))
                                {
                                    if (decimal.TryParse(riboflavinText, out decimal riboflavin))
                                    {
                                        decimal riboflavinMilligrams = riboflavin / 100m * 1.3m; // Riboflavin (Vitamin B2) - RDI: 1.3mg

                                        RiboflavinEntry.Text = riboflavinMilligrams.ToString();
                                    }
                                }

                                string niacinText = NiacinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(niacinText))
                                {
                                    if (decimal.TryParse(niacinText, out decimal niacin))
                                    {
                                        decimal niacinMilligrams = niacin / 100m * 16m; // Niacin (Vitamin B3) - RDI: 16mg

                                        NiacinEntry.Text = niacinMilligrams.ToString();
                                    }
                                }

                                // Vitamin B6 - RDI: 1.3mg
                                string pyridoxinText = PyridoxinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(pyridoxinText))
                                {
                                    if (decimal.TryParse(pyridoxinText, out decimal pyridoxin))
                                    {
                                        decimal pyridoxinMilligrams = pyridoxin / 100m * 1.3m;

                                        PyridoxinEntry.Text = pyridoxinMilligrams.ToString();
                                    }
                                }

                                // Vitamin B7 - RDI: 30mcg
                                string biotinText = BiotinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(biotinText))
                                {
                                    if (decimal.TryParse(biotinText, out decimal biotin))
                                    {
                                        decimal biotinMicrograms = biotin / 100m * 30m;

                                        BiotinEntry.Text = biotinMicrograms.ToString();
                                    }
                                }


                                // Folate - RDI: 400mcg
                                string folateText = FolicAcidEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(folateText))
                                {
                                    if (decimal.TryParse(folateText, out decimal folate))
                                    {
                                        decimal folateMicrograms = folate / 100m * 400m;

                                        FolicAcidEntry.Text = folateMicrograms.ToString();
                                    }
                                }

                                // Vitamin B12 - RDI: 2.4mcg
                                string cobalaminText = CobalaminEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(cobalaminText))
                                {
                                    if (decimal.TryParse(cobalaminText, out decimal cobalamin))
                                    {
                                        decimal cobalaminMicrograms = cobalamin / 100m * 2.4m;

                                        CobalaminEntry.Text = cobalaminMicrograms.ToString();
                                    }
                                }


                            }

                            if (CategoryPicker.SelectedItem.ToString() == "N/A" || CategoryPicker.SelectedItem.ToString() == "Select a category")
                            {
                                Food.Category = null;
                                CustomFood.Category = null;
                            }
                            else
                            {
                                Food.Category = CategoryPicker.SelectedItem.ToString();
                                CustomFood.Category = CategoryPicker.SelectedItem.ToString();
                            }

                            switch (mealPicker.SelectedItem.ToString())
                            {
                                case "Select a meal":
                                    Food.MealType = -1;
                                    CustomFood.MealType = -1;
                                    break;
                                case "Breakfast":
                                    Food.MealType = 0;
                                    CustomFood.MealType = 0;
                                    break;
                                case "Lunch":
                                    Food.MealType = 1;
                                    CustomFood.MealType = 1;
                                    break;
                                case "Dinner":
                                    Food.MealType = 2;
                                    CustomFood.MealType = 2;
                                    break;
                                case "Snack":
                                    Food.MealType = 3;
                                    CustomFood.MealType = 3;
                                    break;
                                case "N/A":
                                    Food.MealType = -1;
                                    CustomFood.MealType = -1;
                                    break;
                                default:
                                    Food.MealType = -1;
                                    CustomFood.MealType = -1;
                                    break;
                            }

                            if (ServingUnitPicker.SelectedIndex == 0)
                            {
                                GramsEntry.Text = ServingSizeEntry.Text;
                            }

                            if (string.IsNullOrWhiteSpace(GramsEntry.Text))
                            {
                                GramsEntry.Text = ServingSizeEntry.Text;
                            }

                            Food.FoodId = null;

                            CustomFood.Barcode = Food.Barcode;
                            CustomFood.FoodName = Food.FoodName;
                            CustomFood.Brand = Food.Brand;
                            CustomFood.ServingSize = Food.ServingSize;
                            CustomFood.ServingUnit = Food.ServingUnit;
                            CustomFood.ServingName = Food.ServingName;
                            CustomFood.Grams = (decimal)Food.Grams;
                            CustomFood.Calories = Food.Calories;
                            CustomFood.Carbs = Food.Carbs;
                            CustomFood.Fat = Food.Fat;
                            CustomFood.Protein = Food.Protein;
                            CustomFood.SatFat = Food.SatFat;
                            CustomFood.PUnSatFat = Food.PUnSatFat;
                            CustomFood.PUnSatFat = Food.MUnSatFat;
                            CustomFood.TransFat = Food.TransFat;
                            CustomFood.Iron = Food.Iron;
                            CustomFood.Calcium = Food.Calcium;
                            CustomFood.Potassium = Food.Potassium;
                            CustomFood.Sodium = Food.Sodium;
                            CustomFood.Cholesterol = Food.Cholesterol;
                            CustomFood.VitaminA = Food.VitaminA;
                            CustomFood.Thiamin = Food.Thiamin;
                            CustomFood.Riboflavin = Food.Riboflavin;
                            CustomFood.Niacin = Food.Niacin;
                            CustomFood.B5 = Food.B5;
                            CustomFood.B6 = Food.B6;
                            CustomFood.B7 = Food.B7;
                            CustomFood.FolicAcid = Food.FolicAcid;
                            CustomFood.B12 = Food.B12;
                            CustomFood.VitaminC = Food.VitaminC;
                            CustomFood.VitaminD = Food.VitaminD;
                            CustomFood.VitaminE = Food.VitaminE;
                            CustomFood.VitaminK = Food.VitaminK;
                            CustomFood.MealType = (int)Food.MealType;
                            CustomFood.Category = Food.Category;

                            (string email, string password) = await _localData.GetUserCredentials();
                            var userID = await _localData.GetUserID();

                            await _dataService.AddNutritionInfoAsync(Food, email, password, userID);
                            await _localData.AddCustomFood(CustomFood);
                            var currentPage = Navigation.NavigationStack.LastOrDefault();
                            Navigation.InsertPageBefore(new FoodInfo(_dataService, _localData, Food, _mealType, _date), currentPage);
                            await Navigation.PopAsync();
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            if (!_isUnitMin)
                            {
                                _isUnitMin = true;
                                // Assuming the weight of the food item is measured in grams
                                // and the recommended daily intake is in milligrams or micrograms, as appropriate.

                                string ironText = IronEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(ironText))
                                {
                                    if (decimal.TryParse(ironText, out decimal iron))
                                    {
                                        decimal ironMilligrams = iron / 100m * 30m; // Iron - RDI: 30mg

                                        IronEntry.Text = ironMilligrams.ToString();
                                    }
                                }

                                string calciumText = CalciumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(calciumText))
                                {
                                    if (decimal.TryParse(calciumText, out decimal calcium))
                                    {
                                        decimal calciumMilligrams = calcium / 100m * 1300m; // Calcium - RDI: 1300mg

                                        CalciumEntry.Text = calciumMilligrams.ToString();
                                    }
                                }

                                string sodiumText = SodiumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(sodiumText))
                                {
                                    if (decimal.TryParse(sodiumText, out decimal sodium))
                                    {
                                        decimal sodiumMilligrams = sodium / 100m * 2300m; // Sodium - RDI: 2300mg

                                        SodiumEntry.Text = sodiumMilligrams.ToString();
                                    }
                                }

                                string potassiumText = PotassiumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(potassiumText))
                                {
                                    if (decimal.TryParse(potassiumText, out decimal potassium))
                                    {
                                        decimal potassiumMilligrams = potassium / 100m * 4700m; // Potassium - RDI: 4700mg

                                        PotassiumEntry.Text = potassiumMilligrams.ToString();
                                    }
                                }

                                string cholesterolText = CholesterolEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(cholesterolText))
                                {
                                    if (decimal.TryParse(cholesterolText, out decimal cholesterol))
                                    {
                                        decimal cholesterolMilligrams = cholesterol / 100m * 300m; // Cholesterol - RDI: 300mg

                                        CholesterolEntry.Text = cholesterolMilligrams.ToString();
                                    }
                                }

                            }

                            if (!_isUnitVit)
                            {
                                _isUnitVit = true;
                                string vitaminAText = VitaminAEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminAText))
                                {
                                    if (decimal.TryParse(vitaminAText, out decimal vitaminA))
                                    {
                                        decimal vitaminAMicrograms = vitaminA / 100m * 900m; // Vitamin A - RDI: 900mcg

                                        VitaminAEntry.Text = vitaminAMicrograms.ToString();
                                    }
                                }

                                string vitaminCText = VitaminCEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminCText))
                                {
                                    if (decimal.TryParse(vitaminCText, out decimal vitaminC))
                                    {
                                        decimal vitaminCMilligrams = vitaminC / 100m * 90m; // Vitamin C - RDI: 90mg

                                        VitaminCEntry.Text = vitaminCMilligrams.ToString();
                                    }
                                }

                                string vitaminDText = VitaminDEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminDText))
                                {
                                    if (decimal.TryParse(vitaminDText, out decimal vitaminD))
                                    {
                                        decimal vitaminDMicrograms = vitaminD / 100m * 15m; // Vitamin D - RDI: 15mcg

                                        VitaminDEntry.Text = vitaminDMicrograms.ToString();
                                    }
                                }

                                string vitaminEText = VitaminEEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminEText))
                                {
                                    if (decimal.TryParse(vitaminEText, out decimal vitaminE))
                                    {
                                        decimal vitaminEMilligrams = vitaminE / 100m * 15m; // Vitamin E - RDI: 15mg

                                        VitaminEEntry.Text = vitaminEMilligrams.ToString();
                                    }
                                }

                                string vitaminKText = VitaminKEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminKText))
                                {
                                    if (decimal.TryParse(vitaminKText, out decimal vitaminK))
                                    {
                                        decimal vitaminKMicrograms = vitaminK / 100m * 120m; // Vitamin K - RDI: 120mcg

                                        VitaminKEntry.Text = vitaminKMicrograms.ToString();
                                    }
                                }

                                string thiaminText = ThiaminEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(thiaminText))
                                {
                                    if (decimal.TryParse(thiaminText, out decimal thiamin))
                                    {
                                        decimal thiaminMilligrams = thiamin / 100m * 1.2m; // Thiamin (Vitamin B1) - RDI: 1.2mg

                                        ThiaminEntry.Text = thiaminMilligrams.ToString();
                                    }
                                }

                                string riboflavinText = RiboflavinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(riboflavinText))
                                {
                                    if (decimal.TryParse(riboflavinText, out decimal riboflavin))
                                    {
                                        decimal riboflavinMilligrams = riboflavin / 100m * 1.3m; // Riboflavin (Vitamin B2) - RDI: 1.3mg

                                        RiboflavinEntry.Text = riboflavinMilligrams.ToString();
                                    }
                                }

                                string niacinText = NiacinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(niacinText))
                                {
                                    if (decimal.TryParse(niacinText, out decimal niacin))
                                    {
                                        decimal niacinMilligrams = niacin / 100m * 16m; // Niacin (Vitamin B3) - RDI: 16mg

                                        NiacinEntry.Text = niacinMilligrams.ToString();
                                    }
                                }

                                // Vitamin B6 - RDI: 1.3mg
                                string pyridoxinText = PyridoxinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(pyridoxinText))
                                {
                                    if (decimal.TryParse(pyridoxinText, out decimal pyridoxin))
                                    {
                                        decimal pyridoxinMilligrams = pyridoxin / 100m * 1.3m;

                                        PyridoxinEntry.Text = pyridoxinMilligrams.ToString();
                                    }
                                }

                                // Vitamin B7 - RDI: 30mcg
                                string biotinText = BiotinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(biotinText))
                                {
                                    if (decimal.TryParse(biotinText, out decimal biotin))
                                    {
                                        decimal biotinMicrograms = biotin / 100m * 30m;

                                        BiotinEntry.Text = biotinMicrograms.ToString();
                                    }
                                }


                                // Folate - RDI: 400mcg
                                string folateText = FolicAcidEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(folateText))
                                {
                                    if (decimal.TryParse(folateText, out decimal folate))
                                    {
                                        decimal folateMicrograms = folate / 100m * 400m;

                                        FolicAcidEntry.Text = folateMicrograms.ToString();
                                    }
                                }

                                // Vitamin B12 - RDI: 2.4mcg
                                string cobalaminText = CobalaminEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(cobalaminText))
                                {
                                    if (decimal.TryParse(cobalaminText, out decimal cobalamin))
                                    {
                                        decimal cobalaminMicrograms = cobalamin / 100m * 2.4m;

                                        CobalaminEntry.Text = cobalaminMicrograms.ToString();
                                    }
                                }


                            }

                            if (CategoryPicker.SelectedItem.ToString() == "N/A" || CategoryPicker.SelectedItem.ToString() == "Select a category")
                            {
                                Food.Category = null;
                                CustomFood.Category = null;
                            }
                            else
                            {
                                Food.Category = CategoryPicker.SelectedItem.ToString();
                                CustomFood.Category = CategoryPicker.SelectedItem.ToString();
                            }

                            switch (mealPicker.SelectedItem.ToString())
                            {
                                case "Select a meal":
                                    Food.MealType = -1;
                                    CustomFood.MealType = -1;
                                    break;
                                case "Breakfast":
                                    Food.MealType = 0;
                                    CustomFood.MealType = 0;
                                    break;
                                case "Lunch":
                                    Food.MealType = 1;
                                    CustomFood.MealType = 1;
                                    break;
                                case "Dinner":
                                    Food.MealType = 2;
                                    CustomFood.MealType = 2;
                                    break;
                                case "Snack":
                                    Food.MealType = 3;
                                    CustomFood.MealType = 3;
                                    break;
                                case "N/A":
                                    Food.MealType = -1;
                                    CustomFood.MealType = -1;
                                    break;
                                default:
                                    Food.MealType = -1;
                                    CustomFood.MealType = -1;
                                    break;
                            }

                            if (ServingUnitPicker.SelectedIndex == 0)
                            {
                                GramsEntry.Text = ServingSizeEntry.Text;
                            }

                            if (string.IsNullOrWhiteSpace(GramsEntry.Text))
                            {
                                GramsEntry.Text = ServingSizeEntry.Text;
                            }

                            CustomFood.Barcode = Food.Barcode;
                            CustomFood.FoodName = Food.FoodName;
                            CustomFood.Brand = Food.Brand;
                            CustomFood.ServingSize = Food.ServingSize;
                            CustomFood.ServingUnit = Food.ServingUnit;
                            CustomFood.ServingName = Food.ServingName;
                            CustomFood.Grams = (decimal)Food.Grams;
                            CustomFood.Calories = Food.Calories;
                            CustomFood.Carbs = Food.Carbs;
                            CustomFood.Fat = Food.Fat;
                            CustomFood.Protein = Food.Protein;
                            CustomFood.SatFat = Food.SatFat;
                            CustomFood.PUnSatFat = Food.PUnSatFat;
                            CustomFood.PUnSatFat = Food.MUnSatFat;
                            CustomFood.TransFat = Food.TransFat;
                            CustomFood.Iron = Food.Iron;
                            CustomFood.Calcium = Food.Calcium;
                            CustomFood.Potassium = Food.Potassium;
                            CustomFood.Sodium = Food.Sodium;
                            CustomFood.Cholesterol = Food.Cholesterol;
                            CustomFood.VitaminA = Food.VitaminA;
                            CustomFood.Thiamin = Food.Thiamin;
                            CustomFood.Riboflavin = Food.Riboflavin;
                            CustomFood.Niacin = Food.Niacin;
                            CustomFood.B5 = Food.B5;
                            CustomFood.B6 = Food.B6;
                            CustomFood.B7 = Food.B7;
                            CustomFood.FolicAcid = Food.FolicAcid;
                            CustomFood.B12 = Food.B12;
                            CustomFood.VitaminC = Food.VitaminC;
                            CustomFood.VitaminD = Food.VitaminD;
                            CustomFood.VitaminE = Food.VitaminE;
                            CustomFood.VitaminK = Food.VitaminK;
                            CustomFood.MealType = (int)Food.MealType;
                            CustomFood.Category = Food.Category;


                            await _localData.AddCustomFood(CustomFood);
                            var currentPage = Navigation.NavigationStack.LastOrDefault();
                            Navigation.InsertPageBefore(new FoodInfo(_dataService, _localData, CustomFood, _mealType, _date), currentPage);
                            await Navigation.PopAsync();
                        }
                    }
                    
                }
                else
                {
                    await DisplayAlert("Alert", "Please select a meal type and category", "OK");
                }
            }
            else
            {
                await DisplayAlert("Alert", "Please fill out all the required entries", "OK");
            }


        }
        else
        {
            DisplayAlert("Notice", "Your food is being uploaded", "OK");
        }
    }




	public bool _isUnitVit = true;

    void SwitchUnitVitClicked(System.Object sender, System.EventArgs e)
    {
		_isUnitVit = !_isUnitVit;

		if (_isUnitVit)
		{
            VitaminUnitButton.Text = "Units";

			VitaminA.Text = "Vitamin A (μg/mcg):";
			Thiamin.Text = "Vitamin B1 [Thiamin] (mg):";
			Riboflavin.Text = "Vitamin B2 [Riboflavin] (mg):";
			Niacin.Text = "Vitamin B3 [Niacin] (mg):";
			PanthoAcid.Text = "Vitamin B5 [Panthothenic Acid] (mg):";
			Pyridoxin.Text = "Vitamin B6 [Pyridoxin] (mg):";
			Biotin.Text = "Vitamin B7 [Biotin] (μg/mcg):";
			FolicAcid.Text = "Vitamin B9 [Folic Acid] (μg/mcg):";
			Cobalamin.Text = "Vitamin B12 [Cobalamin] (μg/mcg):";
			VitaminC.Text = "Vitamin C (mg):";
			VitaminD.Text = "Vitamin D (μg/mcg):";
			VitaminE.Text = "Vitamin E (mg):";
			VitaminK.Text = "Vitamin K (μg/mcg):";

        }
		else
		{
            VitaminUnitButton.Text = "%";

            VitaminA.Text = "Vitamin A (%):";
            Thiamin.Text = "Vitamin B1 [Thiamin] (%):";
            Riboflavin.Text = "Vitamin B2 [Riboflavin] (%):";
            Niacin.Text = "Vitamin B3 [Niacin] (%):";
            PanthoAcid.Text = "Vitamin B5 [Panthothenic Acid] (%):";
            Pyridoxin.Text = "Vitamin B6 [Pyridoxin] (%):";
            Biotin.Text = "Vitamin B7 [Biotin] (%):";
            FolicAcid.Text = "Vitamin B9 [Folic Acid] (%):";
            Cobalamin.Text = "Vitamin B12 [Cobalamin] (%:";
            VitaminC.Text = "Vitamin C (%):";
            VitaminD.Text = "Vitamin D (%):";
            VitaminE.Text = "Vitamin E (%):";
            VitaminK.Text = "Vitamin K (%):";
        }
    }

    public bool _isUnitMin = true;

    void SwitchUnitMinClicked(System.Object sender, System.EventArgs e)
    {
        _isUnitMin = !_isUnitMin;

        if (_isUnitMin)
        {
            MineralUnitButton.Text = "Units";

            Iron.Text = "Iron (mg):";
            Calcium.Text = "Calcium (mg):";
            Potassium.Text = "Potassium (mg):";
            Sodium.Text = "Sodium (mg):";
            Cholesterol.Text = "Cholesterol (mg):";
                
        }
        else
        {
            MineralUnitButton.Text = "%";

            Iron.Text = "Iron (%):";
            Calcium.Text = "Calcium (%):";
            Potassium.Text = "Potassium (%):";
            Sodium.Text = "Sodium (%):";
            Cholesterol.Text = "Cholesterol (%):";
        }
    }

    void ServingUnitPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    {
        if (ServingUnitPicker.SelectedIndex == 0)
        {
            TotalGrams.IsVisible = false;
            GramsEntry.IsVisible = false;
            GramsEntry.Text = ServingSizeEntry.Text;
        }
        else
        {
            TotalGrams.IsVisible = true;
            GramsEntry.IsVisible = true;
            GramsEntry.Text = "";
        }
    }


}

