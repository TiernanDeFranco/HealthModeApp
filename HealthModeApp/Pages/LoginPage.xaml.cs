

using System.Data.SqlTypes;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using HealthModeApp.DataServices;

namespace HealthModeApp.Pages;

public partial class LoginPage : ContentPage
{


    public readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

    public LoginPage(IRestDataService dataService, ISQLiteDataService localData)
	{
		InitializeComponent();
        _localData = localData;
        _dataService = dataService;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
       

    }

    async void LoginButton_Clicked(System.Object sender, System.EventArgs e)
    {
        var email = EmailLogin.Text;
        var password = PasswordLogin.Text;
        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
        {
            string salt = await _dataService.GetSaltByEmailAsync(email);


            if (salt != null)
            {
                salt = salt.Replace("\"", "");
                string hashedPassword = HashAndSaltPassword(password, salt);
                Debug.WriteLine(hashedPassword);
                Debug.WriteLine(salt);
                string success = await _dataService.LoginAsync(email, hashedPassword);
                if (success == "AllClear")
                {
                   int userID = await _dataService.GetUserIDByEmailAsync(email); Debug.WriteLine(userID);
                    (var userInfo, bool seesAds) = await _dataService.GetUserInfoOnLoginAsync(userID);
                    Debug.WriteLine(seesAds);
                    Debug.WriteLine("Add User:");
                    await _localData.AddUserAsync(userID, userInfo.Email, userInfo.Username, userInfo.Password, seesAds, userInfo.WeightPlan, userInfo.MainGoals, userInfo.Units, userInfo.Sex, userInfo.HeightCm, userInfo.Birthday, userInfo.Weight, userInfo.GoalWeight, userInfo.ActivityLevel);
                    await Navigation.PopModalAsync();
                }

                else await DisplayAlert("Notice", "Invalid password", "OK");
            }
            else 
            {
              
                await DisplayAlert("Notice", "No account found with the specified email address.", "OK");
            }
        }
        else if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Notice", "Email field is empty", "OK");
        }
        else if (string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Notice", "Password field is empty", "OK");
        }

    }

    async void RegisterButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushModalAsync(new SignUpPage(_localData, _dataService));
    }


    public static string HashAndSaltPassword(string password, string salt)
    {
        string saltedPassword = password + salt; Debug.WriteLine(saltedPassword);
        var sha256 = SHA256.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword); Debug.WriteLine(passwordBytes);
        byte[] hashedBytes = sha256.ComputeHash(passwordBytes); Debug.WriteLine(hashedBytes);
        string hashedPassword = Convert.ToBase64String(hashedBytes); Debug.WriteLine(hashedPassword);
        hashedPassword = hashedPassword.Replace("+", "");
        hashedPassword = hashedPassword.Replace("/", "");
        return hashedPassword;
    }











}
