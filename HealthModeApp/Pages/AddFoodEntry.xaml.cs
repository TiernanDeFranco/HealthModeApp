using System.ComponentModel;
using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages;

[QueryProperty(nameof(Food), "Food")]
public partial class AddFoodEntry : ContentPage
{
	public readonly IRestDataService _dataService;
	NutritionModel _nutritionModel;
    public string barcodeResult { get; set; }




    public NutritionModel Food
	{
		get => _nutritionModel;
		set
		{
			_nutritionModel = value;
			OnPropertyChanged(nameof(Food));
		}
	}


	protected override void OnAppearing()
	{
		base.OnAppearing();

        // Hide the TabBar on this page
        Shell.SetTabBarIsVisible(this, false);

    }



    public AddFoodEntry(IRestDataService dataService)
	{
		InitializeComponent();
		_nutritionModel = new NutritionModel();
		_dataService = dataService;
        BindingContext = this;

		

    }

	async void OnUploadEntryClicked(object sender, EventArgs e)
	{
		Debug.WriteLine("Uploading data");
		if (BarcodeEntry.Text == null)
		{
			Food.Barcode = Food.FoodName;
		}

		await _dataService.AddNutritionInfoAsync(Food);
        await Navigation.PopAsync();

		
    }


}

