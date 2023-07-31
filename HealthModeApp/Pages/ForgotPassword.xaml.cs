using System.Security.Cryptography;
using System.Text;
using HealthModeApp.DataServices;

namespace HealthModeApp.Pages;

public partial class ForgotPassword : ContentPage
{

    public readonly IRestDataService _dataService;
    string emailInput;
    string codeInput;

    public ForgotPassword(IRestDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        var width = DeviceDisplay.MainDisplayInfo.Width * .28;
        EmailFrame.WidthRequest = width;
        CodeFrame.WidthRequest = width;
        PasswordFrame.WidthRequest = width;

    }

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        emailInput = EmailName.Text.Trim();

        if (!string.IsNullOrEmpty(emailInput))
        {
            await _dataService.PasswordRecovery(emailInput);
            EmailFrame.IsVisible = false;
            CodeFrame.IsVisible = true;
        }
        else
        {
            await DisplayAlert("Notice", "Please fill out the email field", "OK");
        }

    }

    async void Button_Clicked_1(System.Object sender, System.EventArgs e)
    {
        codeInput = CodeField.Text.Trim();
        if (!string.IsNullOrEmpty(codeInput))
        {
            string result = await _dataService.CodeVerification(emailInput, codeInput);

            if (result == "CodeMatch")
            {
                CodeFrame.IsVisible = false;
                PasswordFrame.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Notice", "That code is invalid", "OK");
            }
        }
        else
        {
            await DisplayAlert("Notice", "Please fill out the code field", "OK");
        }
    }

    async void Button_Clicked_2(System.Object sender, System.EventArgs e)
    {
        string password = PasswordField.Text.Trim();
        string confirmPassword = ConfirmField.Text.Trim();

        if (!string.IsNullOrEmpty(password) && (!string.IsNullOrEmpty(confirmPassword)))
        {
            if (password == confirmPassword)
            {
                string salt = await _dataService.GetSaltByEmailAsync(emailInput);

                if (salt != null)
                {
                    salt = salt.Replace("\"", "");
                    string hashedPassword = HashAndSaltPassword(password, salt);
                    await _dataService.PasswordChange(emailInput, codeInput, hashedPassword);
                    await Navigation.PopModalAsync();
                }
            }
            else
            {
                await DisplayAlert("Notice", "Passwords do not match", "OK");
            }
        }
        else
        {
            await DisplayAlert("Notice", "Please fill out all the fields", "OK");
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

    async void BackButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

}
