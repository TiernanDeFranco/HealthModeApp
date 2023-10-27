using System.Diagnostics;
using System.Text.Json;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.Progress;

public partial class NewBodyFat : ContentPage
{
    
    private readonly ISQLiteDataService _localData;
    int userID;
    DateTime _date;
    WeightTable _weightEntry;

    public NewBodyFat(ISQLiteDataService localData, DateTime date, WeightTable weightEntry)
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
        DateHandler();
        await Task.Delay(100);

        if (_weightEntry == null)
        {
            var number = await _localData.GetBodyFat(userID, _date);
            number = Math.Round(number, 1);
            BodyFatEntry.Placeholder = number.ToString("0.#");
            BodyFatEntry.Text = number.ToString("0.#");
        }
        else
        {
            var number = Math.Round(_weightEntry.BodyFat, 1);
            BodyFatEntry.Placeholder = number.ToString("0.#");
            BodyFatEntry.Text = number.ToString("0.#");
        }
    }

    async void DateHandler()
    {
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];

        DateSelect.Format = $"   {dateFormat}";
        DateSelect.Date = _date;
    }

    async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
    {
        Debug.WriteLine("Clicked");
        userID = await _localData.GetUserID();
        decimal.TryParse(BodyFatEntry.Text, out decimal bodyFatValue);
        Debug.WriteLine(bodyFatValue);
        bool entryExists = await _localData.GetWeightForDay(userID, DateSelect.Date);
        if (!entryExists)
        {
            await _localData.AddBodyFatEntry(userID, DateSelect.Date, bodyFatValue);
        }
        else
        {
            await _localData.UpdateBodyFatEntry(userID, DateSelect.Date, bodyFatValue);
        }
        await Navigation.PopAsync();
        
    }
}
