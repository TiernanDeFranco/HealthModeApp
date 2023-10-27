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
using Mopups.Services;
using HealthModeApp.Pages.Popups;

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
    List<string> measureOptions = new List<string>();
    List<string> rangeOptions = new List<string>();

    ToolbarItem addEntry;

    public WeightProgress(IRestDataService dataService, ISQLiteDataService localData)
    {
        InitializeComponent();
        _dataService = dataService;
        _localData = localData;


        rangeOptions.Add("1 Week");
        rangeOptions.Add("1 Month");
        rangeOptions.Add("2 Months");
        rangeOptions.Add("3 Months");
        rangeOptions.Add("6 Months");
        rangeOptions.Add("1 Year");
        rangeOptions.Add("All");

        RangePicker.Title = rangeOptions[0];
        RangePicker.ItemSource = rangeOptions;


        measureOptions.Add("Weight"); //0
        measureOptions.Add("Body Fat %"); //1
        measureOptions.Add("Neck"); //2
        measureOptions.Add("Chest"); //3
        measureOptions.Add("Arms");//4
        measureOptions.Add("Waist"); //5
        measureOptions.Add("Hips"); //6
        measureOptions.Add("Thighs"); //7
        measureOptions.Add("Calves"); //8

        MeasurementPicker.Title = measureOptions[0];
        MeasurementPicker.ItemSource = measureOptions;

        addEntry = new ToolbarItem
        {
            Text = "New Weight +"
        };
        addEntry.Clicked += AddEntryClicked;

        ToolbarItems.Add(addEntry);

    }



    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Hide the TabBar on this page
        Shell.SetTabBarIsVisible(this, false);

        switch (measurementInt)
        {
            case 0:
                FillWeightGraph(rangeInt);
                break;
            case 1:
                FillBodyFatGraph(rangeInt);
                break;
            case 2:
                FillNeckGraph(rangeInt);
                break;

            case 3:
                FillChestGraph(rangeInt);
                break;

            case 4:
                FillArmsGraph(rangeInt);
                break;

            case 5:
                FillWaistGraph(rangeInt);
                break;

            case 6:
                FillHipsGraph(rangeInt);
                break;

            case 7:
                FillThighsGraph(rangeInt);
                break;

            case 8:
                FillCalvesGraph(rangeInt);
                break;
        }

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

    async void FillWeightGraph(int index)
    {
        WeightFrame.IsVisible = true;
        BodyFatFrame.IsVisible = false;
        NeckFrame.IsVisible = false;
        ChestFrame.IsVisible = false;
        ArmsFrame.IsVisible = false;
        WaistFrame.IsVisible = false;
        HipsFrame.IsVisible = false;
        ThighsFrame.IsVisible = false;
        CalvesFrame.IsVisible = false;

        WeightDifIcon.IsVisible = true;
        InfoButton.IsVisible = true;

        addEntry.Text = "New Weight +";

        WeightEntries.Opacity = 0;

        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
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

        if (weights.Count == 0 || weights == null)
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
            WeightChart.FadeTo(1, 250);
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


            WeightEntries.ItemTemplate = null;
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
                if (weightUnit != "kg")
                {
                    weightLabel.SetBinding(Label.TextProperty, new Binding("Weight", stringFormat: "{0} lbs"));
                }
                else
                {
                    weightLabel.SetBinding(Label.TextProperty, new Binding("Weight", stringFormat: "{0} kg"));
                }

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

    async void FillBodyFatGraph(int index)
    {
        WeightFrame.IsVisible = false;
        BodyFatFrame.IsVisible = true;
        NeckFrame.IsVisible = false;
        ChestFrame.IsVisible = false;
        ArmsFrame.IsVisible = false;
        WaistFrame.IsVisible = false;
        HipsFrame.IsVisible = false;
        ThighsFrame.IsVisible = false;
        CalvesFrame.IsVisible = false;

        WeightDifIcon.IsVisible = false;
        InfoButton.IsVisible = true; ;

        addEntry.Text = "New BF% +";

        WeightChart.Opacity = 0;
        BodyFatEntries.Opacity = 0;

        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];

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
        BodyFatEntries.ItemsSource = null;
        weights = await _localData.GetBodyFatIndex(userID, index);


        if (weights.Count == 0 || weights == null)
        {
            BodyFatEntries.Opacity = 0;
            WeightChart.FadeTo(0, 500);
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Entries In This Range";
        }

        if (weights.Count > 0)
        {
            BodyFatEntries.FadeTo(1, 250);
            WeightChart.FadeTo(1, 250);
            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            MeasurementPicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startBodyFat;
            decimal endBodyFat;

            startBodyFat = firstWeight.BodyFat;
            endBodyFat = lastWeight.BodyFat;

            StartWeight.Text = startBodyFat.ToString("0.#") + "%\nStarting";

            EndWeight.Text = endBodyFat.ToString("0.#") + "%\nCurrent";
            decimal deltaBF = startBodyFat - endBodyFat;
            if (deltaBF > 0)
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("-0.#") + "%\nChange";
            }
            else if (deltaBF < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaBF, 2)).ToString("+0.#") + "%\nChange";
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("0.#") + "%\nChange";
            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;


            BodyFatEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            BodyFatEntries.RowHeight = 88;
            BodyFatEntries.ItemTemplate = new DataTemplate(() =>
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
                        new ColumnDefinition { Width = GridLength.Auto }
                        }
                };

                Label bodyFatLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                bodyFatLabel.SetBinding(Label.TextProperty, new Binding("BodyFat", stringFormat: "{0}%"));

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


                Grid.SetRow(bodyFatLabel, 0);
                Grid.SetColumn(bodyFatLabel, 0);
                grid.Children.Add(bodyFatLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                return new ViewCell { View = grid };

            });

            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.BodyFat));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeightValue = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.BodyFat));
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
                Name = "Body Fat %"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.BodyFat).FirstOrDefault()?.BodyFat;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.BodyFat).FirstOrDefault()?.BodyFat;


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



            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 0.5,
                     MinStep = 0.5,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - .5,
                     MaxLimit = (double)largestWeight + .5,
                }
           };



        }
    await BodyFatEntries.FadeTo(1, 500);

    WeightChart.HeightRequest = ChartFrame.Height* .65;
        
    }

    string _measureUnit;

    async void FillNeckGraph(int index)
    {
        WeightFrame.IsVisible = false;
        BodyFatFrame.IsVisible = false;
        NeckFrame.IsVisible = true;
        ChestFrame.IsVisible = false;
        ArmsFrame.IsVisible = false;
        WaistFrame.IsVisible = false;
        HipsFrame.IsVisible = false;
        ThighsFrame.IsVisible = false;
        CalvesFrame.IsVisible = false;

        WeightDifIcon.IsVisible = false;
        InfoButton.IsVisible = true;

        addEntry.Text = "New Measurement +";

        WeightChart.Opacity = 0;
        NeckEntries.Opacity = 0;


        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        var measureUnit = unitList[1];

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

        switch (measureUnit)
        {
            case "inch":
                _measureUnit = "In";
                break;
            case "cm":
                _measureUnit = "Cm";
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
        NeckEntries.ItemsSource = null;
        weights = await _localData.GetNeckIndex(userID, index);
        



        if (weights.Count == 0 || weights == null)
        {
            NeckEntries.Opacity = 0;
            WeightChart.FadeTo(0, 500);
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Entries In This Range";
        }

        if (weights.Count > 0)
        {
            NeckEntries.FadeTo(1, 250);
            WeightChart.FadeTo(1, 250);
            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            MeasurementPicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            foreach (var entry in weights)
            {
                if (measureUnit == "cm")
                { entry.Neck = (entry.Neck * (decimal)2.54); }
                entry.Neck = Math.Round(entry.Neck, 1);
            }

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startMeasure;
            decimal endMeasure;

            startMeasure = firstWeight.Neck;
            endMeasure = lastWeight.Neck;

            StartWeight.Text = startMeasure.ToString("0.#") + $" {_measureUnit}\nStarting";

            EndWeight.Text = endMeasure.ToString("0.#") + $" {_measureUnit}\nCurrent";
            decimal deltaBF = startMeasure - endMeasure;
            if (deltaBF > 0)
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("-0.#") + $" {_measureUnit}\nChange";
            }
            else if (deltaBF < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaBF, 2)).ToString("+0.#") + $" {_measureUnit}\nChange";
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("0.#") + $" {_measureUnit}\nChange";
            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;


            NeckEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            NeckEntries.RowHeight = 88;
            NeckEntries.ItemTemplate = new DataTemplate(() =>
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
                        new ColumnDefinition { Width = GridLength.Auto }
                        },
                    BackgroundColor = Colors.Transparent
                    
                };

                Label MeasureLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                if (measureUnit == "cm")
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Neck", stringFormat: "{0} cm"));
                }
                else
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Neck", stringFormat: "{0} inches"));
                }

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


                Grid.SetRow(MeasureLabel, 0);
                Grid.SetColumn(MeasureLabel, 0);
                grid.Children.Add(MeasureLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                return new ViewCell { View = grid };

            });

            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Neck));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeightValue = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.Neck));
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
                Name = "Neck Circumference"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.Neck).FirstOrDefault()?.Neck;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Neck).FirstOrDefault()?.Neck;


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



            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 0.25,
                     MinStep = 0.25,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - .5,
                     MaxLimit = (double)largestWeight + .5,
                }
           };



        }
        await NeckEntries.FadeTo(1, 500);

        WeightChart.HeightRequest = ChartFrame.Height * .65;

    }

    async void FillChestGraph(int index)
    {
        WeightFrame.IsVisible = false;
        BodyFatFrame.IsVisible = false;
        NeckFrame.IsVisible = false;
        ChestFrame.IsVisible = true;
        ArmsFrame.IsVisible = false;
        WaistFrame.IsVisible = false;
        HipsFrame.IsVisible = false;
        ThighsFrame.IsVisible = false;
        CalvesFrame.IsVisible = false;

        WeightDifIcon.IsVisible = false;
        InfoButton.IsVisible = true;

        addEntry.Text = "New Measurement +";

        WeightChart.Opacity = 0;
        ChestEntries.Opacity = 0;


        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        var measureUnit = unitList[1];

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

        switch (measureUnit)
        {
            case "inch":
                _measureUnit = "In";
                break;
            case "cm":
                _measureUnit = "Cm";
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
        ChestEntries.ItemsSource = null;
        weights = await _localData.GetChestIndex(userID, index);




        if (weights.Count == 0 || weights == null)
        {
            ChestEntries.Opacity = 0;
            WeightChart.FadeTo(0, 500);
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Entries In This Range";
        }

        if (weights.Count > 0)
        {
            ChestEntries.FadeTo(1, 250);
            WeightChart.FadeTo(1, 250);
            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            MeasurementPicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            foreach (var entry in weights)
            {
                if (measureUnit == "cm")
                { entry.Chest = (entry.Chest * (decimal)2.54); }
                entry.Chest = Math.Round(entry.Chest, 1);
            }

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startMeasure;
            decimal endMeasure;

            startMeasure = firstWeight.Chest;
            endMeasure = lastWeight.Chest;

            StartWeight.Text = startMeasure.ToString("0.#") + $" {_measureUnit}\nStarting";

            EndWeight.Text = endMeasure.ToString("0.#") + $" {_measureUnit}\nCurrent";
            decimal deltaBF = startMeasure - endMeasure;
            if (deltaBF > 0)
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("-0.#") + $" {_measureUnit}\nChange";
            }
            else if (deltaBF < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaBF, 2)).ToString("+0.#") + $" {_measureUnit}\nChange";
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("0.#") + $" {_measureUnit}\nChange";
            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;


            ChestEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            ChestEntries.RowHeight = 88;
            ChestEntries.ItemTemplate = new DataTemplate(() =>
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
                        new ColumnDefinition { Width = GridLength.Auto }
                        },
                    BackgroundColor = Colors.Transparent
                };

                Label MeasureLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                if (measureUnit == "cm")
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Chest", stringFormat: "{0} cm"));
                }
                else
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Chest", stringFormat: "{0} inches"));
                }

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


                Grid.SetRow(MeasureLabel, 0);
                Grid.SetColumn(MeasureLabel, 0);
                grid.Children.Add(MeasureLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                return new ViewCell { View = grid };

            });

            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Chest));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeightValue = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.Chest));
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
                Name = "Chest Circumference"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.Chest).FirstOrDefault()?.Chest;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Chest).FirstOrDefault()?.Chest;


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



            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 0.25,
                     MinStep = 0.25,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - .5,
                     MaxLimit = (double)largestWeight + .5,
                }
           };



        }
        await ChestEntries.FadeTo(1, 500);

        WeightChart.HeightRequest = ChartFrame.Height * .65;

    }

    async void FillArmsGraph(int index)
    {
        WeightFrame.IsVisible = false;
        BodyFatFrame.IsVisible = false;
        NeckFrame.IsVisible = false;
        ChestFrame.IsVisible = false;
        ArmsFrame.IsVisible = true;
        WaistFrame.IsVisible = false;
        HipsFrame.IsVisible = false;
        ThighsFrame.IsVisible = false;
        CalvesFrame.IsVisible = false;

        WeightDifIcon.IsVisible = false;
        InfoButton.IsVisible = true;

        addEntry.Text = "New Measurement +";

        WeightChart.Opacity = 0;
        ArmsEntries.Opacity = 0;


        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        var measureUnit = unitList[1];

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

        switch (measureUnit)
        {
            case "inch":
                _measureUnit = "In";
                break;
            case "cm":
                _measureUnit = "Cm";
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
        ArmsEntries.ItemsSource = null;
        weights = await _localData.GetArmsIndex(userID, index);




        if (weights.Count == 0 || weights == null)
        {
            ArmsEntries.Opacity = 0;
            WeightChart.FadeTo(0, 500);
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Entries In This Range";
        }

        if (weights.Count > 0)
        {
            ArmsEntries.FadeTo(1, 250);
            WeightChart.FadeTo(1, 250);
            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            MeasurementPicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            foreach (var entry in weights)
            {
                if (measureUnit == "cm")
                { entry.Arms = (entry.Arms * (decimal)2.54); }
                entry.Arms = Math.Round(entry.Arms, 1);
            }

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startMeasure;
            decimal endMeasure;

            startMeasure = firstWeight.Arms;
            endMeasure = lastWeight.Arms;

            StartWeight.Text = startMeasure.ToString("0.#") + $" {_measureUnit}\nStarting";

            EndWeight.Text = endMeasure.ToString("0.#") + $" {_measureUnit}\nCurrent";
            decimal deltaBF = startMeasure - endMeasure;
            if (deltaBF > 0)
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("-0.#") + $" {_measureUnit}\nChange";
            }
            else if (deltaBF < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaBF, 2)).ToString("+0.#") + $" {_measureUnit}\nChange";
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("0.#") + $" {_measureUnit}\nChange";
            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;


            ArmsEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            ArmsEntries.RowHeight = 88;
            ArmsEntries.ItemTemplate = new DataTemplate(() =>
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
                        new ColumnDefinition { Width = GridLength.Auto }
                        },
                    BackgroundColor = Colors.Transparent
                };

                Label MeasureLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                if (measureUnit == "cm")
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Arms", stringFormat: "{0} cm"));
                }
                else
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Arms", stringFormat: "{0} inches"));
                }

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


                Grid.SetRow(MeasureLabel, 0);
                Grid.SetColumn(MeasureLabel, 0);
                grid.Children.Add(MeasureLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                return new ViewCell { View = grid };

            });

            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Arms));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeightValue = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.Arms));
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
                Name = "Arm Measurement"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.Arms).FirstOrDefault()?.Arms;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Arms).FirstOrDefault()?.Arms;


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



            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 0.25,
                     MinStep = 0.25,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - .5,
                     MaxLimit = (double)largestWeight + .5,
                }
           };



        }
        await ArmsEntries.FadeTo(1, 500);

        WeightChart.HeightRequest = ChartFrame.Height * .65;

    }

    async void FillWaistGraph(int index)
    {
        WeightFrame.IsVisible = false;
        BodyFatFrame.IsVisible = false;
        NeckFrame.IsVisible = false;
        ChestFrame.IsVisible = false;
        ArmsFrame.IsVisible = false;
        WaistFrame.IsVisible = true;
        HipsFrame.IsVisible = false;
        ThighsFrame.IsVisible = false;
        CalvesFrame.IsVisible = false;

        WeightDifIcon.IsVisible = false;
        InfoButton.IsVisible = true;

        addEntry.Text = "New Measurement +";

        WeightChart.Opacity = 0;
        WaistEntries.Opacity = 0;


        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        var measureUnit = unitList[1];

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

        switch (measureUnit)
        {
            case "inch":
                _measureUnit = "In";
                break;
            case "cm":
                _measureUnit = "Cm";
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
        WaistEntries.ItemsSource = null;
        weights = await _localData.GetWaistIndex(userID, index);




        if (weights.Count == 0 || weights == null)
        {
            WaistEntries.Opacity = 0;
            WeightChart.FadeTo(0, 500);
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Entries In This Range";
        }

        if (weights.Count > 0)
        {
            WaistEntries.FadeTo(1, 250);
            WeightChart.FadeTo(1, 250);
            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            MeasurementPicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            foreach (var entry in weights)
            {
                if (measureUnit == "cm")
                { entry.Waist = (entry.Waist * (decimal)2.54); }
                entry.Waist = Math.Round(entry.Waist, 1);
            }

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startMeasure;
            decimal endMeasure;

            startMeasure = firstWeight.Waist;
            endMeasure = lastWeight.Waist;

            StartWeight.Text = startMeasure.ToString("0.#") + $" {_measureUnit}\nStarting";

            EndWeight.Text = endMeasure.ToString("0.#") + $" {_measureUnit}\nCurrent";
            decimal deltaBF = startMeasure - endMeasure;
            if (deltaBF > 0)
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("-0.#") + $" {_measureUnit}\nChange";
            }
            else if (deltaBF < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaBF, 2)).ToString("+0.#") + $" {_measureUnit}\nChange";
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("0.#") + $" {_measureUnit}\nChange";
            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;


            WaistEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            WaistEntries.RowHeight = 88;
            WaistEntries.ItemTemplate = new DataTemplate(() =>
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
                        new ColumnDefinition { Width = GridLength.Auto }
                        },
                    BackgroundColor = Colors.Transparent
                };

                Label MeasureLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                if (measureUnit == "cm")
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Waist", stringFormat: "{0} cm"));
                }
                else
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Waist", stringFormat: "{0} inches"));
                }

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


                Grid.SetRow(MeasureLabel, 0);
                Grid.SetColumn(MeasureLabel, 0);
                grid.Children.Add(MeasureLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                return new ViewCell { View = grid };

            });

            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Waist));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeightValue = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.Waist));
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
                Name = "Arm Measurement"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.Waist).FirstOrDefault()?.Waist;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Waist).FirstOrDefault()?.Waist;


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



            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 0.5,
                     MinStep = 0.5,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - .5,
                     MaxLimit = (double)largestWeight + .5,
                }
           };



        }
        await WaistEntries.FadeTo(1, 500);

        WeightChart.HeightRequest = ChartFrame.Height * .65;

    }

    async void FillHipsGraph(int index)
    {
        WeightFrame.IsVisible = false;
        BodyFatFrame.IsVisible = false;
        NeckFrame.IsVisible = false;
        ChestFrame.IsVisible = false;
        ArmsFrame.IsVisible = false;
        WaistFrame.IsVisible = false;
        HipsFrame.IsVisible = true;
        ThighsFrame.IsVisible = false;
        CalvesFrame.IsVisible = false;

        WeightDifIcon.IsVisible = false;
        InfoButton.IsVisible = true;

        addEntry.Text = "New Measurement +";

        WeightChart.Opacity = 0;
        HipsEntries.Opacity = 0;


        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        var measureUnit = unitList[1];

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

        switch (measureUnit)
        {
            case "inch":
                _measureUnit = "In";
                break;
            case "cm":
                _measureUnit = "Cm";
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
        HipsEntries.ItemsSource = null;
        weights = await _localData.GetHipsIndex(userID, index);




        if (weights.Count == 0 || weights == null)
        {
            HipsEntries.Opacity = 0;
            WeightChart.FadeTo(0, 500);
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Entries In This Range";
        }

        if (weights.Count > 0)
        {
            HipsEntries.FadeTo(1, 250);
            WeightChart.FadeTo(1, 250);
            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            MeasurementPicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            foreach (var entry in weights)
            {
                if (measureUnit == "cm")
                { entry.Hips = (entry.Hips * (decimal)2.54); }
                entry.Hips = Math.Round(entry.Hips, 1);
            }

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startMeasure;
            decimal endMeasure;

            startMeasure = firstWeight.Hips;
            endMeasure = lastWeight.Hips;

            StartWeight.Text = startMeasure.ToString("0.#") + $" {_measureUnit}\nStarting";

            EndWeight.Text = endMeasure.ToString("0.#") + $" {_measureUnit}\nCurrent";
            decimal deltaBF = startMeasure - endMeasure;
            if (deltaBF > 0)
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("-0.#") + $" {_measureUnit}\nChange";
            }
            else if (deltaBF < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaBF, 2)).ToString("+0.#") + $" {_measureUnit}\nChange";
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("0.#") + $" {_measureUnit}\nChange";
            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;


            HipsEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            HipsEntries.RowHeight = 88;
            HipsEntries.ItemTemplate = new DataTemplate(() =>
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
                        new ColumnDefinition { Width = GridLength.Auto }
                        },
                    BackgroundColor = Colors.Transparent
                };

                Label MeasureLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                if (measureUnit == "cm")
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Hips", stringFormat: "{0} cm"));
                }
                else
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Hips", stringFormat: "{0} inches"));
                }

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


                Grid.SetRow(MeasureLabel, 0);
                Grid.SetColumn(MeasureLabel, 0);
                grid.Children.Add(MeasureLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                return new ViewCell { View = grid };

            });

            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Hips));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeightValue = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.Hips));
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
                Name = "Hips Measurement"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.Hips).FirstOrDefault()?.Hips;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Hips).FirstOrDefault()?.Hips;


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



            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 0.5,
                     MinStep = 0.5,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - .5,
                     MaxLimit = (double)largestWeight + .5,
                }
           };



        }
        await HipsEntries.FadeTo(1, 500);

        WeightChart.HeightRequest = ChartFrame.Height * .65;

    }

    async void FillThighsGraph(int index)
    {
        WeightFrame.IsVisible = false;
        BodyFatFrame.IsVisible = false;
        NeckFrame.IsVisible = false;
        ChestFrame.IsVisible = false;
        ArmsFrame.IsVisible = false;
        WaistFrame.IsVisible = false;
        HipsFrame.IsVisible = false;
        ThighsFrame.IsVisible = true;
        CalvesFrame.IsVisible = false;

        WeightDifIcon.IsVisible = false;
        InfoButton.IsVisible = true;

        addEntry.Text = "New Measurement +";

        WeightChart.Opacity = 0;
        ThighsEntries.Opacity = 0;


        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        var measureUnit = unitList[1];

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

        switch (measureUnit)
        {
            case "inch":
                _measureUnit = "In";
                break;
            case "cm":
                _measureUnit = "Cm";
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
        ThighsEntries.ItemsSource = null;
        weights = await _localData.GetThighsIndex(userID, index);




        if (weights.Count == 0 || weights == null)
        {
            ThighsEntries.Opacity = 0;
            WeightChart.FadeTo(0, 500);
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Entries In This Range";
        }

        if (weights.Count > 0)
        {
            ThighsEntries.FadeTo(1, 250);
            WeightChart.FadeTo(1, 250);
            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            MeasurementPicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            foreach (var entry in weights)
            {
                if (measureUnit == "cm")
                { entry.Thighs = (entry.Thighs * (decimal)2.54); }
                entry.Thighs = Math.Round(entry.Thighs, 1);
            }

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startMeasure;
            decimal endMeasure;

            startMeasure = firstWeight.Thighs;
            endMeasure = lastWeight.Thighs;

            StartWeight.Text = startMeasure.ToString("0.#") + $" {_measureUnit}\nStarting";

            EndWeight.Text = endMeasure.ToString("0.#") + $" {_measureUnit}\nCurrent";
            decimal deltaBF = startMeasure - endMeasure;
            if (deltaBF > 0)
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("-0.#") + $" {_measureUnit}\nChange";
            }
            else if (deltaBF < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaBF, 2)).ToString("+0.#") + $" {_measureUnit}\nChange";
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("0.#") + $" {_measureUnit}\nChange";
            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;


            ThighsEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            ThighsEntries.RowHeight = 88;
            ThighsEntries.ItemTemplate = new DataTemplate(() =>
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
                        new ColumnDefinition { Width = GridLength.Auto }
                        },
                    BackgroundColor = Colors.Transparent
                };

                Label MeasureLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                if (measureUnit == "cm")
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Thighs", stringFormat: "{0} cm"));
                }
                else
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Thighs", stringFormat: "{0} inches"));
                }

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


                Grid.SetRow(MeasureLabel, 0);
                Grid.SetColumn(MeasureLabel, 0);
                grid.Children.Add(MeasureLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                return new ViewCell { View = grid };

            });

            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Thighs));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeightValue = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.Thighs));
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
                Name = "Thigh Measurement"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.Thighs).FirstOrDefault()?.Thighs;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Thighs).FirstOrDefault()?.Thighs;


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



            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 0.5,
                     MinStep = 0.5,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - .5,
                     MaxLimit = (double)largestWeight + .5,
                }
           };



        }
        await ThighsEntries.FadeTo(1, 500);

        WeightChart.HeightRequest = ChartFrame.Height * .65;

    }

    async void FillCalvesGraph(int index)
    {
        WeightFrame.IsVisible = false;
        BodyFatFrame.IsVisible = false;
        NeckFrame.IsVisible = false;
        ChestFrame.IsVisible = false;
        ArmsFrame.IsVisible = false;
        WaistFrame.IsVisible = false;
        HipsFrame.IsVisible = false;
        ThighsFrame.IsVisible = false;
        CalvesFrame.IsVisible = true;

        WeightDifIcon.IsVisible = false;
        InfoButton.IsVisible = true;

        addEntry.Text = "New Measurement +";

        WeightChart.Opacity = 0;
        CalvesEntries.Opacity = 0;


        var userID = await _localData.GetUserID();

        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        var measureUnit = unitList[1];

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

        switch (measureUnit)
        {
            case "inch":
                _measureUnit = "In";
                break;
            case "cm":
                _measureUnit = "Cm";
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
        CalvesEntries.ItemsSource = null;
        weights = await _localData.GetCalvesIndex(userID, index);




        if (weights.Count == 0 || weights == null)
        {
            CalvesEntries.Opacity = 0;
            WeightChart.FadeTo(0, 500);
            ChartValues = new ObservableCollection<DateTimePoint>();
            StartWeight.IsVisible = false;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = false;

            EndWeight.Text = "No Entries In This Range";
        }

        if (weights.Count > 0)
        {
            CalvesEntries.FadeTo(1, 250);
            WeightChart.FadeTo(1, 250);
            ChartValues = new ObservableCollection<DateTimePoint>();

            RangePicker.IsVisible = true;
            MeasurementPicker.IsVisible = true;
            StartWeight.IsVisible = true;
            EndWeight.IsVisible = true;
            WeightDifference.IsVisible = true;

            foreach (var entry in weights)
            {
                if (measureUnit == "cm")
                { entry.Calves = (entry.Calves * (decimal)2.54); }
                entry.Calves = Math.Round(entry.Calves, 1);
            }

            var firstWeight = weights.First();
            var lastWeight = weights.Last();

            decimal startMeasure;
            decimal endMeasure;

            startMeasure = firstWeight.Calves;
            endMeasure = lastWeight.Calves;

            StartWeight.Text = startMeasure.ToString("0.#") + $" {_measureUnit}\nStarting";

            EndWeight.Text = endMeasure.ToString("0.#") + $" {_measureUnit}\nCurrent";
            decimal deltaBF = startMeasure - endMeasure;
            if (deltaBF > 0)
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("-0.#") + $" {_measureUnit}\nChange";
            }
            else if (deltaBF < 0)
            {
                WeightDifference.Text = Math.Abs(Math.Round(deltaBF, 2)).ToString("+0.#") + $" {_measureUnit}\nChange";
            }
            else
            {
                WeightDifference.Text = Math.Round(deltaBF, 2).ToString("0.#") + $" {_measureUnit}\nChange";
            }

            var firstday = weights.OrderBy(w => w.Date).FirstOrDefault();
            var lastday = weights.OrderByDescending(w => w.Date).FirstOrDefault();
            beginningDate = firstday.Date;
            endDate = lastday.Date;


            CalvesEntries.ItemsSource = weights.OrderByDescending(w => w.Date);



            CalvesEntries.RowHeight = 88;
            CalvesEntries.ItemTemplate = new DataTemplate(() =>
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
                        new ColumnDefinition { Width = GridLength.Auto }
                        },
                    BackgroundColor = Colors.Transparent
                };

                Label MeasureLabel = new Label
                {
                    FontFamily = "Lato-Bold",
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
                if (measureUnit == "cm")
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Calves", stringFormat: "{0} cm"));
                }
                else
                {
                    MeasureLabel.SetBinding(Label.TextProperty, new Binding("Calves", stringFormat: "{0} inches"));
                }

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


                Grid.SetRow(MeasureLabel, 0);
                Grid.SetColumn(MeasureLabel, 0);
                grid.Children.Add(MeasureLabel);


                Grid.SetRow(dateLabel, 1);
                Grid.SetColumn(dateLabel, 0);
                grid.Children.Add(dateLabel);

                return new ViewCell { View = grid };

            });

            foreach (var weight in weights)
            {
                ChartValues.Add(new DateTimePoint(weight.Date, (double)weight.Calves));
            }

            if (weights.All(w => w.Date != DateTime.Today))
            {
                var lastWeightValue = weights.Last();
                ChartValues.Add(new DateTimePoint(DateTime.Today, (double)lastWeightValue.Calves));
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
                Name = "Calve Measurement"
            };

            WeightChart.Series = new ISeries[] { series };

            decimal smallestWeight = (decimal)weights.OrderBy(w => w.Calves).FirstOrDefault()?.Calves;
            decimal largestWeight = (decimal)weights.OrderByDescending(w => w.Calves).FirstOrDefault()?.Calves;


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



            WeightChart.YAxes = new[]
               {
                new Axis
                {
                     TextSize = 18,
                     UnitWidth = 0.5,
                     MinStep = 0.5,
                     LabelsPaint = new SolidColorPaint(colorMode),
                     MinLimit = (double)smallestWeight - .5,
                     MaxLimit = (double)largestWeight + .5,
                }
           };



        }
        await CalvesEntries.FadeTo(1, 500);

        WeightChart.HeightRequest = ChartFrame.Height * .65;

    }

    async void AddEntryClicked(System.Object sender, System.EventArgs e)
    {
        switch (measurementInt)
            {
            case 0:
                //Weight
                await Navigation.PushAsync(new NewWeightEntry(_dataService, _localData, DateTime.Today, null));
            break;

            case 1:
                //BodyFat
                await Navigation.PushAsync(new NewBodyFat(_localData, DateTime.Today, null));
            break;

            case 2:
                //Neck
                await Navigation.PushAsync(new NewNeck(_localData, DateTime.Today, null));
                break;

            case 3:
                //Neck
                await Navigation.PushAsync(new NewChestEntry(_localData, DateTime.Today, null));
                break;

            case 4:
                //Neck
                await Navigation.PushAsync(new NewArmsEntry(_localData, DateTime.Today, null));
                break;

            case 5:
                //Neck
                await Navigation.PushAsync(new NewWaistEntry(_localData, DateTime.Today, null));
                break;

            case 6:
                //Neck
                await Navigation.PushAsync(new NewHipsEntry(_localData, DateTime.Today, null));
                break;

            case 7:
                //Neck
                await Navigation.PushAsync(new NewThighsEntry(_localData, DateTime.Today, null));
                break;

            case 8:
                //Neck
                await Navigation.PushAsync(new NewCalvesEntry(_localData, DateTime.Today, null));
                break;
        }
        }



        async void WeightEntries_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as WeightTable;
            if (selectedItem != null)
            {
                switch (measurementInt)
                {
                    case 0:
                        await Navigation.PushAsync(new NewWeightEntry(_dataService, _localData, selectedItem.Date, selectedItem));
                    break;

                    case 1:
                        await Navigation.PushAsync(new NewBodyFat(_localData, selectedItem.Date, selectedItem));
                    break;

                    case 2:
    
                    await Navigation.PushAsync(new NewNeck(_localData, selectedItem.Date, selectedItem));
                    break;

                case 3:
                    //Neck
                    await Navigation.PushAsync(new NewChestEntry(_localData, selectedItem.Date, selectedItem));
                    break;

                case 4:
                    //Neck
                    await Navigation.PushAsync(new NewArmsEntry(_localData, selectedItem.Date, selectedItem));
                    break;

                case 5:
                    //Neck
                    await Navigation.PushAsync(new NewWaistEntry(_localData, selectedItem.Date, selectedItem));
                    break;

                case 6:
                    //Neck
                    await Navigation.PushAsync(new NewHipsEntry(_localData, selectedItem.Date, selectedItem));
                    break;

                case 7:
                    //Neck
                    await Navigation.PushAsync(new NewThighsEntry(_localData, selectedItem.Date, selectedItem));
                    break;

                case 8:
                    //Neck
                    await Navigation.PushAsync(new NewCalvesEntry(_localData, selectedItem.Date, selectedItem));
                    break;

            }
                WeightEntries.SelectedItem = null; // Deselect the selected item
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
                await MopupService.Instance.PushAsync(new InfoPopup("Oops", "You need to have weights and pictures stored to be able to compare and share your progress"));
            }
        }

        int rangeInt = 0;

        void RangePicker_SelectedIndexChanged(System.Object sender, System.Int32 index)
        {
            rangeInt = index;

            switch (measurementInt)
            {
                case 0:
                    FillWeightGraph(index);
                    break;
                case 1:
                    FillBodyFatGraph(index);
                    break;

                case 2:
                    FillNeckGraph(index);
                    break;

            case 3:
                FillChestGraph(index);
                break;

            case 4:
                FillArmsGraph(index);
                break;

            case 5:
                FillWaistGraph(index);
                break;

            case 6:
                FillHipsGraph(index);
                break;

            case 7:
                FillThighsGraph(index);
                break;

            case 8:
                FillCalvesGraph(index);
                break;
        }
        }

        int measurementInt = 0;

        void MeasurementPicker_SelectedIndexChanged(System.Object sender, System.Int32 index)
        {
            measurementInt = index;

            switch (index)
            {
                case 0: //Weight
                    FillWeightGraph(rangeInt);
                    break;
                case 1: //BodyFat
                    FillBodyFatGraph(rangeInt);
                    break;

                case 2: //Neck
                    FillNeckGraph(rangeInt);
                break;

            case 3:
                FillChestGraph(rangeInt);
                break;

            case 4:
                FillArmsGraph(rangeInt);
                break;

            case 5:
                FillWaistGraph(rangeInt);
                break;

            case 6:
                FillHipsGraph(rangeInt);
                break;

            case 7:
                FillThighsGraph(rangeInt);
                break;

            case 8:
                FillCalvesGraph(rangeInt);
                break;
        }
        }

    async void InfoButton_Clicked(System.Object sender, System.EventArgs e)
    {
        switch (measurementInt)
        {
            case 0: //Weight Scales
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 0));
            break;

            case 1: //Body Fat Scales
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 1));
                break;

            case 2: //MeasuringTapes
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 2));
                break;

            case 3: //MeasuringTapes
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 2));
                break;

            case 4: //MeasuringTapes
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 2));
                break;

            case 5: //MeasuringTapes
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 2));
                break;

            case 6: //MeasuringTapes
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 2));
                break;

            case 7: //MeasuringTapes
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 2));
                break;

            case 8: //MeasuringTapes
                await MopupService.Instance.PushAsync(new LinkPopup(_dataService, 2));
                break;
        }
    }


}

