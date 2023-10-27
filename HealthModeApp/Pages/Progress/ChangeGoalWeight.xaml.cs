using System.Diagnostics;
using System.Text.Json;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using HealthModeApp.Pages.Popups;
using Mopups.Services;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.Progress;

public partial class ChangeGoalWeight
{
	private readonly IRestDataService _dataService;
	private readonly ISQLiteDataService _localData;

	int _userID;
	decimal _weight;
    decimal _goalWeight;
	int _weightPlan;
    string _weightUnit;

    string email;
    string password;

    UserData userData;

    int rateInt = 1;

    public string unitEnergy = "cal";

    public ChangeGoalWeight(IRestDataService dataService, ISQLiteDataService localData)
	{
		InitializeComponent();
		_dataService = dataService;
		_localData = localData;
		PopulateData();

       
	}

	async void PopulateData()
	{
		_userID = await _localData.GetUserID();
		_weight = await _localData.GetWeight(_userID, DateTime.Today);
		userData = await _localData.GetUserAsync(_userID);

        email = userData.Email;
        password = userData.Password;

		_weightPlan = userData.WeightPlan;
        UpdateWeightButtons();
        var units = JsonSerializer.Deserialize<List<string>>(userData.Units);
        WeightUnit.Text = units[0];
        _weightUnit = units[0];
        unitEnergy = units[3];
        Debug.WriteLine(_weightUnit);

        _goalWeight = userData.GoalWeight;
        
        if (_weightUnit == "kg")
        {
            GoalWeightEntry.Text = Math.Round(_goalWeight / (decimal)2.2).ToString();
            TimelineSelector.ItemSource = new List<string>
        {
            "0.25 kg per week",
            "0.5 kg per week",
            "1 kg per week"
        };

        }
        else
        {
            GoalWeightEntry.Text = _goalWeight.ToString();
            TimelineSelector.ItemSource = new List<string>
        {
            "0.5 lbs per week",
            "1 lb per week",
            "2 lbs per week"
        };
        }

        TimelineSelector.Title = TimelineSelector.ItemSource[1];
        rateInt = 1;

       
    }

    void LoseWeightClicked(System.Object sender, System.EventArgs e)
    {
		_weightPlan = -1;

        if (_weightUnit == "kg")
        {
            GoalWeightEntry.Text = Math.Round(Math.Round((_weight / (decimal)2.2) - (decimal)2.5), 0).ToString();
        }
        else
        {
            GoalWeightEntry.Text = Math.Round(_weight - 5, 0).ToString();
        }

        TimelineSelector.IsVisible = true;

        UpdateWeightButtons();
    }

    void MaintainWeightClicked(System.Object sender, System.EventArgs e)
    {
        _weightPlan = 0;

        if (_weightUnit == "kg")
        {
            GoalWeightEntry.Text = (Math.Round(_weight / (decimal)2.2).ToString());
        }
        else
        {
            GoalWeightEntry.Text = (_weight).ToString();
        }

        TimelineSelector.IsVisible = false;
        

        UpdateWeightButtons();
    }

    void GainWeightClicked(System.Object sender, System.EventArgs e)
    {
        _weightPlan = 1;

        if (_weightUnit == "kg")
        {
            GoalWeightEntry.Text = Math.Round(Math.Round((_weight / (decimal)2.2) + (decimal)2.5), 0).ToString();
        }
        else
        {
            GoalWeightEntry.Text = Math.Round(_weight + 5, 0).ToString();
        }

        TimelineSelector.IsVisible = true;

        UpdateWeightButtons();
    }

    void UpdateWeightButtons()
    {

        switch (_weightPlan)
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

    async void UpdateGoalWeight_Clicked(System.Object sender, System.EventArgs e)
    {
        if (decimal.TryParse(GoalWeightEntry.Text, out var vGoalWeight))
        {
            if (_weightUnit == "kg")
            {
                vGoalWeight = Math.Round(vGoalWeight * (decimal)2.2);

            }

            Debug.WriteLine(CalculateCalories(userData));

            Debug.WriteLine(vGoalWeight);

            UserInfo goalWeight = new UserInfo
            {
                GoalWeight = vGoalWeight,
                WeightPlan = _weightPlan
            };

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                UpdateGoalWeight.IsVisible = false;
                Loading.IsVisible = true;
                bool success = await _dataService.UpdateUserInfoAsync(goalWeight, email, password, _userID);
                if (success)
                {
                    await _localData.UpdateUserAsync(_userID, null, null, null, null, _weightPlan, null, null, null, null, null, null, vGoalWeight);
                    await MopupService.Instance.PopAsync();
                }
                else
                {
                    await _localData.UpdateUserAsync(_userID, null, null, null, null, _weightPlan, null, null, null, null, null, null, vGoalWeight);
                    await MopupService.Instance.PushAsync(new InfoPopup("Oops", "There was an issue updating your goal weight."));
                    UpdateGoalWeight.IsVisible = true;
                    Loading.IsVisible = false;
                }
            }
            else
            {
                await _localData.UpdateUserAsync(_userID, null, null, null, null, _weightPlan, null, null, null, null, null, null, vGoalWeight);
                await MopupService.Instance.PopAsync();
            }

            
        }
        else
        {
            await MopupService.Instance.PushAsync(new InfoPopup("Oops", "There was an issue with the weight you entered, please try again"));

        }


    }

    void TimelineSelector_SelectedIndexChanged(System.Object sender, System.Int32 e)
    {
        rateInt = e;
    }


    public int CalculateCalories(UserData userData)
    {
        int calorieGoal = 2000;
        double bmr = 0;
        double weightKg = (double)userData.Weight * 0.453592; // Convert weight from lbs to kg

        int age = DateTime.Now.Year - userData.Birthday.Year;
        if (DateTime.Now.DayOfYear < userData.Birthday.DayOfYear)
        {
            age--; // Birthday hasn't occurred yet this year, so subtract 1
        }

        // Calculate BMR based on sex and age
        if (userData.Sex == 0) // Male
        {
            bmr = 88.362 + (13.397 * weightKg) + (4.799 * (double)userData.HeightCm) - (5.677 * age);
        }
        else if (userData.Sex == 1) // Female
        {
            bmr = 447.593 + (9.247 * weightKg) + (3.098 * (double)userData.HeightCm) - (4.330 * age);
        }

        // Factor in the activity level to calculate maintenance calories
        double tdee = bmr;
        switch (userData.ActivityLevel)
        {
            case 0:
                tdee = bmr * 1.35;
                break;
            case 1:
                tdee = bmr * 1.42;
                break;
            case 2:
                tdee = bmr * 1.587;
                break;
            case 3:
                tdee = bmr * 1.77;
                break;
            case 4:
                tdee = bmr * 2;
                break;
            default:
                break;
        }

        calorieGoal = Convert.ToInt32(Math.Round(tdee));

        switch (rateInt)
        {
            case 0:
                if (_weightPlan == -1) calorieGoal -= 250;
                else if (_weightPlan == 1) calorieGoal += 250;
                break;

            case 1:
                if (_weightPlan == -1) calorieGoal -= 500;
                else if (_weightPlan == 1) calorieGoal += 500;
                break;

            case 2:
                if (_weightPlan == -1) calorieGoal -= 1000;
                else if (_weightPlan == 1) calorieGoal += 1000;
                break;
        }



        


        return calorieGoal;
    }

    void GoalWeightEntry_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        CloseWhenBackgroundIsClicked = false;
    }

    void GoalWeightEntry_Unfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        CloseWhenBackgroundIsClicked = true;
    }
}
