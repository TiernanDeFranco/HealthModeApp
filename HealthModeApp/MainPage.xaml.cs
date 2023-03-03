using System.Diagnostics;
using HealthModeApp.DataServices;

namespace HealthModeApp;

public partial class MainPage : ContentPage
{
    private IRestDataService _dataService;

    public MainPage(IRestDataService dataService)
	{
		InitializeComponent();

		_dataService = dataService;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		dataList.ItemsSource = await _dataService.GetAllNutritionInfoAsync();
	}

	async void OnAddDataClicked(object sender, EventArgs e)
	{
		Debug.WriteLine("----> Add button clicked");
	}

	async void OnEditDataClicked(object sender, SelectionChangedEventArgs e)
	{
		Debug.WriteLine("----> Edit button clicked");

    }

    async void OnRefreshClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("----> Update button clicked");
        dataList.ItemsSource = await _dataService.GetAllNutritionInfoAsync();
    }

}

//test t2