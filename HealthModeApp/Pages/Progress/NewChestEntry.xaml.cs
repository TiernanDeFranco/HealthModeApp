using System.Text.Json;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.Progress;

public partial class NewChestEntry : ContentPage
{
    private readonly ISQLiteDataService _localData;
    int userID;
    DateTime _date;
    WeightTable _weightEntry;

    public NewChestEntry(ISQLiteDataService localData, DateTime date, WeightTable weightEntry)
    {
        InitializeComponent();
        _localData = localData;
        _date = date;
        _weightEntry = weightEntry;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Hide the TabBar on this page
        Shell.SetTabBarIsVisible(this, false);

        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var measureUnit = unitList[1];

        DateHandler();
        await Task.Delay(100);

        if (_weightEntry == null)
        {
            var number = await _localData.GetChest(userID, _date);
            if (measureUnit == "cm") number *= Math.Round((decimal)2.54, 2);
            MEntry.Placeholder = number.ToString("0.#");
            MEntry.Text = number.ToString("0.#");
        }
        else
        {
            var number = Math.Round(_weightEntry.Chest, 2);

            MEntry.Placeholder = number.ToString("0.#");
            MEntry.Text = number.ToString("0.#");
        }
    }

    string _measureUnit;

    async void DateHandler()
    {
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        _measureUnit = unitList[1];

        DateSelect.Format = $"   {dateFormat}";
        DateSelect.Date = _date;

        if (_measureUnit == "cm")
        {
            UnitLabel.Text = "Cm";
        }
        else
        {
            UnitLabel.Text = "Inches";
        }
    }

    async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
    {
        
        userID = await _localData.GetUserID();
        decimal.TryParse(MEntry.Text, out decimal Mvalue);

        decimal INCHMValue;

        if (_measureUnit == "cm")
        {
            INCHMValue = Mvalue / (decimal)2.54;
        }
        else
        {
            INCHMValue = Mvalue;
        }

        bool entryExists = await _localData.GetWeightForDay(userID, DateSelect.Date);
        if (!entryExists)
        {
            await _localData.AddChestEntry(userID, DateSelect.Date, INCHMValue);
        }
        else
        {
            await _localData.UpdateChestEntry(userID, DateSelect.Date, INCHMValue);
        }
        await Navigation.PopAsync();

    }
}
