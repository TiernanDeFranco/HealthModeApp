using System.Diagnostics;
using System.Text.Json;
using System.Web;
using HealthModeApp.DataServices;
using Microsoft.Maui.Controls;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.FoodJournalPage
{
    public partial class MealPage : ContentPage
    {
        private readonly ISQLiteDataService _localData;
        public IList<LoggedFoodTable> LoggedFoods { get; set; }
        public int totalCal = 0;

        int userID;
 

        public MealPage(ISQLiteDataService dataService, List<int> MealIDs)
        {
            InitializeComponent();
            _localData = dataService;
            PopulateList(MealIDs);
        }

       

        protected override void OnAppearing()
        {
            base.OnAppearing();
            totalCal = 0;
            Shell.SetTabBarIsVisible(this, false);
        }



        public async void PopulateList(List<int> loggedFoodIDs)
        {
            userID = await _localData.GetUserID();
            List<LoggedFoodTable> loggedFoods = new List<LoggedFoodTable>();
            var listView = mealInfoList;

            var items = new List<LoggedFoodTable>();

            foreach (int loggedFoodID in loggedFoodIDs)
            {
                LoggedFoodTable loggedFood = await _localData.GetLoggedFoodDetails(userID, loggedFoodID, "MealPage");

                items.Add(loggedFood);

                totalCal += (int)Math.Round(loggedFood.Calories);

            }

            listView.ItemsSource = items;
            listView.RowHeight = 100;
            listView.ItemTemplate = new DataTemplate(() =>
            {
               
                Grid grid = new Grid
                {
                    Margin = new Thickness(10),
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
                    FontFamily = "Lato-Bold",
                    FontSize = 15
                };
                foodNameValueLabel.SetBinding(Label.TextProperty, new Binding("FoodName"));

                Label brandValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.FromRgb(128, 128, 128),
                    FontSize = 13
            };
                brandValueLabel.SetBinding(Label.TextProperty, new Binding("Brand"));

                
                Label servingValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                servingValueLabel.SetBinding(Label.TextProperty, new Binding("ServingAmount", stringFormat: "{0} - "));

                Label servingLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Text = "Servings:"
                };

               

               

                Label gramsValueLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
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
    }
}