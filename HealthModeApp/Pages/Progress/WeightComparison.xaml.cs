using HealthModeApp.DataServices;
using SkiaSharp;
using System.Text.Json;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.Progress;

public partial class WeightComparison : ContentPage
{
    private readonly ISQLiteDataService _localData;

    decimal deltaWeight;
    string weightUnit;

	public WeightComparison(WeightTable beforeWeight, WeightTable afterWeight, ISQLiteDataService localData)
	{
		InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
        _localData = localData;

        PopulateFrame(beforeWeight, afterWeight);

        
    }

    async void PopulateFrame(WeightTable beforeWeight, WeightTable afterWeight)
    {
        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        weightUnit = unitList[0];


        switch (dateFormat)
        {
            case "MM/dd/yy":
                BeforeDate.Text = beforeWeight.Date.ToString("MMM d, yyyy");
                AfterDate.Text = afterWeight.Date.ToString("MMM d, yyyy");
                break;

            case "dd/MM/yy":
                BeforeDate.Text = beforeWeight.Date.ToString("d MMM, yyyy");
                AfterDate.Text = afterWeight.Date.ToString("d MMM, yyyy");
                break;

            case "yy/MM/dd":
                BeforeDate.Text = beforeWeight.Date.ToString("MMM d YYYY");
                AfterDate.Text = afterWeight.Date.ToString("MMM d YYYY");
                break;
        }

        

        if (beforeWeight.ProgressPicture != null && beforeWeight.ProgressPicture.Length > 0)
        {
            BeforePic.Source = ImageSource.FromStream(() => new MemoryStream(beforeWeight.ProgressPicture));
        }
        if (afterWeight.ProgressPicture != null && afterWeight.ProgressPicture.Length > 0)
        {
            AfterPic.Source = ImageSource.FromStream(() => new MemoryStream(afterWeight.ProgressPicture));
        }

        var scale = DeviceDisplay.MainDisplayInfo.Width * .15; ;

        BeforePic.WidthRequest = scale;
        BeforePic.HeightRequest = scale;
        AfterPic.WidthRequest = scale;
        AfterPic.HeightRequest = scale;

        StartWeightLabel.Text = beforeWeight.Weight.ToString($"0.# {weightUnit}");
        CurrentWeightLabel.Text = afterWeight.Weight.ToString($"0.# {weightUnit}");

        deltaWeight = beforeWeight.Weight - afterWeight.Weight;

        if (deltaWeight > 0)
        {
            DeltaWeightLabel.Text = Math.Round(deltaWeight, 2).ToString($"0.# {weightUnit}\n Lost");
        }
        else if (deltaWeight < 0)
        {
            DeltaWeightLabel.Text = Math.Abs(Math.Round(deltaWeight, 2)).ToString($"0.# {weightUnit}\n Gained");
        }
        else
        {
            DeltaWeightLabel.Text = Math.Round(deltaWeight, 2).ToString($"0.# {weightUnit}\nChange");
        }
    }




}
