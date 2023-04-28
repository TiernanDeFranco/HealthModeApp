
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using Microsoft.Maui.Controls;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class FoodSearch : ContentPage
{
    private readonly ISQLiteDataService _localData;
    private readonly IRestDataService _dataService;

    int filters = 0;
    DateTime _date;
    int _mealType;

    List<NutritionModel> searchResults;

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
    }

    async void SeesAds()
    {
        bool ads = await _localData.GetSeesAds();

        Ad1.IsVisible = ads;
        Ad2.IsVisible = ads;


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
                break;
        }
    }


    void AllClicked(System.Object sender, System.EventArgs e)
    {
        filters = 0;
        UpdateFilter();
    }

    void MyMealsClicked(System.Object sender, System.EventArgs e)
    {
        filters = 1;
        UpdateFilter();
    }

    void MyFoodsClicked(System.Object sender, System.EventArgs e)
    {
        filters = 2;
        UpdateFilter();
    }

    void SearchButtonPressed(System.Object sender, System.EventArgs e)
    {
        PopulateSearchList(SearchFoods.Text);
    }



    async void PopulateSearchList(string foodName)
    {
        LoadingBar.IsVisible = true;
        LoadingBar.IsRunning = true;
        SearchResultList.IsVisible = false;

        

        switch (filters)
        {
            case 0:
        searchResults = await _dataService.GetNutritionInfoNameAsync(foodName);
            break;
            case 1:
                Debug.WriteLine("Search My Meals");
                break;
            case 2:
                Debug.WriteLine("Search My Foods");
                break;
        }   

      
        SearchResultList.IsVisible = true;
        LoadingBar.IsVisible = false;
        LoadingBar.IsRunning = false;

        var listView = SearchResultList;

        listView.ItemsSource = searchResults;
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
            servingValueLabel.SetBinding(Label.TextProperty, new Binding("ServingSize", stringFormat: "{0:N0}g"));

            Label servingNameLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromRgb(128, 128, 128)
            };
            servingNameLabel.SetBinding(Label.TextProperty, new Binding("ServingName", stringFormat: "({0})"));

            Grid servingGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }
                },
                ColumnSpacing = 8
            };

            servingGrid.SetRow(servingValueLabel, 0);
            servingGrid.SetColumn(servingValueLabel, 0);
            servingGrid.Children.Add(servingValueLabel);

            servingGrid.SetRow(servingNameLabel, 0);
            servingGrid.SetColumn(servingNameLabel, 1);
            servingGrid.Children.Add(servingNameLabel);


            StackLayout servingLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
{
        servingGrid
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
        await Navigation.PushAsync(new BarcodeScan(_dataService, _localData));

    }

    async void CategoriesClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CategorySearch(_dataService, _localData));

    }

    async void SearchResultList_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as NutritionModel;
        if (selectedItem != null)
        {
            await Navigation.PushAsync(new FoodInfo(_dataService, _localData, selectedItem, _mealType, _date));
            SearchResultList.SelectedItem = null; // Deselect the selected item

        }
    }

}

