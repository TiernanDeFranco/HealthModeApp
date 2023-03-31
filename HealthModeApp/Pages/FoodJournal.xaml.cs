using System.Diagnostics;
using HealthModeApp.DataServices;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages;

public partial class FoodJournal : ContentPage
{
    private readonly ISQLiteDataService _dataService;
    public int fontSize = 12;

    public int meal1Cal = 0;
    public int meal2Cal = 0;
    public int meal3Cal = 0;
    public int meal4Cal = 0;
    public int meal5Cal = 0;
    public int meal6Cal = 0;

    public FoodJournal()
    {
        InitializeComponent();
    }

    public FoodJournal(ISQLiteDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

    }

    void DatePicker_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {
        PopulateMealGrids(DatePicker.Date);
    }

    public async void PopulateMealGrids(DateTime selectedDate)
    {
        var loggedFoods = await _dataService.GetLoggedFoods(selectedDate);
        // Populate meal1 grid
        var meal1Foods = loggedFoods.Where(f => f.MealType == 1);
        var meal1Grid = (Grid)FindByName("Meal1Grid");

        // Remove all rows except the first one
        for (int i = meal1Grid.RowDefinitions.Count - 1; i >= 1; i--)
        {
            meal1Grid.RowDefinitions.RemoveAt(i);
            meal1Grid.Children.RemoveAt(i * 2 + 1);
            meal1Grid.Children.RemoveAt(i * 2);
        }

        foreach (var food in meal1Foods)
        {

            var foodName = new Label
            {
                Text = food.FoodName,
                FontSize = fontSize
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize
            };

            meal1Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            int rowIndex = meal1Grid.RowDefinitions.Count - 1;

            // Remove existing child elements (except the first row)
            if (rowIndex > 0)
            {
                meal1Grid.Children.RemoveAt(rowIndex * 2); // Remove foodName label
                meal1Grid.Children.RemoveAt(rowIndex * 2); // Remove calories label
            }

            Grid.SetRow(foodName, rowIndex);
            Grid.SetColumn(foodName, 0);
            meal1Grid.Children.Add(foodName);

            Grid.SetRow(calories, rowIndex);
            Grid.SetColumn(calories, 1);
            meal1Grid.Children.Add(calories);

            meal1Cal += (int)Math.Round(food.Calories);
        }
        meal1CalLabel.Text = meal1Cal.ToString();

        // Populate meal2 grid
        var meal2Foods = loggedFoods.Where(f => f.MealType == 2);
        var meal2Grid = (Grid)FindByName("Meal2Grid");

        // Remove all rows except the first one
        for (int i = meal2Grid.RowDefinitions.Count - 1; i >= 1; i--)
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
                FontSize = fontSize
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize
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
        var meal3Grid = (Grid)FindByName("Meal3Grid");

        // Remove all rows except the first one
        for (int i = meal3Grid.RowDefinitions.Count - 1; i >= 1; i--)
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
                FontSize = fontSize
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize
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
        var meal4Grid = (Grid)FindByName("Meal4Grid");

        // Remove all rows except the first one
        for (int i = meal4Grid.RowDefinitions.Count - 1; i >= 1; i--)
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
                FontSize = fontSize
            };
            var calories = new Label
            {
                Text = food.Calories.ToString(),
                FontSize = fontSize
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
        var meal5Grid = (Grid)FindByName("Meal5Grid");

        // Remove all rows except the first one
        for (int i = meal5Grid.RowDefinitions.Count - 1; i >= 1; i--)
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
                FontSize = fontSize
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
        var meal6Grid = (Grid)FindByName("Meal6Grid");

        // Remove all rows except the first one
        for (int i = meal6Grid.RowDefinitions.Count - 1; i >= 1; i--)
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
                FontSize = fontSize
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


    }

}
