using System.Text.Json;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class WaterUpdate : ContentPage
{
    ISQLiteDataService _localData;
    IRestDataService _dataService;
    double volume1;
    double volume2;
    double volume3;
    double volume4;
    string _waterUnit;
    int userID;
    int _waterID;
    WaterTable _water;

	public WaterUpdate(ISQLiteDataService localData, IRestDataService dataService, WaterTable selectedItem)
	{
		InitializeComponent();
        _localData = localData;
        _dataService = dataService;
        _waterID = selectedItem.WaterID;
        _water = selectedItem;

        WaterIcon.Source = new ImageSourceConverter().ConvertFrom(selectedItem.WaterImage) as ImageSource;

        waterFrame.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .31;
        WaterEntry.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .05;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        _waterUnit = unitList[2];
        decimal mLVolume = 0;

        switch (_waterUnit)
        {
            case "fl oz":
                volume1 = 8;
                volume2 = 12;
                volume3 = 16.9;
                volume4 = 32;
                break;

            case "cups":
                volume1 = 1;
                volume2 = 1.5;
                volume3 = 2;
                volume4 = 4;
                break;

            case "mL":
                volume1 = 235;
                volume2 = 355;
                volume3 = 500;
                volume4 = 1000;
                break;
        }

        Volume1.Text = volume1.ToString($"0.# {_waterUnit}");
        Volume2.Text = volume2.ToString($"0.# {_waterUnit}");
        Volume3.Text = volume3.ToString($"0.# {_waterUnit}");
        Volume4.Text = volume4.ToString($"0.# {_waterUnit}");
        WaterLabel.Text = _waterUnit.ToString();
        WaterEntry.Text = null;
        Unit.Text = _waterUnit;
        Volume.Text = _water.WaterVolume.ToString("0.# ");
    }

    async void ChangeUnitClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushModalAsync(new Pages.UnitPage(_localData, _dataService));

    }

    void Volume1_Clicked(System.Object sender, System.EventArgs e)
    {
        if (double.TryParse(WaterEntry.Text, out var volume))
        {
            // Perform operations using the volume1 variable as a double
            // For example:
            volume += volume1;

            // Convert the updated volume1 back to string and assign it to WaterEntry.Text
            WaterEntry.Text = volume.ToString("0.#");
        }
        else
        {
            WaterEntry.Text = volume1.ToString("0.#");
        }

    }

    void Volume2_Clicked(System.Object sender, System.EventArgs e)
    {
        if (double.TryParse(WaterEntry.Text, out var volume))
        {
            // Perform operations using the volume1 variable as a double
            // For example:
            volume += volume2;

            // Convert the updated volume1 back to string and assign it to WaterEntry.Text
            WaterEntry.Text = volume.ToString("0.#");
        }
        else
        {
            WaterEntry.Text = volume2.ToString("0.#");
        }
    }

    void Volume3_Clicked(System.Object sender, System.EventArgs e)
    {
        if (double.TryParse(WaterEntry.Text, out var volume))
        {
            // Perform operations using the volume1 variable as a double
            // For example:
            volume += volume3;

            // Convert the updated volume1 back to string and assign it to WaterEntry.Text
            WaterEntry.Text = volume.ToString("0.#");
        }
        else
        {
            WaterEntry.Text = volume3.ToString("0.#");
        }
    }

    void Volume4_Clicked(System.Object sender, System.EventArgs e)
    {
        if (double.TryParse(WaterEntry.Text, out var volume))
        {
            // Perform operations using the volume1 variable as a double
            // For example:
            volume += volume4;

            // Convert the updated volume1 back to string and assign it to WaterEntry.Text
            WaterEntry.Text = volume.ToString("0.#");
        }
        else
        {
            WaterEntry.Text = volume4.ToString("0.#");
        }
    }

    async void AddWaterEntry_Clicked(System.Object sender, System.EventArgs e)
    {
        decimal mLVolume = 0;
        userID = await _localData.GetUserID();
        if (decimal.TryParse(WaterEntry.Text, out var volume))
        {
            switch (_waterUnit)
            {
                case "mL":
                    mLVolume = volume;
                    break;
                case "fl oz":
                    mLVolume = Math.Round(volume * (decimal)29.5735, 1);
                    break;
                case "cups":
                    mLVolume = Math.Round(volume * (decimal)236.588, 1);
                    break;
            }

            try
            {
                await _localData.UpdateWaterEntry(_waterID, mLVolume);
            }
            catch
            {
                await DisplayAlert("Notice", "Something went wrong trying to add that water entry, sorry :(", "OK");
            }
            await Navigation.PopAsync();
           
        }
        else
        {
            await DisplayAlert("Notice", "Something went wrong trying to add that water entry, sorry :(", "OK");
        }


    }

    async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
    {
        await _localData.DeleteWaterEntry(_waterID);
        await Navigation.PopAsync();
    }
}
