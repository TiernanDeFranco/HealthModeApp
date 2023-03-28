using HealthModeApp.Pages;

namespace HealthModeApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AddFoodEntry), typeof(AddFoodEntry));
        Routing.RegisterRoute(nameof(Dashboard), typeof(Dashboard));

        TabBar.CurrentItem = TabBar.Items[2];
    }
}
