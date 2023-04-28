using BarcodeScanner.Mobile;
using HealthModeApp.DataServices;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class BarcodeScan : ContentPage
{
    private readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

	public BarcodeScan(IRestDataService dataService, ISQLiteDataService localData)
	{
        BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeScanner.Mobile.BarcodeFormats.Upca);
		InitializeComponent();
		CallPermission();
        _dataService = dataService;
        _localData = localData;
        
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
		if (permission) Camera.TorchOn = true;

    }


    async void CameraView_OnDetected(System.Object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
    {
        BarcodeResult result = e.BarcodeResults.FirstOrDefault();

        if (result != null)
        {
            string barcodeValue = result.DisplayValue;

            var foodInfo = await _dataService.GetNutritionInfoBarcodeAsync(barcodeValue);
            if (foodInfo == null)
            {
              
               bool yes = await DisplayAlert("Notice", "There is currently no entry for a food with that barcode in the database \n\n Do you want to add the correct nutritional information for it?\n\n", "Yes", "No");
                if (yes)
                {
                    var currentPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1];
                    Navigation.InsertPageBefore(new AddFoodEntry(_dataService, _localData, barcodeValue), currentPage);
                    await Navigation.PopAsync();
                }
                else
                {
                    await Navigation.PopAsync();
                }



            }
            else
            {
               // await Navigation.PushAsync(new FoodInfo(barcodeValue));
            }


            
        }
    }


}
