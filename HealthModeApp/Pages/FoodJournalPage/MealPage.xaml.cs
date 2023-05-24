using System.Diagnostics;
using System.Text.Json;
using System.Web;
using HealthModeApp.DataServices;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Maui.Controls;
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


        public MealPage(ISQLiteDataService localData, IRestDataService dataService, List<int> MealIDs, DateTime date, int mealType)
        {
            InitializeComponent();
            _localData = localData;
            PopulateMealName();
            _dataService = dataService;
            _mealType = mealType;
            _date = date;
            PopulateList(MealIDs);
            PopulateFoodInfo();
            SeesAds();
        }

        async void PopulateMealName()
        {
            var mealNames = await _localData.GetMealNames();

            switch (_mealType)
            {
                case 1:
                    MealName.Text = $"{mealNames[0]}\nDetails";
                    break;
                case 2:
                    MealName.Text = $"{mealNames[1]}\nDetails";
                    break;
                case 3:
                    MealName.Text = $"{mealNames[2]}\nDetails";
                    break;
                case 4:
                    MealName.Text = $"{mealNames[3]}\nDetails";
                    break;
                case 5:
                    MealName.Text = $"{mealNames[4]}\nDetails";
                    break;
                case 6:
                    MealName.Text = $"{mealNames[5]}\nDetails";
                    break;
            }
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
            energyUnit = unitList[2];

            foreach (var food in items)
            {
                
                food.Carbs = Math.Round((decimal)food.Carbs, 1);
                food.Fat = Math.Round((decimal)food.Fat, 1);
                food.Protein = Math.Round((decimal)food.Protein, 1);
                food.TotalGrams = Math.Round((decimal)food.TotalGrams, 1);
                food.ServingAmount = Math.Round((decimal)food.ServingAmount, 2);

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
                    RowSpacing = 2,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Star},
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
                servingValueLabel.SetBinding(Label.TextProperty, new Binding("ServingAmount", stringFormat: "{0} - "));

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
                gramsValueLabel.SetBinding(Label.TextProperty, new Binding("TotalGrams", stringFormat: "{0}g"));

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

                Grid.SetRow(InfoLayout, 3);
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


            var foods = new List<LoggedFoodTable>();

            var loggedFoods = await _localData.GetLoggedFoods(userID, _date);

            var userInfo = await _localData.GetUserAsync(userID);
            var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
            energyUnit = unitList[2];

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
    }
}