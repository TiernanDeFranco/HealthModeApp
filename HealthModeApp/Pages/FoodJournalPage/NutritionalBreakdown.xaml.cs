using HealthModeApp.DataServices;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Diagnostics;
using System.Text.Json;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class NutritionalBreakdown : ContentPage
{
    private readonly ISQLiteDataService _localData;

    public NutritionalBreakdown(ISQLiteDataService localData, DateTime setDate)
    {
        Shell.SetTabBarIsVisible(this, false);
        InitializeComponent();
        _localData = localData;
        DateSelect.Date = setDate;
        DateHandler();
        PopulateFoodInfo();
        SeesAds();
        ThaiminName.IsVisible = true;
        ThiaminValue.IsVisible = true;
        ThiaminLeft.IsVisible = true;
        ThiaminBar.IsVisible = true;

        RiboName.IsVisible = true;
        RiboflavinValue.IsVisible = true;
        RiboflavinLeft.IsVisible = true;
        RiboflavinBar.IsVisible = true;

        NiacinName.IsVisible = true;
        NiacinValue.IsVisible = true;
        NiacinLeft.IsVisible = true;
        NiacinBar.IsVisible = true;

        B5Name.IsVisible = true;
        B5Value.IsVisible = true;
        B5Left.IsVisible = true;
        B5Bar.IsVisible = true;

        B6Name.IsVisible = true;
        B6Value.IsVisible = true;
        B6Left.IsVisible = true;
        B6Bar.IsVisible = true;

        BiotinName.IsVisible = true;
        BiotinValue.IsVisible = true;
        BiotinLeft.IsVisible = true;
        BiotinBar.IsVisible = false;

        FolicName.IsVisible = true;
        FolicAcidValue.IsVisible = true;
        FolicAcidLeft.IsVisible = true;
        FolicAcidBar.IsVisible = true;

        CobalaminName.IsVisible = true;
        CobalaminValue.IsVisible = true;
        CobalaminLeft.IsVisible = true;
        CobalaminBar.IsVisible = true;

    }

    async void DateHandler()
    {

        if (DateSelect.Date == DateTime.Today)
        {
            TodayButton.IsVisible = false;
        }
        else
        {
            TodayButton.IsVisible = true;

            var display = await _localData.GetPopUpSeen("TodayButtonPopup");
            if (display == false)
            {
                await DisplayAlert("Notice", "To navigate back the current date, you can click the newly created 'Jump To Today' Button", "OK");
                await _localData.AddPopUpSeen("TodayButtonPopup", true);
            }
        }

        var  userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[4];

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            DateSelect.Format = $"{dateFormat}";
        }
        else
        {
            DateSelect.Format = $"   {dateFormat}";
        }
    }


    async void SeesAds()
    {
        Ad.IsVisible = await _localData.GetSeesAds();
    }


    int totalCal = 0;
    decimal totalCarb = 0;
    decimal totalFat = 0;
    decimal totalProtein = 0;
    public LoggedFoodTable food;

    public int goalCal = 0;
    public string energyUnit;

    double totalSugar;
    double totalSatFat;
    double totalMUnSatFat;
    double totalPUnSatFat;
    double totalTransFat;

    double totalIron;
    double totalCalcium;
    double totalPotassium;
    double totalSodium;
    double totalCholesterol;

    double totalVitaminA;
    double totalThiamin;
    double totalRiboflavin;
    double totalNiacin;
    double totalB5;
    double totalB6;
    double totalBiotin;
    double totalFolicAcid;
    double totalCobalamin;
    double totalVitaminC;
    double totalVitaminD;
    double totalVitaminE;
    double totalVitaminK;

    async void PopulateFoodInfo()
    {
        totalCal = 0;
        totalCarb = 0;
        totalFat = 0;
        totalProtein = 0;
        goalCal = 0;

        totalSugar = 0;
        totalSatFat = 0;
        totalPUnSatFat = 0;
        totalMUnSatFat = 0;
        totalTransFat = 0;

        totalIron = 0;
        totalCalcium = 0;
        totalPotassium = 0;
        totalSodium = 0;
        totalCholesterol = 0;

        totalVitaminA = 0;
        totalThiamin = 0;
        totalRiboflavin = 0;
        totalNiacin = 0;
        totalB5 = 0;
        totalB6 = 0;
        totalBiotin = 0;
        totalFolicAcid = 0;
        totalCobalamin = 0;
        totalVitaminC = 0;
        totalVitaminD = 0;
        totalVitaminE = 0;
        totalVitaminK = 0;


        var userID = await _localData.GetUserID();


        var foods = new List<LoggedFoodTable>();

        var loggedFoods = await _localData.GetLoggedFoods(userID, DateSelect.Date);
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        energyUnit = unitList[2];


        var mealFoodIDs = loggedFoods.Select(f => f.LoggedFoodID).ToList();

        foreach (int foodID in mealFoodIDs)
        {
            food = await _localData.GetLoggedFoodDetails(userID, foodID);

            foods.Add(food);


        }

        var nutritionGoals = await _localData.GetNutritionGoals(userID, DateSelect.Date);
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
                    totalCarb += Math.Round((decimal)food.Carbs, 5);
                    totalFat += Math.Round((decimal)food.Fat, 5);
                    totalProtein += Math.Round((decimal)food.Protein, 5);

                totalSugar += Math.Round((double)(food.Sugar + food.AddSugar ?? 0), 3);
                totalSatFat += Math.Round((double)(food.SatFat ?? 0), 3);
                totalPUnSatFat += Math.Round((double)(food.PUnSatFat ?? 0), 3);
                totalMUnSatFat += Math.Round((double)(food.MUnSatFat ?? 0), 3);
                totalTransFat += Math.Round((double)(food.TransFat ?? 0), 3);

                totalIron += Math.Round((double)(food.Iron ?? 0), 3);
                totalCalcium += Math.Round((double)(food.Calcium ?? 0), 3);
                totalPotassium += Math.Round((double)(food.Potassium ?? 0), 3);
                totalSodium += Math.Round((double)(food.Sodium ?? 0), 3);
                totalCholesterol += Math.Round((double)(food.Cholesterol ?? 0), 3);

                totalVitaminA += Math.Round((double)(food.VitaminA ?? 0), 3);
                totalThiamin += Math.Round((double)(food.Thiamin ?? 0), 3);
                totalRiboflavin += Math.Round((double)(food.Riboflavin ?? 0), 3);
                totalNiacin += Math.Round((double)(food.Niacin ?? 0), 3);
                totalB5 += Math.Round((double)(food.B5 ?? 0), 3);
                totalB6 += Math.Round((double)(food.B6 ?? 0), 3);
                totalBiotin += Math.Round((double)(food.B7 ?? 0), 3);
                totalFolicAcid += Math.Round((double)(food.FolicAcid ?? 0), 3);
                totalCobalamin += Math.Round((double)(food.B12 ?? 0), 3);
                totalVitaminC += Math.Round((double)(food.VitaminC ?? 0), 3);
                totalVitaminD += Math.Round((double)(food.VitaminD ?? 0), 3);
                totalVitaminE += Math.Round((double)(food.VitaminE ?? 0), 3);
                totalVitaminK += Math.Round((double)(food.VitaminK ?? 0), 3);

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

            double frameWidth = PieGrid.Width;
            Debug.WriteLine(frameWidth);
            if (frameWidth < 350) frameWidth = PieGrid.Width * 1.05;
            if (frameWidth < 330) frameWidth = PieGrid.Width;


            double innerRadius = frameWidth * .5;
            if (innerRadius >= 170) innerRadius = frameWidth * .52;
            if (innerRadius < 170 && innerRadius >= 150) innerRadius = frameWidth * .38;
            if (frameWidth < 150) innerRadius = frameWidth * .3;

            Donut.WidthRequest = frameWidth * .55;
            Donut.HeightRequest = frameWidth * .55;

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
            

            Donut.InitialRotation = 80;

            if (foods.Count > 0)
            {
                CalorieLabel.Text = totalCal.ToString();
                CarbPercent.Text = (carbPercent / 100).ToString("0.#" + "%");
                FatPercent.Text = (fatPercent / 100).ToString("0.#" + "%");
                ProteinPercent.Text = (proteinPercent / 100).ToString("0.#" + "%");
            }

            if (foods.Count == 0)
            {
                CalorieLabel.Text = totalCal.ToString();
                CarbPercent.Text = (0).ToString("0.#" + "%");
                FatPercent.Text = (0).ToString("0.#" + "%");
                ProteinPercent.Text = (0).ToString("0.#" + "%");
            }

            CarbLabel.Text = totalCarb.ToString("0.#" + "g");
            FatLabel.Text = totalFat.ToString("0.#" + "g");
            ProteinLabel.Text = totalProtein.ToString("0.#" + "g");

            double carbPercentage = ((double)totalCarb / nutritionGoals.CarbGoal);
            double fatPercentage = ((double)totalFat / nutritionGoals.FatGoal);
            double proteinPercentage = ((double)totalProtein / nutritionGoals.ProteinGoal);

            CarbGoal.Text = carbPercentage.ToString("0.#" + "%");
            FatGoal.Text = fatPercentage.ToString("0.#" + "%");
            ProteinGoal.Text = proteinPercentage.ToString("0.#" + "%");

            Debug.WriteLine(carbPercentage);
            Debug.WriteLine(fatPercentage);
            Debug.WriteLine(proteinPercentage);

            

            CarbBar.ProgressTo((carbPercentage + .01), 1500, Easing.CubicOut);
            FatBar.ProgressTo((fatPercentage + .01), 1500, Easing.CubicOut);
            ProteinBar.ProgressTo((proteinPercentage + .01), 1500, Easing.CubicOut);
            CalorieBar.ProgressTo(((double)totalCal / goalCal) + .02, 1200, Easing.CubicOut);

            CalorieBar.WidthRequest = frameWidth / 5;


            if (totalCal < 100) { CalorieLabel.FontSize = 32;                           CalIcon.WidthRequest = 30; CalIcon.HeightRequest = 30; }
            else if (totalCal < 1000 && totalCal >= 100) { CalorieLabel.FontSize = 30; CalIcon.WidthRequest = 25; CalIcon.HeightRequest = 25; }
            else if (totalCal < 10000 && totalCal >= 1000) { CalorieLabel.FontSize = 28; CalIcon.WidthRequest = 23; CalIcon.HeightRequest = 23; }
            else if (totalCal >= 10000) { CalorieLabel.FontSize = 25;                CalIcon.WidthRequest = 21; CalIcon.HeightRequest = 21; }

            Donut.Series = SeriesCollection;






            SugarValue.Text = totalSugar.ToString("0.#g");
            SugarLeft.Text = (nutritionGoals.SugarGoal - totalSugar).ToString("0.#g");

            SatFatValue.Text = totalSatFat.ToString("0.#g");
            SatFatLeft.Text = (nutritionGoals.SatdFatGoal - totalSatFat).ToString("0.#g");

            PUnSatFatValue.Text = totalPUnSatFat.ToString("0.#g");
            PUnSatFatLeft.Text = (nutritionGoals.PUnSatFatGoal - totalPUnSatFat).ToString("0.#g");

            MUnSatFatValue.Text = totalMUnSatFat.ToString("0.#g");
            MUnSatFatLeft.Text = (nutritionGoals.MUnSatFatGoal - totalMUnSatFat).ToString("0.#g");

            TransFatValue.Text = totalTransFat.ToString("0.#g");
            TransFatLeft.Text = (nutritionGoals.TransFatGoal - totalTransFat).ToString("0.#g");

            IronValue.Text = totalIron.ToString("0.#mg");
            IronLeft.Text = (nutritionGoals.IronGoal - totalIron).ToString("0.#mg");

            CalciumValue.Text = totalCalcium.ToString("0.#mg");
            CalciumLeft.Text = (nutritionGoals.CalciumGoal - totalCalcium).ToString("0.#mg");

            PotassiumValue.Text = totalPotassium.ToString("0.#mg");
            PotassiumLeft.Text = (nutritionGoals.PotassiumGoal - totalPotassium).ToString("0.#mg");

            SodiumValue.Text = totalSodium.ToString("0.#mg");
            SodiumLeft.Text = (nutritionGoals.SodiumGoal - totalSodium).ToString("0.#mg");

            CholesterolValue.Text = totalCholesterol.ToString("0.#mg");
            CholesterolLeft.Text = (nutritionGoals.CholesterolGoal - totalCholesterol).ToString("0.#mg");


            VitaminAValue.Text = totalVitaminA.ToString("0.#μg");
            VitaminALeft.Text = (nutritionGoals.VitaminAGoal - totalVitaminA).ToString("0.#μg");

            ThiaminValue.Text = totalThiamin.ToString("0.#mg");
            ThiaminLeft.Text = (nutritionGoals.ThiaminGoal - totalThiamin).ToString("0.#mg");

            RiboflavinValue.Text = totalRiboflavin.ToString("0.#mg");
            RiboflavinLeft.Text = (nutritionGoals.RiboflavinGoal - totalRiboflavin).ToString("0.#mg");

            NiacinValue.Text = totalNiacin.ToString("0.#mg");
            NiacinLeft.Text = (nutritionGoals.NiacinGoal - totalNiacin).ToString("0.#mg");

            B5Value.Text = totalB5.ToString("0.#mg");
            B5Left.Text = (nutritionGoals.VitaminB5Goal - totalB5).ToString("0.#mg");

            B6Value.Text = totalB6.ToString("0.#mg");
            B6Left.Text = (nutritionGoals.VitaminB6Goal - totalB6).ToString("0.#mg");

            BiotinValue.Text = totalBiotin.ToString("0.#μg");
            BiotinLeft.Text = (nutritionGoals.BiotinGoal - totalBiotin).ToString("0.#μg");

            FolicAcidValue.Text = totalFolicAcid.ToString("0.#μg");
            FolicAcidLeft.Text = (nutritionGoals.FolicAcidGoal - totalFolicAcid).ToString("0.#μg");

            CobalaminValue.Text = totalCobalamin.ToString("0.#μg");
            CobalaminLeft.Text = (nutritionGoals.CobalamineGoal - totalCobalamin).ToString("0.#μg");

            VitaminCValue.Text = totalVitaminC.ToString("0.#mg");
            VitaminCLeft.Text = (nutritionGoals.VitaminCGoal - totalVitaminC).ToString("0.#mg");

            VitaminDValue.Text = totalVitaminD.ToString("0.#μg");
            VitaminDLeft.Text = (nutritionGoals.VitaminDGoal - totalVitaminD).ToString("0.#μg");

            VitaminEValue.Text = totalVitaminE.ToString("0.#mg");
            VitaminELeft.Text = (nutritionGoals.VitaminEGoal - totalVitaminE).ToString("0.#mg");

            VitaminKValue.Text = totalVitaminK.ToString("0.#μg");
            VitaminKLeft.Text = (nutritionGoals.VitaminKGoal - totalVitaminK).ToString("0.#μg");



            SugarBar.ProgressTo((totalSugar/nutritionGoals.SugarGoal) + .02, 1500, Easing.CubicOut);

            SatFatBar.ProgressTo((totalSatFat / nutritionGoals.SatdFatGoal) + .02, 1500, Easing.CubicOut);
            PUnSatFatBar.ProgressTo((totalPUnSatFat / nutritionGoals.PUnSatFatGoal) + .02, 1500, Easing.CubicOut);
            MUnSatFatBar.ProgressTo((totalMUnSatFat / nutritionGoals.MUnSatFatGoal) + .02, 1500, Easing.CubicOut);
            TransFatBar.ProgressTo((totalTransFat) + .02, 1500, Easing.CubicOut);

            IronBar.ProgressTo((totalIron / nutritionGoals.IronGoal) + .02, 1500, Easing.CubicOut);
            CalciumBar.ProgressTo((totalCalcium / nutritionGoals.CalciumGoal) + .02, 1500, Easing.CubicOut);
            PotassiumBar.ProgressTo((totalPotassium / nutritionGoals.PotassiumGoal) + .02, 1500, Easing.CubicOut);
            SodiumBar.ProgressTo((totalSodium / nutritionGoals.SodiumGoal) + .02, 1500, Easing.CubicOut);
            CholesterolBar.ProgressTo((totalCholesterol / nutritionGoals.CholesterolGoal) + .02, 1500, Easing.CubicOut);

            VitaminABar.ProgressTo((totalVitaminA / nutritionGoals.VitaminAGoal) + .02, 1500, Easing.CubicOut);
            ThiaminBar.ProgressTo((totalThiamin / nutritionGoals.ThiaminGoal) + .02, 1500, Easing.CubicOut);
            RiboflavinBar.ProgressTo((totalRiboflavin / nutritionGoals.RiboflavinGoal) + .02, 1500, Easing.CubicOut);
            NiacinBar.ProgressTo((totalNiacin / nutritionGoals.NiacinGoal) + .02, 1500, Easing.CubicOut);
            B5Bar.ProgressTo((totalB5 / nutritionGoals.VitaminB5Goal) + .02, 1500, Easing.CubicOut);
            B6Bar.ProgressTo((totalB6 / nutritionGoals.VitaminB6Goal) + .02, 1500, Easing.CubicOut);
            BiotinBar.ProgressTo((totalBiotin / nutritionGoals.BiotinGoal) + .02, 1500, Easing.CubicOut);
            FolicAcidBar.ProgressTo((totalFolicAcid / nutritionGoals.FolicAcidGoal) + .02, 1500, Easing.CubicOut);
            CobalaminBar.ProgressTo((totalCobalamin / nutritionGoals.CobalamineGoal) + .02, 1500, Easing.CubicOut);
            VitaminCBar.ProgressTo((totalVitaminC / nutritionGoals.VitaminCGoal) + .02, 1500, Easing.CubicOut);
            VitaminDBar.ProgressTo((totalVitaminD / nutritionGoals.VitaminDGoal) + .02, 1500, Easing.CubicOut);
            VitaminEBar.ProgressTo((totalVitaminE / nutritionGoals.VitaminEGoal) + .02, 1500, Easing.CubicOut);
            VitaminKBar.ProgressTo((totalVitaminK / nutritionGoals.VitaminKGoal) + .02, 1500, Easing.CubicOut);

        }


    }


    public static (double carbPercent, double fatPercent, double proteinPercent) CalculateMacrosPercentage(int totalCalories = 0, decimal totalCarbs = 0, decimal totalFat = 0, decimal totalProtein = 0)
    {
        // Calculate the percentage of calories from each macro
        double carbCalories = (double)totalCarbs * 4;
        double proteinCalories = (double)totalProtein * 4;
        double fatCalories = (double)totalFat * 9;
        double carbPercentage = carbCalories / totalCalories * 100;
        double proteinPercentage = proteinCalories / totalCalories * 100;
        double fatPercentage = fatCalories / totalCalories * 100;

        // Return a Tuple containing the macro percentages
        return (carbPercentage, fatPercentage, proteinPercentage);
    }




    void DateSelect_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
        {
            DateHandler();
            PopulateFoodInfo();
        }

        void TodayButton_Clicked(System.Object sender, System.EventArgs e)
        {
            DateSelect.Date = DateTime.Today;
        }

   
    
}
