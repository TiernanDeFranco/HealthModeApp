using System.Diagnostics;
using System.Text.Json;
using System.Web;
using HealthModeApp.DataServices;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Maui.Controls;
using Mopups.Services;
using SkiaSharp;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.FoodJournalPage
{
    public partial class MealPage : ContentPage
    {
        private readonly ISQLiteDataService _localData;
        private readonly IRestDataService _dataService;
        public IList<LoggedFoodTable> LoggedFoods { get; set; }
        public int totalCal = 0;
        public LoggedFoodTable loggedFood;
        int userID;
        int _mealType;
        DateTime _date;
        List<LoggedFoodTable> foods;


        public MealPage(ISQLiteDataService localData, IRestDataService dataService, List<int> MealIDs, DateTime date, int mealType, string mealName)
        {
            InitializeComponent();
            _localData = localData;
            MealName.Text = $"{mealName}\nDetails";
            _dataService = dataService;
            _mealType = mealType;
            _date = date;
            PopulateList(MealIDs);
            PopulateFoodInfo();
            SeesAds();
        }

        

        async void SeesAds()
        {
            
            //Ad.IsVisible = await _localData.GetSeesAds();
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            totalCal = 0;
            Shell.SetTabBarIsVisible(this, false);
            SeesAds();
        }



    public async void PopulateList(List<int> loggedFoodIDs)
        {
            
            userID = await _localData.GetUserID();
            List<LoggedFoodTable> loggedFoods = new List<LoggedFoodTable>();
            var listView = mealInfoList;

            var items = new List<LoggedFoodTable>();

            foreach (int loggedFoodID in loggedFoodIDs)
            {
                loggedFood = await _localData.GetLoggedFoodDetails(userID, loggedFoodID);

                items.Add(loggedFood);

                totalCal += (int)Math.Round((decimal)loggedFood.Calories);

            }

            var userInfo = await _localData.GetUserAsync(userID);
            var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
            energyUnit = unitList[3];

            foreach (var food in items)
            {
                
                food.Carbs = Math.Round((decimal)food.Carbs, 1);
                food.Fat = Math.Round((decimal)food.Fat, 1);
                food.Protein = Math.Round((decimal)food.Protein, 1);
                food.TotalGrams = Math.Round((decimal)food.TotalGrams, 1);
                food.DisplayGrams = food.TotalGrams.ToString("0.##");
                food.ServingAmount = Math.Round((decimal)food.ServingAmount, 3);
                food.DisplayServing = Math.Round((decimal)food.TotalGrams / (decimal)food.Grams, 2).ToString("0.##");
                TimeSpan foodTime = food.Time.TimeOfDay;
                Debug.WriteLine(food.Grams);
                Debug.WriteLine(food.TotalGrams);
                if (food.ServingUnit == "oz")
                {
                    food.DisplayGrams = Math.Round(food.TotalGrams * (decimal)28.3495, 2).ToString("0.##");
                    food.DisplayGrams += "oz";
                    food.DisplayServing = Math.Round(food.TotalGrams * (decimal)28.3495 / ((decimal)food.Grams), 2).ToString("0.##");
                }
                else if (food.ServingUnit == "mL")
                {
                    food.DisplayGrams = food.TotalGrams.ToString("0.##");
                    food.DisplayGrams += "mL";
                }
                else if (food.ServingUnit == "g")
                {
                    food.DisplayGrams = food.TotalGrams.ToString("0.##");
                    food.DisplayGrams += "g";
                }


                if (foodTime >= new TimeSpan(0, 0, 0) && foodTime < new TimeSpan(0, 15, 0))
                {
                    food.ClockEmoji = "🕛"; // 12 o'clock emoji
                }
                else if (foodTime >= new TimeSpan(0, 15, 0) && foodTime < new TimeSpan(0, 45, 0))
                {
                    food.ClockEmoji = "🕧"; // 12:30 emoji
                }
                else if (foodTime >= new TimeSpan(0, 45, 0) && foodTime < new TimeSpan(1, 15, 0))
                {
                    food.ClockEmoji = "🕐"; // 1 o'clock emoji
                }
                else if (foodTime >= new TimeSpan(1, 15, 0) && foodTime < new TimeSpan(1, 45, 0))
                {
                    food.ClockEmoji = "🕜"; // 1:30 emoji
                }
                else if (foodTime >= new TimeSpan(1, 45, 0) && foodTime < new TimeSpan(2, 15, 0))
                {
                    food.ClockEmoji = "🕑"; // 2:00 o'clock emoji
                }
                else if (foodTime >= new TimeSpan(2, 15, 0) && foodTime < new TimeSpan(2, 45, 0))
                {
                    food.ClockEmoji = "🕝"; // 2:30 emoji
                }
                else if (foodTime >= new TimeSpan(2, 45, 0) && foodTime < new TimeSpan(3, 15, 0))
                {
                    food.ClockEmoji = "🕒"; // 3:00 emoji
                }
                else if (foodTime >= new TimeSpan(3, 15, 0) && foodTime < new TimeSpan(3, 45, 0))
                {
                    food.ClockEmoji = "🕞"; // 3:30 emoji
                }
                else if (foodTime >= new TimeSpan(3, 45, 0) && foodTime < new TimeSpan(4, 15, 0))
                {
                    food.ClockEmoji = "🕓"; // 4:00 emoji
                }
                else if (foodTime >= new TimeSpan(4, 15, 0) && foodTime < new TimeSpan(4, 45, 0))
                {
                    food.ClockEmoji = "🕟"; // 4:30 emoji
                }
                else if (foodTime >= new TimeSpan(4, 45, 0) && foodTime < new TimeSpan(5, 15, 0))
                {
                    food.ClockEmoji = "🕔"; // 5:00 emoji
                }
                else if (foodTime >= new TimeSpan(5, 15, 0) && foodTime < new TimeSpan(5, 45, 0))
                {
                    food.ClockEmoji = "🕠"; // 5:30 emoji
                }
                else if (foodTime >= new TimeSpan(5, 45, 0) && foodTime < new TimeSpan(6, 15, 0))
                {
                    food.ClockEmoji = "🕕"; // 6:00 emoji
                }
                else if (foodTime >= new TimeSpan(6, 15, 0) && foodTime < new TimeSpan(6, 45, 0))
                {
                    food.ClockEmoji = "🕡"; // 6:30 emoji
                }
                else if (foodTime >= new TimeSpan(6, 45, 0) && foodTime < new TimeSpan(7, 15, 0))
                {
                    food.ClockEmoji = "🕖"; // 7:00 emoji
                }
                else if (foodTime >= new TimeSpan(7, 15, 0) && foodTime < new TimeSpan(7, 45, 0))
                {
                    food.ClockEmoji = "🕢"; // 7:30 emoji
                }
                else if (foodTime >= new TimeSpan(7, 45, 0) && foodTime < new TimeSpan(8, 15, 0))
                {
                    food.ClockEmoji = "🕗"; // 8:00 emoji
                }
                else if (foodTime >= new TimeSpan(8, 15, 0) && foodTime < new TimeSpan(8, 45, 0))
                {
                    food.ClockEmoji = "🕣"; // 8:30 emoji
                }
                else if (foodTime >= new TimeSpan(8, 45, 0) && foodTime < new TimeSpan(9, 15, 0))
                {
                    food.ClockEmoji = "🕘"; // 9:00 emoji
                }
                else if (foodTime >= new TimeSpan(9, 15, 0) && foodTime < new TimeSpan(9, 45, 0))
                {
                    food.ClockEmoji = "🕤"; // 9:30 emoji
                }
                else if (foodTime >= new TimeSpan(9, 45, 0) && foodTime < new TimeSpan(10, 15, 0))
                {
                    food.ClockEmoji = "🕙"; // 10:00 emoji
                }
                else if (foodTime >= new TimeSpan(10, 15, 0) && foodTime < new TimeSpan(10, 45, 0))
                {
                    food.ClockEmoji = "🕥"; // 10:30 emoji
                }
                else if (foodTime >= new TimeSpan(10, 45, 0) && foodTime < new TimeSpan(11, 15, 0))
                {
                    food.ClockEmoji = "🕚"; // 11:00 emoji
                }
                else if (foodTime >= new TimeSpan(11, 15, 0) && foodTime < new TimeSpan(11, 45, 0))
                {
                    food.ClockEmoji = "🕦"; // 11:30 emoji
                }
                else if (foodTime >= new TimeSpan(11, 45, 0) && foodTime < new TimeSpan(12, 15, 0))
                {
                    food.ClockEmoji = "🕛"; // 12:00 emoji
                }
                else if (foodTime >= new TimeSpan(12, 15, 0) && foodTime < new TimeSpan(12, 45, 0))
                {
                    food.ClockEmoji = "🕧"; // 12:30 emoji
                }
                else if (foodTime >= new TimeSpan(12, 45, 0) && foodTime < new TimeSpan(13, 15, 0))
                {
                    food.ClockEmoji = "🕐"; // 1 o'clock emoji
                }
                else if (foodTime >= new TimeSpan(13, 15, 0) && foodTime < new TimeSpan(13, 45, 0))
                {
                    food.ClockEmoji = "🕜"; // 1:30 emoji
                }
                else if (foodTime >= new TimeSpan(13, 45, 0) && foodTime < new TimeSpan(14, 15, 0))
                {
                    food.ClockEmoji = "🕑"; // 2:00 o'clock emoji
                }
                else if (foodTime >= new TimeSpan(14, 15, 0) && foodTime < new TimeSpan(14, 45, 0))
                {
                    food.ClockEmoji = "🕝"; // 2:30 emoji
                }
                else if (foodTime >= new TimeSpan(14, 45, 0) && foodTime < new TimeSpan(15, 15, 0))
                {
                    food.ClockEmoji = "🕒"; // 3:00 emoji
                }
                else if (foodTime >= new TimeSpan(15, 15, 0) && foodTime < new TimeSpan(15, 45, 0))
                {
                    food.ClockEmoji = "🕞"; // 3:30 emoji
                }
                else if (foodTime >= new TimeSpan(15, 45, 0) && foodTime < new TimeSpan(16, 15, 0))
                {
                    food.ClockEmoji = "🕓"; // 4:00 emoji
                }
                else if (foodTime >= new TimeSpan(16, 15, 0) && foodTime < new TimeSpan(16, 45, 0))
                {
                    food.ClockEmoji = "🕟"; // 4:30 emoji
                }
                else if (foodTime >= new TimeSpan(16, 45, 0) && foodTime < new TimeSpan(17, 15, 0))
                {
                    food.ClockEmoji = "🕔"; // 5:00 emoji
                }
                else if (foodTime >= new TimeSpan(17, 15, 0) && foodTime < new TimeSpan(17, 45, 0))
                {
                    food.ClockEmoji = "🕠"; // 5:30 emoji
                }
                else if (foodTime >= new TimeSpan(17, 45, 0) && foodTime < new TimeSpan(18, 15, 0))
                {
                    food.ClockEmoji = "🕕"; // 6:00 emoji
                }
                else if (foodTime >= new TimeSpan(18, 15, 0) && foodTime < new TimeSpan(18, 45, 0))
                {
                    food.ClockEmoji = "🕡"; // 6:30 emoji
                }
                else if (foodTime >= new TimeSpan(18, 45, 0) && foodTime < new TimeSpan(19, 15, 0))
                {
                    food.ClockEmoji = "🕖"; // 7:00 emoji
                }
                else if (foodTime >= new TimeSpan(19, 15, 0) && foodTime < new TimeSpan(19, 45, 0))
                {
                    food.ClockEmoji = "🕢"; // 7:30 emoji
                }
                else if (foodTime >= new TimeSpan(19, 45, 0) && foodTime < new TimeSpan(20, 15, 0))
                {
                    food.ClockEmoji = "🕗"; // 8:00 emoji
                }
                else if (foodTime >= new TimeSpan(20, 15, 0) && foodTime < new TimeSpan(20, 45, 0))
                {
                    food.ClockEmoji = "🕣"; // 8:30 emoji
                }
                else if (foodTime >= new TimeSpan(20, 45, 0) && foodTime < new TimeSpan(21, 15, 0))
                {
                    food.ClockEmoji = "🕘"; // 9:00 emoji
                }
                else if (foodTime >= new TimeSpan(21, 15, 0) && foodTime < new TimeSpan(21, 45, 0))
                {
                    food.ClockEmoji = "🕤"; // 9:30 emoji
                }
                else if (foodTime >= new TimeSpan(21, 45, 0) && foodTime < new TimeSpan(22, 15, 0))
                {
                    food.ClockEmoji = "🕙"; // 10:00 emoji
                }
                else if (foodTime >= new TimeSpan(22, 15, 0) && foodTime < new TimeSpan(22, 45, 0))
                {
                    food.ClockEmoji = "🕥"; // 10:30 emoji
                }
                else if (foodTime >= new TimeSpan(22, 45, 0) && foodTime < new TimeSpan(23, 15, 0))
                {
                    food.ClockEmoji = "🕚"; // 11:00 emoji
                }
                else if (foodTime >= new TimeSpan(23, 15, 0) && foodTime < new TimeSpan(23, 45, 0))
                {
                    food.ClockEmoji = "🕦"; // 11:30 emoji
                }
                else if (foodTime >= new TimeSpan(23, 45, 0) && foodTime < new TimeSpan(23, 59, 59))
                {
                    food.ClockEmoji = "🕛"; // 12:00 emoji
                }








                switch (energyUnit)
                {
                    case "kCal":
                        food.Calories = Math.Round((decimal)food.Calories, 1);
                        break;
                    case "cal":
                        food.Calories = Math.Round((decimal)food.Calories, 1);
                        break;
                    case "kJ":
                        food.Calories = Math.Round((decimal)food.Calories * (decimal)4.184, 1);
                        break;
                }
            }


            listView.ItemsSource = items;
            listView.RowHeight = 90;
            listView.ItemTemplate = new DataTemplate(() =>
            {
               
                Grid grid = new Grid
                {
                    Margin = new Thickness(8),
                    RowSpacing = 3,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Star},
                        new RowDefinition { Height = GridLength.Star },
                        new RowDefinition { Height = GridLength.Star },
                        new RowDefinition { Height = GridLength.Star },
                        new RowDefinition { Height = GridLength.Star }
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Star }
                    }
                };

                
                Label foodNameValueLabel = new Label
                {
                    LineBreakMode = LineBreakMode.WordWrap,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontFamily = "Lato-Bold",
                    FontSize = 15
                };
                foodNameValueLabel.SetBinding(Label.TextProperty, new Binding("FoodName"));

                Label brandValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(128, 128, 128),
                    FontSize = 13
            };
                brandValueLabel.SetBinding(Label.TextProperty, new Binding("Brand"));

                
                Label servingValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                servingValueLabel.SetBinding(Label.TextProperty, new Binding("DisplayServing", stringFormat: "{0} - "));


                Label servingLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "Servings:"
                };

               

               

                Label gramsValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                gramsValueLabel.SetBinding(Label.TextProperty, new Binding("DisplayGrams"));

                StackLayout servingLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 3,
                    Children =
    {
        servingLabel,
        servingValueLabel,
        gramsValueLabel

    }
                };

                Label timeLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontFamily = "Lato-Bold",
                    TextColor = Colors.SlateGrey
                };

                // Assuming "Time" is a DateTime property
                timeLabel.SetBinding(Label.TextProperty, new Binding("Time", stringFormat: "{0:h:mm tt}"));
                Label clockLabel = new Label {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                clockLabel.SetBinding(Label.TextProperty, new Binding("ClockEmoji"));
                

                StackLayout timeLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 3,
                    Children =
                    {
                        clockLabel,
                        timeLabel
                    }
                };

                var calorieIcon = new Image
                {
                    Source = "calicon",
                    WidthRequest = 16,
                    HeightRequest = 16,

                };
                Label caloriesValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontFamily = "Lato-Bold",
                    TextColor = Color.FromRgb(247, 23, 53)
                };
                caloriesValueLabel.SetBinding(Label.TextProperty, new Binding("Calories"));

                var carbIcon = new Image
                {
                    Source = "carbicon",
                    WidthRequest = 16,
                    HeightRequest = 16,

                };
                Label carbsValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(245, 166, 35)

                };
                carbsValueLabel.SetBinding(Label.TextProperty, new Binding("Carbs"));


                var fatIcon = new Image
                {
                    Source = "faticon",
                    WidthRequest = 16,
                    HeightRequest = 16,

                };
                Label fatValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(74, 144, 226)
                };
                fatValueLabel.SetBinding(Label.TextProperty, new Binding("Fat"));


                var proteinIcon = new Image
                {
                    Source = "proteinicon",
                    WidthRequest = 16,
                    HeightRequest = 16,

                };
                Label proteinValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(126, 211, 33)
                };
                proteinValueLabel.SetBinding(Label.TextProperty, new Binding("Protein"));

                var carbsL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { carbIcon, carbsValueLabel }
                };

                var fatL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { fatIcon, fatValueLabel }
                };

                var proteinL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { proteinIcon, proteinValueLabel }
                };

                var macrosLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 5,
                    Children = { carbsL, fatL, proteinL }
                };

                var calLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 1,
                    Children = { calorieIcon, caloriesValueLabel }
                };

                var InfoLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 5,
                    Children = { macrosLayout, calLayout }
                };



                Grid.SetRow(foodNameValueLabel, 0);
                Grid.SetColumn(foodNameValueLabel, 0);
                grid.Children.Add(foodNameValueLabel);


                Grid.SetRow(brandValueLabel, 1);
                Grid.SetColumn(brandValueLabel, 0);
                grid.Children.Add(brandValueLabel);

                Grid.SetRow(servingLayout, 2);
                Grid.SetColumn(servingLayout, 0);
                grid.Children.Add(servingLayout);

                Grid.SetRow(timeLayout, 3);
                Grid.SetColumn(timeLayout, 0);
                grid.Children.Add(timeLayout);

                Grid.SetRow(InfoLayout, 4);
                Grid.SetColumn(InfoLayout, 0);
                grid.Children.Add(InfoLayout);


                return new ViewCell { View = grid };

            });
        }

        decimal totalCarb = 0;
        decimal totalFat = 0;
        decimal totalProtein = 0;
        public LoggedFoodTable food;

        public int goalCal = 0;
        public string energyUnit;

        async void PopulateFoodInfo()
        {
            totalCal = 0;
            totalCarb = 0;
            totalFat = 0;
            totalProtein = 0;
            goalCal = 0;

            var userID = await _localData.GetUserID();


            foods = new List<LoggedFoodTable>();

            var loggedFoods = await _localData.GetLoggedFoods(userID, _date);

            var userInfo = await _localData.GetUserAsync(userID);
            var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
            energyUnit = unitList[3];

            var mealFoodIDs = loggedFoods.Where(f => f.MealType == _mealType).Select(f => f.LoggedFoodID).ToList();

            foreach (int foodID in mealFoodIDs)
            {
                food = await _localData.GetLoggedFoodDetails(userID, foodID);

                foods.Add(food);


            }

            var nutritionGoals = await _localData.GetNutritionGoals(userID, _date);
            if (nutritionGoals != null)
            {
                switch (energyUnit)
                {
                    case "kCal":
                        goalCal = nutritionGoals.CalorieGoal;
                        break;
                    case "cal":
                        goalCal = nutritionGoals.CalorieGoal;
                        break;
                    case "kJ":
                        goalCal = (int)Math.Round(nutritionGoals.CalorieGoal * 4.184);
                        break;
                }

                foreach (var food in foods)
                {
                    totalCarb += Math.Round((decimal)food.Carbs, 5);
                    totalFat += Math.Round((decimal)food.Fat, 5);
                    totalProtein += Math.Round((decimal)food.Protein, 5);
                }


                var (carbPercent, fatPercent, proteinPercent) = CalculateMacrosPercentage(totalCal, totalCarb, totalFat, totalProtein);

                switch (energyUnit)
                {
                    case "kCal":
                        totalCal = ((int)Math.Round((decimal)totalCal));
                        break;
                    case "cal":
                        totalCal = ((int)Math.Round((decimal)totalCal));
                        break;
                    case "kJ":
                        totalCal = ((int)Math.Round((decimal)totalCal * (decimal)4.184));
                        break;
                }

                double frameWidth = mealInfoFrame.Width;
                if (frameWidth < 350) frameWidth = mealInfoFrame.Width * 1.05;
                if (frameWidth < 330) frameWidth = mealInfoFrame.Width;


                double innerRadius = frameWidth * .5;
                if (innerRadius >= 170) innerRadius = frameWidth * .52;
                if (innerRadius < 170 && innerRadius >= 150) innerRadius = frameWidth * .38;
                if (frameWidth < 150) innerRadius = frameWidth * .3;

                PieChart.WidthRequest = frameWidth * .55;
                PieChart.HeightRequest = frameWidth * .55;

                ISeries[] SeriesCollection = new ISeries[]
                    {


                    new PieSeries<double>
                    {
                        Values = new List<double> { carbPercent },
                        Name = "Carbohydrate %",
                        Fill = new SolidColorPaint(SKColor.Parse("#F5A623")),
                        InnerRadius = innerRadius

                    },
                    new PieSeries<double>
                    {
                        Values = new List<double> { fatPercent },
                        Name = "Fat %",
                        Fill = new SolidColorPaint(SKColor.Parse("#4A90E2")),
                        InnerRadius = innerRadius
                    },
                     new PieSeries<double>
                    {
                        Values = new List<double> { proteinPercent },
                        Name = "Protein %",
                        Fill = new SolidColorPaint(SKColor.Parse("#7ED321")),
                        InnerRadius = innerRadius
                    },




                    };


                PieChart.InitialRotation = 80;

                if (foods.Count > 0)
                {
                    CalorieLabel.Text = totalCal.ToString();
                    CarbPercent.Text = (carbPercent / 100).ToString("0.#" + "%");
                    FatPercent.Text = (fatPercent / 100).ToString("0.#" + "%");
                    ProteinPercent.Text = (proteinPercent / 100).ToString("0.#" + "%");
                }

                if (foods.Count == 0)
                {
                    CalorieLabel.Text = totalCal.ToString();
                    CarbPercent.Text = (0).ToString("0.#" + "%");
                    FatPercent.Text = (0).ToString("0.#" + "%");
                    ProteinPercent.Text = (0).ToString("0.#" + "%");
                }

                CarbLabel.Text = totalCarb.ToString("0.#" + "g");
                FatLabel.Text = totalFat.ToString("0.#" + "g");
                ProteinLabel.Text = totalProtein.ToString("0.#" + "g");

                

                if (totalCal < 100) { CalorieLabel.FontSize = 32; CalIcon.WidthRequest = 30; CalIcon.HeightRequest = 30; }
                else if (totalCal < 1000 && totalCal >= 100) { CalorieLabel.FontSize = 30; CalIcon.WidthRequest = 25; CalIcon.HeightRequest = 25; }
                else if (totalCal < 10000 && totalCal >= 1000) { CalorieLabel.FontSize = 28; CalIcon.WidthRequest = 23; CalIcon.HeightRequest = 23; }
                else if (totalCal >= 10000) { CalorieLabel.FontSize = 25; CalIcon.WidthRequest = 21; CalIcon.HeightRequest = 21; }

                PieChart.Series = SeriesCollection;
            }


        }

        public static (double carbPercent, double fatPercent, double proteinPercent) CalculateMacrosPercentage(int totalCalories = 0, decimal totalCarbs = 0, decimal totalFat = 0, decimal totalProtein = 0)
        {
            // Calculate the percentage of calories from each macro
            double carbCalories = (double)totalCarbs * 4;
            double proteinCalories = (double)totalProtein * 4;
            double fatCalories = (double)totalFat * 9;
            double carbPercentage = carbCalories / totalCalories * 100;
            double proteinPercentage = proteinCalories / totalCalories * 100;
            double fatPercentage = fatCalories / totalCalories * 100;

            // Return a Tuple containing the macro percentages
            return (carbPercentage, fatPercentage, proteinPercentage);
        }


        async void mealInfoList_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as LoggedFoodTable;
            if (selectedItem != null)
            {
                await Navigation.PushAsync(new FoodUpdate(_localData, selectedItem));
                mealInfoList.SelectedItem = null; // Deselect the selected item

            }
        }

        void LogFoodClicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FoodSearch(_localData, _dataService, _date, _mealType));

        }

        void NutritionBreakdown(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new NutritionalBreakdown(_localData, _date));

        }

        async void InfoButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await MopupService.Instance.PushAsync(new MealMicroBreakdown(foods));
        }
    }
}