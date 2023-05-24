
using HealthModeApp.DataServices;
using HealthModeApp.Pages;
using HealthModeApp.Pages.FoodJournalPage;
using SQLite;

using BarcodeScanner.Mobile;

using SkiaSharp.Views.Maui.Controls.Hosting;

namespace HealthModeApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseSkiaSharp(true)
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Lato-Bold.ttf", "Lato-Bold");
				fonts.AddFont("Lato-Regular.ttf", "Lato-Regular");
			})
			.ConfigureMauiHandlers(handlers =>
			{
				handlers.AddBarcodeScannerHandler();
			});

		



		builder.Services.AddSingleton<IRestDataService, RestDataService>();

        builder.Services.AddSingleton<ISQLiteDataService, SQLiteDataService>();


        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<SignUpPage>();
        builder.Services.AddTransient<Workouts>();
        builder.Services.AddTransient<Dashboard>();

        builder.Services.AddTransient<FoodJournal>();
		builder.Services.AddTransient<MealPage>();
        builder.Services.AddTransient<FoodSearch>();
        builder.Services.AddTransient<BarcodeScan>();
        builder.Services.AddTransient<AddFoodEntry>();
        builder.Services.AddTransient<FoodInfo>();

        builder.Services.AddTransient<MorePage>();







        return builder.Build();
	}
}
