using HealthModeApp.Pages.FoodJournalPage;

namespace HealthModeApp.Pages;

public partial class MainPage : Shell
{
	public MainPage()
	{
		InitializeComponent();
        

       

        TabBar.CurrentItem = TabBar.Items[2];
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
    }
}
