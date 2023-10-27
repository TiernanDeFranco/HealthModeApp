using Mopups.Services;

namespace HealthModeApp.Pages.Popups;

public partial class InfoPopup
{
	public InfoPopup(string title, string text)
	{
		InitializeComponent();
		TitleLabel.Text = title;
		TextLabel.Text = text;
		
	}

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
		MopupService.Instance.PopAsync();
    }
}
