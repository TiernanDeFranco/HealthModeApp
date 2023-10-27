
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using Microsoft.Maui.Controls;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class FoodSearch : ContentPage
{
    private readonly ISQLiteDataService _localData;
    private readonly IRestDataService _dataService;

    int filters = 0;
    DateTime _date;
    int _mealType;
    List<CustomFoods> localResults;
    List<NutritionModel> searchResults;
    List<CustomFoods> recentFoodsList;
    string _foodName;
    int limit = 50;
    int offset = 0;
    bool recentFood = true;


    public FoodSearch(ISQLiteDataService localData, IRestDataService dataService, DateTime date, int mealType = 1)
	{
		InitializeComponent();
        _localData = localData;
        _dataService = dataService;
        _mealType = mealType;
        _date = date;
        UpdateFilter();
        

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);
        SeesAds();
        PopulateRecents();
    }

    async void SeesAds()
    {
      
    }

    async void PopulateRecents()
    {
        limit = 50;
        offset = 0;

        recentFood = false;

        LoadingBar.IsVisible = true;

        SearchResultList.ItemsSource = null;

        var userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        energyUnit = unitList[3];

        recentFoodsList = await _localData.GetRecentFoods();
        LoadingBar.IsVisible = false;

            SearchLabel.Text = "Recent Foods";
                foreach (var result in recentFoodsList)
                {
                    switch (energyUnit)
                    {
                        case "kCal":
                            result.Calories = ((int)Math.Round(result.Calories));
                            break;
                        case "cal":
                            result.Calories = ((int)Math.Round(result.Calories));
                            break;
                        case "kJ":
                            result.Calories = ((int)Math.Round(result.Calories * (decimal)4.184));
                            break;
                    }
                }


       
        SearchResultList.ItemsSource = recentFoodsList;
        SearchResultList.IsVisible = true;

        SearchResultList.RowHeight = 85;
        SearchResultList.ItemTemplate = new DataTemplate(() =>
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




            Label servingNameLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromRgb(128, 128, 128)
            };
            servingNameLabel.SetBinding(Label.TextProperty, new Binding("ServingName", stringFormat: "[{0}]"));



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
            caloriesValueLabel.SetBinding(Label.TextProperty, new Binding("Calories", stringFormat: "{0:N0}"));


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
            carbsValueLabel.SetBinding(Label.TextProperty, new Binding("Carbs", stringFormat: "{0:N0}"));


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
            fatValueLabel.SetBinding(Label.TextProperty, new Binding("Fat", stringFormat: "{0:N0}"));


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
            proteinValueLabel.SetBinding(Label.TextProperty, new Binding("Protein", stringFormat: "{0:N0}"));

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

            Grid.SetRow(servingNameLabel, 2);
            Grid.SetColumn(servingNameLabel, 0);
            grid.Children.Add(servingNameLabel);

            Grid.SetRow(InfoLayout, 3);
            Grid.SetColumn(InfoLayout, 0);
            grid.Children.Add(InfoLayout);


            return new ViewCell { View = grid };

        });
    }

    void UpdateFilter()
    {
        const double boldFontSize = 17;
        const double regularFontSize = 15;

        switch (filters)
        {
            case 0:
                AllButton.FontFamily = "Lato-Bold";
                AllButton.FontSize = boldFontSize;
                MealsButton.FontFamily = "Lato-Regular";
                MealsButton.FontSize = regularFontSize;
                FoodsButton.FontFamily = "Lato-Regular";
                FoodsButton.FontSize = regularFontSize;

                CustomFoodAdd.IsVisible = false;
                break;
            case 1:
                AllButton.FontFamily = "Lato-Regular";
                AllButton.FontSize = regularFontSize;
                MealsButton.FontFamily = "Lato-Bold";
                MealsButton.FontSize = boldFontSize;
                FoodsButton.FontFamily = "Lato-Regular";
                FoodsButton.FontSize = regularFontSize;
                break;
            case 2:
                AllButton.FontFamily = "Lato-Regular";
                AllButton.FontSize = regularFontSize;
                MealsButton.FontFamily = "Lato-Regular";
                MealsButton.FontSize = regularFontSize;
                FoodsButton.FontFamily = "Lato-Bold";
                FoodsButton.FontSize = boldFontSize;

                CustomFoodAdd.IsVisible = true;
                break;
        }
    }

    string energyUnit;

    void AllClicked(System.Object sender, System.EventArgs e)
    {
        SearchFoods.Text = "";
        SearchResultList.ItemsSource = null;
        filters = 0;
        UpdateFilter();
        PopulateRecents();
        LoadMore.IsVisible = false;
    }

    void MyMealsClicked(System.Object sender, System.EventArgs e)
    {
        SearchFoods.Text = "";
        SearchResultList.ItemsSource = null;
        filters = 1;
        UpdateFilter();
        SearchLabel.Text = "My Meals";
        LoadMore.IsVisible = false;
        recentFood = false;
    }

    async void MyFoodsClicked(System.Object sender, System.EventArgs e)
    {
        SearchFoods.Text = "";
        SearchLabel.Text = "My Foods";
        SearchResultList.ItemsSource = null;
        LoadMore.IsVisible = false;
        filters = 2;
        UpdateFilter();

        SearchResultList.IsVisible = false;
        LoadingBar.IsVisible = true;
        LoadingBar.IsRunning = true;
        recentFood = false;

        var userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        energyUnit = unitList[3];

        localResults = await _localData.GetCustomFoods();
        var listView = SearchResultList;

        SearchResultList.IsVisible = true;
        LoadingBar.IsVisible = false;
        LoadingBar.IsRunning = false;


        foreach (var result in localResults)
        {
            switch (energyUnit)
            {
                case "kCal":
                    result.Calories = ((int)Math.Round(result.Calories));
                    break;
                case "cal":
                    result.Calories = ((int)Math.Round(result.Calories));
                    break;
                case "kJ":
                    result.Calories = ((int)Math.Round(result.Calories * (decimal)4.184));
                    break;
            }
        }

        listView.ItemsSource = localResults;
        listView.RowHeight = 85;
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


            

            Label servingNameLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromRgb(128, 128, 128)
            };
            servingNameLabel.SetBinding(Label.TextProperty, new Binding("ServingName", stringFormat: "[{0}]"));

           

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
            caloriesValueLabel.SetBinding(Label.TextProperty, new Binding("Calories", stringFormat: "{0:N0}"));


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
            carbsValueLabel.SetBinding(Label.TextProperty, new Binding("Carbs", stringFormat: "{0:N0}"));


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
            fatValueLabel.SetBinding(Label.TextProperty, new Binding("Fat", stringFormat: "{0:N0}"));


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
            proteinValueLabel.SetBinding(Label.TextProperty, new Binding("Protein", stringFormat: "{0:N0}"));

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

            Grid.SetRow(servingNameLabel, 2);
            Grid.SetColumn(servingNameLabel, 0);
            grid.Children.Add(servingNameLabel);

            Grid.SetRow(InfoLayout, 3);
            Grid.SetColumn(InfoLayout, 0);
            grid.Children.Add(InfoLayout);


            return new ViewCell { View = grid };

        });
    }

    void SearchButtonPressed(System.Object sender, System.EventArgs e)
    {
        PopulateSearchList(SearchFoods.Text);
    }

    

    async void PopulateSearchList(string foodName)
    {
        SearchLabel.Text = "";
        LoadMore.IsVisible = false;
        recentFood = false;
        _foodName = foodName.Trim();

        limit = 50;
        offset = 0;

        LoadingBar.IsVisible = true;
        LoadingBar.IsRunning = true;
        SearchResultList.IsVisible = false;

        var userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        energyUnit = unitList[3];

        var listView = SearchResultList;

        switch (filters)
        {
            case 0:
                searchResults = await _dataService.GetNutritionInfoNameAsync(foodName);
                SearchLabel.Text = "Food Results";
                if (searchResults.Count > 49)
                {
                    LoadMore.IsVisible = true;
                }

                listView.ItemsSource = searchResults;

                foreach (var result in searchResults)
                {
                    switch (energyUnit)
                    {
                        case "kCal":
                            result.Calories = ((int)Math.Round(result.Calories));
                            break;
                        case "cal":
                            result.Calories = ((int)Math.Round(result.Calories));
                            break;
                        case "kJ":
                            result.Calories = ((int)Math.Round(result.Calories * (decimal)4.184));
                            break;
                    }
                }

                break;

            case 1:
                Debug.WriteLine("Search My Meals");
                break;

            case 2: 
                localResults = await _localData.GetCustomFoodByName(foodName);
                SearchLabel.Text = "My Foods";


                SearchResultList.IsVisible = true;
                LoadingBar.IsVisible = false;
                LoadingBar.IsRunning = false;

                if (localResults != null)
                {
                    foreach (var result in localResults)
                    {
                        switch (energyUnit)
                        {
                            case "kCal":
                                result.Calories = ((int)Math.Round(result.Calories));
                                break;
                            case "cal":
                                result.Calories = ((int)Math.Round(result.Calories));
                                break;
                            case "kJ":
                                result.Calories = ((int)Math.Round(result.Calories * (decimal)4.184));
                                break;
                        }
                    }

                    listView.ItemsSource = localResults;
                }
                else
                {
                    // Handle the case where no matching results were found
                    listView.ItemsSource = null; // Provide an empty list as the ItemsSource
                }
                break;
        }




        SearchResultList.IsVisible = true;
        LoadingBar.IsVisible = false;
        LoadingBar.IsRunning = false;

        listView.RowHeight = 85;
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


            

            Label servingNameLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromRgb(128, 128, 128)
            };
            servingNameLabel.SetBinding(Label.TextProperty, new Binding("ServingName", stringFormat: "[{0}]"));

            


            StackLayout servingLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
{
        servingNameLabel
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
            caloriesValueLabel.SetBinding(Label.TextProperty, new Binding("Calories", stringFormat: "{0:N0}"));


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
            carbsValueLabel.SetBinding(Label.TextProperty, new Binding("Carbs", stringFormat: "{0:N0}"));


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
            fatValueLabel.SetBinding(Label.TextProperty, new Binding("Fat", stringFormat: "{0:N0}"));


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
            proteinValueLabel.SetBinding(Label.TextProperty, new Binding("Protein", stringFormat: "{0:N0}"));

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

    async void BarcodeScanClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new BarcodeScan(_dataService, _localData, _mealType, _date));

    }

    async void CategoriesClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CategorySearch(_dataService, _localData));

    }

    async void SearchResultList_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItemN = e.SelectedItem as NutritionModel;
        var selectedItemC = e.SelectedItem as CustomFoods;


        switch (filters)
        {
            case 0:
                if (selectedItemN != null)
                {
                   await Navigation.PushAsync(new FoodInfo(_dataService, _localData, selectedItemN, _mealType, _date));
                    
                }
                else if (selectedItemC != null)
                {
                    await Navigation.PushAsync(new FoodInfo(_dataService, _localData, selectedItemC, _mealType, _date));
                }
                break;

            case 1:
                break;

            case 2:
                if (selectedItemC != null)
                {
                    await Navigation.PushAsync(new FoodInfo(_dataService, _localData, selectedItemC, _mealType, _date));
                }
                break;
        }

        if (SearchResultList.SelectedItem != null)
        {
            SearchResultList.SelectedItem = null; // Deselect the selected item
        }
    }

    async void LoadMore_Clicked(System.Object sender, System.EventArgs e)
    {
        var listView = SearchResultList;

        offset = limit;
        limit += 50;

        LoadingBar.IsVisible = true;
        LoadingBar.IsRunning = true;
        SearchResultList.IsVisible = false;

        var furtherResults = (await _dataService.GetNutritionInfoNameAsync(_foodName, limit, offset));
        searchResults = searchResults.Concat(furtherResults).ToList();


        LoadingBar.IsVisible = false;
        LoadingBar.IsRunning = false;
        SearchResultList.IsVisible = true;

        if (searchResults.Count < searchResults.Count + (limit - 1))
        {
            LoadMore.IsVisible = false;
        }

        listView.ItemsSource = searchResults;

        foreach (var result in searchResults)
        {
            switch (energyUnit)
            {
                case "kCal":
                    result.Calories = ((int)Math.Round(result.Calories));
                    break;
                case "cal":
                    result.Calories = ((int)Math.Round(result.Calories));
                    break;
                case "kJ":
                    result.Calories = ((int)Math.Round(result.Calories * (decimal)4.184));
                    break;
            }
        }
    }

    async void CustomFoodAdd_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new AddNonBarcodeFood(_dataService, _localData, _mealType, _date));
    }

    void SearchFoods_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        var searchName = SearchFoods.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(searchName))
        {
            if (SearchResultList.ItemsSource == recentFoodsList)
            {
                recentFoodsList = recentFoodsList.OrderBy(item =>
                 {
                     if (item.FoodName.ToLower().StartsWith(searchName) || item.Brand.ToLower().StartsWith(searchName))
                     {
                         return 0;
                     }
                     else if (item.FoodName.ToLower().Contains(searchName) || item.Brand.ToLower().Contains(searchName))
                     {
                         return 1;
                     }
                     return 2;
                 }).ThenBy(item => !item.FoodName.ToLower().StartsWith(searchName)) // True comes after False, so items that start with searchName come first
                    .ThenBy(item => !item.Brand.ToLower().StartsWith(searchName)) // Same logic for Brand
                    .ThenBy(item => item.Brand)
                    .ThenBy(item => item.FoodName)
                    .ToList();

                SearchResultList.ItemsSource = recentFoodsList;
            }
            else if (SearchResultList.ItemsSource == localResults)
            {
                    localResults = localResults.OrderBy(item =>
                    {
                        if (item.FoodName.ToLower().StartsWith(searchName) || item.Brand.ToLower().StartsWith(searchName))
                        {
                            return 0;
                        }
                        else if (item.FoodName.ToLower().Contains(searchName) || item.Brand.ToLower().Contains(searchName))
                        {
                            return 1;
                        }
                        return 2;
                    }).ThenBy(item => !item.FoodName.ToLower().StartsWith(searchName)) // True comes after False, so items that start with searchName come first
                    .ThenBy(item => !item.Brand.ToLower().StartsWith(searchName)) // Same logic for Brand
                    .ThenBy(item => item.Brand)
                    .ThenBy(item => item.FoodName)
                    .ToList();

                SearchResultList.ItemsSource = localResults;

            }
        }

    }
}

