namespace HealthModeApp.Pages.Popups;
using HealthModeApp.Pages;
using HealthModeApp.Pages.FoodJournalPage;
using HealthModeApp.Pages.WorkoutPages;
using HealthModeApp.Pages.ProfilePage;
using HealthModeApp.Pages.Progress;
using Mopups.Services;
using System.Diagnostics;
using HealthModeApp.CustomControls;

public partial class ListPopup
{

    OptionSelector sendBack;

    public ListPopup(OptionSelector sender, string title, List<string> listItems, Color titleColor = null)
	{
		InitializeComponent();
        sendBack = sender;
        TitleLabel.Text = title;
		PickerList.ItemsSource = listItems;

		if (titleColor != null)
		{
			TitleLabel.TextColor = titleColor;
		}
	}

    public event EventHandler<object> ItemSelected;

    void PickerList_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        sendBack.InvokeItemSelected(e.SelectedItem);
        sendBack.InvokeItemIndexSelected(e.SelectedItemIndex);
       
        Debug.WriteLine("popup activated");
        MopupService.Instance.PopAsync();
    }
}
