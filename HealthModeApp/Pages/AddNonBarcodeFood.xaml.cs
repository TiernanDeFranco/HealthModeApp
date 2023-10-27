using System.ComponentModel;
using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using HealthModeApp.Pages.FoodJournalPage;

namespace HealthModeApp.Pages;

[QueryProperty(nameof(Food), "Food")]
public partial class AddNonBarcodeFood : ContentPage
{
	public readonly IRestDataService _dataService;
	public readonly ISQLiteDataService _localData;
	NutritionModel _nutritionModel;
    CustomFoods _customFood;
	public string _barcode;
    DateTime _date;
    int _mealType;

    bool mealSelected = false;
    bool categorySelected = false;

    public AddNonBarcodeFood(IRestDataService dataService, ISQLiteDataService localData, int mealType, DateTime date)
    {

        InitializeComponent();
        _nutritionModel = new NutritionModel();
        _customFood = new CustomFoods();
        _dataService = dataService;
		_localData = localData;
        _date = date;
        _mealType = mealType;

        BindingContext = this;
        Food.ServingUnit = "g";
        CustomFood.ServingUnit = "g";


        Sodium.Text = "Sodium (mg):";
        Cholesterol.Text = "Cholesterol (mg):";



        VitaminA.Text = "Vitamin A (μg):";
        Thiamin.Text = "Thiamin [B1] (mg):";
        Riboflavin.Text = "Riboflavin [B2] (mg):";
        Niacin.Text = "Niacin [B3] (mg):";
        PanthoAcid.Text = "Panthothenic Acid [B5] (mg):";
        Pyridoxin.Text = "Vitamin B6 [Pyridoxin] (mg):";
        Biotin.Text = "Biotin [B7] (μg):";
        FolicAcid.Text = "Folic Acid/Folate [B9] (μg):";
        Cobalamin.Text = "Vitamin B12 [Cobalamin] (μg):";
        VitaminC.Text = "Vitamin C (mg):";
        VitaminD.Text = "Vitamin D (mg):";
        VitaminE.Text = "Vitamin E (mg):";
        VitaminK.Text = "Vitamin K (μg):";

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
                break;

            case "cal":
                EnergyLabel.Text = "Calories:";
                break;

            case "kJ":
                EnergyLabel.Text = "Kilojoules (kJ):";
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

        Food.ServingName = Food.ServingName.TrimEnd();
        CustomFood.ServingName = Food.ServingName.TrimEnd();

        if (!uploading)
        {

            if ((!string.IsNullOrWhiteSpace(FoodEntry.Text)) && (!string.IsNullOrWhiteSpace(BrandEntry.Text)) && (!string.IsNullOrWhiteSpace(ServingSizeEntry.Text)) && (!string.IsNullOrWhiteSpace(EnergyEntry.Text)) && (!string.IsNullOrWhiteSpace(CarbEntry.Text)) && (!string.IsNullOrWhiteSpace(FatEntry.Text)) && (!string.IsNullOrWhiteSpace(ProteinEntry.Text)))
            {
                if ((mealSelected) && (categorySelected))
                {
                    bool terms = await DisplayAlert("Notice", "Does the nutrition label match the information you provided here?", "Yes", "No");
                    if (terms)
                    {
                        AddFood.IsVisible = false;
                        Loading.IsVisible = true;


                        uploading = true;
                        var foodInfo = await _dataService.GetNutritionInfoBarcodeAsync(_barcode);
                        
                        if (foodInfo == null)
                        {

                            // Assuming the weight of the food item is measured in grams
                            // and the recommended daily intake is in milligrams or micrograms, as appropriate.


                            if (ironPercent)
                            {
                                string ironText = IronEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(ironText))
                                {
                                    if (decimal.TryParse(ironText, out decimal iron))
                                    {
                                        decimal ironMilligrams = iron / 100m * 30m; // Iron - RDI: 30mg

                                        IronEntry.Text = ironMilligrams.ToString();
                                    }
                                }
                            }

                            if (calciumPercent)
                            {
                                string calciumText = CalciumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(calciumText))
                                {
                                    if (decimal.TryParse(calciumText, out decimal calcium))
                                    {
                                        decimal calciumMilligrams = calcium / 100m * 1300m; // Calcium - RDI: 1300mg

                                        CalciumEntry.Text = calciumMilligrams.ToString();
                                    }
                                }
                            }

                            if (sodiumPercent)
                            {
                                string sodiumText = SodiumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(sodiumText))
                                {
                                    if (decimal.TryParse(sodiumText, out decimal sodium))
                                    {
                                        decimal sodiumMilligrams = sodium / 100m * 2300m; // Sodium - RDI: 2300mg

                                        SodiumEntry.Text = sodiumMilligrams.ToString();
                                    }
                                }
                            }

                            if (potassiumPercent)
                            {
                                string potassiumText = PotassiumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(potassiumText))
                                {
                                    if (decimal.TryParse(potassiumText, out decimal potassium))
                                    {
                                        decimal potassiumMilligrams = potassium / 100m * 4700m; // Potassium - RDI: 4700mg

                                        PotassiumEntry.Text = potassiumMilligrams.ToString();
                                    }
                                }
                            }

                            if (cholestPercent)
                            {
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



                            if (vitAPercent)
                            {
                                string vitaminAText = VitaminAEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminAText))
                                {
                                    if (decimal.TryParse(vitaminAText, out decimal vitaminA))
                                    {
                                        decimal vitaminAMicrograms = vitaminA / 100m * 900m; // Vitamin A - RDI: 900mcg

                                        VitaminAEntry.Text = vitaminAMicrograms.ToString();
                                    }
                                }
                            }

                            if (vitCPercent)
                            {
                                string vitaminCText = VitaminCEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminCText))
                                {
                                    if (decimal.TryParse(vitaminCText, out decimal vitaminC))
                                    {
                                        decimal vitaminCMilligrams = vitaminC / 100m * 90m; // Vitamin C - RDI: 90mg

                                        VitaminCEntry.Text = vitaminCMilligrams.ToString();
                                    }
                                }
                            }

                            if (vitDPercent)
                            {
                                string vitaminDText = VitaminDEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminDText))
                                {
                                    if (decimal.TryParse(vitaminDText, out decimal vitaminD))
                                    {
                                        decimal vitaminDMicrograms = vitaminD / 100m * 15m; // Vitamin D - RDI: 15mcg

                                        VitaminDEntry.Text = vitaminDMicrograms.ToString();
                                    }
                                }
                            }

                            if (vitEPercent)
                            {
                                string vitaminEText = VitaminEEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminEText))
                                {
                                    if (decimal.TryParse(vitaminEText, out decimal vitaminE))
                                    {
                                        decimal vitaminEMilligrams = vitaminE / 100m * 15m; // Vitamin E - RDI: 15mg

                                        VitaminEEntry.Text = vitaminEMilligrams.ToString();
                                    }
                                }
                            }

                            if (vitKPercent)
                            {
                                string vitaminKText = VitaminKEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminKText))
                                {
                                    if (decimal.TryParse(vitaminKText, out decimal vitaminK))
                                    {
                                        decimal vitaminKMicrograms = vitaminK / 100m * 120m; // Vitamin K - RDI: 120mcg

                                        VitaminKEntry.Text = vitaminKMicrograms.ToString();
                                    }
                                }
                            }

                            if (thiaminPercent)
                            {
                                string thiaminText = ThiaminEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(thiaminText))
                                {
                                    if (decimal.TryParse(thiaminText, out decimal thiamin))
                                    {
                                        decimal thiaminMilligrams = thiamin / 100m * 1.2m; // Thiamin (Vitamin B1) - RDI: 1.2mg

                                        ThiaminEntry.Text = thiaminMilligrams.ToString();
                                    }
                                }
                            }

                            if (riboflavinPercent)
                            {
                                string riboflavinText = RiboflavinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(riboflavinText))
                                {
                                    if (decimal.TryParse(riboflavinText, out decimal riboflavin))
                                    {
                                        decimal riboflavinMilligrams = riboflavin / 100m * 1.3m; // Riboflavin (Vitamin B2) - RDI: 1.3mg

                                        RiboflavinEntry.Text = riboflavinMilligrams.ToString();
                                    }
                                }
                            }

                            if (niacinPercent)
                            {
                                string niacinText = NiacinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(niacinText))
                                {
                                    if (decimal.TryParse(niacinText, out decimal niacin))
                                    {
                                        decimal niacinMilligrams = niacin / 100m * 16m; // Niacin (Vitamin B3) - RDI: 16mg

                                        NiacinEntry.Text = niacinMilligrams.ToString();
                                    }
                                }
                            }

                            if (b6Percent)
                            {
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
                            }

                            if (biotinPercent)
                            {
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
                            }

                            if (folatePercent)
                            {
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
                            }

                            if (b12Percent)
                            {
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








                            if (Food.ServingUnit == "g") Food.Grams = Food.ServingSize;
                            else if (Food.ServingUnit == "mL") Food.Grams = Food.ServingSize;
                            else if (Food.ServingUnit == "oz") Food.Grams = Food.ServingSize * (decimal)28.3495;

                            Food.UserID = await _localData.GetUserID();
                            Food.Category = OtherCat.Text.Trim();
                            if (Food.UserID != 12)
                                    {
                                Food.Barcode = "General" + Food.FoodName.Trim().Replace(" ", "");
                                    }
                            else if (Food.UserID == 12)
                            {
                                Food.Barcode = "Verified" + Food.FoodName.Trim().Replace(" ", "");
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

                            (string email, string password) = await _localData.GetUserCredentials();
                            var userID = await _localData.GetUserID();

                            await _dataService.AddNutritionInfoAsync(Food, email, password, userID);
                           
                            var currentPage = Navigation.NavigationStack.LastOrDefault();
                            Navigation.InsertPageBefore(new FoodInfo(_dataService, _localData, Food, _mealType, _date), currentPage);
                            await Navigation.PopAsync();
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            if (ironPercent)
                            {
                                string ironText = IronEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(ironText))
                                {
                                    if (decimal.TryParse(ironText, out decimal iron))
                                    {
                                        decimal ironMilligrams = iron / 100m * 30m; // Iron - RDI: 30mg

                                        IronEntry.Text = ironMilligrams.ToString();
                                    }
                                }
                            }

                            if (calciumPercent)
                            {
                                string calciumText = CalciumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(calciumText))
                                {
                                    if (decimal.TryParse(calciumText, out decimal calcium))
                                    {
                                        decimal calciumMilligrams = calcium / 100m * 1300m; // Calcium - RDI: 1300mg

                                        CalciumEntry.Text = calciumMilligrams.ToString();
                                    }
                                }
                            }

                            if (sodiumPercent)
                            {
                                string sodiumText = SodiumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(sodiumText))
                                {
                                    if (decimal.TryParse(sodiumText, out decimal sodium))
                                    {
                                        decimal sodiumMilligrams = sodium / 100m * 2300m; // Sodium - RDI: 2300mg

                                        SodiumEntry.Text = sodiumMilligrams.ToString();
                                    }
                                }
                            }

                            if (potassiumPercent)
                            {
                                string potassiumText = PotassiumEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(potassiumText))
                                {
                                    if (decimal.TryParse(potassiumText, out decimal potassium))
                                    {
                                        decimal potassiumMilligrams = potassium / 100m * 4700m; // Potassium - RDI: 4700mg

                                        PotassiumEntry.Text = potassiumMilligrams.ToString();
                                    }
                                }
                            }

                            if (cholestPercent)
                            {
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



                            if (vitAPercent)
                            {
                                string vitaminAText = VitaminAEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminAText))
                                {
                                    if (decimal.TryParse(vitaminAText, out decimal vitaminA))
                                    {
                                        decimal vitaminAMicrograms = vitaminA / 100m * 900m; // Vitamin A - RDI: 900mcg

                                        VitaminAEntry.Text = vitaminAMicrograms.ToString();
                                    }
                                }
                            }

                            if (vitCPercent)
                            {
                                string vitaminCText = VitaminCEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminCText))
                                {
                                    if (decimal.TryParse(vitaminCText, out decimal vitaminC))
                                    {
                                        decimal vitaminCMilligrams = vitaminC / 100m * 90m; // Vitamin C - RDI: 90mg

                                        VitaminCEntry.Text = vitaminCMilligrams.ToString();
                                    }
                                }
                            }

                            if (vitDPercent)
                            {
                                string vitaminDText = VitaminDEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminDText))
                                {
                                    if (decimal.TryParse(vitaminDText, out decimal vitaminD))
                                    {
                                        decimal vitaminDMicrograms = vitaminD / 100m * 15m; // Vitamin D - RDI: 15mcg

                                        VitaminDEntry.Text = vitaminDMicrograms.ToString();
                                    }
                                }
                            }

                            if (vitEPercent)
                            {
                                string vitaminEText = VitaminEEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminEText))
                                {
                                    if (decimal.TryParse(vitaminEText, out decimal vitaminE))
                                    {
                                        decimal vitaminEMilligrams = vitaminE / 100m * 15m; // Vitamin E - RDI: 15mg

                                        VitaminEEntry.Text = vitaminEMilligrams.ToString();
                                    }
                                }
                            }

                            if (vitKPercent)
                            {
                                string vitaminKText = VitaminKEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(vitaminKText))
                                {
                                    if (decimal.TryParse(vitaminKText, out decimal vitaminK))
                                    {
                                        decimal vitaminKMicrograms = vitaminK / 100m * 120m; // Vitamin K - RDI: 120mcg

                                        VitaminKEntry.Text = vitaminKMicrograms.ToString();
                                    }
                                }
                            }

                            if (thiaminPercent)
                            {
                                string thiaminText = ThiaminEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(thiaminText))
                                {
                                    if (decimal.TryParse(thiaminText, out decimal thiamin))
                                    {
                                        decimal thiaminMilligrams = thiamin / 100m * 1.2m; // Thiamin (Vitamin B1) - RDI: 1.2mg

                                        ThiaminEntry.Text = thiaminMilligrams.ToString();
                                    }
                                }
                            }

                            if (riboflavinPercent)
                            {
                                string riboflavinText = RiboflavinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(riboflavinText))
                                {
                                    if (decimal.TryParse(riboflavinText, out decimal riboflavin))
                                    {
                                        decimal riboflavinMilligrams = riboflavin / 100m * 1.3m; // Riboflavin (Vitamin B2) - RDI: 1.3mg

                                        RiboflavinEntry.Text = riboflavinMilligrams.ToString();
                                    }
                                }
                            }

                            if (niacinPercent)
                            {
                                string niacinText = NiacinEntry.Text?.Trim();
                                if (!string.IsNullOrWhiteSpace(niacinText))
                                {
                                    if (decimal.TryParse(niacinText, out decimal niacin))
                                    {
                                        decimal niacinMilligrams = niacin / 100m * 16m; // Niacin (Vitamin B3) - RDI: 16mg

                                        NiacinEntry.Text = niacinMilligrams.ToString();
                                    }
                                }
                            }

                            if (b6Percent)
                            {
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
                            }

                            if (biotinPercent)
                            {
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
                            }

                            if (folatePercent)
                            {
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
                            }

                            if (b12Percent)
                            {
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



                            if (Food.ServingUnit == "g") Food.Grams = Food.ServingSize;
                            else if (Food.ServingUnit == "mL") Food.Grams = Food.ServingSize;
                            else if (Food.ServingUnit == "oz") Food.Grams = Food.ServingSize * (decimal)28.3495;

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
                    await DisplayAlert("Alert", "Please categorize the food into a meal and food group", "OK");
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




    void BarcodeEntry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        BarcodeEntry.Text = "General" + BarcodeEntry.Text;
    }

    void GramsClicked(System.Object sender, System.EventArgs e)
    {
        GramsButton.Background = Color.FromRgb(75, 158, 227);
        OzButton.Background = Colors.Transparent;
        mLButton.Background = Colors.Transparent;

        Food.ServingUnit = "g";
        CustomFood.ServingUnit = "g";

    }

    void OzClicked(System.Object sender, System.EventArgs e)
    {
        GramsButton.Background = Colors.Transparent;
        OzButton.Background = Color.FromRgb(75, 158, 227);
        mLButton.Background = Colors.Transparent;

        Food.ServingUnit = "oz";
        CustomFood.ServingUnit = "oz";
    }

    void mLClicked(System.Object sender, System.EventArgs e)
    {
        GramsButton.Background = Colors.Transparent;
        OzButton.Background = Colors.Transparent;
        mLButton.Background = Color.FromRgb(75, 158, 227);

        Food.ServingUnit = "mL";
        CustomFood.ServingUnit = "mL";
    }



    void BreakfastButton_Clicked(System.Object sender, System.EventArgs e)
    {
        BreakfastButton.Background = Color.FromRgb(75, 158, 227);
        LunchButton.Background = Colors.Transparent;
        DinnerButton.Background = Colors.Transparent;
        SnackButton.Background = Colors.Transparent;
        NoMealButton.Background = Colors.Transparent;

        Food.MealType = 0;

        mealSelected = true;
    }

    void LunchButton_Clicked(System.Object sender, System.EventArgs e)
    {
        LunchButton.Background = Color.FromRgb(75, 158, 227);
        BreakfastButton.Background = Colors.Transparent;
        DinnerButton.Background = Colors.Transparent;
        SnackButton.Background = Colors.Transparent;
        NoMealButton.Background = Colors.Transparent;

        Food.MealType = 1;

        mealSelected = true;
    }

    void DinnerButton_Clicked(System.Object sender, System.EventArgs e)
    {
        DinnerButton.Background = Color.FromRgb(75, 158, 227);
        LunchButton.Background = Colors.Transparent;
        BreakfastButton.Background = Colors.Transparent;
        SnackButton.Background = Colors.Transparent;
        NoMealButton.Background = Colors.Transparent;

        Food.MealType = 2;

        mealSelected = true;
    }

    void SnackButton_Clicked(System.Object sender, System.EventArgs e)
    {
        SnackButton.Background = Color.FromRgb(75, 158, 227);
        LunchButton.Background = Colors.Transparent;
        DinnerButton.Background = Colors.Transparent;
        BreakfastButton.Background = Colors.Transparent;
        NoMealButton.Background = Colors.Transparent;

        Food.MealType = 3;

        mealSelected = true;
    }

    void NoMealButton_Clicked(System.Object sender, System.EventArgs e)
    {
        NoMealButton.Background = Color.FromRgb(75, 158, 227);
        LunchButton.Background = Colors.Transparent;
        DinnerButton.Background = Colors.Transparent;
        SnackButton.Background = Colors.Transparent;
        BreakfastButton.Background = Colors.Transparent;

        Food.MealType = -1;

        mealSelected = true;
    }



    void FruitsButton_Clicked(System.Object sender, System.EventArgs e)
    {
        FruitsButton.Background = Color.FromRgb(75, 158, 227);
        VegButton.Background = Colors.Transparent;
        MeatsButton.Background = Colors.Transparent;
        GrainsButton.Background = Colors.Transparent;
        DairyButton.Background = Colors.Transparent;
        BeverageButton.Background = Colors.Transparent;
        PackagedButton.Background = Colors.Transparent;
        NoCatButton.Background = Colors.Transparent;

        OtherCat.Text = "Fruits";

        categorySelected = true;
    }

    void VegButton_Clicked(System.Object sender, System.EventArgs e)
    {
        VegButton.Background = Color.FromRgb(75, 158, 227);
        FruitsButton.Background = Colors.Transparent;
        MeatsButton.Background = Colors.Transparent;
        GrainsButton.Background = Colors.Transparent;
        DairyButton.Background = Colors.Transparent;
        BeverageButton.Background = Colors.Transparent;
        PackagedButton.Background = Colors.Transparent;
        NoCatButton.Background = Colors.Transparent;

        OtherCat.Text = "Vegetables";

        categorySelected = true;
    }

    void MeatsButton_Clicked(System.Object sender, System.EventArgs e)
    {
        MeatsButton.Background = Color.FromRgb(75, 158, 227);
        VegButton.Background = Colors.Transparent;
        FruitsButton.Background = Colors.Transparent;
        GrainsButton.Background = Colors.Transparent;
        DairyButton.Background = Colors.Transparent;
        BeverageButton.Background = Colors.Transparent;
        PackagedButton.Background = Colors.Transparent;
        NoCatButton.Background = Colors.Transparent;

        OtherCat.Text = "Meats";

        categorySelected = true;
    }

    void GrainsButton_Clicked(System.Object sender, System.EventArgs e)
    {
        GrainsButton.Background = Color.FromRgb(75, 158, 227);
        VegButton.Background = Colors.Transparent;
        MeatsButton.Background = Colors.Transparent;
        FruitsButton.Background = Colors.Transparent;
        DairyButton.Background = Colors.Transparent;
        BeverageButton.Background = Colors.Transparent;
        PackagedButton.Background = Colors.Transparent;
        NoCatButton.Background = Colors.Transparent;

        OtherCat.Text = "Grains";

        categorySelected = true;
    }

    void DairyButton_Clicked(System.Object sender, System.EventArgs e)
    {
        DairyButton.Background = Color.FromRgb(75, 158, 227);
        VegButton.Background = Colors.Transparent;
        MeatsButton.Background = Colors.Transparent;
        GrainsButton.Background = Colors.Transparent;
        FruitsButton.Background = Colors.Transparent;
        BeverageButton.Background = Colors.Transparent;
        NoCatButton.Background = Colors.Transparent;
        PackagedButton.Background = Colors.Transparent;

        OtherCat.Text = "Dairy";

        categorySelected = true;
    }

    void BeverageButton_Clicked(System.Object sender, System.EventArgs e)
    {
        BeverageButton.Background = Color.FromRgb(75, 158, 227);
        VegButton.Background = Colors.Transparent;
        MeatsButton.Background = Colors.Transparent;
        GrainsButton.Background = Colors.Transparent;
        DairyButton.Background = Colors.Transparent;
        FruitsButton.Background = Colors.Transparent;
        NoCatButton.Background = Colors.Transparent;
        PackagedButton.Background = Colors.Transparent;

        OtherCat.Text = "Beverage";

        categorySelected = true;
    }

    void PackagedButton_Clicked(System.Object sender, System.EventArgs e)
    {
        PackagedButton.Background = Color.FromRgb(75, 158, 227);
        VegButton.Background = Colors.Transparent;
        MeatsButton.Background = Colors.Transparent;
        GrainsButton.Background = Colors.Transparent;
        DairyButton.Background = Colors.Transparent;
        BeverageButton.Background = Colors.Transparent;
        FruitsButton.Background = Colors.Transparent;
        NoCatButton.Background = Colors.Transparent;

        OtherCat.Text = "Packaged";

        categorySelected = true;
    }

    void NoCatButton_Clicked(System.Object sender, System.EventArgs e)
    {
        NoCatButton.Background = Color.FromRgb(75, 158, 227);
        VegButton.Background = Colors.Transparent;
        MeatsButton.Background = Colors.Transparent;
        GrainsButton.Background = Colors.Transparent;
        DairyButton.Background = Colors.Transparent;
        BeverageButton.Background = Colors.Transparent;
        FruitsButton.Background = Colors.Transparent;
        PackagedButton.Background = Colors.Transparent;

        OtherCat.Text = null;

        categorySelected = true;
    }

    bool cholestPercent = false;
    bool sodiumPercent = false;

    void CholestUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (cholestPercent)
        {
            CholestUnitButton.Text = "mg";
            Cholesterol.Text = "Cholesterol (mg):";
        }
        else
        {
            CholestUnitButton.Text = " % ";
            Cholesterol.Text = "Cholesterol (%):";
        }

        cholestPercent = !cholestPercent;
    }

    void SodiumUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (sodiumPercent)
        {
            SodiumUnitButton.Text = "mg";
            Sodium.Text = "Sodium (mg):";
        }
        else
        {
            SodiumUnitButton.Text = " % ";
            Sodium.Text = "Sodium (%):";
        }

        sodiumPercent = !sodiumPercent;
    }

    bool vitDPercent = false;
    bool calciumPercent = false;
    bool ironPercent = false;
    bool potassiumPercent = false;
    bool vitAPercent = false;
    bool vitCPercent = false;
    bool vitEPercent = false;
    bool vitKPercent = false;
    bool thiaminPercent = false;
    bool riboflavinPercent = false;
    bool niacinPercent = false;
    bool b6Percent = false;
    bool folatePercent = false;
    bool b12Percent = false;
    bool biotinPercent = false;
    bool b5Percent = false;


    void VitaminDUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (vitDPercent)
        {
            VitaminDUnitButton.Text = "μg";
            VitaminD.Text = "Vitamin D (μg):";
        }
        else
        {
            VitaminDUnitButton.Text = " % ";
            VitaminD.Text = "Vitamin D (%):";
        }

        vitDPercent = !vitDPercent;
    }

    void CalciumUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (calciumPercent)
        {
            CalciumUnitButton.Text = "mg";
            Calcium.Text = "Calcium (mg):";
        }
        else
        {
            CalciumUnitButton.Text = " % ";
            Calcium.Text = "Calcium (%):";
        }

        calciumPercent = !calciumPercent;
    }

    void IronUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (ironPercent)
        {
            IronUnitButton.Text = "mg";
            Iron.Text = "Iron (mg):";
        }
        else
        {
            IronUnitButton.Text = " % ";
            Iron.Text = "Iron (%):";
        }

        ironPercent = !ironPercent;
    }

    void PotassiumUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (potassiumPercent)
        {
            PotassiumUnitButton.Text = "mg";
            Potassium.Text = "Potassium (mg):";
        }
        else
        {
            PotassiumUnitButton.Text = " % ";
            Potassium.Text = "Potassium (%):";
        }

        potassiumPercent = !potassiumPercent;
    }

    void VitaminAUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (vitAPercent)
        {
            VitaminAUnitButton.Text = "μg";
            VitaminA.Text = "Vitamin A (μg):";
        }
        else
        {
            VitaminAUnitButton.Text = " % ";
            VitaminA.Text = "Vitamin A (%):";
        }

        vitAPercent = !vitAPercent;
    }

    void VitaminCUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (vitCPercent)
        {
            VitaminCUnitButton.Text = "mg";
            VitaminC.Text = "Vitamin C (mg):";
        }
        else
        {
            VitaminCUnitButton.Text = " % ";
            VitaminC.Text = "Vitamin C (%):";
        }

        vitCPercent = !vitCPercent;
    }

    void VitaminEUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (vitEPercent)
        {
            VitaminEUnitButton.Text = "mg";
            VitaminE.Text = "Vitamin E (mg):";
        }
        else
        {
            VitaminEUnitButton.Text = " % ";
            VitaminE.Text = "Vitamin E (%):";
        }

        vitEPercent = !vitEPercent;
    }

    void VitaminKUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (vitKPercent)
        {
            VitaminKUnitButton.Text = "μg";
            VitaminK.Text = "Vitamin K (μg):";
        }
        else
        {
            VitaminKUnitButton.Text = " % ";
            VitaminK.Text = "Vitamin K (%):";
        }

        vitKPercent = !vitKPercent;
    }

    void ThiaminUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (thiaminPercent)
        {
            ThiaminUnitButton.Text = "mg";
            Thiamin.Text = "Thiamin [B1] (mg):";
        }
        else
        {
            ThiaminUnitButton.Text = " % ";
            Thiamin.Text = "Thiamin [B1] (%):";
        }

        thiaminPercent = !thiaminPercent;
    }

    void RiboflavinUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (riboflavinPercent)
        {
            RiboflavinUnitButton.Text = "mg";
            Riboflavin.Text = "Riboflavin [B2] (mg):";
        }
        else
        {
            RiboflavinUnitButton.Text = " % ";
            Riboflavin.Text = "Riboflavin [B2] (%):";
        }

        riboflavinPercent = !riboflavinPercent;
    }

    void NiacinUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (niacinPercent)
        {
            NiacinUnitButton.Text = "mg";
            Niacin.Text = "Niacin [B3] (mg):";
        }
        else
        {
            NiacinUnitButton.Text = " % ";
            Niacin.Text = "Niacin [B3] (%):";
        }

        niacinPercent = !niacinPercent;
    }

    void B6UnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (b6Percent)
        {
            B6UnitButton.Text = "mg";
            Pyridoxin.Text = "Vitamin B6 [Pyridoxin] (mg):";
        }
        else
        {
            B6UnitButton.Text = " % ";
            Pyridoxin.Text = "Vitamin B6 [Pyridoxin] (%):";
        }

        b6Percent = !b6Percent;
    }

    void FolateUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (folatePercent)
        {
            FolateUnitButton.Text = "μg";
            FolicAcid.Text = "Folic Acid/Folate [B9] (μg):";
        }
        else
        {
            FolateUnitButton.Text = " % ";
            FolicAcid.Text = "Folic Acid/Folate [B9] (%):";
        }

        folatePercent = !folatePercent;
    }

    void B12UnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (b12Percent)
        {
            B12UnitButton.Text = "μg";
            Cobalamin.Text = "Vitamin B12 [Cobalamin] (μg):";
        }
        else
        {
            B12UnitButton.Text = " % ";
            Cobalamin.Text = "Vitamin B12 [Cobalamin] (%):";
        }

        b12Percent = !b12Percent;
    }

    void BiotinUnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (biotinPercent)
        {
            BiotinUnitButton.Text = "μg";
            Biotin.Text = "Biotin [B7] (μg):";
        }
        else
        {
            BiotinUnitButton.Text = " % ";
            Biotin.Text = "Biotin [B7] (%):";
        }

        biotinPercent = !biotinPercent;
    }

    void B5UnitButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (b5Percent)
        {
            B5UnitButton.Text = "mg";
            PanthoAcid.Text = "Panthothenic Acid [B5] (mg):";
        }
        else
        {
            B5UnitButton.Text = " % ";
            PanthoAcid.Text = "Panthothenic Acid [B5] (%):";
        }

        b5Percent = !b5Percent;
    }

    void OtherCat_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
       
        categorySelected = true;
    }

    void OtherCat_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        NoCatButton.Background = Color.FromRgb(75, 158, 227);
        VegButton.Background = Colors.Transparent;
        MeatsButton.Background = Colors.Transparent;
        GrainsButton.Background = Colors.Transparent;
        DairyButton.Background = Colors.Transparent;
        BeverageButton.Background = Colors.Transparent;
        FruitsButton.Background = Colors.Transparent;
        PackagedButton.Background = Colors.Transparent;

        if (OtherCat.Text == "Fruits" || OtherCat.Text == "Vegetables" || OtherCat.Text == "Meats" || OtherCat.Text == "Grains" || OtherCat.Text == "Processed" || OtherCat.Text == "Dairy" || OtherCat.Text == "Beverage")
        {
            OtherCat.Text = null;
        }

    }
}

