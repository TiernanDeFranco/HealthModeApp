using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using HealthModeApp.DataServices;
using HealthModeApp.Pages.FoodJournalPage;
using Mopups.Services;
using HealthModeApp.CustomControls;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages;

public partial class FoodJournal : ContentPage
{
    private readonly ISQLiteDataService _localData;
    private readonly IRestDataService _dataService;

    public int fontSize = 14;
    public int iconSize = 10;

    public int meal1Cal = 0;
    public int meal2Cal = 0;
    public int meal3Cal = 0;
    public int meal4Cal = 0;
    public int meal5Cal = 0;
    public int meal6Cal = 0;

    public int goalCal = 0000;
    public int consumedCal = 0;
    public int remainingCal = 0000;

    int waterGoal;

    public int goalWater = 0;
    public int consumedWater = 0;
    public int remainingWater = 0;

    double volume1 = 0;
    double volume2 = 0;
    double volume3 = 0;
    double volume4 = 0;

    string _waterUnit;

    double numberOfMeals;

    List<int> meal1FoodIDs = new List<int>();
    List<int> meal2FoodIDs = new List<int>();
    List<int> meal3FoodIDs = new List<int>();
    List<int> meal4FoodIDs = new List<int>();
    List<int> meal5FoodIDs = new List<int>();
    List<int> meal6FoodIDs = new List<int>();

    public int userID;

    public DateTime dateSelected;

    public string energyUnit;

    public FoodJournal()
    {
        InitializeComponent();
    }

    public FoodJournal(ISQLiteDataService localData, IRestDataService dataService)
    {
        _localData = localData;
        _dataService = dataService;

        InitializeComponent();
        
        logFoodToolbarItem = new ToolbarItem
        {
            Text = "Log Food"
        };
        logFoodToolbarItem.Clicked += LogFoodClicked;

        ToolbarItems.Add(logFoodToolbarItem);

        TranslatePage();

       


        // _localData.AddLoggedFood(4, DateTime.Today, 1, DateTime.Today, 1, 24, 1, "1", "Food Name That Is Very Long Yeah", "Brand", 24, "Name", 94, 6, 2, 3, 1, 1, 1, 2, 1, 1, 1, 1, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 11, 1, 1);
        DatePicker.Date = DateTime.Today;
        WaterDatePicker.Date = DateTime.Today;
        dateSelected = DateTime.Today;
        SeesAds();

        
       
    }

    string carbLogo;
    string fatLogo;
    string proteinLogo;

    async void TranslatePage()
    {
        GoalText.Text = await _localData.GetTranslationByKey("Goal");
        WaterGoalText.Text = GoalText.Text;

        ConsumedText.Text = await _localData.GetTranslationByKey("Consumed");
        WaterConsumeText.Text = ConsumedText.Text;
        
        RemainingText.Text = await _localData.GetTranslationByKey("Remaining");
        WaterRemainText.Text = RemainingText.Text;

        carbLogo = await _localData.GetTranslationByKey("CarbIcon");
        fatLogo = await _localData.GetTranslationByKey("FatIcon");
        proteinLogo = await _localData.GetTranslationByKey("ProteinIcon");

        logFoodToolbarItem.Text = await _localData.GetTranslationByKey("LogFood");
    }

    public async void PopulateMealNames()
    {
        

        MealsGrid.RowDefinitions.Clear();

        numberOfMeals = await _localData.SetMealNumber(DatePicker.Date);
        Debug.WriteLine(numberOfMeals);

    

        switch (numberOfMeals)
        {
            case 1:
                
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = false;
                Meal3Frame.IsVisible = false;
                Meal4Frame.IsVisible = false;
                Meal5Frame.IsVisible = false;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8,12,8,3);

                MealsGrid.SetRowSpan(BaseFrame, 3);
                MealsGrid.SetColumnSpan(BaseFrame, 2);

                MealsGrid.SetRowSpan(Meal1Frame, 3);
                MealsGrid.SetColumnSpan(Meal1Frame, 2);

              

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .18;

                meal1CalLabel.FontSize = 23.5;
                Meal1Name.FontSize = 23;

               
                break;

            case 2:
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = false;
                Meal4Frame.IsVisible = false;
                Meal5Frame.IsVisible = false;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8, 12, 8, 3);

                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumnSpan(BaseFrame, 2);
                MealsGrid.SetRow(BaseFrame, 0);
                MealsGrid.SetColumn(BaseFrame, 0);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumn(Meal1Frame, 0);
                MealsGrid.SetColumnSpan(Meal1Frame, 2);

                MealsGrid.SetRow(Meal2Frame, 1);
                MealsGrid.SetRowSpan(Meal2Frame, 1);
                MealsGrid.SetColumnSpan(Meal2Frame, 2);
                MealsGrid.SetColumn(Meal2Frame, 0);



                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .12;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .12;

                meal1CalLabel.FontSize = 21;
                Meal1Name.FontSize = 20;

                meal2CalLabel.FontSize = 21;
                Meal2Name.FontSize = 20;

                

                break;

