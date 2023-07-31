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
        SeesAds();

       

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

    protected override void OnDisappearing()
    {
        WorkoutPageGrid.FadeTo(1, 100);
        ExerciseSearch.FadeTo(0, 100);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SeesAds();
    }

    async void SeesAds()
    {
        
    }

    void ExerciseLibraryTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        BodyPartGrid.WidthRequest = ExerciseSearch.Width * .98;
        BodyPartGrid.HeightRequest = BodyPartGrid.Width / 4.5;

        WorkoutPageGrid.FadeTo(0, 250);
        ExerciseSearch.FadeTo(1, 220);

    }

    void StopSearchingClicked(System.Object sender, System.EventArgs e)
    {
        WorkoutPageGrid.FadeTo(1, 220);
        ExerciseSearch.FadeTo(0, 250);
    }

    void RecoveryClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new Recovery());
    }
}
