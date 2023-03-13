using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace HealthModeApp.Pages;

public partial class BarcodeScan : ContentPage
{
	public BarcodeScan()
    {
		InitializeComponent();
    
    }

    void BarcodesDetected(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        
        Shell.Current.GoToAsync(nameof(AddFoodEntry));
    }

}
