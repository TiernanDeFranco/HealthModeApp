using HealthModeApp.Data;
using HealthModeApp.DataServices;
using HealthModeApp.Pages;
using Microsoft.EntityFrameworkCore;
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
				fonts.AddFont("Lato-Bold.ttf", "Lato-Bold");
				fonts.AddFont("Lato-Regular.ttf", "Lato-Regular");
			});

		



		builder.Services.AddSingleton<IRestDataService, RestDataService>();

		builder.Services.AddTransient<BarcodeScan>();
		builder.Services.AddTransient<AddFoodEntry>();
        builder.Services.AddTransient<Dashboard>();

		builder.Services.AddDbContext<AppDBContext>(opt =>opt.UseSqlite("Data Source=healthmodelocal.db"));







        return builder.Build();
	}
}
