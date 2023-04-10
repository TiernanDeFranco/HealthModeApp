
using HealthModeApp.DataServices;
using HealthModeApp.Pages;
using HealthModeApp.Pages.FoodJournalPage;
using SQLite;

namespace HealthModeApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Lato-Bold.ttf", "Lato-Bold");
				fonts.AddFont("Lato-Regular.ttf", "Lato-Regular");
			});

		



		builder.Services.AddSingleton<IRestDataService, RestDataService>();

        builder.Services.AddSingleton<ISQLiteDataService, SQLiteDataService>();

      


        builder.Services.AddTransient<AddFoodEntry>();
        builder.Services.AddTransient<Dashboard>();
        builder.Services.AddTransient<FoodJournal>();
		builder.Services.AddTransient<MealPage>();







        return builder.Build();
	}
}
