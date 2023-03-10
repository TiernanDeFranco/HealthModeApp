using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using HealthModeApp.Pages;

namespace HealthModeApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    async void OnScanFoodClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            {nameof(NutritionModel), new NutritionModel() }
        };

        await Shell.Current.GoToAsync(nameof(AddFoodEntry), navigationParameter);
    }
}

//test t2