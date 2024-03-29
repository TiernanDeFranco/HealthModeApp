﻿using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using HealthModeApp.DataServices;
using Microsoft.Maui.Controls;

namespace HealthModeApp.Pages;


public partial class SignUpPage : ContentPage
{
    private readonly ISQLiteDataService _localData;
    private readonly IRestDataService _dataService;
    public int weightGoal = -4; //-1 lose 0 maintain 1 gain
    public int activityLevel = 0;
    public int stage = 1;
    public int maxStage = 6;
    double heightCm = 0;
    decimal weight;
    decimal goalWeight; 
    decimal height;
    int calorieGoal;

    void UpdatePage()
    {
        var prog = (double)(stage-1) / (double)maxStage;
        SignUpProg.ProgressTo((prog + .01), 1000, Easing.CubicOut);
        
        switch (weightGoal)
        {
            case -1:
                GoalWeight.IsVisible = true;
                Grid.SetColumnSpan(Weight, 1);
                break;
            case 0:
                GoalWeight.IsVisible = false;
                Grid.SetColumnSpan(Weight, 2);
                GoalWeight.Text = Weight.Text;
                break;
            case 1:
                GoalWeight.IsVisible = true;
                Grid.SetColumnSpan(Weight, 1);
                break;

        }

        switch (stage)
        {
            case 1:
                Grid1.IsVisible = true;
                Grid2.IsVisible = false;
                Grid3.IsVisible = false;
                Grid4.IsVisible = false;
                Grid5.IsVisible = false;
                Grid6.IsVisible = false;
                SignUpFrame.IsVisible = false;
                break;

            case 2:
                Grid1.IsVisible = false;
                Grid2.IsVisible = true;
                Grid3.IsVisible = false;
                Grid4.IsVisible = false;
                Grid5.IsVisible = false;
                Grid6.IsVisible = false;
                SignUpFrame.IsVisible = false;
                break;

            case 3:
                Grid1.IsVisible = false;
                Grid2.IsVisible = false;
                Grid3.IsVisible = true;
                Grid4.IsVisible = false;
                Grid5.IsVisible = false;
                Grid6.IsVisible = false;
                SignUpFrame.IsVisible = false;
                break;

            case 4:
                Grid1.IsVisible = false;
                Grid2.IsVisible = false;
                Grid3.IsVisible = false;
                Grid4.IsVisible = true;
                Grid5.IsVisible = false;
                Grid6.IsVisible = false;
                SignUpFrame.IsVisible = false;
                break;

            case 5:
                Grid1.IsVisible = false;
                Grid2.IsVisible = false;
                Grid3.IsVisible = false;
                Grid4.IsVisible = false;
                Grid5.IsVisible = true;
                Grid6.IsVisible = false;
                SignUpFrame.IsVisible = false;
                break;

            case 6:
                Grid1.IsVisible = false;
                Grid2.IsVisible = false;
                Grid3.IsVisible = false;
                Grid4.IsVisible = false;
                Grid5.IsVisible = false;
                Grid6.IsVisible = true;
                SignUpFrame.IsVisible = false;
                break;

            case 7:
                Grid1.IsVisible = false;
                Grid2.IsVisible = false;
                Grid3.IsVisible = false;
                Grid4.IsVisible = false;
                Grid5.IsVisible = false;
                Grid6.IsVisible = false;
                SignUpFrame.IsVisible = true;
                break;


        };
    }

