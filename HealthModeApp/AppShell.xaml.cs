using HealthModeApp.Pages;
using HealthModeApp.Pages.FoodJournalPage;
using SQLite;


namespace HealthModeApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(AddFoodEntry), typeof(AddFoodEntry));
        Routing.RegisterRoute(nameof(MealPage), typeof(MealPage));
        Routing.RegisterRoute(nameof(FoodJournal), typeof(FoodJournal));


        TabBar.CurrentItem = TabBar.Items[2];
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

   
}

   