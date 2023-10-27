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
        emailInput = emailInput.ToLower();

        if (!string.IsNullOrEmpty(emailInput))
        {
            EmailRecover.IsVisible = false;
            EmailWait.IsVisible = true;
            await _dataService.PasswordRecovery(emailInput, 0);
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
            VerifyCode.IsVisible = false;
            CodeWait.IsVisible = true;
            ResendCode.InputTransparent = true;

            string result = await _dataService.CodeVerification(emailInput, codeInput);

            if (result == "CodeMatch")
            {
                CodeFrame.IsVisible = false;
                PasswordFrame.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Notice", "That code is invalid", "OK");

                CodeWait.IsVisible = false;
                VerifyCode.IsVisible = true;
                ResendCode.InputTransparent = false;
            }
        }
        else
        {
            await DisplayAlert("Notice", "Please fill out the code field", "OK");
        }
    }

    async void Button_Clicked_2(System.Object sender, System.EventArgs e)
    {
        ChangeButton.IsVisible = false;
        LoadingChange.IsVisible = true;

        if (!string.IsNullOrEmpty(PasswordField.Text) && (!string.IsNullOrEmpty(ConfirmField.Text)))
        {
            string password = PasswordField.Text.Trim();
            string confirmPassword = ConfirmField.Text.Trim();
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
                LoadingChange.IsVisible = false;
                ChangeButton.IsVisible = true;
                await DisplayAlert("Notice", "Passwords do not match", "OK");
            }
        }
        else
        {
            LoadingChange.IsVisible = false;
            ChangeButton.IsVisible = true;
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

    void Button_Clicked_3(System.Object sender, System.EventArgs e)
    {
        var message = GenerateCode();
        DisplayAlert("Code:", message, "OK");
    }



    string GenerateCode()
    {
        var random = new Random();
        var code = new char[5];
        var forbiddenWords = new List<string> { "COCK", "C0CK", "NIG", "N1G", "N!G", "ASS", "AS$", "A$S", "A$$",
            "CUNT", "DICK", "D1CK", "D!CK", "SLUT", "PENIS", "PEN15", "PEN!S", "PEN1S", "SHIT", "SH1T", "SH!T", "!@#$%&", "&%$#@!",
            "%#@", "&@%", "&!@", "234", "ABCDEFGH", "123", "456", "789", "ABC", "DEF", "TUV", "XYZ", "911", "SLAVE", "SL@VE", "SHAT",
            "AAA", "BBB", "CCC", "DDD", "EEE", "FFF", "GGG", "HHH", "III", "JJJ", "KKK", "FAG", "F@G", "!!", "@@", "##", "$$", "%%", "&&",
            "HICK", "FUK", "FUC", "PEDO", "PHILE", "RAPE", "R@PE", "CUM", "COON", "SPIC", "SPOOK", "KILL", "K!LL", "K1LL", "POO", "PEE",
            "SUC", "SUK", "SEX", "SEC", "SEK", "METH", "CRACK", "DIE", "D!E", "D1E", "HELL", "HE!", "HE1", "HEL", "LICK", "KOCK", "KOK", "K0K", "K0C",
            "TIT", "T!T", "T1T", "BOO", "B00", "BO0", "500", "5OO)", "OBS", "NUT", "JERK", "JACK", "PUSSY", "VAG", "SQU", "GTFO", "LMA", "QUIT", "STOP",
            "DAD", "MOM", "CODE", "PASS", "C0DE", "KEY", "RECOV", "666", "777", "GOO", "SPI", "SP1", "SP!", "JIZZ", "PORN", "CP", "CHIL",
            "SCAM", "HELP", "FAKE", "SEND", "NUDE", "NAKED", "GER", "GGA", "NULL", "@$", "@S", "SAC", "KNO", "COOCH", "PP", "KKK"};


        string letters = "ABCDDFFGHKJKLMNPQRSTVWXYZ";
        string numbers = "23456789";
        string specials = "@$$&??";

        code[0] = letters[random.Next(letters.Length)];
        code[1] = (letters + letters + numbers)[random.Next((letters + letters + numbers).Length)];
        code[2] = specials[random.Next(specials.Length)];
        code[3] = numbers[random.Next(numbers.Length)];
        code[4] = letters[random.Next(letters.Length)];

        var generatedCode = new string(code);

        // Check if the generated code contains any forbidden words
        foreach (var forbiddenWord in forbiddenWords)
        {
            if (generatedCode.Contains(forbiddenWord))
            {
                return GenerateCode();
            }
        }

        return generatedCode;
    }
}
