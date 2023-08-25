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
}
