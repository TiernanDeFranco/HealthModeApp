using System.Diagnostics;
using System.Text.Json;
using HealthModeApp.DataServices;
using HealthModeApp.Pages.FoodJournalPage;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages;

public partial class FoodJournal : ContentPage
{
    private readonly ISQLiteDataService _localData;
    public int fontSize = 14;

    public int meal1Cal = 0;
    public int meal2Cal = 0;
    public int meal3Cal = 0;
    public int meal4Cal = 0;
    public int meal5Cal = 0;
    public int meal6Cal = 0;

    public int goalCal = 2000;
    public int consumedCal = 0;
    public int remainingCal = 2000;

    List<int> meal1FoodIDs = new List<int>();
    List<int> meal2FoodIDs = new List<int>();
    List<int> meal3FoodIDs = new List<int>();
    List<int> meal4FoodIDs = new List<int>();
    List<int> meal5FoodIDs = new List<int>();
    List<int> meal6FoodIDs = new List<int>();

    public FoodJournal()
    {
        InitializeComponent();
    }

    public FoodJournal(ISQLiteDataService dataService)
    {
        InitializeComponent();
        _localData = dataService;
      //_localData.AddLoggedFood(DateTime.Today, 1, DateTime.Today, 1, 24, 1, "1", "Food Name That Is Very Long Yeah", "Brand", 24, "Name", 94, 6, 2, 3, 1, 1, 1, 2, 1, 1, 1, 1, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 11, 1, 1);
        DatePicker.Date = DateTime.Today;
        PopulateMealGrids(DatePicker.Date);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DateHandler();
        
    }

    async void DateHandler()
    {
        if (DatePicker.Date == DateTime.Today)
        {
            TodayButton.IsVisible = false;
        }
        else
        {
            TodayButton.IsVisible = true;
          
            var display = await _localData.GetPopUpSeen("TodayButtonPopup");
            if (display == false)
            {
                await DisplayAlert("Notice", "To navigate back the current date, you can click the newly created 'Jump To Today' Button", "OK");
                await _localData.AddPopUpSeen("TodayButtonPopup", true);
            }
        }
    }
  

    void DatePicker_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {
        DateHandler();
        PopulateMealGrids(DatePicker.Date);
    }


    #region PassFoodsToMealPage
    private void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
    {
        var mealPage = new MealPage(_localData, meal1FoodIDs);

        Navigation.PushAsync(mealPage);
    }
    #endregion

    public async void PopulateMealGrids(DateTime selectedDate)
    {
        meal1Cal = 0;
        meal2Cal = 0;
        meal3Cal = 0;
        meal4Cal = 0;
        meal5Cal = 0;
        meal6Cal = 0;

        var loggedFoods = await _localData.GetLoggedFoods(selectedDate);
        // Populate meal1 grid
        var meal1Foods = loggedFoods.Where(f => f.MealType == 1);
        meal1FoodIDs = meal1Foods.Select(f => f.LoggedFoodID).ToList();
        var meal1Grid = (Grid)FindByName("Meal1Grid");

        // Clear existing items from the grid
        meal1Grid.Children.Clear();
        meal1Grid.RowDefinitions.Clear();




        foreach (var food in meal1Foods)
        {
            var foodName = new Label
            {
                Text = food.FoodName,
                FontSize = fontSize,
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var carbIcon = new Image
            {
                Source = "carbicon",
                WidthRequest = 16,
                HeightRequest = 16,

            };

            var carbs = new Label
            {
                Text = food.Carbs.ToString(),
                FontSize = fontSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromRgb(245, 166, 35)
            };

            var fatIcon = new Image
            {
                Source = "faticon",
                WidthRequest = 16,
                HeightRequest = 16,

            };

            var fat = new Label
            {
                Text = food.Fat.ToString(),
                FontSize = fontSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromRgb(74, 144, 226)
            };

            var proteinIcon = new Image
            {
                Source = "proteinicon",
                WidthRequest = 16,
                HeightRequest = 16,

            };

            var protein = new Label
            {
                Text = food.Protein.ToString(),
                FontSize = fontSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromRgb(126, 211, 33)
            };

            var calIcon = new Image
            {
                Source = "calicon",
                WidthRequest = 14,
                HeightRequest = 14,
                
            };




            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromRgb(247, 23, 53)
            };

            var carbsL = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Children = { carbIcon, carbs}
            };

            var fatL = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Children = { fatIcon, fat }
            };

