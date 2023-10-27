using System.Globalization;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using HealthModeApp.Pages.Popups;
using Microsoft.Maui.Controls.Shapes;
using Mopups.Services;

namespace HealthModeApp.Pages.ProfilePage;

public partial class ChangeFlair
{
    public readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;

    UserProfile _sender;

    int flairID = 3;
    string flairString;

    int userID;
    string password;
    string email;

    public ChangeFlair(UserProfile sender, IRestDataService dataService, ISQLiteDataService localData)
    {
        InitializeComponent();
        _dataService = dataService;
        _localData = localData;
        _sender = sender;

       

        GetData();
    }

    List<FlairTable> _flairList;
    string cdnUrl = "https://d2f1hfw011wycq.cloudfront.net";

    async void GetData()
    {
        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var colorHex = userInfo.PictureBGColor;
        password = userInfo.Password;
        email = userInfo.Email;

        
        FlairLabel.Text = userInfo.Flair;
        FlairBG.BackgroundColor = Color.FromHex(userInfo.FlairColor);
       
       



        _flairList = await _dataService.GetFlairList(userID);
        FlairLoad.IsVisible = false;
        FlairList.IsVisible = true;

        foreach (var flair in _flairList)
        {
            flair.FlairBG = Color.FromHex(flair.ColorCode);
        }

        if (_flairList.Count < 4) FlairList.HeightRequest = 75;
        else if (_flairList.Count < 7) FlairList.HeightRequest = 125;
        else if (_flairList.Count < 10) FlairList.HeightRequest = 160;
        else if (_flairList.Count > 9) FlairList.HeightRequest = 180;



        FlairList.ItemsSource = _flairList;
        FlairList.ItemTemplate = new DataTemplate(() =>
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

            Label flairLabel = new Label
            {
                FontFamily = "Lato-Regular",
                FontSize = 12,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            flairLabel.SetBinding(Label.TextProperty, new Binding("FlairName"));

            var boolToColorConverter = new BoolToColorConverter();
            flairLabel.SetBinding(Label.TextColorProperty, new Binding("IsBlackText", converter: boolToColorConverter));

            Border flairBG = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 5 },
                Padding = new Thickness(1,3,1,3),
                Content = flairLabel,
                VerticalOptions = LayoutOptions.Center,
            };
            flairBG.SetBinding(Border.BackgroundColorProperty, new Binding("FlairBG"));


            Grid.SetRow(flairBG, 0);
            Grid.SetColumn(flairBG, 0);
            grid.Children.Add(flairBG);


            return grid;

        });




    }

    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBlackText)
            {
                return isBlackText ? Colors.Black : Colors.White;
            }

            throw new InvalidOperationException("Value must be a boolean");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack not implemented");
        }
    }

    int _flairID;
    string _flairName;
    string _flairBG;
    bool _isBlackText;

    void FlairList_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            // Get the selected item(s). For Single SelectionMode, you can access it like this:
            var selectedItem = e.CurrentSelection[0] as FlairTable; // Replace YourItemType with your actual data type.


            _flairID = selectedItem.FlairID;
            _flairName = selectedItem.FlairName;
            _flairBG = selectedItem.ColorCode;
            _isBlackText = selectedItem.IsBlackText;

            FlairLabel.Text = _flairName;
            FlairBG.BackgroundColor = selectedItem.FlairBG;
            FlairLabel.TextColor = _isBlackText ? Colors.Black : Colors.White;

        }
        FlairList.SelectedItem = null;
    }

    async void ChangeButton_Clicked(System.Object sender, System.EventArgs e)
    {
        ChangeButton.IsVisible = false;
        Loading.IsVisible = true;


        UserInfo flairInfo = new UserInfo
        {
            FlairID = _flairID
        };



        bool success = await _dataService.UpdateUserInfoAsync(flairInfo, email, password, userID);

        if (success)
        {
            await _localData.UpdateFlair(_flairName, _flairBG, _isBlackText);
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
}
