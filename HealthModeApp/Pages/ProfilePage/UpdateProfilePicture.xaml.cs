using System.Diagnostics;
using System.Globalization;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using HealthModeApp.Pages.Popups;
using Maui.ColorPicker;
using Mopups.Services;

namespace HealthModeApp.Pages.ProfilePage;

public partial class UpdateProfilePicture
{
    public readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

    UserProfile _sender;

    Color _color;
    Color _unChangedColor;

    int pfpID = 3;
    string pfpPath;

    int userID;
    string password;
    string email;

    public UpdateProfilePicture(UserProfile sender, IRestDataService dataService, ISQLiteDataService localData, ImageSource pfpSource)
    {
        InitializeComponent();
        _dataService = dataService;
        _localData = localData;
        _sender = sender;

        ProfilePicture.Source = pfpSource;

        GetData();
    }

    List<ProfilePictures> _pfpList;
    string cdnUrl = "https://d2f1hfw011wycq.cloudfront.net";

    async void GetData()
    {
        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var colorHex = userInfo.PictureBGColor;
        password = userInfo.Password;
        email = userInfo.Email;
        _unChangedColor = Color.FromHex(colorHex);
        _color = _unChangedColor;
        PfpHolder.BackgroundColor = _unChangedColor;
        ColorToCoordinates(_unChangedColor);
        HexCodeEntry.Text = _unChangedColor.ToRgbaHex().ToString();

       

        _pfpList = await _dataService.GetPfpList(userID);
        PfpLoad.IsVisible = false;
        PfpList.IsVisible = true;

        if (_pfpList.Count < 4) PfpList.HeightRequest = 75;
        else if (_pfpList.Count < 7) PfpList.HeightRequest = 140;
        else if (_pfpList.Count > 6) PfpList.HeightRequest = 165;

        foreach (var pfp in _pfpList)
        {
            pfp.DummyImage = cdnUrl + "/profile-pictures" + pfp.FilePath;
        }

        PfpList.ItemsSource = _pfpList;
        PfpList.ItemTemplate = new DataTemplate(() =>
        {

            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(3),
                RowSpacing = 2,
                RowDefinitions =
                        {
                        new RowDefinition { Height = GridLength.Auto},
                       new RowDefinition { Height = GridLength.Auto},
                        },
                ColumnDefinitions =
                        {
                        new ColumnDefinition { Width = GridLength.Star }
                        }
            };

            Label pictureLabel = new Label
            {
                FontFamily = "Lato-Bold",
                FontSize = 15,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
                pictureLabel.SetBinding(Label.TextProperty, new Binding("PictureName"));


            Image profilePic = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 40,
                HeightRequest = 40

            };
            profilePic.SetBinding(Image.SourceProperty, new Binding("DummyImage"));

            Grid.SetRow(pictureLabel, 1);
            Grid.SetColumn(pictureLabel, 0);
            grid.Children.Add(pictureLabel);

            Grid.SetRow(profilePic, 0);
            Grid.SetColumn(profilePic, 0);
            grid.Children.Add(profilePic);


            return grid;

        });




    }

   


    async void ChangeButton_Clicked(System.Object sender, System.EventArgs e)
    {
        Debug.WriteLine($"Unchanged: {_unChangedColor.ToHex()}");
        Debug.WriteLine($"Color: {_color.ToHex()}");

        //Method for updating background color
        ChangeButton.IsVisible = false;
        Loading.IsVisible = true;


        UserInfo pfpInfo = new UserInfo
        {
            ProfilePictureID = pfpID,
            PictureBGColor = _color.ToHex()
        };

       

       bool success = await _dataService.UpdateUserInfoAsync(pfpInfo, email, password, userID);

        if (success)
        {
            Debug.WriteLine(pfpPath);
            await _localData.UpdatePFP(pfpPath, _color.ToHex());
            await MopupService.Instance.PopAsync();
            _sender.PopulateData();
        }
        else
        {
            await MopupService.Instance.PushAsync(new InfoPopup("Oops", "Something went wrong and your profile picture could not be updated at this time :("));
            Loading.IsVisible = false;
            ChangeButton.IsVisible = true;
            
        }
    }

    int timesChanged;

    void PfpColorPicker_PickedColorChanged(System.Object sender, Microsoft.Maui.Graphics.Color col)
    {
       // Debug.WriteLine(timesChanged);
        if (_color != col)
        {
            PfpHolder.BackgroundColor = col;
            ColorPreview.BackgroundColor = col;
            HexCodeEntry.Text = col.ToHex().ToString();

            Color inverseColor = new Color(1 - col.Red, 1 - col.Green, 1 - col.Blue);

            HexCodeEntry.TextColor = inverseColor;

            if (timesChanged < 2)
            {
                _color = _unChangedColor;
            }
            else
            {
                _color = col;
            }

            timesChanged++;
        }
    }

    private string acceptableChars = "#0123456789ABCDEFabcdef";

    void HexCodeEntry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (HexCodeEntry.Text.Any(c => !acceptableChars.Contains(c)))
        {
            // Reset the text to "#"
            HexCodeEntry.Text = "#";
        }

        try
        {
            if (focused)
            {
                
                if (!string.IsNullOrWhiteSpace(HexCodeEntry.Text) || (HexCodeEntry.Text != _color.ToHex()))
                {
                    string hexCode = HexCodeEntry.Text;

                    if (HexCodeEntry.Text == "") HexCodeEntry.Text = "#";
                    

                    if (HexCodeEntry.Text.Contains(" "))
                    {
                        // Remove the space character
                        HexCodeEntry.Text = HexCodeEntry.Text.Replace(" ", "");
                    }

                        Color col = Color.FromHex(hexCode);

                    _color = col;
                    ColorPreview.BackgroundColor = col;
                    PfpHolder.BackgroundColor = col;



                    Color inverseColor = new Color(1 - col.Red, 1 - col.Green, 1 - col.Blue);

                    HexCodeEntry.TextColor = inverseColor;

                   

                }

            }

            if (timesChanged < 2)
            {
                _color = _unChangedColor;
            }
            else
            {
                _color = Color.FromHex(HexCodeEntry.Text);
            }

            timesChanged++;

           

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    bool focused = false;

    void HexCodeEntry_Unfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        focused = false;
        HexCodeEntry.Text = _color.ToHex().ToString();
     //   ColorToCoordinates(_color);
        CloseWhenBackgroundIsClicked = true;
        
    }

    void HexCodeEntry_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        CloseWhenBackgroundIsClicked = false;
        focused = true;
    }

    void ColorToCoordinates(Color color)
    {
        double saturation = color.GetSaturation();

        // Calculate brightness (value) based on the maximum channel value.
        double maxChannel = Math.Max(color.Red, Math.Max(color.Green, color.Blue));
        double minChannel = Math.Min(color.Red, Math.Min(color.Green, color.Blue));

        // Calculate brightness (value) based on the color's channels.
        double brightness = (maxChannel + minChannel) / 2.0000;

        // Calculate Y coordinate based on saturation and brightness.
        double y = 1.00000 - saturation * brightness;

        // Custom adjustment for colors in the green spectrum and Y > 0.6.
        if (color.Green > color.Red && color.Green > color.Blue && y > 0.55 && y < 0.85)
        {
            y = 0.6 + (y - 0.6) * 1.05; // Adjust this factor as needed.
        }


        double x = color.GetHue();
        Debug.WriteLine(x);
        Debug.WriteLine(y);
        PfpColorPicker.PointerRingPositionXUnits = x * 1.00000000000;
        PfpColorPicker.PointerRingPositionYUnits = y * 1.00000000000;
    }

    void PfpList_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            // Get the selected item(s). For Single SelectionMode, you can access it like this:
            var selectedItem = e.CurrentSelection[0] as ProfilePictures; // Replace YourItemType with your actual data type.


            pfpID = selectedItem.PictureID;
            pfpPath = selectedItem.FilePath;

            var pfpSource = cdnUrl + "/profile-pictures" + pfpPath;
            ProfilePicture.Source = new UriImageSource { Uri = new System.Uri(pfpSource) };


        }
        PfpList.SelectedItem = null;
    }



}
