using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages;

public partial class Workouts : ContentPage
{
    private readonly ISQLiteDataService _localData;

    public Workouts()
    {
        InitializeComponent();

    }

    public Workouts(ISQLiteDataService localData)
    {
        InitializeComponent();
        _localData = localData;
        SeesAds();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SeesAds();
    }

    async void SeesAds()
    {
        Ad.IsVisible = await _localData.GetSeesAds();
        if (Ad.IsVisible)
        {
            WorkoutDash.Margin = new Thickness(0, 0, 0, 1);
        }
        else
        {
            WorkoutDash.Margin = new Thickness(0, 0, 0, 2);
        }
    }
}
