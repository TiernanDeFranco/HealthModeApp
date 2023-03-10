using HealthModeApp.Pages;

namespace HealthModeApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(BarcodeScan), typeof(BarcodeScan));
		Routing.RegisterRoute(nameof(AddFoodEntry), typeof(AddFoodEntry));
    }
}
