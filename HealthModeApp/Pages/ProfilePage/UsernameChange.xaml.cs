using HealthModeApp.DataServices;
using HealthModeApp.Pages.Popups;
using Mopups.Services;

namespace HealthModeApp.Pages.ProfilePage;

public partial class UsernameChange
{

    public readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

    string email;
    string password;
    int userID;
    UserProfile _sender;

    public UsernameChange(UserProfile sender, IRestDataService dataService, ISQLiteDataService localData)
	{
		InitializeComponent();
        _dataService = dataService;
        _localData = localData;
        _sender = sender;

        var width = DeviceDisplay.MainDisplayInfo.Width * .28;

        UsernameField.WidthRequest = width;
        GetUserID();

	}

    async void GetUserID()
    {
        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        email = userInfo.Email;
        password = userInfo.Password;
    }



    async void ChangeButton_Clicked(System.Object sender, System.EventArgs e)
    {
        CloseWhenBackgroundIsClicked = false;
        ChangeButton.IsVisible = false;
        Loading.IsVisible = true;

        if (!string.IsNullOrEmpty(UsernameField.Text))
        {
            string newUsername = UsernameField.Text.TrimEnd();
            var output = await _dataService.UpdateUsername(email, password, newUsername);
            if (output == "Success")
            {
                await _localData.UpdateUserAsync(userID, null, newUsername);
                await MopupService.Instance.PopAsync();
                _sender.PopulateData();
            }
            else
            {
                await MopupService.Instance.PushAsync(new InfoPopup("Oops", output));
                Loading.IsVisible = false;
                ChangeButton.IsVisible = true;
                CloseWhenBackgroundIsClicked = true;
            }
        }
        else
        {
            Loading.IsVisible = false;
            ChangeButton.IsVisible = true;
            await MopupService.Instance.PushAsync(new InfoPopup("Oops", "Please fill out the field"));
            CloseWhenBackgroundIsClicked = true;
        }
    }

    void UsernameField_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        CloseWhenBackgroundIsClicked = false;
    }

    void UsernameField_Unfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        CloseWhenBackgroundIsClicked = true;
    }
}
