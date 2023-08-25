namespace HealthModeApp.Pages.Progress;

using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using LiveChartsCore;

using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.Defaults;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using LiveChartsCore.Kernel.Sketches;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel;
using System.Globalization;
using Microsoft.Maui;

public partial class WeightProgress : ContentPage
{
    private readonly IRestDataService _dataService;
    private readonly ISQLiteDataService _localData;
    public ObservableCollection<ISeries> Series { get; set; }
    string _dateFormat;
    ImageSource _imageSource;

    DateTime beginningDate;
    DateTime endDate;

    List<WeightTable> weights;

    public WeightProgress(IRestDataService dataService, ISQLiteDataService localData)
    {
        InitializeComponent();
        _dataService = dataService;
        _localData = localData;

        var rangeOptions = new List<string>();
        rangeOptions.Add("1 Week");
        rangeOptions.Add("1 Month");
        rangeOptions.Add("2 Months");
        rangeOptions.Add("3 Months");
        rangeOptions.Add("6 Months");
        rangeOptions.Add("1 Year");
        rangeOptions.Add("All");

        RangePicker.ItemsSource = rangeOptions;
        RangePicker.SelectedIndex = 0;

        
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();
        FillGraph(RangePicker.SelectedIndex);
        // Hide the TabBar on this page
        Shell.SetTabBarIsVisible(this, false);

        
    }

    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is byte[]))
                return null;

            var byteArray = value as byte[];

            return ImageSource.FromStream(() => new MemoryStream(byteArray));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class ImageSourceConverter
    {
        public static ImageSource ConvertFromByteArray(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            return ImageSource.FromStream(() => new MemoryStream(byteArray));
        }
    }


    public ObservableCollection<DateTimePoint> ChartValues { get; set; }

    async void FillGraph(int index)
    {
        WeightEntries.Opacity = 0;

        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[4];
        var weightUnit = unitList[0];
        var weightPlan = userInfo.WeightPlan;

        switch (dateFormat)
        {
            case "MM/dd/yy":
                _dateFormat = "MMM d";
                break;

            case "dd/MM/yy":
                _dateFormat = "d MMM";
                break;

            case "yy/MM/dd":
                _dateFormat = "MMM d";
                break;
        }

        

        AppTheme currentTheme = Application.Current.RequestedTheme;
        var colorMode = SKColors.CornflowerBlue;
        switch (currentTheme)
        {
            case AppTheme.Light:
                colorMode = SKColors.DarkSlateGray;
                break;
            case AppTheme.Dark:
                colorMode = SKColors.White;
                break;
            case AppTheme.Unspecified:
                colorMode = SKColors.DarkSlateGray;
                break;

        }
        weights = null;
        WeightEntries.ItemsSource = null;
        weights = await _localData.GetWeightsIndex(userID, index);

        if (weights.Count == 0)
        {
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Weights In This Range";

            if (weightPlan == -1)
            {
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-7), 15));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-6), 12));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-5), 11));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-4), 13));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 9));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 6));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 4));
                ChartValues.Add(new DateTimePoint(DateTime.Today, 4));
            }
            else if (weightPlan == 0)
            {
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-7), 8));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-6), 8));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-5), 10));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-4), 10));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 9));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 7));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 7));
                ChartValues.Add(new DateTimePoint(DateTime.Today, 8));
            }
            else if (weightPlan == 1)
            {
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-7), 5));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-6), 5));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-5), 8));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-4), 10));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 9));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 9));
                ChartValues.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 11));
                ChartValues.Add(new DateTimePoint(DateTime.Today, 13));
            }

            var Weightseries = new LineSeries<DateTimePoint>()
            {
                Values = ChartValues,
                LineSmoothness = 0.1f, // Optional: adjust the line smoothness to your liking

                TooltipLabelFormatter = (chartPoint) =>
                    $"{new DateTime((long)chartPoint.SecondaryValue):MMMM d}: {chartPoint.PrimaryValue}",
                GeometryFill = new SolidColorPaint(SKColor.Parse("#67ade6")),
                Stroke = new SolidColorPaint(SKColor.Parse("#4b9fe3")) { StrokeThickness = 10 },
                Fill = null,
                GeometryStroke = null,

                GeometrySize = 3
            };

            WeightChart.Series = new ISeries[] { Weightseries };

            WeightChart.XAxes = new[]
                    {
                    new Axis
                        {
                             Labeler = value => new DateTime((long) value).ToString($"{_dateFormat}"),
                             UnitWidth = TimeSpan.FromDays(1).Ticks,
                             MinStep = TimeSpan.FromDays(1).Ticks,
                             TextSize = 20,
                             LabelsPaint = new SolidColorPaint(colorMode)



                        }
                     };

                    WeightChart.YAxes = new[]
                   {
                        new Axis
                            {
                                 TextSize = 21,
                                 MinStep = 3,
                                 LabelsPaint = new SolidColorPaint(SKColors.Transparent),
                                 MinLimit = 0,
                                 MaxLimit = 20
                            }
                   };
        }

        if (weights.Count > 0)
        {

            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startWeight;
            decimal endWeight;

            if (weightUnit == "kg")
            {
                startWeight = firstWeight.Weight / (decimal)2.2;
                endWeight = lastWeight.Weight / (decimal)2.2;
            }
            else { startWeight = firstWeight.Weight; endWeight = lastWeight.Weight; }

            StartWeight.Text = startWeight.ToString($"0.# {weightUnit}\nStarting");
            EndWeight.Text = endWeight.ToString($"0.# {weightUnit}\nCurrent");
            decimal deltaWeight = startWeight - endWeight;
            if (deltaWeight > 0)
            {
                WeightDifference.Text = Math.Round(deltaWeight, 2).ToString($"-0.# {weightUnit}\nChange");
            }
            else if (deltaWeight < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaWeight, 2)).ToString($"+0.# {weightUnit}\nChange");
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaWeight, 2).ToString($"0.# {weightUnit}\nChange");
            }



            foreach (var weight in weights)
            {
                if (weightUnit == "kg")
                { weight.Weight = (weight.Weight / (decimal)2.2); }
                weight.Weight = Math.Round(weight.Weight, 1);



            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;

            WeightEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            WeightEntries.RowHeight = 88;
            WeightEntries.ItemTemplate = new DataTemplate(() =>
            {

                Grid grid = new Grid
                {
                    Margin = new Thickness(8),
                    RowSpacing = 2,
                    RowDefinitions =
                        {
                        new RowDefinition { Height = GridLength.Auto},
                        new RowDefinition { Height = GridLength.Star }
                        },
                    ColumnDefinitions =
                        {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Star }
                        }
                };

                Label weightLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                weightLabel.SetBinding(Label.TextProperty, new Binding("Weight"));

                Label dateLabel = new Label
                {
                    FontFamily = "Lato-Regular",
                    FontSize = 13,
                    TextColor = Colors.LightSlateGray,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                if (_dateFormat != "MMM d")
                {
                    dateLabel.SetBinding(Label.TextProperty, new Binding("Date", stringFormat: "{0:dddd, d MMMM, yyyy}"));
                }
                else
                {
                    dateLabel.SetBinding(Label.TextProperty, new Binding("Date", stringFormat: "{0:dddd, MMMM d, yyyy}"));
                }

                var size = DeviceDisplay.MainDisplayInfo.Width * .06;

                Image progressPic = new Image
                {
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = size

                };
                progressPic.SetBinding(Image.SourceProperty, new Binding("ProgressPicture", converter: new ByteArrayToImageSourceConverter()));

                Grid.SetRow(weightLabel, 0);
                Grid.SetColumn(weightLabel, 0);
                grid.Children.Add(weightLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                Grid.SetRowSpan(progressPic, 2);
                Grid.SetColumn(progressPic, 1);
                grid.Children.Add(progressPic);


                return new ViewCell { View = grid };

            });





            

                foreach (var weight in weights)
                {
                    ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Weight));
                }

                if (weights.All(w => w.Date != DateTime.Today))
                {
                    var lastWeightValue = weights.Last();
                    ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.Weight));
                }


            var geoSize = 1;
            var stroke = 12;
            var minStep = TimeSpan.FromDays(1).Ticks;
            var unitWidth = TimeSpan.FromDays(1).Ticks;

            string color = "#4b9fe3";

            TimeSpan duration = endDate - beginningDate;
            int dayRange = duration.Days + 1;

            if (dayRange < 8)
            {
                minStep = TimeSpan.FromDays(1).Ticks;
                unitWidth = TimeSpan.FromDays(1).Ticks;
            }
            else if (dayRange > 7 && dayRange < 12)
            {
                minStep = TimeSpan.FromDays(1.5).Ticks;
                unitWidth = TimeSpan.FromDays(1.5).Ticks;
            }
            else if (dayRange > 11 && dayRange < 15)
            {
                minStep = TimeSpan.FromDays(2).Ticks;
                unitWidth = TimeSpan.FromDays(2).Ticks;
            }
            else if (dayRange > 14 && dayRange < 21)
            {
                minStep = TimeSpan.FromDays(3).Ticks;
                unitWidth = TimeSpan.FromDays(3).Ticks;
            }
            else if (dayRange > 20 && dayRange < 32)
            {
                minStep = TimeSpan.FromDays(4).Ticks;
                unitWidth = TimeSpan.FromDays(4).Ticks;
            }
            else if (dayRange > 31 && dayRange < 57)
            {
                minStep = TimeSpan.FromDays(6).Ticks;
                unitWidth = TimeSpan.FromDays(6).Ticks;
            }
            else if (dayRange > 56 && dayRange < 68)
            {
                minStep = TimeSpan.FromDays(7).Ticks;
                unitWidth = TimeSpan.FromDays(7).Ticks;
            }
            else if (dayRange > 67 && dayRange < 80)
            {
                minStep = TimeSpan.FromDays(10).Ticks;
                unitWidth = TimeSpan.FromDays(10).Ticks;
            }
            else if (dayRange > 79 && dayRange < 90)
            {
                minStep = TimeSpan.FromDays(12).Ticks;
                unitWidth = TimeSpan.FromDays(12).Ticks;
            }
            else if (dayRange > 89 && dayRange < 120)
            {
                minStep = TimeSpan.FromDays(14).Ticks;
                unitWidth = TimeSpan.FromDays(14).Ticks;
            }
            else if (dayRange > 119 && dayRange < 150)
            {
                minStep = TimeSpan.FromDays(20).Ticks;
                unitWidth = TimeSpan.FromDays(20).Ticks;
            }
            else if (dayRange > 149 && dayRange < 230)
            {
                minStep = TimeSpan.FromDays(30).Ticks;
                unitWidth = TimeSpan.FromDays(30).Ticks;
            }
            else if (dayRange > 229 && dayRange < 300)
            {
                minStep = TimeSpan.FromDays(35).Ticks;
                unitWidth = TimeSpan.FromDays(35).Ticks;
            }
            else if (dayRange > 299 && dayRange < 340)
            {
                minStep = TimeSpan.FromDays(40).Ticks;
                unitWidth = TimeSpan.FromDays(40).Ticks;
            }
            else if (dayRange > 339 && dayRange < 501)
            {
                minStep = TimeSpan.FromDays(60).Ticks;
                unitWidth = TimeSpan.FromDays(60).Ticks;
            }
            else if (dayRange > 500 && dayRange < 750)
            {
                minStep = TimeSpan.FromDays(90).Ticks;
                unitWidth = TimeSpan.FromDays(90).Ticks;
            }
            else if (dayRange > 749 && dayRange < 1000)
            {
                minStep = TimeSpan.FromDays(120).Ticks;
                unitWidth = TimeSpan.FromDays(120).Ticks;
            }
            else if (dayRange > 999 && dayRange < 1201)
            {
                minStep = TimeSpan.FromDays(150).Ticks;
                unitWidth = TimeSpan.FromDays(150).Ticks;
            }
            else if (dayRange > 1200 && dayRange < 2001)
            {
                minStep = TimeSpan.FromDays(200).Ticks;
                unitWidth = TimeSpan.FromDays(200).Ticks;
            }
            else if (dayRange > 2000 && dayRange < 2301)
            {
                minStep = TimeSpan.FromDays(250).Ticks;
                unitWidth = TimeSpan.FromDays(250).Ticks;
            }
            else if (dayRange > 2300 && dayRange < 2800)
            {
                minStep = TimeSpan.FromDays(300).Ticks;
                unitWidth = TimeSpan.FromDays(300).Ticks;
            }
            else if (dayRange > 2799)
            {
                minStep = TimeSpan.FromDays(365).Ticks;
                unitWidth = TimeSpan.FromDays(365).Ticks;
            }

            switch (index)
            {
                case 0:
                    color = "#4b9fe3";
                    geoSize = 18;
                    stroke = 12;
                    break;
                case 1:
                    color = "#F71735";
                    geoSize = 0;
                    stroke = 10;
                    break;

                case 2:
                    color = "#f23f99";
                    geoSize = 0;
                    stroke = 8;
                    break;

                case 3:
                    color = "#F5A623";
                    geoSize = 0;
                    stroke = 6;
                    break;
                case 4:
                    color = "#74A57F";
                    geoSize = 0;
                    stroke = 5;
                    break;
                case 5:
                    color = "#9e6bb5";
                    geoSize = 0;
                    stroke = 5;
                    break;
                case 6:
                    color = "#8fc4cf";
                    geoSize = 1;
                    stroke = 5;
                    break;
            }








            var series = new LineSeries<DateTimePoint>()
            {
                Values = ChartValues,
                LineSmoothness = 0.1f, // Optional: adjust the line smoothness to your liking

                TooltipLabelFormatter = (chartPoint) =>
                    $"{new DateTime((long)chartPoint.SecondaryValue):MMMM d}: {chartPoint.PrimaryValue}",
                GeometryStroke = new SolidColorPaint(SKColor.Parse(color)) { StrokeThickness = stroke / 2 },
                Stroke = new SolidColorPaint(SKColor.Parse(color)) { StrokeThickness = stroke },
                Fill = null,
                GeometryFill = new SolidColorPaint(SKColors.White),
                GeometrySize = geoSize,
                Name = "Weight"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.Weight).FirstOrDefault()?.Weight;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Weight).FirstOrDefault()?.Weight;

            WeightChart.XAxes = new[]
                {
                new Axis
                {
                     Labeler = value => new DateTime((long) value).ToString($"{_dateFormat}"),
                     UnitWidth = unitWidth,
                     MinStep = minStep,
                     TextSize = 21,
                     LabelsPaint = new SolidColorPaint(colorMode)

    }
            };

            var weightVariance = largestWeight - smallestWeight;

            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 15,
                     UnitWidth = 1,
                     MinStep = 1,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - 5,
                     MaxLimit = (double)largestWeight + 5,
                }
            };

        }

       
        await WeightEntries.FadeTo(1, 500);

        WeightChart.HeightRequest = ChartFrame.Height * .65;
    }




    async void AddEntryClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new NewWeightEntry(_dataService, _localData, DateTime.Today, null));
    }

    void RangePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    {
        
        FillGraph(RangePicker.SelectedIndex);
    }

    async void WeightEntries_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as WeightTable;
        if (selectedItem != null)
        {
            await Navigation.PushAsync(new NewWeightEntry(_dataService, _localData, selectedItem.Date, selectedItem));
            WeightEntries.SelectedItem = null; // Deselect the selected item

        }
    }

    async void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if (weights.Count > 0)
        {
            var beforeWeight = weights.First();
            var afterWeight = weights.Last();
            await Navigation.PushAsync(new WeightComparison(beforeWeight, afterWeight, _localData));
        }
        else
        {
            await DisplayAlert("Oops", "You need to have weights and pictures stored to be able to compare and share your progress", "Ok");
        }
    }

    async void ImageButton_Clicked(System.Object sender, System.EventArgs e)
    {
        WeightTable beforeWeight = new WeightTable();
        WeightTable afterWeight = new WeightTable();

        if (weights.Count > 0)
        {
            foreach (var weight in weights)
            {
                if (weight.ProgressPicture != null)
                {
                    beforeWeight = weight; // Set the current weight to beforeWeight
                    break; // Exit the loop
                }
            }

            weights.Reverse();

            foreach (var weight in weights)
            {
                if (weight.ProgressPicture != null)
                {
                    afterWeight = weight; // Set the current weight to beforeWeight
                    break; // Exit the loop
                }
            }

            if (beforeWeight != null && afterWeight != null)
            {
                await Navigation.PushAsync(new WeightComparison(beforeWeight, afterWeight, _localData));
            }
        }

        else
        {
            await DisplayAlert("Oops", "You need to have weights and pictures stored to be able to compare and share your progress", "Ok");
        }
    }
}

