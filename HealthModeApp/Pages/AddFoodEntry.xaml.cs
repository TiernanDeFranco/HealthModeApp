using System.ComponentModel;
using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages;

[QueryProperty(nameof(Food), "Food")]
public partial class AddFoodEntry : ContentPage
{
	private readonly IRestDataService _dataService;
	NutritionModel _nutritionModel;

	public NutritionModel Food
	{
		get => _nutritionModel;
		set
		{
			_nutritionModel = value;
			OnPropertyChanged(nameof(Food));
		}
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
		Debug.WriteLine(Food);
		await _dataService.AddNutritionInfoAsync(Food);
	}


}
