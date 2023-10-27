namespace HealthModeApp.Pages;

public partial class UpdatePage : ContentPage
{
	public UpdatePage(string version)
	{
		InitializeComponent();
		VersionText.Text = $"Your app's current version of  [{version}]  is out of date and needs to be updated";
	}
}
