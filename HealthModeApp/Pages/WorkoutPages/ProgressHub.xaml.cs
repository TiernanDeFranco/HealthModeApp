namespace HealthModeApp.Pages.WorkoutPages;

using HealthModeApp.DataServices;
using HealthModeApp.Pages.Progress;

public partial class ProgressHub : ContentPage
{
    private readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

	public ProgressHub(IRestDataService dataService, ISQLiteDataService localData)
	{
		InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
        _dataService = dataService;
        _localData = localData;
	}

    void WeightTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new WeightProgress(_dataService, _localData));
    }
}
