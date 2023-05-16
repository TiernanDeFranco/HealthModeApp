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

        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[4];
        var weightUnit = unitList[0];

       

        switch (dateFormat)
        {
            case "MM/dd/yy":
                _dateFormat = "M/d";
                break;

            case "dd/MM/yy":
                _dateFormat = "d/M";
                break;

            case "yy/MM/dd":
                _dateFormat = "M/d";
                break;
        }

        var weights = await _localData.GetWeightsIndex(userID, index);
        if (weights.Count > 0)
        {
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

            StartWeight.Text = startWeight.ToString("0.#\nStarting Weight");
            EndWeight.Text = endWeight.ToString("0.#\nCurrent Weight");
            decimal deltaWeight = startWeight - endWeight;
            if (deltaWeight > 0)
            {
                WeightDifference.Text = Math.Round(deltaWeight, 2).ToString("-0.#\nChange");
            }
            else if (deltaWeight < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaWeight, 2)).ToString("+0.#\nChange");
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaWeight, 2).ToString("0.#\nChange");
            }

        }

        foreach (var weight in weights)
        {
            if (weightUnit == "kg")
            { weight.Weight = (weight.Weight / (decimal)2.2); }
            weight.Weight = Math.Round(weight.Weight, 1);

            


        }

        WeightEntries.ItemsSource = weights.OrderByDescending(w => w.Date);
        WeightEntries.RowHeight = 85;
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
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };
            weightLabel.SetBinding(Label.TextProperty, new Binding("Weight"));

            Label dateLabel = new Label
            {
                FontFamily = "Lato-Regular",
                FontSize = 14,
                TextColor = Colors.LightSlateGray,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };
            if (_dateFormat != "M/d")
            {
                dateLabel.SetBinding(Label.TextProperty, new Binding("Date", stringFormat: "{0:dddd, d MMMM, yyyy}"));
            }
            else
            {
                dateLabel.SetBinding(Label.TextProperty, new Binding("Date", stringFormat: "{0:dddd, MMMM d, yyyy}"));
            }

            Image progressPic = new Image
            {
                Aspect = Aspect.AspectFit
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





        ChartValues = new ObservableCollection<DateTimePoint>();
        if (weights.Count > 0)
        {
            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Weight));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeight = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeight.Weight));
            }
        }
        else
        {
            var userInfoWeight = await _localData.GetUserAsync(userID);
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)userInfoWeight.Weight));

        }



        var geoSize = 1;
        var stroke = 12;
        var minStep = TimeSpan.FromDays(1).Ticks;
        var unitWidth = TimeSpan.FromDays(1).Ticks;

        string color = "#4b9fe3";

        if (weights.Count > 0 && weights.Count < 8)
        {
            minStep = TimeSpan.FromDays(1).Ticks;
            unitWidth = TimeSpan.FromDays(1).Ticks;
        }
        else if (weights.Count > 7 && weights.Count < 32)
        {
            minStep = TimeSpan.FromDays(3).Ticks;
            unitWidth = TimeSpan.FromDays(3).Ticks;
        }
        else if (weights.Count > 31 && weights.Count < 63)
        {
            minStep = TimeSpan.FromDays(14).Ticks;
            unitWidth = TimeSpan.FromDays(14).Ticks;
        }
        else if (weights.Count > 62 && weights.Count < 94)
        {
            minStep = TimeSpan.FromDays(21).Ticks;
            unitWidth = TimeSpan.FromDays(21).Ticks;
        }
        else if (weights.Count > 93 && weights.Count < 190)
        {
            minStep = TimeSpan.FromDays(30).Ticks;
            unitWidth = TimeSpan.FromDays(30).Ticks;
        }
        else if (weights.Count > 189 && weights.Count < 370)
        {
            minStep = TimeSpan.FromDays(30).Ticks;
            unitWidth = TimeSpan.FromDays(30).Ticks;
        }
        else if (weights.Count > 369 && weights.Count < 700)
        {
            minStep = TimeSpan.FromDays(60).Ticks;
            unitWidth = TimeSpan.FromDays(60).Ticks;
        }
        else if (weights.Count > 699)
        {
            minStep = TimeSpan.FromDays(90).Ticks;
            unitWidth = TimeSpan.FromDays(90).Ticks;
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
            GeometryStroke = new SolidColorPaint(SKColor.Parse(color)) { StrokeThickness = stroke/2 },
            Stroke = new SolidColorPaint(SKColor.Parse(color)) { StrokeThickness = stroke },
            Fill = null,
            GeometryFill = new SolidColorPaint(SKColors.White),
            GeometrySize = geoSize,
        };

        WeightChart.Series = new ISeries[] { series };

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
        decimal smallestWeight = (decimal)weights.OrderBy(w => w.Weight).FirstOrDefault()?.Weight;
        decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Weight).FirstOrDefault()?.Weight;

        WeightChart.XAxes = new[]
            {
                new Axis
                {
                     Labeler = value => new DateTime((long) value).ToString($"{_dateFormat}"),
                     UnitWidth = unitWidth,
                     MinStep = minStep,
                     TextSize = 25,
                     LabelsPaint = new SolidColorPaint(colorMode)

    }
            };

        WeightChart.YAxes = new[]
           {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 1,
                     MinStep = 1,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - 5,
                     MaxLimit = (double)largestWeight + 5,
                }
            };



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
}