            case 3:
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = true;
                Meal4Frame.IsVisible = false;
                Meal5Frame.IsVisible = false;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8, 12, 8, 3);

                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumnSpan(BaseFrame, 2);
                MealsGrid.SetRow(BaseFrame, 0);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumnSpan(Meal1Frame, 2);

                MealsGrid.SetRow(Meal2Frame, 1);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumn(Meal2Frame, 0);
                MealsGrid.SetColumnSpan(Meal2Frame, 2);

                MealsGrid.SetRow(Meal3Frame, 2);
                MealsGrid.SetRowSpan(Meal3Frame, 1);
                MealsGrid.SetColumnSpan(Meal3Frame, 2);

                meal1CalLabel.FontSize = 20;
                Meal1Name.FontSize = 19;

                meal2CalLabel.FontSize = 20;
                Meal2Name.FontSize = 19;

                meal3CalLabel.FontSize = 20;
                Meal3Name.FontSize = 19;

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal3Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;

               

                break;

            case 4:

                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = true;
                Meal4Frame.IsVisible = true;
                Meal5Frame.IsVisible = false;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8, 12, 8, 3);

                MealsGrid.SetRow(BaseFrame, 2);
                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumn(BaseFrame, 0);
                MealsGrid.SetColumnSpan(BaseFrame, 1);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumnSpan(Meal1Frame, 2);

                MealsGrid.SetRow(Meal2Frame, 1);
                MealsGrid.SetRowSpan(Meal2Frame, 1);
                MealsGrid.SetColumn(Meal2Frame, 0);
                MealsGrid.SetColumnSpan(Meal2Frame, 2);

                MealsGrid.SetRow(Meal3Frame, 2);
                MealsGrid.SetRowSpan(Meal3Frame, 1);
                MealsGrid.SetColumn(Meal3Frame, 0);

                MealsGrid.SetRow(Meal4Frame, 2);
                MealsGrid.SetRowSpan(Meal4Frame, 1);
                MealsGrid.SetColumn(Meal4Frame, 1);


                meal1CalLabel.FontSize = meal4CalLabel.FontSize; 
                Meal1Name.FontSize = Meal4Name.FontSize;

                meal2CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal2Name.FontSize = Meal4Name.FontSize;

                meal3CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal3Name.FontSize = Meal4Name.FontSize;

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal3Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal4Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;

              

                break;

            case 4.1:

                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = true;
                Meal4Frame.IsVisible = true;
                Meal5Frame.IsVisible = false;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8, 12, 8, 3);

                MealsGrid.SetRow(BaseFrame, 2);
                MealsGrid.SetColumn(BaseFrame, 0);
                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumnSpan(BaseFrame, 1);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetColumn(Meal1Frame, 0);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumnSpan(Meal1Frame, 1);

                MealsGrid.SetRow(Meal2Frame, 0);
                MealsGrid.SetColumn(Meal2Frame, 1);
                MealsGrid.SetRowSpan(Meal2Frame, 1);
                MealsGrid.SetColumnSpan(Meal2Frame, 1);

                MealsGrid.SetRow(Meal3Frame, 1);
                MealsGrid.SetColumnSpan(Meal3Frame, 2);
                MealsGrid.SetRowSpan(Meal3Frame, 1);
               

                MealsGrid.SetRow(Meal4Frame, 2);
                MealsGrid.SetRowSpan(Meal4Frame, 1);
                MealsGrid.SetColumn(Meal4Frame, 0);
                MealsGrid.SetColumnSpan(Meal4Frame, 2);

                meal1CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal1Name.FontSize = Meal4Name.FontSize;

                meal2CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal2Name.FontSize = Meal4Name.FontSize;

                meal3CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal3Name.FontSize = Meal4Name.FontSize;

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal3Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal4Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;

                

                break;

            case 4.2:

                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = true;
                Meal4Frame.IsVisible = true;
                Meal5Frame.IsVisible = false;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8, 12, 8, 3);

                MealsGrid.SetRow(BaseFrame, 2);
                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumnSpan(BaseFrame, 1);
                MealsGrid.SetColumn(BaseFrame, 0);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetColumnSpan(Meal1Frame, 2);
                MealsGrid.SetColumn(Meal1Frame, 0);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
      

                MealsGrid.SetRow(Meal2Frame, 1);
                MealsGrid.SetColumn(Meal2Frame, 0);
                MealsGrid.SetRowSpan(Meal2Frame, 1);
                MealsGrid.SetColumnSpan(Meal2Frame, 1);

                MealsGrid.SetRow(Meal3Frame, 1);
                MealsGrid.SetColumn(Meal3Frame, 1);
                MealsGrid.SetRowSpan(Meal3Frame, 1);
                MealsGrid.SetColumnSpan(Meal3Frame, 1);

                MealsGrid.SetRow(Meal4Frame, 2);
                MealsGrid.SetColumn(Meal4Frame, 0);
                MealsGrid.SetColumnSpan(Meal4Frame, 2);
                MealsGrid.SetRowSpan(Meal4Frame, 1);

                meal1CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal1Name.FontSize = Meal4Name.FontSize;

                meal2CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal2Name.FontSize = Meal4Name.FontSize;

                meal3CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal3Name.FontSize = Meal4Name.FontSize;

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal3Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal4Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;

               

                break;


            case 5:

                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = true;
                Meal4Frame.IsVisible = true;
                Meal5Frame.IsVisible = true;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8, 12, 8, 3);

                MealsGrid.SetRow(BaseFrame, 2);
                MealsGrid.SetColumn(BaseFrame, 0);
                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumnSpan(BaseFrame, 1);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetColumnSpan(Meal1Frame, 2);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumn(Meal1Frame, 0);

                MealsGrid.SetRow(Meal2Frame, 1);
                MealsGrid.SetColumn(Meal2Frame, 0);
                MealsGrid.SetRowSpan(Meal2Frame, 1);
                MealsGrid.SetColumnSpan(Meal2Frame, 1);

                MealsGrid.SetRow(Meal3Frame, 1);
                MealsGrid.SetColumn(Meal3Frame, 1);
                MealsGrid.SetRowSpan(Meal3Frame, 1);
                MealsGrid.SetColumnSpan(Meal3Frame, 1);

                MealsGrid.SetRow(Meal4Frame, 2);
                MealsGrid.SetColumn(Meal4Frame, 0);
                MealsGrid.SetRowSpan(Meal4Frame, 1);
                MealsGrid.SetColumnSpan(Meal4Frame, 1);

                MealsGrid.SetRow(Meal5Frame, 2);
                MealsGrid.SetColumn(Meal5Frame, 1);
                MealsGrid.SetRowSpan(Meal5Frame, 1);
                MealsGrid.SetColumnSpan(Meal5Frame, 1);

                meal1CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal1Name.FontSize = Meal4Name.FontSize;

                meal2CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal2Name.FontSize = Meal4Name.FontSize;

                meal3CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal3Name.FontSize = Meal4Name.FontSize;

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal3Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal4Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;

                

                break;

            case 5.1:

                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = true;
                Meal4Frame.IsVisible = true;
                Meal5Frame.IsVisible = true;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8, 12, 8, 8);

                MealsGrid.SetRow(BaseFrame, 2);
                MealsGrid.SetColumn(BaseFrame, 0);
                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumnSpan(BaseFrame, 1);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetColumn(Meal1Frame, 0);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumnSpan(Meal1Frame, 1);

                MealsGrid.SetRow(Meal2Frame, 0);
                MealsGrid.SetColumn(Meal2Frame, 1);
                MealsGrid.SetRowSpan(Meal2Frame, 1);
                MealsGrid.SetColumnSpan(Meal2Frame, 1);

                MealsGrid.SetRow(Meal3Frame, 1);
                MealsGrid.SetColumn(Meal3Frame, 0);
                MealsGrid.SetRowSpan(Meal3Frame, 1);
                MealsGrid.SetColumnSpan(Meal3Frame, 1);

                MealsGrid.SetRow(Meal4Frame, 1);
                MealsGrid.SetColumn(Meal4Frame, 1);
                MealsGrid.SetRowSpan(Meal4Frame, 1);
                MealsGrid.SetColumnSpan(Meal4Frame, 1);

                MealsGrid.SetRow(Meal5Frame, 2);
                MealsGrid.SetColumnSpan(Meal5Frame, 2);
                MealsGrid.SetRowSpan(Meal5Frame, 1);
                MealsGrid.SetColumn(Meal5Frame, 0);

                meal1CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal1Name.FontSize = Meal4Name.FontSize;

                meal2CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal2Name.FontSize = Meal4Name.FontSize;

                meal3CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal3Name.FontSize = Meal4Name.FontSize;

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal3Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal4Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;

               

                break;

            case 5.2:

                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = true;
                Meal4Frame.IsVisible = true;
                Meal5Frame.IsVisible = true;
                Meal6Frame.IsVisible = false;

                MealsGrid.Margin = new Thickness(8, 12, 8, 3);

                MealsGrid.SetRow(BaseFrame, 2);
                MealsGrid.SetColumn(BaseFrame, 0);
                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumnSpan(BaseFrame, 1);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetColumn(Meal1Frame, 0);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumnSpan(Meal1Frame, 1);

                MealsGrid.SetRow(Meal2Frame, 0);
                MealsGrid.SetColumn(Meal2Frame, 1);
                MealsGrid.SetRowSpan(Meal2Frame, 1);
                MealsGrid.SetColumnSpan(Meal2Frame, 1);

                MealsGrid.SetRow(Meal3Frame, 1);
                MealsGrid.SetColumnSpan(Meal3Frame, 2);
                MealsGrid.SetRowSpan(Meal3Frame, 1);
                MealsGrid.SetColumn(Meal3Frame, 0);

                MealsGrid.SetRow(Meal4Frame, 2);
                MealsGrid.SetColumn(Meal4Frame, 0);
                MealsGrid.SetRowSpan(Meal4Frame, 1);
                MealsGrid.SetColumnSpan(Meal4Frame, 1);

                MealsGrid.SetRow(Meal5Frame, 2);
                MealsGrid.SetColumn(Meal5Frame, 1);
                MealsGrid.SetRowSpan(Meal5Frame, 1);
                MealsGrid.SetColumnSpan(Meal5Frame, 1);

                meal1CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal1Name.FontSize = Meal4Name.FontSize;

                meal2CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal2Name.FontSize = Meal4Name.FontSize;

                meal3CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal3Name.FontSize = Meal4Name.FontSize;

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal3Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal4Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;

          

                break;

            case 6:

                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MealsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


                Meal1Frame.IsVisible = true;
                Meal2Frame.IsVisible = true;
                Meal3Frame.IsVisible = true;
                Meal4Frame.IsVisible = true;
                Meal5Frame.IsVisible = true;
                Meal6Frame.IsVisible = true;


                MealsGrid.Margin = new Thickness(8, 12, 8, 3);

                MealsGrid.SetRow(BaseFrame, 2);
                MealsGrid.SetColumn(BaseFrame, 0);
                MealsGrid.SetRowSpan(BaseFrame, 1);
                MealsGrid.SetColumnSpan(BaseFrame, 1);

                MealsGrid.SetRow(Meal1Frame, 0);
                MealsGrid.SetColumn(Meal1Frame, 0);
                MealsGrid.SetRowSpan(Meal1Frame, 1);
                MealsGrid.SetColumnSpan(Meal1Frame, 1);

                MealsGrid.SetRow(Meal2Frame, 0);
                MealsGrid.SetColumn(Meal2Frame, 1);
                MealsGrid.SetRowSpan(Meal2Frame, 1);
                MealsGrid.SetColumnSpan(Meal2Frame, 1);

                MealsGrid.SetRow(Meal3Frame, 1);
                MealsGrid.SetColumn(Meal3Frame, 0);
                MealsGrid.SetRowSpan(Meal3Frame, 1);
                MealsGrid.SetColumnSpan(Meal3Frame, 1);

                MealsGrid.SetRow(Meal4Frame, 1);
                MealsGrid.SetColumn(Meal4Frame, 1);
                MealsGrid.SetRowSpan(Meal4Frame, 1);
                MealsGrid.SetColumnSpan(Meal4Frame, 1);

                MealsGrid.SetRow(Meal5Frame, 2);
                MealsGrid.SetColumn(Meal5Frame, 0);
                MealsGrid.SetRowSpan(Meal5Frame, 1);
                MealsGrid.SetColumnSpan(Meal5Frame, 1);

                MealsGrid.SetRow(Meal6Frame, 2);
                MealsGrid.SetColumn(Meal6Frame, 1);
                MealsGrid.SetRowSpan(Meal6Frame, 1);
                MealsGrid.SetColumnSpan(Meal6Frame, 1);

                meal1CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal1Name.FontSize = Meal4Name.FontSize;

                meal2CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal2Name.FontSize = Meal4Name.FontSize;

                meal3CalLabel.FontSize = meal4CalLabel.FontSize;
                Meal3Name.FontSize = Meal4Name.FontSize;

                Meal1Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal2Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal3Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal4Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal5Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                Meal6Scroll.HeightRequest = DeviceDisplay.MainDisplayInfo.Height * .06;
                break;
        }

        var scale = 42;
        Meal1Button.WidthRequest = scale;
        Meal1Button.HeightRequest = scale;

        Meal2Button.WidthRequest = scale;
        Meal2Button.HeightRequest = scale;

        Meal3Button.WidthRequest = scale;
        Meal3Button.HeightRequest = scale;

        Meal4Button.WidthRequest = scale;
        Meal4Button.HeightRequest = scale;

        Meal5Button.WidthRequest = scale;
        Meal5Button.HeightRequest = scale;

        Meal6Button.WidthRequest = scale;
        Meal6Button.HeightRequest = scale;


        var mealNames = await _localData.GetMealNames(DatePicker.Date);

        if (mealNames == null || mealNames.Count == 0)
        {
            // Create default meal names
            var defaultMealNames = new List<string> { "Breakfast", "Lunch", "Dinner", "Snack", "Meal 5", "Meal 6" };

            // Add the default meal names to the MealNames table
            int mealNum = 1;
            foreach (var name in defaultMealNames)
            {

                var mealName = new MealNames { MealName = name };
                await _localData.AddMealName(mealName, mealNum, new DateTime(1900, 1, 1));
                mealNum++;
            }


        }
        else
        {
            // Retrieve the meal names again now that they have been added
            mealNames = await _localData.GetMealNames(DatePicker.Date);

            // Set the text of the labels to the corresponding meal names
            if (mealNames.Count >= 1)
                Meal1Name.Text = mealNames[0];

            if (mealNames.Count >= 2)
                Meal2Name.Text = mealNames[1];

            if (mealNames.Count >= 3)
                Meal3Name.Text = mealNames[2];

            if (mealNames.Count >= 4)
                Meal4Name.Text = mealNames[3];

            if (mealNames.Count >= 5)
                Meal5Name.Text = mealNames[4];

            if (mealNames.Count >= 6)
                Meal6Name.Text = mealNames[5];

        }

       

    }

    async void SeesAds()
    {
        
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, true);
        DateHandler();
        SeesAds();



        await Task.Delay(100);
        
        PopulateMealGrids(DatePicker.Date);
        PopulateWater(WaterDatePicker.Date);
    }


        async void DateHandler()
    {
        userID = await _localData.GetUserID();
        var userInfo = await _localData.GetUserAsync(userID);
        var unitList = JsonSerializer.Deserialize<List<string>>(userInfo.Units);
        var dateFormat = unitList[5];
        energyUnit = unitList[3];
        _waterUnit = unitList[2];

       

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            DatePicker.Format = $"{dateFormat}";
            WaterDatePicker.Format = DatePicker.Format;
        }
        else
        {
            DatePicker.Format = $"   {dateFormat}";
            WaterDatePicker.Format = DatePicker.Format;
        }


        if (dateSelected == DateTime.Today)
        {
            TodayButton.IsVisible = false;
            WaterTodayButton.IsVisible = false;
            userID = await _localData.GetUserID();
            
            var nutritionGoals = await _localData.GetNutritionGoals(userID, dateSelected);
            waterGoal = await _localData.GetWaterGoal(dateSelected);
            if (nutritionGoals != null)
            {

                Debug.WriteLine(nutritionGoals.CalorieGoal);
                if (waterGoal == 0) waterGoal = 2500;

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

                switch (_waterUnit)
                {
                    case "fl oz":
                        volume1 = 8;
                        volume2 = 12;
                        volume3 = 16.9;
                        volume4 = 32;
                        goalWater = (int)(Math.Round(waterGoal / 28.574));
                        break;

                    case "cups":
                        volume1 = 1;
                        volume2 = 1.5;
                        volume3 = 2;
                        volume4 = 4;
                        goalWater = (int)(Math.Round(waterGoal / 236.6));
                        break;

                    case "mL":
                        volume1 = 235;
                        volume2 = 355;
                        volume3 = 500;
                        volume4 = 1000;
                        goalWater = (int)Math.Round((decimal)waterGoal);
                        break;
                }

                Volume1.Text = volume1.ToString($"0.# {_waterUnit}");
                Volume2.Text = volume2.ToString($"0.# {_waterUnit}");
                Volume3.Text = volume3.ToString($"0.# {_waterUnit}");
                Volume4.Text = volume4.ToString($"0.# {_waterUnit}");
                WaterLabel.Text = _waterUnit.ToString();
                WaterEntry.Text = null;

                consumedCal = 0;
                remainingCal = 0;

                GoalLabel.Text = goalCal.ToString();
                consumedCal = (meal1Cal + meal2Cal + meal3Cal + meal4Cal + meal5Cal + meal6Cal);
                ConsumedLabel.Text = consumedCal.ToString();
                remainingCal = goalCal - consumedCal;
                RemainingLabel.Text = remainingCal.ToString();

                WaterGoalLabel.Text = goalWater.ToString();
                WaterConsumedLabel.Text = consumedWater.ToString();
                remainingWater = goalWater - consumedWater;
                WaterRemainingLabel.Text = remainingWater.ToString();


                

            }
        }
        else
        {
            TodayButton.IsVisible = true;
            WaterTodayButton.IsVisible = true;

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
        dateSelected = DatePicker.Date;
        PopulateMealGrids(DatePicker.Date);
    }

    void WaterDatePicker_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {
        DateHandler();
        dateSelected = WaterDatePicker.Date;
        PopulateWater(WaterDatePicker.Date);
    }


    #region PassFoodsToMealPage
    private async void Meal1Tapped(System.Object sender, System.EventArgs e)
    {
        var loggedFoods = await _localData.GetLoggedFoods(userID, DatePicker.Date);

        var mealFoods = loggedFoods.Where(f => f.MealType == 1);
        meal1FoodIDs = mealFoods.Select(f => f.LoggedFoodID).ToList();
        await Navigation.PushAsync(new MealPage(_localData, _dataService, meal1FoodIDs, DatePicker.Date, 1, Meal1Name.Text));
    }

    private async void Meal2Tapped(System.Object sender, System.EventArgs e)
    {
        var loggedFoods = await _localData.GetLoggedFoods(userID, DatePicker.Date);

        var mealFoods = loggedFoods.Where(f => f.MealType == 2);

        meal2FoodIDs = mealFoods.Select(f => f.LoggedFoodID).ToList();
        await Navigation.PushAsync(new MealPage(_localData, _dataService, meal2FoodIDs, DatePicker.Date, 2, Meal2Name.Text));
    }

    private async void Meal3Tapped(System.Object sender, System.EventArgs e)
    {
        var loggedFoods = await _localData.GetLoggedFoods(userID, DatePicker.Date);

        var mealFoods = loggedFoods.Where(f => f.MealType == 3);
        meal3FoodIDs = mealFoods.Select(f => f.LoggedFoodID).ToList();
        await Navigation.PushAsync(new MealPage(_localData, _dataService, meal3FoodIDs, DatePicker.Date, 3, Meal3Name.Text));
    }

    private async void Meal4Tapped(System.Object sender, System.EventArgs e)
    {
        var loggedFoods = await _localData.GetLoggedFoods(userID, DatePicker.Date);

        var mealFoods = loggedFoods.Where(f => f.MealType == 4);
        meal4FoodIDs = mealFoods.Select(f => f.LoggedFoodID).ToList();
        await Navigation.PushAsync(new MealPage(_localData, _dataService, meal4FoodIDs, DatePicker.Date, 4, Meal4Name.Text));
    }

    private async void Meal5Tapped(System.Object sender, System.EventArgs e)
    {
        var loggedFoods = await _localData.GetLoggedFoods(userID, DatePicker.Date);

        var mealFoods = loggedFoods.Where(f => f.MealType == 5);
        meal5FoodIDs = mealFoods.Select(f => f.LoggedFoodID).ToList();
        await Navigation.PushAsync(new MealPage(_localData, _dataService, meal5FoodIDs, DatePicker.Date, 5, Meal5Name.Text));
    }

    private async void Meal6Tapped(System.Object sender, System.EventArgs e)
    {
        var loggedFoods = await _localData.GetLoggedFoods(userID, DatePicker.Date);

        var mealFoods = loggedFoods.Where(f => f.MealType == 6);
        meal6FoodIDs = mealFoods.Select(f => f.LoggedFoodID).ToList();
        await Navigation.PushAsync(new MealPage(_localData, _dataService, meal6FoodIDs, DatePicker.Date, 6, Meal6Name.Text));
    }
    #endregion

    public async void PopulateWater(DateTime selectedDate)
    {
        await WaterListView.FadeTo(0, 250);
        userID = await _localData.GetUserID();
        var nutritionGoals = await _localData.GetNutritionGoals(userID, selectedDate);
        var waterList = await _localData.GetWaterEntries(userID, selectedDate);

        waterList.Reverse();

        if (waterList.Count > 0) { ListFrame.FadeTo(1, 220); }
        else ListFrame.FadeTo(0, 250);

        consumedWater = 0;
        foreach (var waters in waterList)
        {
            consumedWater += (int)Math.Round(waters.WaterVolume,0);
            switch (_waterUnit)
            {
                case "fl oz":
                    waters.WaterVolume = (Math.Round(waters.WaterVolume / (decimal)29.5735, 1));
                    waters.WaterUnit = "fl oz";
                    if (waters.WaterVolume <= 8) waters.WaterImage = "watericon";
                    else if (waters.WaterVolume > 8 && waters.WaterVolume < 32) waters.WaterImage = "waterbottle";
                    else waters.WaterImage = "waterjug";
                    break;

                case "cups":
                    waters.WaterVolume = (Math.Round((waters.WaterVolume / (decimal)236.588), 1));
                    waters.WaterUnit = "cups";
                    if (waters.WaterVolume <= 1) waters.WaterImage = "watericon";
                    else if (waters.WaterVolume > 1 && waters.WaterVolume < 4) waters.WaterImage = "waterbottle";
                    else waters.WaterImage = "waterjug";
                    break;

                case "mL":
                    waters.WaterVolume = Math.Round(waters.WaterVolume);
                    waters.WaterUnit = "mL";
                    if (waters.WaterVolume <= 240) waters.WaterImage = "watericon";
                    else if (waters.WaterVolume > 240 && waters.WaterVolume < 1000) waters.WaterImage = "waterbottle";
                    else waters.WaterImage = "waterjug";
                    break;
            }
        }

       

            switch (_waterUnit)
            {
                case "fl oz":
                    volume1 = 8;
                    volume2 = 12;
                    volume3 = 16.9;
                    volume4 = 32;
                    goalWater = (int)(Math.Round(waterGoal / 28.574));
                    consumedWater = (int)(Math.Round(consumedWater / 28.574));
                    break;

                case "cups":
                    volume1 = 1;
                    volume2 = 1.5;
                    volume3 = 2;
                    volume4 = 4;
                    goalWater = (int)(Math.Round(waterGoal / 236.6));
                    consumedWater = (int)(Math.Round(consumedWater / 236.6));
                    break;

                case "mL":
                    volume1 = 235;
                    volume2 = 355;
                    volume3 = 500;
                    volume4 = 1000;
                    goalWater = (int)Math.Round((decimal)waterGoal);
                consumedWater = (int)Math.Round((decimal)consumedWater);
                    break;
            }

        Volume1.Text = volume1.ToString($"0.# {_waterUnit}");
        Volume2.Text = volume2.ToString($"0.# {_waterUnit}");
        Volume3.Text = volume3.ToString($"0.# {_waterUnit}");
        Volume4.Text = volume4.ToString($"0.# {_waterUnit}");
        WaterLabel.Text = _waterUnit.ToString();
        WaterEntry.Text = null;

        WaterListView.ItemsSource = waterList;
        WaterListView.ItemTemplate = new DataTemplate(() =>
        {

            Grid grid = new Grid
            {
                Margin = new Thickness(8),
                RowSpacing = 2,
                RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto}
                    },
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Star }
                    },
                ColumnSpacing = 5
            };

            Label waterVolume = new Label
            {
                FontFamily = "Lato-Bold",
                FontSize = 15,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            waterVolume.SetBinding(Label.TextProperty, new Binding("WaterVolume"));

            Label waterUnit = new Label
            {
                FontFamily = "Lato-Regular",
                FontSize = 15,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            waterUnit.SetBinding(Label.TextProperty, new Binding("WaterUnit"));

            StackLayout waterLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 3,
                Children =
                {
                    waterVolume,
                    waterUnit
                }
            };

            Image picture = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 25,
                HeightRequest = 25
                
            };

            picture.SetBinding(Image.SourceProperty, new Binding("WaterImage"));


            Grid.SetRow(waterLayout, 0);
            Grid.SetColumn(waterLayout, 1);
            grid.Children.Add(waterLayout);


            Grid.SetRow(picture, 0);
            Grid.SetColumn(picture, 0);
            grid.Children.Add(picture);


            return new ViewCell { View = grid };

        });

        

            WaterGoalLabel.Text = goalWater.ToString();
            WaterConsumedLabel.Text = consumedWater.ToString();
            remainingWater = goalWater - consumedWater;
            WaterRemainingLabel.Text = remainingWater.ToString();
            WaterListView.FadeTo(1, 150);

            await WaterGoalBar.ProgressTo(((double)consumedWater/goalWater) + .001, 500, Easing.CubicInOut);
            
        
    }



    


    public async void PopulateMealGrids(DateTime selectedDate)
    {

        PopulateMealNames();

        userID = await _localData.GetUserID();
        var nutritionGoals = await _localData.GetNutritionGoals(userID, DatePicker.Date);
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

            meal1Cal = 0;
            meal2Cal = 0;
            meal3Cal = 0;
            meal4Cal = 0;
            meal5Cal = 0;
            meal6Cal = 0;


            var loggedFoods = await _localData.GetLoggedFoods(userID, selectedDate);
            // Populate meal1 grid
            var meal1Foods = loggedFoods.Where(f => f.MealType == 1);
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

                var carbIcon = new MacroIcon
                {
                    Text = carbLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(245, 166, 35)


                };

                var carbs = new Label
                {
                    Text = Math.Round((decimal)food.Carbs).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(245, 166, 35)
                };

                var fatIcon = new MacroIcon
                {
                    Text = fatLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(74, 144, 226)

                };

                var fat = new Label
                {
                    Text = Math.Round((decimal)food.Fat).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(74, 144, 226)
                };

                var proteinIcon = new MacroIcon
                {
                    Text = proteinLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(126, 211, 33)

                };

                var protein = new Label
                {
                    Text = Math.Round((decimal)food.Protein).ToString(),
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
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(247, 23, 53)
                };

                

                switch (energyUnit)
                {
                    case "kCal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "cal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "kJ":
                        food.Calories = ((int)Math.Round((decimal)food.Calories * (decimal)4.184));
                        break;
                }

                calories.Text = food.Calories.ToString();

                var carbsL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { carbIcon, carbs }
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
                    Children = { carbsL, fatL, proteinL }
                };

                var calLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 1,
                    Children = { calIcon, calories }
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


                meal1Cal += (int)Math.Round((decimal)food.Calories);
            }

            meal1CalLabel.Text = meal1Cal.ToString();

            // Populate meal2 grid
            var meal2Foods = loggedFoods.Where(f => f.MealType == 2);
            var meal2Grid = (Grid)FindByName("Meal2Grid");

            // Clear existing items from the grid
            meal2Grid.Children.Clear();
            meal2Grid.RowDefinitions.Clear();




            foreach (var food in meal2Foods)
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

                var carbIcon = new MacroIcon
                {
                    Text = carbLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(245, 166, 35)


                };

                var carbs = new Label
                {
                    Text = Math.Round((decimal)food.Carbs).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(245, 166, 35)
                };

                var fatIcon = new MacroIcon
                {
                    Text = fatLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(74, 144, 226)

                };

                var fat = new Label
                {
                    Text = Math.Round((decimal)food.Fat).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(74, 144, 226)
                };

                var proteinIcon = new MacroIcon
                {
                    Text = proteinLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(126, 211, 33)

                };

                var protein = new Label
                {
                    Text = Math.Round((decimal)food.Protein).ToString(),
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
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(247, 23, 53)
                };

                switch (energyUnit)
                {
                    case "kCal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "cal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "kJ":
                        food.Calories = ((int)Math.Round((decimal)food.Calories * (decimal)4.184));
                        break;
                }

                calories.Text = food.Calories.ToString();

                var carbsL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { carbIcon, carbs }
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
                    Children = { carbsL, fatL, proteinL }
                };

                var calLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 1,
                    Children = { calIcon, calories }
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

                meal2Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                int rowIndex = meal2Grid.RowDefinitions.Count - 1;



                meal2Grid.SetRow(gridItem, rowIndex);
                meal2Grid.SetColumn(gridItem, 0);
                meal2Grid.Children.Add(gridItem);
                Debug.WriteLine(rowIndex);

                meal2Cal += (int)Math.Round((decimal)food.Calories);
            }

            meal2CalLabel.Text = meal2Cal.ToString();

            // Populate meal2 grid
            var meal3Foods = loggedFoods.Where(f => f.MealType == 3);
            var meal3Grid = (Grid)FindByName("Meal3Grid");

            // Clear existing items from the grid
            meal3Grid.Children.Clear();
            meal3Grid.RowDefinitions.Clear();




            foreach (var food in meal3Foods)
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

                var carbIcon = new MacroIcon
                {
                    Text = carbLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(245, 166, 35)


                };

                var carbs = new Label
                {
                    Text = Math.Round((decimal)food.Carbs).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(245, 166, 35)
                };

                var fatIcon = new MacroIcon
                {
                    Text = fatLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(74, 144, 226)

                };

                var fat = new Label
                {
                    Text = Math.Round((decimal)food.Fat).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(74, 144, 226)
                };

                var proteinIcon = new MacroIcon
                {
                    Text = proteinLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(126, 211, 33)

                };

                var protein = new Label
                {
                    Text = Math.Round((decimal)food.Protein).ToString(),
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
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(247, 23, 53)
                };

                switch (energyUnit)
                {
                    case "kCal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "cal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "kJ":
                        food.Calories = ((int)Math.Round((decimal)food.Calories * (decimal)4.184));
                        break;
                }

                calories.Text = food.Calories.ToString();

                var carbsL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { carbIcon, carbs }
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
                    Children = { carbsL, fatL, proteinL }
                };

                var calLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 1,
                    Children = { calIcon, calories }
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

                meal3Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                int rowIndex = meal3Grid.RowDefinitions.Count - 1;



                meal3Grid.SetRow(gridItem, rowIndex);
                meal3Grid.SetColumn(gridItem, 0);
                meal3Grid.Children.Add(gridItem);
                Debug.WriteLine(rowIndex);


                meal3Cal += (int)Math.Round((decimal)food.Calories);
            }

            meal3CalLabel.Text = meal3Cal.ToString();

            // Populate meal2 grid
           var meal4Foods = loggedFoods.Where(f => f.MealType == 4);
            var meal4Grid = (Grid)FindByName("Meal4Grid");

            // Clear existing items from the grid
            meal4Grid.Children.Clear();
            meal4Grid.RowDefinitions.Clear();




            foreach (var food in meal4Foods)
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

                var carbIcon = new MacroIcon
                {
                    Text = carbLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(245, 166, 35)


                };

                var carbs = new Label
                {
                    Text = Math.Round((decimal)food.Carbs).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(245, 166, 35)
                };

                var fatIcon = new MacroIcon
                {
                    Text = fatLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(74, 144, 226)

                };

                var fat = new Label
                {
                    Text = Math.Round((decimal)food.Fat).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(74, 144, 226)
                };

                var proteinIcon = new MacroIcon
                {
                    Text = proteinLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(126, 211, 33)

                };

                var protein = new Label
                {
                    Text = Math.Round((decimal)food.Protein).ToString(),
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
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(247, 23, 53)
                };

                

                switch (energyUnit)
                {
                    case "kCal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "cal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "kJ":
                        food.Calories = ((int)Math.Round((decimal)food.Calories * (decimal)4.184));
                        break;
                }

                calories.Text = food.Calories.ToString();

                var carbsL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { carbIcon, carbs }
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
                    Children = { carbsL, fatL, proteinL }
                };

                var calLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 1,
                    Children = { calIcon, calories }
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

                meal4Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                int rowIndex = meal4Grid.RowDefinitions.Count - 1;



                meal4Grid.SetRow(gridItem, rowIndex);
                meal4Grid.SetColumn(gridItem, 0);
                meal4Grid.Children.Add(gridItem);
                Debug.WriteLine(rowIndex);


                meal4Cal += (int)Math.Round((decimal)food.Calories);
            }

            meal4CalLabel.Text = meal4Cal.ToString();

            // Populate meal2 grid
            var meal5Foods = loggedFoods.Where(f => f.MealType == 5);
            var meal5Grid = (Grid)FindByName("Meal5Grid");

            // Clear existing items from the grid
            meal5Grid.Children.Clear();
            meal5Grid.RowDefinitions.Clear();




            foreach (var food in meal5Foods)
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

                var carbIcon = new MacroIcon
                {
                    Text = carbLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(245, 166, 35)


                };

                var carbs = new Label
                {
                    Text = Math.Round((decimal)food.Carbs).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(245, 166, 35)
                };

                var fatIcon = new MacroIcon
                {
                    Text = fatLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(74, 144, 226)

                };

                var fat = new Label
                {
                    Text = Math.Round((decimal)food.Fat).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(74, 144, 226)
                };

                var proteinIcon = new MacroIcon
                {
                    Text = proteinLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(126, 211, 33)

                };

                var protein = new Label
                {
                    Text = Math.Round((decimal)food.Protein).ToString(),
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
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(247, 23, 53)
                };

                switch (energyUnit)
                {
                    case "kCal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "cal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "kJ":
                        food.Calories = ((int)Math.Round((decimal)food.Calories*(decimal)4.184));
                        break;
                }

                calories.Text = food.Calories.ToString();

                var carbsL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { carbIcon, carbs }
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
                    Children = { carbsL, fatL, proteinL }
                };

                var calLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 1,
                    Children = { calIcon, calories }
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

                meal5Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                int rowIndex = meal5Grid.RowDefinitions.Count - 1;



                meal5Grid.SetRow(gridItem, rowIndex);
                meal5Grid.SetColumn(gridItem, 0);
                meal5Grid.Children.Add(gridItem);
                Debug.WriteLine(rowIndex);


                meal5Cal += (int)Math.Round((decimal)food.Calories);
            }

            meal5CalLabel.Text = meal5Cal.ToString();

            var meal6Foods = loggedFoods.Where(f => f.MealType == 6);
            var meal6Grid = (Grid)FindByName("Meal6Grid");

            // Clear existing items from the grid
            meal6Grid.Children.Clear();
            meal6Grid.RowDefinitions.Clear();




            foreach (var food in meal6Foods)
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

                var carbIcon = new MacroIcon
                {
                    Text = carbLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(245, 166, 35)


                };

                var carbs = new Label
                {
                    Text = Math.Round((decimal)food.Carbs).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(245, 166, 35)
                };

                var fatIcon = new MacroIcon
                {
                    Text = fatLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(74, 144, 226)

                };

                var fat = new Label
                {
                    Text = Math.Round((decimal)food.Fat).ToString(),
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(74, 144, 226)
                };

                var proteinIcon = new MacroIcon
                {
                    Text = proteinLogo,
                    FontSize = iconSize,
                    IconColor = Color.FromRgb(126, 211, 33)

                };

                var protein = new Label
                {
                    Text = Math.Round((decimal)food.Protein).ToString(),
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
                    FontSize = fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromRgb(247, 23, 53)
                };

                switch (energyUnit)
                {
                    case "kCal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "cal":
                        food.Calories = ((int)Math.Round((decimal)food.Calories));
                        break;
                    case "kJ":
                        food.Calories = ((int)Math.Round((decimal)food.Calories * (decimal)4.184));
                        break;
                }

                calories.Text = food.Calories.ToString();

                var carbsL = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                    Children = { carbIcon, carbs }
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
                    Children = { carbsL, fatL, proteinL }
                };

                var calLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 1,
                    Children = { calIcon, calories }
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

                meal6Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                int rowIndex = meal6Grid.RowDefinitions.Count - 1;

                meal6Grid.SetRow(gridItem, rowIndex);
                meal6Grid.SetColumn(gridItem, 0);
                meal6Grid.Children.Add(gridItem);
                Debug.WriteLine(rowIndex);



                meal6Cal += (int)Math.Round((decimal)food.Calories);

                }
            }

            meal6CalLabel.Text = meal6Cal.ToString();

            GoalLabel.Text = goalCal.ToString();
            consumedCal = (meal1Cal + meal2Cal + meal3Cal + meal4Cal + meal5Cal + meal6Cal);
            ConsumedLabel.Text = consumedCal.ToString();
            remainingCal = goalCal - consumedCal;
            RemainingLabel.Text = remainingCal.ToString();


            CalorieGoalBar.ProgressTo(((double)consumedCal/goalCal) + .02,1200, Easing.CubicOut);


        List<Grid> mealGrids = new List<Grid> { Meal1Grid, Meal2Grid, Meal3Grid, Meal4Grid, Meal5Grid, Meal6Grid };

        foreach (Grid mealGrid in mealGrids)
        {
            mealGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            int mealGridRowIndex = mealGrid.RowDefinitions.Count - 1;

            var gridExtender = new Label
            {
                Text = "  \n  ",
                HorizontalOptions = LayoutOptions.Center
            };

            mealGrid.SetRow(gridExtender, mealGridRowIndex);
            mealGrid.SetColumn(gridExtender, 0);
            mealGrid.Children.Add(gridExtender);
        }



        
    }

        void TodayButtonClicked(System.Object sender, System.EventArgs e)
        {
            DatePicker.Date = DateTime.Today;
            WaterDatePicker.Date = DateTime.Today;
            dateSelected = DateTime.Today;
            DateHandler();
            PopulateWater(WaterDatePicker.Date);
        }

        void LogFoodClicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FoodSearch(_localData, _dataService, DatePicker.Date));

        }

        async void SettingsClicked(System.Object sender, System.EventArgs e)
        {

         await MopupService.Instance.PushAsync(new FoodJournalSettings(this, _localData, DatePicker.Date));


        }

        void FoodInfoTapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new NutritionalBreakdown(_localData, DatePicker.Date));
        }


        void LogFoodMeal1(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FoodSearch(_localData, _dataService, DatePicker.Date, 1));

        }

        void LogFoodMeal2(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FoodSearch(_localData, _dataService, DatePicker.Date, 2));
        }

        void LogFoodMeal3(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FoodSearch(_localData, _dataService, DatePicker.Date, 3));
        }

        void LogFoodMeal4(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FoodSearch(_localData, _dataService, DatePicker.Date, 4));
        }

        void LogFoodMeal5(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FoodSearch(_localData, _dataService, DatePicker.Date, 5));
        }

        void LogFoodMeal6(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FoodSearch(_localData, _dataService, DatePicker.Date, 6));
        }

    async void ChangeUnitClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushModalAsync(new Pages.UnitPage(_localData, _dataService));
        await Task.Delay(100);

    }

    ToolbarItem logFoodToolbarItem;

    void WaterXClicked(System.Object sender, System.EventArgs e)
    {
        Title = "Food Journal";

        logFoodToolbarItem = new ToolbarItem
        {
            Text = "Log Food"
        };
        logFoodToolbarItem.Clicked += LogFoodClicked;

        ToolbarItems.Add(logFoodToolbarItem);

        DatePicker.Date = WaterDatePicker.Date;

        WaterGrid.FadeTo(0, 250);
        WaterInfoFrame.FadeTo(0, 270);

        MealsGrid.FadeTo(1, 300);
        InfoFrame.FadeTo(1, 320);

        PopulateMealGrids(DatePicker.Date);
    }

    void WaterAdd_Clicked(System.Object sender, System.EventArgs e)
    {
        Title = "Water Journal";

        ToolbarItems.Remove(logFoodToolbarItem);

        WaterDatePicker.Date = DatePicker.Date;

        waterFrame.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .32;
        WaterEntry.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * .05;
        MealsGrid.FadeTo(0, 250);
        InfoFrame.FadeTo(0, 275);

        WaterGrid.FadeTo(1, 300);
        WaterInfoFrame.FadeTo(1, 320);

        PopulateWater(WaterDatePicker.Date);

    }

    void Volume1_Clicked(System.Object sender, System.EventArgs e)
    {
        if (double.TryParse(WaterEntry.Text, out var volume))
        {
            // Perform operations using the volume1 variable as a double
            // For example:
            volume += volume1;

            // Convert the updated volume1 back to string and assign it to WaterEntry.Text
            WaterEntry.Text = volume.ToString("0.#");
        }
        else
        {
            WaterEntry.Text = volume1.ToString("0.#");
        }

    }

    void Volume2_Clicked(System.Object sender, System.EventArgs e)
    {
        if (double.TryParse(WaterEntry.Text, out var volume))
        {
            // Perform operations using the volume1 variable as a double
            // For example:
            volume += volume2;

            // Convert the updated volume1 back to string and assign it to WaterEntry.Text
            WaterEntry.Text = volume.ToString("0.#");
        }
        else
        {
            WaterEntry.Text = volume2.ToString("0.#");
        }
    }

    void Volume3_Clicked(System.Object sender, System.EventArgs e)
    {
        if (double.TryParse(WaterEntry.Text, out var volume))
        {
            // Perform operations using the volume1 variable as a double
            // For example:
            volume += volume3;

            // Convert the updated volume1 back to string and assign it to WaterEntry.Text
            WaterEntry.Text = volume.ToString("0.#");
        }
        else
        {
            WaterEntry.Text = volume3.ToString("0.#");
        }
    }

    void Volume4_Clicked(System.Object sender, System.EventArgs e)
    {
        if (double.TryParse(WaterEntry.Text, out var volume))
        {
            // Perform operations using the volume1 variable as a double
            // For example:
            volume += volume4;

            // Convert the updated volume1 back to string and assign it to WaterEntry.Text
            WaterEntry.Text = volume.ToString("0.#");
        }
        else
        {
            WaterEntry.Text = volume4.ToString("0.#");
        }
    }

    async void AddWaterEntry_Clicked(System.Object sender, System.EventArgs e)
    {
        decimal mLVolume = 0;
        userID = await _localData.GetUserID();
        if (decimal.TryParse(WaterEntry.Text, out var volume))
        {
            switch (_waterUnit)
            {
                case "mL":
                    mLVolume = volume;
                    break;
                case "fl oz":
                    mLVolume = Math.Round(volume * (decimal)29.5735, 1);
                    break;
                case "cups":
                    mLVolume = Math.Round(volume * (decimal)236.588, 1);
                    break;

            }
            
            try
            {
                await _localData.AddWaterEntry(userID, WaterDatePicker.Date, mLVolume);
            }
            catch
            {
               await  DisplayAlert("Notice", "Something went wrong trying to add that water entry, sorry :(", "OK");
            }
            WaterEntry.Text = null;
            PopulateWater(WaterDatePicker.Date);
        }
        else
        {
            
        }


    }

    async void waterList_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as WaterTable;
        if (selectedItem != null)
        {
            await Navigation.PushAsync(new WaterUpdate(_localData, _dataService, selectedItem));
            WaterListView.SelectedItem = null; // Deselect the selected item

        }
    }

    
}

