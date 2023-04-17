using HealthModeApp.Pages;
using SQLite;

namespace HealthModeApp;

public partial class App : Application
{
    public App()
	{
        InitializeComponent();

        MainPage = new MainPage();
    }
}
