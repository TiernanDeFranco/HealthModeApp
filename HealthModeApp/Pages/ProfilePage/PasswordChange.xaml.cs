using System.Security.Cryptography;
using System.Text;
using HealthModeApp.DataServices;
using HealthModeApp.Pages.Popups;
using Mopups.Services;

namespace HealthModeApp.Pages.ProfilePage;

public partial class PasswordChange
{
    public readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;
    int userID;
    string email;
    string currentPassword;

    public PasswordChange(IRestDataService dataService, ISQLiteDataService localData)
    {
        InitializeComponent();
        _dataService = dataService;
        _localData = localData;
        var width = DeviceDisplay.MainDisplayInfo.Width * .28;

        PasswordFrame.WidthRequest = width;
        ConfirmField.WidthRequest = PasswordFrame.Width;

        GetUserID();

    }

    async void GetUserID()
    {
        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        email = userInfo.Email;
        currentPassword = userInfo.Password;
    }

    
    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        CloseWhenBackgroundIsClicked = false;
        ChangeButton.IsVisible = false;
        Loading.IsVisible = true;

        if (!string.IsNullOrEmpty(PasswordField.Text) && (!string.IsNullOrEmpty(ConfirmField.Text)))
        {
            string password = PasswordField.Text.Trim();
            string confirmPassword = ConfirmField.Text.Trim();

            if (password == confirmPassword)
            {
                string salt = await _dataService.GetSaltByEmailAsync(email);

                if (salt != null)
                {
                    salt = salt.Replace("\"", "");
                    string hashedPassword = HashAndSaltPassword(password, salt);
                    bool success = await _dataService.InternalPasswordChange(email, currentPassword, hashedPassword);
                    if (success)
                    {
                        await _localData.UpdateUserAsync(userID, null, null, hashedPassword);
                        await MopupService.Instance.PopAsync();
                    }
                    else
                    {
                        await MopupService.Instance.PushAsync(new InfoPopup("Oops", "Unable to change password, please try again later.\nIf this issue persists, try logging out and clicking 'Forgot Password'"));
                        await MopupService.Instance.PopAsync();
                    }
                }
            }
            else
            {
                Loading.IsVisible = false;
                ChangeButton.IsVisible = true;
                await MopupService.Instance.PushAsync(new InfoPopup("Notice", "Passwords do not match"));
                CloseWhenBackgroundIsClicked = true;
            }
        }
        else
        {
            Loading.IsVisible = false;
            ChangeButton.IsVisible = true;
            await MopupService.Instance.PushAsync(new InfoPopup("Oops", "Please fill out all the fields"));
            CloseWhenBackgroundIsClicked = true;
        }
    }

    public static string HashAndSaltPassword(string password, string salt)
    {
        string saltedPassword = password + salt;
        var sha256 = SHA256.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword);
        byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
        string hashedPassword = Convert.ToBase64String(hashedBytes);
        hashedPassword = hashedPassword.Replace("+", "");
        hashedPassword = hashedPassword.Replace("/", "");
        return hashedPassword;
    }

}
