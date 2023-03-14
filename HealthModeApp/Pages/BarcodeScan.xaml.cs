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
            Formats = BarcodeFormats.OneDimensional,
            TryHarder = true

        };
        barcodeScanner.AutoFocus();
    }
    protected override void OnAppearing()
        {
            base.OnAppearing();

        barcodeScanner.IsDetecting = true;
        barcodeScanner.IsEnabled = true;
        
    }
            
         

    

    void barcodesDetected(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        barcodeScanner.IsDetecting = false;
        barcodeScanner.IsEnabled = false;
        Dispatcher.Dispatch( () =>
        {
          Shell.Current.GoToAsync(nameof(AddFoodEntry));
        });
    }

}
