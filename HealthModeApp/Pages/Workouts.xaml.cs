using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using HealthModeApp.Pages.WorkoutPages;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Maui.Graphics.Text;
using SkiaSharp;

namespace HealthModeApp.Pages;

public partial class Workouts : ContentPage
{
    private readonly ISQLiteDataService _localData;
    private readonly IRestDataService _dataService;

    public Workouts()
    {
        InitializeComponent();
        
    }

    async void TranslatePage()
    {
        ProgressLabel.Text = await _localData.GetTranslationByKey("Progress");
        RecoveryLabel.Text = await _localData.GetTranslationByKey("Recovery");
        WarmupLabel.Text = await _localData.GetTranslationByKey("WarmupStretch");
        WorkoutLabel.Text = await _localData.GetTranslationByKey("Workouts");
        ExLibrary.Text = await _localData.GetTranslationByKey("ExerciseLibrary");
        RelaxText.Text = await _localData.GetTranslationByKey("Relax");

        StepLabel.Text = await _localData.GetTranslationByKey("Steps");
        //Make adaptive to cal, kj
        CaloriesBurnedLabel.Text = await _localData.GetTranslationByKey("Calories");
        ExerciseMinutesLabel.Text = await _localData.GetTranslationByKey("Exercise");
    }

    public ISeries[] Series { get; set; } =
   {
        new LineSeries<double>
        {
            LineSmoothness = 0.2f, // Optional: adjust the line smoothness to your liking
            Values = new double[] { 1, 8, 5, 3, 12, 7, 17, 28 },
            Fill = null,
            GeometryFill = new SolidColorPaint(SKColor.Parse("#4b9fe3")),
            Stroke = new SolidColorPaint(SKColor.Parse("#4b9fe3")) { StrokeThickness = 14 },
            GeometryStroke = null
        }
    };



    public Workouts(ISQLiteDataService localData)
    {
        InitializeComponent();
        _localData = localData;

        var scale = DeviceDisplay.MainDisplayInfo.Width * .13;

        ProgressChart.HeightRequest = scale * .93;

        MeditationIcon.WidthRequest = scale;
        MeditationIcon.HeightRequest = scale;

        StretchIcon.WidthRequest = scale;
        StretchIcon.HeightRequest = scale * .98;

        DumbbellIcon.WidthRequest = scale;
        DumbbellIcon.HeightRequest = scale;

        ExerciseLibrary.WidthRequest = scale;
        ExerciseLibrary.HeightRequest = scale;

        RecoveryIcon.WidthRequest = scale;
        RecoveryIcon.HeightRequest = scale;

        ProgressChart.XAxes = new[]
            {
                new Axis
                {
                     TextSize = 0,
                     LabelsPaint = new SolidColorPaint(SKColors.Transparent)
                }
             };

        ProgressChart.YAxes = new[]
           {
                new Axis
                {
                     TextSize = 0,
                     LabelsPaint = new SolidColorPaint(SKColors.Transparent),
                }
           };

        ProgressChart.Series = Series;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        TranslatePage();
    }

    void ProgressTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new ProgressHub(_dataService, _localData));
    }

    void ExerciseLibraryTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new ExerciseSearch());
    }


    void RecoveryTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new Recovery());
    }

    void RelaxationTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new RelaxVideo());
    }
}
