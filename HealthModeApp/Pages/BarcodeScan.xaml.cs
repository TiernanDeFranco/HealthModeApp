using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using System;

namespace HealthModeApp.Pages;

public partial class BarcodeScan : ContentPage
{
	public BarcodeScan()
    {
		InitializeComponent();
        barcodeScanner.Options = new BarcodeReaderOptions()
        {
            AutoRotate = true,
            Formats = BarcodeFormats.OneDimensional,
        };
    
    }

    void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        
        Shell.Current.GoToAsync(nameof(AddFoodEntry));
    }

}
