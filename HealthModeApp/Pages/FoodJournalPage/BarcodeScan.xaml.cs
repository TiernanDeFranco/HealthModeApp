using HealthModeApp.DataServices;
using BarcodeScanner.Mobile;
using HealthModeApp.Models;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class BarcodeScan : ContentPage
{
    private readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

    public NutritionModel _food;
    int _mealType;
    DateTime _date;

    public BarcodeScan(IRestDataService dataService, ISQLiteDataService localData, int mealType, DateTime date)
	{
        BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.Upca | BarcodeFormats.Upce | BarcodeFormats.Ean13 | BarcodeFormats.Ean8);
		InitializeComponent();
		CallPermission();
        _dataService = dataService;
        _localData = localData;
        _mealType = mealType;
        _date = date;
        
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Hide the TabBar on this page
        Shell.SetTabBarIsVisible(this, false);

    }

    async void CallPermission()
	{
     bool permission = await BarcodeScanner.Mobile.Methods.AskForRequiredPermission();
        if (permission)
        {
            Camera.TorchOn = true;
        }

    }


    async void CameraView_OnDetected(System.Object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
    {
        Loading.IsVisible = true;
        BarcodeResult result = e.BarcodeResults.FirstOrDefault();

        if (result != null)
        {
            string barcodeValue = result.DisplayValue;

            var customFood = await _localData.GetCustomFoodByBarcode(barcodeValue);

            if (customFood == null)
            {
                var foodInfo = await _dataService.GetNutritionInfoBarcodeAsync(barcodeValue);

                if (foodInfo == null)
                {
                    bool shouldAddFood = await DisplayAlert("Notice", "There is currently no entry for a food with that barcode in the database.\n\nDo you want to add the correct nutritional information for it?", "Yes", "No");

                    if (shouldAddFood)
                    {
                        var currentPage = Navigation.NavigationStack.LastOrDefault();
                        Navigation.InsertPageBefore(new AddFoodEntry(_dataService, _localData, barcodeValue, _mealType, _date), currentPage);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Loading.IsVisible = false;
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    var currentPage = Navigation.NavigationStack.LastOrDefault();
                    Navigation.InsertPageBefore(new FoodInfo(_dataService, _localData, foodInfo, _mealType, _date), currentPage);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                var currentPage = Navigation.NavigationStack.LastOrDefault();
                Navigation.InsertPageBefore(new FoodInfo(_dataService, _localData, customFood, _mealType, _date), currentPage);
                await Navigation.PopAsync();
            }
        }

    }


}
