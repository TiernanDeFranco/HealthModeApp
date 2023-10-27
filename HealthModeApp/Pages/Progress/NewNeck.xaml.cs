using System.Diagnostics;
using System.Text.Json;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.Progress;

public partial class NewNeck : ContentPage
{
    
    private readonly ISQLiteDataService _localData;
    int userID;
    DateTime _date;
    WeightTable _weightEntry;

    public NewNeck(ISQLiteDataService localData, DateTime date, WeightTable weightEntry)
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
            var number = await _localData.GetNeck(userID, _date);
           if (measureUnit == "cm") number *= Math.Round((decimal)2.54, 2);
            NeckEntry.Placeholder = number.ToString("0.#");
            NeckEntry.Text = number.ToString("0.#");
        }
        else
        {
            var number = Math.Round(_weightEntry.Neck, 2);
           
            NeckEntry.Placeholder = number.ToString("0.#");
            NeckEntry.Text = number.ToString("0.#");
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
        Debug.WriteLine("Clicked");
        userID = await _localData.GetUserID();
        decimal.TryParse(NeckEntry.Text, out decimal neckValue);

        decimal INCHneckValue;

        if (_measureUnit == "cm")
        {
            INCHneckValue = neckValue / (decimal)2.54;
        }
        else
        {
            INCHneckValue = neckValue;
        }

        bool entryExists = await _localData.GetWeightForDay(userID, DateSelect.Date);
        if (!entryExists)
        {
            await _localData.AddNeckEntry(userID, DateSelect.Date, INCHneckValue);
        }
        else
        {
            await _localData.UpdateNeckEntry(userID, DateSelect.Date, INCHneckValue);
        }
        await Navigation.PopAsync();
        
    }
}
