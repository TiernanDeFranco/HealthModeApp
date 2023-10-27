using System.Diagnostics;
using System.Text.Json;
using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages;

public partial class UnitPage : ContentPage
{
    private readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

    public int unitWeight = 0;
    public int unitLength = 0;
    public int unitWater = 1;
    public int unitEnergy = 1;
    public int EunitEnergy = 1;
    public int formatDate = 1;
    List<string> _unitList;

    public UnitPage(ISQLiteDataService localData, IRestDataService dataService)
	{

        InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
        _localData = localData;
        _dataService = dataService;
        GetDefaults();
        
	}

    async void GetDefaults()
    {
       int userID = await _localData.GetUserID();
       var userInfo = await _localData.GetUserAsync(userID);
       string unitListJson = userInfo.Units;
        Debug.WriteLine(unitListJson);

       var unitList = JsonSerializer.Deserialize<List<string>>(unitListJson);
        _unitList = unitList;
        Debug.WriteLine(unitList[0]);
            string weightUnit = unitList[0];
            string lengthUnit = unitList[1];
            string waterUnit = unitList[2];
            string foodUnit = unitList[3];
            string exerciseUnit = unitList[4];
            string dateFormat = unitList[5];
        Debug.WriteLine(unitList[5]);
            if (weightUnit == "lbs")
            {
                unitWeight = 0;
            Debug.WriteLine("hello");
            }
            else if (weightUnit == "kg")
            {
                unitWeight = 1;
            }

            if (lengthUnit == "inch")
            {
            unitLength = 0;
            }
        else
        {
            unitLength = 1;
        }

            if (waterUnit == "fl oz")
            {
                unitWater = 0;
            }
            else if (waterUnit == "cups")
            {
                unitWater = 1;
            }
            else if (waterUnit == "mL")
            {
                unitWater = 2;
            }

            if (foodUnit == "kCal")
            {
                unitEnergy = 0;
            }
            else if (foodUnit == "cal")
            {
                unitEnergy = 1;
            }
            else if (foodUnit == "kJ")
            {
                unitEnergy = 2;
            }

            if (exerciseUnit == "kCal")
            {
                EunitEnergy = 0;
            }
            else if (exerciseUnit == "cal")
            {
                EunitEnergy = 1;
            }
            else if (exerciseUnit == "kJ")
            {
                EunitEnergy = 2;
            }

            if (dateFormat == "yy/MM/dd")
            {
                formatDate = 2;
            }
            else if (dateFormat == "MM/dd/yy")
            {
                formatDate = 1;
            }
            else if (dateFormat == "dd/MM/yy")
            {
                formatDate = 0;
            }

        UpdateMeasurementButtons();
    }


    void UpdateMeasurementButtons()
    {
        var weightLbs = LbsButton;
        var weightKg = KgButton;
        var lengthIn = InButton;
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
                lengthIn.Background = Color.FromRgb(75, 158, 227);
                lengthCm.Background = Color.FromRgba(0, 0, 0, 0);
                break;
            case 1:
                lengthIn.Background = Color.FromRgba(0, 0, 0, 0);
                lengthCm.Background = Color.FromRgb(75, 158, 227);
                break;
        }

        switch (unitWater)
        {
            case 0:
                drinkWaterFlOz.BackgroundColor = Color.FromRgb(75, 158, 227);
                drinkWaterCups.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                drinkWaterML.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                drinkWaterFlOz.Background = Color.FromRgb(75, 158, 227);
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
                DateDisplay.Text = DateTime.Today.ToString("d MMM, yyyy");

                break;
            case 1:
                dateFormatDmy.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                dateFormatMdy.BackgroundColor = Color.FromRgb(75, 158, 227);
                dateFormatYmd.BackgroundColor = Color.FromRgba(0, 0, 0, 0);

                dateFormatDmy.Background = Color.FromRgba(0, 0, 0, 0);
                dateFormatMdy.Background = Color.FromRgb(75, 158, 227);
                dateFormatYmd.Background = Color.FromRgba(0, 0, 0, 0);
                DateDisplay.Text = DateTime.Today.ToString("MMM d, yyyy");


                break;
            case 2:
                dateFormatDmy.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                dateFormatMdy.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                dateFormatYmd.BackgroundColor = Color.FromRgb(75, 158, 227);

                dateFormatDmy.Background = Color.FromRgba(0, 0, 0, 0);
                dateFormatMdy.Background = Color.FromRgba(0, 0, 0, 0);
                dateFormatYmd.Background = Color.FromRgb(75, 158, 227);
                DateDisplay.Text = DateTime.Today.ToString("yyyy, MMMM, d");

                break;
        }

        DmyButton.Text = DateTime.Today.ToString("d/MM/yyyy");
        MdyButton.Text = DateTime.Today.ToString("MM/d/yyyy");
        YmdButton.Text = DateTime.Today.ToString("yyyy/MM/d");
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



    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        string json = ReturnUnitList();
        
        int _userID = await _localData.GetUserID();
        await _localData.UpdateUserAsync(_userID, null, null, null, null, null, null, json);
        Debug.WriteLine(json);
        UserInfo userInfo = new UserInfo
        {
            UserID = _userID,
            Units = json
            
        };
        LoadingBar.IsVisible = true;
        LoadingBar.IsRunning = true;
        SaveButton.IsVisible = false;
        (string email, string password) = await _localData.GetUserCredentials();
        await _dataService.UpdateUserInfoAsync(userInfo, email, password, _userID);
        var serial = JsonSerializer.Serialize(userInfo);
        Debug.WriteLine(serial);
        await Navigation.PopAsync();
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
            case 2:
                unitList.Add("yy/MM/dd");

                break;
            case 1:
                unitList.Add("MM/dd/yy");
                break;
            case 0:
                unitList.Add("dd/MM/yy");
                break;
        }

        string json = JsonSerializer.Serialize(unitList);

        return json;
    }

   
}
