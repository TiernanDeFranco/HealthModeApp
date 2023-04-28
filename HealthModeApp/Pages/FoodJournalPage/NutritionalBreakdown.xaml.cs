using HealthModeApp.DataServices;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class NutritionalBreakdown : ContentPage
{
	private readonly ISQLiteDataService _localData;

	public NutritionalBreakdown(ISQLiteDataService localData, DateTime setDate)
	{
        Shell.SetTabBarIsVisible(this, false);
        InitializeComponent();
		_localData = localData;
        DateSelect.Date = setDate;
        DateHandler();
	}

    async void DateHandler()
    {
       
        if (DateSelect.Date == DateTime.Today)
        {
            TodayButton.IsVisible = false;
        }
        else
        {
            TodayButton.IsVisible = true;

            var display = await _localData.GetPopUpSeen("TodayButtonPopup");
            if (display == false)
            {
                await DisplayAlert("Notice", "To navigate back the current date, you can click the newly created 'Jump To Today' Button", "OK");
                await _localData.AddPopUpSeen("TodayButtonPopup", true);
            }
        }
    }

    async void PopulateFoodInfo()
	{

	}



    void DateSelect_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {
        DateHandler();
		PopulateFoodInfo();
    }

    void TodayButton_Clicked(System.Object sender, System.EventArgs e)
    {
        DateSelect.Date = DateTime.Today;
    }
}
