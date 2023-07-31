
using System.Diagnostics;
using System.Text.Json;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.Progress;

public partial class NewWeightEntry : ContentPage
{
    private readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;
    int userID;
    DateTime _date;
    byte[] pictureBytes;
    WeightTable _weightEntry;

    public NewWeightEntry(IRestDataService dataService, ISQLiteDataService localData, DateTime date, WeightTable weightEntry)
	{
		InitializeComponent();
        _dataService = dataService;
        _localData = localData;
        _date = date;
        _weightEntry = weightEntry;
        if (_weightEntry != null) pictureBytes = weightEntry.ProgressPicture;

        if (pictureBytes != null && pictureBytes.Length > 0)
        {
            var imageSource = ImageSource.FromStream(() => new MemoryStream(pictureBytes));
            ProgressPicture.Source = imageSource;
            PictureFrame.IsVisible = true;
        }
    }

   

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Hide the TabBar on this page
        Shell.SetTabBarIsVisible(this, false);
        DateHandler();
        userID = await _localData.GetUserID();
        await Task.Delay(100);
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var weightUnit = unitList[0];


        if (_weightEntry == null)
        {
            var number = await _localData.GetWeight(userID, _date);
            if (weightUnit == "kg") number /= Math.Round((decimal)2.2, 1);
            WeightEntry.Placeholder = number.ToString();
            WeightEntry.Text = number.ToString();
        }
        else
        {
            var number = Math.Round(_weightEntry.Weight, 1);
            WeightEntry.Placeholder = number.ToString();
            WeightEntry.Text = number.ToString();
        }

       

    }

    async void DateHandler()
    {
        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[4];

        DateSelect.Format = $"   {dateFormat}";
        DateSelect.Date = _date;
    }

    async void ProgressPicture_Clicked(System.Object sender, System.EventArgs e)
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Photos>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Photos>();
        }

        if (status == PermissionStatus.Granted)
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    pictureBytes = memoryStream.ToArray();
                }

                if (pictureBytes != null && pictureBytes.Length > 0)
                {
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(pictureBytes));
                    ProgressPicture.Source = imageSource;
                    ProgressPicture.IsAnimationPlaying = false;
                    PictureFrame.Opacity = 0;
                    PictureFrame.IsVisible = true;
                    PictureFrame.FadeTo(1, 500);
                    ProgressPicture.IsAnimationPlaying = true;
                    ProgressPicture.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .27;
                    ProgressPicture.Aspect = Aspect.AspectFit;
                }




            }
            else { PictureFrame.IsVisible = false; ProgressPicture.Source = null; pictureBytes = null; }
        }
        else
        {
            await DisplayAlert("Notice", "You need to give HealthMode access to your photos", "OK");
        }
        
    }

    async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
    {
        userID = await _localData.GetUserID();

        decimal.TryParse(WeightEntry.Text, out decimal weightValue);
        if (weightValue >= 50)
        {
            var userInfo = await _localData.GetUserAsync(userID);
            var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
            var weightUnit = unitList[0];

            decimal weightLbs = 0;
            switch (weightUnit)
            {
                case "lbs":
                    weightLbs = weightValue;
                    break;

                case "kg":
                    weightLbs = weightValue* (decimal)2.2;
                    break;
            }

            bool weightExists = await _localData.GetWeightForDay(userID, DateSelect.Date);

            if (!weightExists)
            {
                await _localData.AddWeightEntry(userID, DateSelect.Date, weightLbs, pictureBytes);
            }
            else
            {
                await _localData.UpdateWeightEntry(userID, DateSelect.Date, weightLbs, pictureBytes);
            }

            

            UserInfo userInfoModel = new UserInfo
            {
                UserID = userID,
                Weight = weightLbs
            };
            if (DateSelect.Date == DateTime.Today)
            {
                EntryFrame.IsVisible = false;
                PictureFrame.IsVisible = false;
                LoadingBar.IsVisible = true;
                LoadingBar.IsRunning = true;
                (string email, string password) = await _localData.GetUserCredentials();
                await _dataService.UpdateUserInfoAsync(userInfoModel, email, password, userID);
            }
            await Navigation.PopAsync();
        }
        else
        {
           await DisplayAlert("Notice", "Please enter a valid weight", "OK");
        }
    }

    async void DeleteImage_Clicked(System.Object sender, System.EventArgs e)
    {
        await PictureFrame.FadeTo(0, 500);
        ProgressPicture.Source = null; pictureBytes = null;
    }
}
