

namespace HealthModeApp.Pages.WorkoutPages;

public partial class RelaxVideo : ContentPage
{
	string videoID = "MrAe-lJCRTM";
	public RelaxVideo()
	{
		InitializeComponent();
		YoutubeVideo.Source = new UrlWebViewSource
		{
			Url = "https://www.youtube.com/embed/" + videoID,
		};
	}
}
