
using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class CategorySearch : ContentPage
{
	private readonly ISQLiteDataService _localData;
	private readonly IRestDataService _dataService;

    List<NutritionModel> searchResults;

    public CategorySearch(IRestDataService dataService, ISQLiteDataService localData)
	{
		InitializeComponent();
		_localData = localData;
		_dataService = dataService;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);
        SeesAds();
    }

    async void SeesAds()
    {
       
    }

    async void PopulateSearch()
    {
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



}