	public SignUpPage(ISQLiteDataService localData, IRestDataService dataService)
	{
		InitializeComponent();
        UpdatePage();
        _localData = localData;
        _dataService = dataService;
        Logo.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .28;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);
        NavigationPage.SetHasNavigationBar(this, false);

    }

    //Weight Goals VVVV

    void LoseWeightClicked(System.Object sender, System.EventArgs e)
    {
        if (weightGoal == -1) { weightGoal = -4; }
        else { weightGoal = -1; }
        UpdateWeightButtons();
    }

    void MaintainWeightClicked(System.Object sender, System.EventArgs e)
    {
        if (weightGoal == 0) { weightGoal = -4; }
        else { weightGoal = 0; }
        UpdateWeightButtons();
    }

    void GainWeightClicked(System.Object sender, System.EventArgs e)
    {
        if (weightGoal == 1) { weightGoal = -4; }
        else { weightGoal = 1; }
        

        UpdateWeightButtons();
    }

    async void BackButton1_Clicked(System.Object sender, System.EventArgs e)
    {
         await Navigation.PopModalAsync(); 
        
    }

    async void NextButton1_Clicked(System.Object sender, System.EventArgs e)
    {
        switch (weightGoal)
        {
            case -4:
                // None of the radio buttons were selected
                await DisplayAlert("Notice", "Please select a goal", "OK");
                stage = 1;
                break;
            case -1:
                //Lose Weight
                stage = 2;
                break;
            case 0:
                //maintain Weight
                stage = 2;
                break;
            case 1:
                //gain weight
                stage = 2;
                break;
        }

        UpdatePage();


    }

    void UpdateWeightButtons()
    {
      
        switch (weightGoal)
        {
            case -4:
                LoseWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                MaintainWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                GainWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                LoseWeightButton.Background = Color.FromRgba(0, 0, 0, 0);
                MaintainWeightButton.Background = Color.FromRgba(0, 0, 0, 0);
                GainWeightButton.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case -1:
                LoseWeightButton.BackgroundColor = Color.FromRgb(75, 158, 227);
                MaintainWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                GainWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                LoseWeightButton.Background = Color.FromRgb(75, 158, 227);
                MaintainWeightButton.Background = Color.FromRgba(0, 0, 0, 0);
                GainWeightButton.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 0:
                LoseWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                MaintainWeightButton.BackgroundColor = Color.FromRgb(75, 158, 227);
                GainWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                LoseWeightButton.Background = Color.FromRgba(0, 0, 0, 0);
                MaintainWeightButton.Background = Color.FromRgb(75, 158, 227);
                GainWeightButton.Background = Color.FromRgba(0, 0, 0, 0);

                break;
            case 1:
                LoseWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                MaintainWeightButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                GainWeightButton.BackgroundColor = Color.FromRgb(75, 158, 227);

                LoseWeightButton.Background = Color.FromRgba(0, 0, 0, 0);
                MaintainWeightButton.Background = Color.FromRgba(0, 0, 0, 0);
                GainWeightButton.Background = Color.FromRgb(75, 158, 227);
                break;
        }

    }
    //Weight Goals ^^^^
    //Health Goals VVVV

    public bool _loseFatClicked = false;
    public bool _buildMuscleClicked = false;
    public bool _modDietClicked = false;
    public bool _activityClicked = false;
    public bool _trackHealthClicked = false;
    public int _amountClicked = 0;

    void LoseFatClicked(System.Object sender, System.EventArgs e)
    {
        if (_amountClicked >= 3 && !_loseFatClicked)
        {
            return; // exit the method without doing anything
        }

        _loseFatClicked = !_loseFatClicked;
        if (_loseFatClicked)
        {
            _amountClicked += 1;
            LoseFatButton.BackgroundColor = Color.FromRgb(75, 158, 227);
            LoseFatButton.Background = Color.FromRgb(75, 158, 227);
        }
        else
        {
            _amountClicked -= 1;
            LoseFatButton.BackgroundColor = Color.FromRgba(0, 0, 0 ,0);
            LoseFatButton.Background = Color.FromRgba(0, 0, 0, 0);
        }
    }


    void BuildMuscleClicked(System.Object sender, System.EventArgs e)
    {
        if (_amountClicked >= 3 && !_buildMuscleClicked)
            return;

        _buildMuscleClicked = !_buildMuscleClicked;

        if (_buildMuscleClicked)
        {
            _amountClicked += 1;
            BuildMuscleButton.BackgroundColor = Color.FromRgb(75, 158, 227);
            BuildMuscleButton.Background = Color.FromRgb(75, 158, 227);
        }
        else
        {
            _amountClicked -= 1;
            BuildMuscleButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            BuildMuscleButton.Background = Color.FromRgba(0, 0, 0, 0);
        }
    }

    void ModDietClicked(System.Object sender, System.EventArgs e)
    {
        if (_amountClicked >= 3 && !_modDietClicked)
            return;

        _modDietClicked = !_modDietClicked;

        if (_modDietClicked)
        {
            _amountClicked += 1;
            ModifyDietButton.BackgroundColor = Color.FromRgb(75, 158, 227);
            ModifyDietButton.Background = Color.FromRgb(75, 158, 227);
        }
        else
        {
            _amountClicked -= 1;
            ModifyDietButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            ModifyDietButton.Background = Color.FromRgba(0, 0, 0, 0);
        }
    }

    void IncActivClicked(System.Object sender, System.EventArgs e)
    {
        if (_amountClicked >= 3 && !_activityClicked)
            return;

        _activityClicked = !_activityClicked;

        if (_activityClicked)
        {
            _amountClicked += 1;
            IncreaseActivityButton.BackgroundColor = Color.FromRgb(75, 158, 227);
            IncreaseActivityButton.Background = Color.FromRgb(75, 158, 227);
        }
        else
        {
            _amountClicked -= 1;
            IncreaseActivityButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            IncreaseActivityButton.Background = Color.FromRgba(0, 0, 0, 0);
        }
    }

    void TrackHealthClicked(System.Object sender, System.EventArgs e)
    {
        if (_amountClicked >= 3 && !_trackHealthClicked)
            return;

        _trackHealthClicked = !_trackHealthClicked;

        if (_trackHealthClicked)
        {
            _amountClicked += 1;
            TrackHealthButton.BackgroundColor = Color.FromRgb(75, 158, 227);
            TrackHealthButton.Background = Color.FromRgb(75, 158, 227);
        }
        else
        {
            _amountClicked -= 1;
            TrackHealthButton.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            TrackHealthButton.Background = Color.FromRgba(0, 0, 0, 0);
        }
    }



    void BackButton2_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 1;
        UpdatePage();
    }

    async void NextButton2_Clicked(System.Object sender, System.EventArgs e)
    {
       if (_amountClicked != 0)
        {
            stage = 3;

        }
       else
        {
            stage = 2;
            await DisplayAlert("Notice", "Please select a goal", "OK");
        }
        UpdatePage();
        UpdateMeasurementButtons();
    }

    //Health Goals ^^^
    //Measurement Choices VVVV

    public int unitWeight = 0;
    public int unitLength = 0;
    public int unitWater = 1;
    public int unitEnergy = 1;
    public int EunitEnergy = 1;
    public int formatDate = 1;

    void UpdateMeasurementButtons()
    {
        var weightLbs = LbsButton;
        var weightKg = KgButton;
        var lengthInch = InButton;
        var lengthCm = CmButton;
        var drinkWaterFlOz = FlOzButton;
        var drinkWaterCups = CupsButton;
        var drinkWaterML = mLButton;
        var energyKcal = KcalButton;
        var energyCalories = CaloriesButton;
        var energyKj = KjButton;
        var energyEKcal = EKcalButton;
        var energyECalories = ECaloriesButton;
        var energyEKj = EKjButton;
        var dateFormatDmy = DmyButton;
        var dateFormatMdy = MdyButton;
        var dateFormatYmd = YmdButton;

        switch (unitWeight)
        {
            case 0:
                weightLbs.BackgroundColor = Color.FromRgb(75, 158, 227);
                weightKg.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                weightLbs.Background = Color.FromRgb(75, 158, 227);
                weightKg.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 1:
                weightLbs.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                weightKg.BackgroundColor = Color.FromRgb(75, 158, 227);

                weightLbs.Background = Color.FromRgba(0, 0, 0, 0);
                weightKg.Background = Color.FromRgb(75, 158, 227);
                break;
        }

        switch (unitLength)
        {
            case 0:
                lengthInch.Background = Color.FromRgb(75, 158, 227);
                lengthCm.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 1:
                lengthInch.Background = Color.FromRgba(0, 0, 0, 0);
                lengthCm.Background = Color.FromRgb(75, 158, 227);
                break;
        }

        switch (unitWater)
        {
            case 0:
                drinkWaterFlOz.BackgroundColor = Color.FromRgb(75, 158, 227);
                drinkWaterCups.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                drinkWaterML.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                drinkWaterFlOz.Background  = Color.FromRgb(75, 158, 227);
                drinkWaterCups.Background = Color.FromRgba(0, 0, 0, 0);
                drinkWaterML.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 1:
                drinkWaterFlOz.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                drinkWaterCups.BackgroundColor = Color.FromRgb(75, 158, 227);
                drinkWaterML.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                drinkWaterFlOz.Background = Color.FromRgba(0, 0, 0, 0);
                drinkWaterCups.Background = Color.FromRgb(75, 158, 227);
                drinkWaterML.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 2:
                drinkWaterFlOz.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                drinkWaterCups.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                drinkWaterML.BackgroundColor = Color.FromRgb(75, 158, 227);

                drinkWaterFlOz.Background = Color.FromRgba(0, 0, 0, 0);
                drinkWaterCups.Background = Color.FromRgba(0, 0, 0, 0);
                drinkWaterML.Background = Color.FromRgb(75, 158, 227);
                break;
        }

        switch (unitEnergy)
        {
            case 0:
                energyKcal.BackgroundColor = Color.FromRgb(75, 158, 227);
                energyCalories.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                energyKj.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                energyKcal.Background = Color.FromRgb(75, 158, 227);
                energyCalories.Background = Color.FromRgba(0, 0, 0, 0);
                energyKj.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 1:
                energyKcal.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                energyCalories.BackgroundColor = Color.FromRgb(75, 158, 227);
                energyKj.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                energyKcal.Background = Color.FromRgba(0, 0, 0, 0);
                energyCalories.Background = Color.FromRgb(75, 158, 227);
                energyKj.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 2:
                energyKcal.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                energyCalories.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                energyKj.BackgroundColor = Color.FromRgb(75, 158, 227);

                energyKcal.Background = Color.FromRgba(0, 0, 0, 0);
                energyCalories.Background = Color.FromRgba(0, 0, 0, 0);
                energyKj.Background = Color.FromRgb(75, 158, 227);
                break;
        }

        switch (EunitEnergy)
        {
            case 0:
                energyEKcal.BackgroundColor = Color.FromRgb(75, 158, 227);
                energyECalories.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                energyEKj.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                energyEKcal.Background = Color.FromRgb(75, 158, 227);
                energyECalories.Background = Color.FromRgba(0, 0, 0, 0);
                energyEKj.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 1:
                energyEKcal.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                energyECalories.BackgroundColor = Color.FromRgb(75, 158, 227);
                energyEKj.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                energyEKcal.Background = Color.FromRgba(0, 0, 0, 0);
                energyECalories.Background = Color.FromRgb(75, 158, 227);
                energyEKj.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 2:
                energyEKcal.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                energyECalories.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                energyEKj.BackgroundColor = Color.FromRgb(75, 158, 227);

                energyEKcal.Background = Color.FromRgba(0, 0, 0, 0);
                energyECalories.Background = Color.FromRgba(0, 0, 0, 0);
                energyEKj.Background = Color.FromRgb(75, 158, 227);
                break;
        }

        switch (formatDate)
        {
            case 0:
                dateFormatDmy.BackgroundColor = Color.FromRgb(75, 158, 227);
                dateFormatMdy.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                dateFormatYmd.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                dateFormatDmy.Background = Color.FromRgb(75, 158, 227);
                dateFormatMdy.Background = Color.FromRgba(0, 0, 0, 0);
                dateFormatYmd.Background = Color.FromRgba(0, 0, 0, 0);

                DateDisplay.Text = DateTime.Today.ToString("dd MMM, yyyy");

                BirthdayDate.Format = "dd/MM/yyyy";
                break;
            case 1:
                dateFormatDmy.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                dateFormatMdy.BackgroundColor = Color.FromRgb(75, 158, 227);
                dateFormatYmd.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                dateFormatDmy.Background = Color.FromRgba(0, 0, 0, 0);
                dateFormatMdy.Background = Color.FromRgb(75, 158, 227);
                dateFormatYmd.Background = Color.FromRgba(0, 0, 0, 0);
                DateDisplay.Text = DateTime.Today.ToString("MMM dd, yyyy");


                BirthdayDate.Format = "MM/dd/yyyy";
                break;
            case 2:
                dateFormatDmy.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                dateFormatMdy.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                dateFormatYmd.BackgroundColor = Color.FromRgb(75, 158, 227);

                dateFormatDmy.Background = Color.FromRgba(0, 0, 0, 0);
                dateFormatMdy.Background = Color.FromRgba(0, 0, 0, 0);
                dateFormatYmd.Background = Color.FromRgb(75, 158, 227);
                DateDisplay.Text = DateTime.Today.ToString("yyyy: MMM dd");

                BirthdayDate.Format = "yyyy/MM/dd";
                break;
        }

        DmyButton.Text = DateTime.Today.ToString("dd/MM/yyyy");
        MdyButton.Text = DateTime.Today.ToString("MM/dd/yyyy");
        YmdButton.Text = DateTime.Today.ToString("yyyy/MM/dd");
        BirthdayDate.MaximumDate = DateTime.Today;
        BirthdayDate.Date = DateTime.Today;
    }

    void LbsClicked(System.Object sender, System.EventArgs e)
    {
        unitWeight = 0;
        UpdateMeasurementButtons();
    }
    void KgClicked(System.Object sender, System.EventArgs e)
    {
        unitWeight = 1;
        UpdateMeasurementButtons();
    }

    void InButton_Clicked(System.Object sender, System.EventArgs e)
    {
        unitLength = 0;
        UpdateMeasurementButtons();
    }

    void CmButton_Clicked(System.Object sender, System.EventArgs e)
    {
        unitLength = 1;
        UpdateMeasurementButtons();
    }

    void FlOzClicked(System.Object sender, System.EventArgs e)
    {
        unitWater = 0;
        UpdateMeasurementButtons();
    }

    void CupsClicked(System.Object sender, System.EventArgs e)
    {
        unitWater = 1;
        UpdateMeasurementButtons();
    }

    void MLClicked(System.Object sender, System.EventArgs e)
    {
        unitWater = 2;
        UpdateMeasurementButtons();
    }

    void kCalClicked(System.Object sender, System.EventArgs e)
    {
        unitEnergy = 0;
        UpdateMeasurementButtons();
    }

    void CaloriesClicked(System.Object sender, System.EventArgs e)
    {
        unitEnergy = 1;
        UpdateMeasurementButtons();
    }

    void kJClicked(System.Object sender, System.EventArgs e)
    {
        unitEnergy = 2;
        UpdateMeasurementButtons();
    }

    void EkCalClicked(System.Object sender, System.EventArgs e)
    {
        EunitEnergy = 0;
        UpdateMeasurementButtons();
    }

    void ECaloriesClicked(System.Object sender, System.EventArgs e)
    {
        EunitEnergy = 1;
        UpdateMeasurementButtons();
    }

    void EkJClicked(System.Object sender, System.EventArgs e)
    {
        EunitEnergy = 2;
        UpdateMeasurementButtons();
    }

    void DMYClicked(System.Object sender, System.EventArgs e)
    {
        formatDate = 0;
        UpdateMeasurementButtons();
    }

    void MDYClicked(System.Object sender, System.EventArgs e)
    {
        formatDate = 1;
        UpdateMeasurementButtons();
    }

    void YMDClicked(System.Object sender, System.EventArgs e)
    {
        formatDate = 2;
        UpdateMeasurementButtons();
    }

    void BackButton3_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 2;
        UpdatePage();
    }

    void NextButton3_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 4;
        if (unitLength == 0)
        {
            heightFeetEntry.IsVisible = true;
            heightInchesEntry.IsVisible = true;
            heightCmEntry.IsVisible = false;
        }
        else
        {
            heightFeetEntry.IsVisible = false;
            heightInchesEntry.IsVisible = false;
            heightCmEntry.IsVisible = true;
        }
        UpdatePage();
    }

    //Unit Select ^^^^


    void CheckNegatives()
    {
        if (!string.IsNullOrEmpty(heightCmEntry.Text) && double.TryParse(heightCmEntry.Text, out double heightCm) && heightCm < 0)
        {
            heightCmEntry.Text = null;
        }

        if (!string.IsNullOrEmpty(heightFeetEntry.Text) && double.TryParse(heightFeetEntry.Text, out double heightFeet) && heightFeet < 0)
        {
            heightFeetEntry.Text = null;
        }

        if (!string.IsNullOrEmpty(heightInchesEntry.Text) && double.TryParse(heightInchesEntry.Text, out double heightInches) && heightInches < 0)
        {
            heightInchesEntry.Text = null;
        }

        if (!string.IsNullOrEmpty(heightInchesEntry.Text) && double.TryParse(heightInchesEntry.Text, out double heightInches12) && heightInches12 > 11.5)
        {
            heightInchesEntry.Text = null;
        }

        if (!string.IsNullOrEmpty(Weight.Text) && double.TryParse(Weight.Text, out double weight) && weight < 0)
        {
            Weight.Text = null;
        }

        if (!string.IsNullOrEmpty(GoalWeight.Text) && double.TryParse(GoalWeight.Text, out double goalWeight) && goalWeight < 0)
        {
            GoalWeight.Text = null;
        }
    }



    async void CheckGoalWeight()
    {
        double.TryParse(GoalWeight.Text, out double goalWeight);

        if (!string.IsNullOrEmpty(heightFeetEntry.Text) && double.TryParse(heightFeetEntry.Text, out double heightFeet) &&
            !string.IsNullOrEmpty(heightInchesEntry.Text) && double.TryParse(heightInchesEntry.Text, out double heightInches))
        {
            // Convert height from feet and inches to inches
            heightInches = heightFeet * 12 + heightInches;

            // Calculate minimum goal weight based on BMI formula
            double bmiMin = 18.5;
            double bmiMinWeight = bmiMin * Math.Pow(heightInches / 39.37, 2.0);
            double goalWeightMin = Math.Round(bmiMinWeight, 1);

            // Convert goal weight to selected unit
            switch (unitWeight)
            {
                case 0: // lbs
                    goalWeightMin = Math.Round(goalWeightMin * 2.20462, 0) + 5;
                    break;
                case 1: // kg
                    goalWeightMin = Math.Round(goalWeightMin, 0) + 3;
                    break;
            }

            if (goalWeight < goalWeightMin)
            {
                bool answer = await DisplayAlert("Notice", "The goal weight you selected may be unhealthy for your height, do you want to set it to the healthier value based on your height?", "OK", "Cancel");
                if (answer)
                {
                    GoalWeight.Text = goalWeightMin.ToString();
                }
            }
        }
    }





    //Stats VVVV
    private int sex = -4;

    void MaleClicked(System.Object sender, System.EventArgs e)
    {
        sex = 0;
        UpdateSex();
    }

    void FemaleClicked(System.Object sender, System.EventArgs e)
    {
        sex = 1;
        UpdateSex();
    }



    void UpdateSex()
    {
        var male = MaleButton;
        var female = FemaleButton;

        switch (sex)
        {
            case 0:
                male.BackgroundColor = Color.FromRgb(75, 158, 227);
                female.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                male.Background = Color.FromRgb(75, 158, 227);
                female.Background = Color.FromRgba(0, 0, 0, 0);
                break;

            case 1:
                female.BackgroundColor = Color.FromRgb(75, 158, 227);
                male.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                female.Background = Color.FromRgb(75, 158, 227);
                male.Background = Color.FromRgba(0, 0, 0, 0);
                break;
        }
    }

    async void SexHelpClicked(System.Object sender, System.EventArgs e)
    {
        await DisplayAlert("Notice", "HealthMode uses your biological sex in the equations for metabolic rate and strength standards as these equations differ based on biological sex. If your gender identity is not the same as your sex assigned at birth, and you have not started gender-affirming medications, select your sex assigned at birth. If you've been on gender-affirming medications for a few months, selecting the option closer to your gender identity may more accurately reflect your metabolic rate and strength potential.\n(You can always change this later as you notice changes)", "OK");
    }

    


    void BackButton4_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 3;
        UpdatePage();
    }

    bool birthdaySelected = false;

    async void NextButton4_Clicked(System.Object sender, System.EventArgs e)
    {
        CheckNegatives();
        if (weightGoal != 0) { CheckGoalWeight();}
        if (weightGoal == 0) { GoalWeight.Text = Weight.Text; }


        if (sex == -4)
        {
            await DisplayAlert("Notice", "Please select your sex \n(Click 'Need Help?' if you're unsure)", "OK");
        }
        else if (birthdaySelected == false)
        {
            await DisplayAlert("Notice", "Please select your birthday.", "OK");
        }
        else if (string.IsNullOrWhiteSpace(heightCmEntry.Text))
        {
            if (string.IsNullOrWhiteSpace(heightFeetEntry.Text) || string.IsNullOrWhiteSpace(heightInchesEntry.Text))
            {
                // Both feet and inches are blank or whitespace
                await DisplayAlert("Notice", "Please enter your height", "OK");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Weight.Text) || string.IsNullOrWhiteSpace(GoalWeight.Text))
                {
                    await DisplayAlert("Notice", "Please enter your weight and goal weight", "OK");
                }
                else
                {
                    stage = 5;
                    UpdateActivity();
                    UpdatePage();
                }
            }

        }
        else if (string.IsNullOrWhiteSpace(Weight.Text) || string.IsNullOrWhiteSpace(GoalWeight.Text))
        {
            await DisplayAlert("Notice", "Please enter your weight and goal weight", "OK");
        }
        else
        {
            stage = 5;

            UpdateActivity();

            UpdatePage();
        } 
    }

    //Stats ^^^^
    //Summary VVVV



 

    void CalculateTDEE()
    {
        double height;
        double weight;

        // Convert height to cm
        if (!string.IsNullOrWhiteSpace(heightCmEntry.Text))
        {
            height = Double.Parse(heightCmEntry.Text);
        }
        else // Feet and inches
        {
            double heightFeet = Double.Parse(heightFeetEntry.Text);
            double heightInches = Double.Parse(heightInchesEntry.Text);
            height = (heightFeet * 30.48) + (heightInches * 2.54);
        }


        // Convert weight to kg
        if (unitWeight == 0) // Pounds
        {
            weight = Double.Parse(Weight.Text) * 0.45359237;
        }
        else // Kilograms
        {
            weight = Double.Parse(Weight.Text);
        }

        double bmr;
        DateTime birthday = BirthdayDate.Date;
        DateTime now = DateTime.Today;
        int age = now.Year - birthday.Year;
        if (birthday > now.AddYears(-age))
        {
            age--;
        }

        if (sex == 0) // Male
        {
            bmr = 88.362 + (13.397 * weight) + (4.799 * height) - (5.677 * age);
        }
        else // Female
        {
            bmr = 447.593 + (9.247 * weight) + (3.098 * height) - (4.330 * age);
        }

        double tdee = bmr;
        switch (activityLevel)
        {
            case 0:
                tdee = bmr * 1.32;
                break;
            case 1:
                tdee = bmr * 1.41;
                break;
            case 2:
                tdee = bmr * 1.585;
                break;
            case 3:
                tdee = bmr * 1.76;
                break;
            case 4:
                tdee = bmr * 2;
                break;
            default:
                break;
        }

        // Adjust TDEE based on weight goal
        switch (weightGoal)
        {
            case -1:
                switch (activityLevel)
                {
                    case 0:
                        tdee *= .76;
                        break;
                    case 1:
                        tdee *= .8;
                        break;
                    case 2:
                        tdee *= .81;
                        break;
                    case 3:
                        tdee *= .84;
                        break;
                    case 4:
                        tdee *= .88;
                        break;
                    default:
                        break;
                }
                break;
            case 0:
                break;
            case 1:
                 switch (activityLevel)
        {
                    case 0:
                        tdee *= 1.14;
                        break;
                    case 1:
                        tdee *= 1.17;
                        break;
                    case 2:
                        tdee *= 1.19;
                        break;
                    case 3:
                        tdee *= 1.22;
                        break;
                    case 4:
                        tdee *= 1.26;
                        break;
                }
                break;
            default:
                break;
        }

        switch (unitEnergy)
        {
            case 0: // kcal
                calorieGoal = Convert.ToInt32(Math.Round(tdee));
                break;
            case 1: // calories
                calorieGoal = Convert.ToInt32(Math.Round(tdee));
                break;
            case 2: // kJ
                calorieGoal = Convert.ToInt32(Math.Round(tdee));
                tdee *= 4.184;
                bmr *= 4.184;
                break;
            default:
                break;
        }

        BMRNumber.Text = Math.Round(bmr).ToString();
        CalorieNumber.Text = Math.Round(tdee).ToString();

        
    }




    void UpdateActivity()
    {
        var sedentary = SedentaryButton;
        var lightActive = LightActiveButton;
        var modActive = ModActiveButton;
        var veryActive = VeryActiveButton;

        switch (activityLevel)
        {
    
            case 0:
                sedentary.BackgroundColor = Color.FromRgb(75, 158, 227);
                lightActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                modActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                veryActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                sedentary.Background = Color.FromRgb(75, 158, 227);
                lightActive.Background = Color.FromRgba(0, 0, 0, 0);
                modActive.Background = Color.FromRgba(0, 0, 0, 0);
                veryActive.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 1:
                sedentary.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                lightActive.BackgroundColor = Color.FromRgb(75, 158, 227);
                modActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                veryActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                sedentary.Background = Color.FromRgba(0, 0, 0, 0);
                lightActive.Background = Color.FromRgb(75, 158, 227);
                modActive.Background = Color.FromRgba(0, 0, 0, 0);
                veryActive.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 2:
                sedentary.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                lightActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                modActive.BackgroundColor = Color.FromRgb(75, 158, 227);
                veryActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                sedentary.Background = Color.FromRgba(0, 0, 0, 0);
                lightActive.Background = Color.FromRgba(0, 0, 0, 0);
                modActive.Background = Color.FromRgb(75, 158, 227);
                veryActive.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 3:
                sedentary.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                lightActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                modActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                veryActive.BackgroundColor = Color.FromRgb(75, 158, 227);

                sedentary.Background = Color.FromRgba(0, 0, 0, 0);
                lightActive.Background = Color.FromRgba(0, 0, 0, 0);
                modActive.Background = Color.FromRgba(0, 0, 0, 0);
                veryActive.Background = Color.FromRgb(75, 158, 227);
                break;
        }
        CalculateTDEE();

    }

  

    async void CalorieNumberTapped(System.Object sender, System.EventArgs e)
    {
        switch (weightGoal)
        {
            case -1:
        await DisplayAlert("Notice", "This is the amount of energy calculated based on your height, weight, sex, and activity level to lose 1lb (2.2kg) per week.\nNote that it may not be exact and you may want to track your weight after a few weeks and modify the goal as needed.", "OK");
            break;

            case 0:
        await DisplayAlert("Notice", "This is the amount of energy calculated based on your height, weight, sex, and activity level to maintain your weight\nNote that it may not be exact and you may want to track your weight after a few weeks and modify the goal as needed.", "OK");
            break;

            case 1:
        await DisplayAlert("Notice", "This is the amount of energy calculated based on your height, weight, sex, and activity level to gain 1lb (2.2kg) per week\nNote that it may not be exact and you may want to track your weight after a few weeks and modify the goal as needed.", "OK");
            break;
        }
    }

    async void BMRNumber_Tapped(System.Object sender, System.EventArgs e)
    {
        await DisplayAlert("Notice", "This is the amount of energy your body uses everyday just by existing.\nIt is not advised to eat below this number", "OK");

    }

    void SedActiveClicked(System.Object sender, System.EventArgs e)
    {
        var sedentary = SedentaryButton;
        var lightActive = LightActiveButton;
        var modActive = ModActiveButton;
        var veryActive = VeryActiveButton;

        activityLevel = 0;
        UpdateActivity();
        sedentary.BackgroundColor = Color.FromRgb(75, 158, 227);
        lightActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        modActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        veryActive.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

        sedentary.Background = Color.FromRgb(75, 158, 227);
        lightActive.Background = Color.FromRgba(0, 0, 0, 0);
        modActive.Background = Color.FromRgba(0, 0, 0, 0);
        veryActive.Background = Color.FromRgba(0, 0, 0, 0);
    }

    void LightActiveClicked(System.Object sender, System.EventArgs e)
    {
        activityLevel = 1;
        UpdateActivity();
    }

    void ModActiveClicked(System.Object sender, System.EventArgs e)
    {
        activityLevel = 2;
        UpdateActivity();
    }

    void VeryActiveClicked(System.Object sender, System.EventArgs e)
    {
        activityLevel = 3;
        UpdateActivity();
    }

    void BackButton5_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 4;
        UpdatePage();
    }

    int waterGoal = 2000;

    void NextButton5_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 6;
        Debug.WriteLine(waterGoal);
        waterGoal = CalculateWaterGoal();
        Debug.WriteLine(waterGoal);
        switch (unitWater)
        {
            case 0:
                var flOzWater = Math.Round(waterGoal / 29.574, 0);
                WaterNumber.Text = flOzWater.ToString();
                WaterLabel.Text = "fl oz";
                break;

            case 1:
                var cupsWater = Math.Round(waterGoal / 236.6, 0);
                WaterNumber.Text = cupsWater.ToString();
                WaterLabel.Text = "cups";
                break;

            case 2:

                WaterNumber.Text = waterGoal.ToString();
                WaterLabel.Text = "mL";
                break;
        }

        UpdatePage();
    }

    async void ActivityHelpClicked (System.Object sender, System.EventArgs e)
    {
        await DisplayAlert("Activity Levels",
     "\nSedentary: Little to no exercise, mostly sitting or lying down. \n\n" +
     "Light Activity: Light exercise or sports 1-3 days per week. \n\n" +
     "Moderate Activity: Moderate exercise or sports 3-5 days per week. \n\n" +
     "Very Active: Hard exercise or sports 6-7 days per week. \n\n Try to be as accurate as possible\n",
     "OK");

    }






    void BackButton6_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 5;
        UpdatePage();
    }

    void NextButton6_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 7;
        PasswordEntry.WidthRequest = UsernameEntry.Width;
        UpdatePage();

        switch (unitWater)
        {
            case 0:
                if (double.TryParse(WaterNumber.Text, out double flOzWater))
                {
                    waterGoal = (int)Math.Round(flOzWater * 29.5735, 0);
                }
                break;

            case 1:
                if (double.TryParse(WaterNumber.Text, out double cupsWater))
                    {
                    waterGoal = (int)Math.Round(cupsWater * 236.588, 0);
                    }
                break;

            case 2:
                if (double.TryParse(WaterNumber.Text, out double mLWater))
                {
                    waterGoal = (int)Math.Round(mLWater, 0);
                }
                break;
        }
    }

    void BackButton7_Clicked(System.Object sender, System.EventArgs e)
    {
        stage = 6;
        UpdatePage();
    }

    public static (string salt, string hashedPassword) HashAndSaltPassword(string password)
    {
        // Generate a 16-byte (128-bit) random salt
        byte[] saltBytes = new byte[16];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        string salt = Convert.ToBase64String(saltBytes); Debug.WriteLine(salt);
        salt = salt.Replace("\"", "");
        salt = salt.Replace("+", "");
        salt = salt.Replace("/", "");
        string saltedPassword = password + salt; Debug.WriteLine(saltedPassword);
        var sha256 = SHA256.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword); Debug.WriteLine(passwordBytes);
        byte[] hashedBytes = sha256.ComputeHash(passwordBytes); Debug.WriteLine(hashedBytes);
        string hashedPassword = Convert.ToBase64String(hashedBytes); Debug.WriteLine(hashedPassword);
        hashedPassword = hashedPassword.Replace("+", "");
        hashedPassword = hashedPassword.Replace("/", "");
        return (salt, hashedPassword);
    }

    bool creatingAccount = false;

    async void SignUpButton_Clicked(System.Object sender, System.EventArgs e)
    {
        SignUpButton.IsVisible = false;
        SignUpLoad.IsVisible = true;

        string email = EmailEntry.Text.Trim().ToLower();
        string username = UsernameEntry.Text.TrimEnd();
        string password = PasswordEntry.Text.Trim();
        string confirmPassword = ConfirmPasswordEntry.Text.Trim();

        if (email == "") { email = null; }
        if (username == "") { username = null; }
        if (password == "") { password = null; }
        if (confirmPassword == "") { confirmPassword = null; }

        if (!creatingAccount)
        {

            if (email != null && username != null && password != null && confirmPassword != null)
            {
                if (password != confirmPassword)
                {
                    // Passwords do not match, show error message
                    await DisplayAlert("Notice", "Passwords do not match", "OK");
                    creatingAccount = false;
                }
                else
                {
                    string responseMessage = await _dataService.CheckUserUniqueAsync(email, username);
                    // Assuming you have a method called AddUserToDatabase to add the user to the database
                    if (responseMessage == "AllClear")
                    {
                        creatingAccount = true;
                        try
                        {
                            (string salt, string hashedPassword) = HashAndSaltPassword(password);

                            if (!string.IsNullOrWhiteSpace(heightCmEntry.Text))
                            {
                                // Use the value entered in the heightCmEntry field
                                if (double.TryParse(heightCmEntry.Text, out double heightCmFromEntry))
                                {
                                    heightCm = heightCmFromEntry;
                                    height = Convert.ToDecimal(heightCm);
                                }
                            }
                            else
                            {
                                // Convert the feet and inches to cm
                                int feet, inches;
                                if (int.TryParse(heightFeetEntry.Text, out feet) && int.TryParse(heightInchesEntry.Text, out inches))
                                {
                                    double heightInches = feet * 12 + inches;
                                    heightCm = heightInches * 2.54;
                                    height = Convert.ToDecimal(heightCm);
                                }
                            }

                            if (unitWeight == 1) // Convert kg to lbs
                            {
                                decimal weightInKg = decimal.Parse(Weight.Text);
                                weight = Math.Round(weightInKg * (decimal)2.2, 1);
                                decimal goalweightInKg = decimal.Parse(GoalWeight.Text);
                                goalWeight = Math.Round(goalweightInKg * (decimal)2.2, 1);

                            }
                            else if (unitWeight == 0)
                            {
                                weight = decimal.Parse(Weight.Text);
                                goalWeight = decimal.Parse(GoalWeight.Text);
                            }


                            // Call a method to add the user to the database with email, username, and hashed password //convert lists to strings (unit maingoals)
                            
                            bool result = await _dataService.AddUserAsync(email, username, hashedPassword, salt, weightGoal, ReturnGoalsList(), ReturnUnitList(), sex, height, BirthdayDate.Date, weight, goalWeight, activityLevel, calorieGoal, waterGoal);
                            if (result)
                            {
                                await DisplayAlert("Success", "Account created successfully", "OK");
                                await Navigation.PopModalAsync();
                            }
                            else
                            {
                                await DisplayAlert("Error", "Something went wrong with account creation", "OK");
                                SignUpButton.IsVisible = true;
                                SignUpLoad.IsVisible = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions that occur during the database insert operation
                            await DisplayAlert("Error", ex.Message, "OK");
                        }
                    }
                    else if (responseMessage == "EmailTaken")
                    {
                        await DisplayAlert("Notice", "The email is already in use.", "OK");
                        creatingAccount = false;
                    }
                    else if (responseMessage == "UsernameTaken")
                    {
                        await DisplayAlert("Notice", "The username is already taken. :(", "OK");
                        creatingAccount = false;
                    }
                    else if (responseMessage == "ForbiddenWord")
                    {
                        await DisplayAlert("Notice", $"You selected username of {username} may have been flagged as inappropriate.", "OK");
                        creatingAccount = false;
                    }
                    else
                    { await DisplayAlert("Notice", "There was an issue checking for if that email/username is already in use. Report this please \n info@healthmode.app", "OK");
                      creatingAccount = false;
                    }
                }
            }
            else { await DisplayAlert("Notice", "Please fill out all the fields.", "OK"); creatingAccount = false; }
        }
        else
        {
            SignUpButton.IsVisible = false;
            SignUpLoad.IsVisible = true;
        }
        SignUpLoad.IsVisible = false;
        SignUpButton.IsVisible = true;
        

    }


    public string ReturnGoalsList()
    {
        List<string> mainGoals = new List<string>();

        if (_loseFatClicked)
        {
            mainGoals.Add("LoseFat");
        }

        if (_buildMuscleClicked)
        {
            mainGoals.Add("BuildMuscle");
        }

        if (_modDietClicked)
        {
            mainGoals.Add("ModifyDiet");
        }

        if (_activityClicked)
        {
            mainGoals.Add("IncreaseActivity");
        }

        if (_trackHealthClicked)
        {
            mainGoals.Add("TrackHealth");
        }

        string json = JsonSerializer.Serialize(mainGoals);
        return json;
    }


    public int CalculateWaterGoal()
    {
        int waterGoal = 2000;
        if (unitWeight == 1) // Convert kg to lbs
        {
            decimal weightInKg = decimal.Parse(Weight.Text);
            weight = Math.Round(weightInKg * (decimal)2.2, 1);

        }
        else if (unitWeight == 0)
        {
            weight = decimal.Parse(Weight.Text);
        }
        int roundedWeight = (int)Math.Round(weight);
        Debug.WriteLine($"Weight{roundedWeight}");
        if (activityLevel == 0)
        {
            waterGoal = (sex == 0) ? roundedWeight * 15 : roundedWeight * 13;
        }
        else if (activityLevel == 1)
        {
            waterGoal = (sex == 0) ? roundedWeight * 17 : roundedWeight * 15;
        }
        else if (activityLevel == 2)
        {
            waterGoal = (sex == 0) ? roundedWeight * 20 : roundedWeight * 17;
        }
        else
        {
            waterGoal = (sex == 0) ? roundedWeight * 22 : roundedWeight * 20;
        }
        Debug.WriteLine(waterGoal);
        waterGoal = (int)Math.Round((decimal)waterGoal);

        Debug.WriteLine($"Water Goal: {waterGoal}");

        return waterGoal;
    }


    public string ReturnUnitList()
    {
        List<string> unitList = new List<string>();

        switch (unitWeight)
        {
            case 0:
                unitList.Add("lbs");
                break;
            case 1:
                unitList.Add("kg");
                break;
        }

        switch (unitLength)
        {
            case 0:
                unitList.Add("inch");
                break;
            case 1:
                unitList.Add("cm");
                break;
        }

        switch (unitWater)
        {
            case 0:
                unitList.Add("fl oz");
                break;
            case 1:
                unitList.Add("cups");
                break;
            case 2:
                unitList.Add("mL");
                break;
        }

        switch (unitEnergy)
        {
            case 0:
                unitList.Add("kCal");
                break;
            case 1:
                unitList.Add("cal");
                break;
            case 2:
                unitList.Add("kJ");
                break;
        }

        switch (EunitEnergy)
        {
            case 0:
                unitList.Add("kCal");
                break;
            case 1:
                unitList.Add("cal");
                break;
            case 2:
                unitList.Add("kJ");
                break;
        }

        switch (formatDate)
        {
            case 0:
                unitList.Add("yy/MM/dd");
               
                break;
            case 1:
                unitList.Add("MM/dd/yy");
                break;
            case 2:
                unitList.Add("dd/MM/yy");
                break;
        }

        string json = JsonSerializer.Serialize(unitList);

        return json;
    }

    void BirthdayDate_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {
        birthdaySelected = true;
    }

    
}