            var proteinL = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Children = { proteinIcon, protein }
            };

            var macrosLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 5,
                Children = {carbsL, fatL, proteinL }
            };

            var calLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 1,
                Children = { calIcon, calories}
            };

            var InfoLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 9,
                Children = { macrosLayout, calLayout }
            };

            var gridItem = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                  
                },

                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };


           

            gridItem.SetRow(foodName, 0);
            gridItem.SetColumnSpan(foodName, 1);
            gridItem.Children.Add(foodName);

            gridItem.SetRow(InfoLayout, 1);
            gridItem.SetColumn(InfoLayout, 0);
            gridItem.Children.Add(InfoLayout);

            meal1Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            int rowIndex = meal1Grid.RowDefinitions.Count - 1;

        

            meal1Grid.SetRow(gridItem, rowIndex);
            meal1Grid.SetColumn(gridItem, 0);
            meal1Grid.Children.Add(gridItem);
            Debug.WriteLine(rowIndex);

      

            meal1Cal += (int)Math.Round(food.Calories);
        }

        meal1CalLabel.Text = meal1Cal.ToString();

        // Populate meal2 grid
        var meal2Foods = loggedFoods.Where(f => f.MealType == 2);
        List<int> meal2FoodIds = meal2Foods.Select(f => f.LoggedFoodID).ToList();
        var meal2Grid = (Grid)FindByName("Meal2Grid");

        // Remove all rows except the first one
        for (int i = meal2Grid.RowDefinitions.Count - 1; i >= 0; i--)
        {
            meal2Grid.RowDefinitions.RemoveAt(i);
            meal2Grid.Children.RemoveAt(i * 2 + 1);
            meal2Grid.Children.RemoveAt(i * 2);
        }

        foreach (var food in meal2Foods)
        {
            var foodName = new Label
            {
                Text = food.FoodName,
                FontSize = fontSize,
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.Center
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.Center
            };

            meal2Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            int rowIndex = meal2Grid.RowDefinitions.Count - 1;

            // Remove existing child elements (except the first row)
            if (rowIndex > 0)
            {
                meal2Grid.Children.RemoveAt(rowIndex * 2); // Remove foodName label
                meal2Grid.Children.RemoveAt(rowIndex * 2); // Remove calories label
            }

            Grid.SetRow(foodName, rowIndex);
            Grid.SetColumn(foodName, 0);
            meal2Grid.Children.Add(foodName);

            Grid.SetRow(calories, rowIndex);
            Grid.SetColumn(calories, 1);
            meal2Grid.Children.Add(calories);

            meal2Cal += (int)Math.Round(food.Calories);
        }
        meal2CalLabel.Text = meal2Cal.ToString();

        // Populate meal3 grid
        var meal3Foods = loggedFoods.Where(f => f.MealType == 3);
        List<int> meal3FoodIds = meal3Foods.Select(f => f.LoggedFoodID).ToList();
        var meal3Grid = (Grid)FindByName("Meal3Grid");

        // Remove all rows except the first one
        for (int i = meal3Grid.RowDefinitions.Count - 1; i >= 0; i--)
        {
            meal3Grid.RowDefinitions.RemoveAt(i);
            meal3Grid.Children.RemoveAt(i * 2 + 1);
            meal3Grid.Children.RemoveAt(i * 2);
        }

        foreach (var food in meal3Foods)
        {

            var foodName = new Label
            {
                Text = food.FoodName,
                FontSize = fontSize,
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.Center
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.Center
            };

            meal3Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            int rowIndex = meal3Grid.RowDefinitions.Count - 1;

            // Remove existing child elements (except the first row)
            if (rowIndex > 0)
            {
                meal3Grid.Children.RemoveAt(rowIndex * 2); // Remove foodName label
                meal3Grid.Children.RemoveAt(rowIndex * 2); // Remove calories label
            }

            Grid.SetRow(foodName, rowIndex);
            Grid.SetColumn(foodName, 0);
            meal3Grid.Children.Add(foodName);

            Grid.SetRow(calories, rowIndex);
            Grid.SetColumn(calories, 1);
            meal3Grid.Children.Add(calories);

            meal3Cal += (int)Math.Round(food.Calories);
        }
        meal3CalLabel.Text = meal3Cal.ToString();

        // Populate meal4 grid
        var meal4Foods = loggedFoods.Where(f => f.MealType == 4);
        List<int> meal4FoodIds = meal4Foods.Select(f => f.LoggedFoodID).ToList();
        var meal4Grid = (Grid)FindByName("Meal4Grid");

        // Remove all rows except the first one
        for (int i = meal4Grid.RowDefinitions.Count - 1; i >= 0; i--)
        {
            meal4Grid.RowDefinitions.RemoveAt(i);
            meal4Grid.Children.RemoveAt(i * 2 + 1);
            meal4Grid.Children.RemoveAt(i * 2);
        }

        foreach (var food in meal4Foods)
        {

            var foodName = new Label
            {
                Text = food.FoodName,
                FontSize = fontSize,
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.Center
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.Center
            };

            meal4Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            int rowIndex = meal4Grid.RowDefinitions.Count - 1;

            // Remove existing child elements (except the first row)
            if (rowIndex > 0)
            {
                meal4Grid.Children.RemoveAt(rowIndex * 2); // Remove foodName label
                meal4Grid.Children.RemoveAt(rowIndex * 2); // Remove calories label
            }

            Grid.SetRow(foodName, rowIndex);
            Grid.SetColumn(foodName, 0);
            meal4Grid.Children.Add(foodName);

            Grid.SetRow(calories, rowIndex);
            Grid.SetColumn(calories, 1);
            meal4Grid.Children.Add(calories);

            meal4Cal += (int)Math.Round(food.Calories);
        }
        meal4CalLabel.Text = meal4Cal.ToString();

        // Populate meal5 grid
        var meal5Foods = loggedFoods.Where(f => f.MealType == 5);
        List<int> meal5FoodIds = meal5Foods.Select(f => f.LoggedFoodID).ToList();
        var meal5Grid = (Grid)FindByName("Meal5Grid");

        // Remove all rows except the first one
        for (int i = meal5Grid.RowDefinitions.Count - 1; i >= 0; i--)
        {
            meal5Grid.RowDefinitions.RemoveAt(i);
            meal5Grid.Children.RemoveAt(i * 2 + 1);
            meal5Grid.Children.RemoveAt(i * 2);
        }

        foreach (var food in meal5Foods)
        {
            var foodName = new Label
            {
                Text = food.FoodName,
                FontSize = fontSize
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize,
                HorizontalTextAlignment = TextAlignment.End
            };

            meal5Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            int rowIndex = meal5Grid.RowDefinitions.Count - 1;

            // Remove existing child elements (except the first row)
            if (rowIndex > 0)
            {
                meal5Grid.Children.RemoveAt(rowIndex * 2); // Remove foodName label
                meal5Grid.Children.RemoveAt(rowIndex * 2); // Remove calories label
            }

            Grid.SetRow(foodName, rowIndex);
            Grid.SetColumn(foodName, 0);
            meal5Grid.Children.Add(foodName);

            Grid.SetRow(calories, rowIndex);
            Grid.SetColumn(calories, 1);
            meal5Grid.Children.Add(calories);

            meal5Cal += (int)Math.Round(food.Calories);
        }
        meal5CalLabel.Text = meal5Cal.ToString();


        var meal6Foods = loggedFoods.Where(f => f.MealType == 6);
        List<int> meal6FoodIds = meal6Foods.Select(f => f.LoggedFoodID).ToList();
        var meal6Grid = (Grid)FindByName("Meal6Grid");

        // Remove all rows except the first one
        for (int i = meal6Grid.RowDefinitions.Count - 1; i >= 0; i--)
        {
            meal6Grid.RowDefinitions.RemoveAt(i);
            meal6Grid.Children.RemoveAt(i * 2 + 1);
            meal6Grid.Children.RemoveAt(i * 2);
        }

        foreach (var food in meal6Foods)
        {
            var foodName = new Label
            {
                Text = food.FoodName,
                FontSize = fontSize
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize,
                HorizontalTextAlignment = TextAlignment.End
            };

            meal6Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            int rowIndex = meal6Grid.RowDefinitions.Count - 1;

            // Remove existing child elements (except the first row)
            if (rowIndex > 0)
            {
                meal6Grid.Children.RemoveAt(rowIndex * 2); // Remove foodName label
                meal6Grid.Children.RemoveAt(rowIndex * 2); // Remove calories label
            }

            Grid.SetRow(foodName, rowIndex);
            Grid.SetColumn(foodName, 0);
            meal6Grid.Children.Add(foodName);

            Grid.SetRow(calories, rowIndex);
            Grid.SetColumn(calories, 1);
            meal6Grid.Children.Add(calories);

            meal6Cal += (int)Math.Round(food.Calories);
        }
        meal6CalLabel.Text = meal6Cal.ToString();

        GoalLabel.Text = goalCal.ToString();
        consumedCal = (meal1Cal + meal2Cal + meal3Cal + meal4Cal + meal5Cal + meal6Cal);
        ConsumedLabel.Text = consumedCal.ToString();
        remainingCal = goalCal - consumedCal;
        RemainingLabel.Text = remainingCal.ToString();

    }

    void TodayButtonClicked(System.Object sender, System.EventArgs e)
    {
        DatePicker.Date = DateTime.Today;
        DateHandler();
    }

}
