using HealthModeApp.DataServices;
using HealthModeApp.Pages;
using ZXing.Net.Maui;

namespace HealthModeApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseBarcodeReader()
            .ConfigureMauiHandlers(h =>
            {
                h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));

                h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraView), typeof(CameraViewHandler));

                h.AddHandler(typeof(ZXing.Net.Maui.Controls.BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
            })
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		



		builder.Services.AddSingleton<IRestDataService, RestDataService>();

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddTransient<BarcodeScan>();
		builder.Services.AddTransient<AddFoodEntry>();


	




		return builder.Build();
	}
}
