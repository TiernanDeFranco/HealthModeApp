using HealthModeApp.Pages.FoodJournalPage;

namespace HealthModeApp.Pages;

public partial class MainPage : Shell
{
	public MainPage()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

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
