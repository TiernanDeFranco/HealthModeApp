using HealthModeApp.DataServices;
using Mopups.Services;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class FoodJournalSettings
{
	private readonly ISQLiteDataService _localData;

    double mealLayout;
    DateTime _date;
    FoodJournal _sender;

	public FoodJournalSettings(FoodJournal sender, ISQLiteDataService localData, DateTime date)
	{
		InitializeComponent();
       
		_localData = localData;
        _date = date;
        _sender = sender;
        MainGrid.TranslateTo(MainGrid.X, -25, 100, Easing.CubicInOut);
        PopulateCurrentNames();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        double mealNum = await _localData.SetMealNumber(_date);
        mealLayout = mealNum;
        switch (mealNum)
        {
            case 1:
                MealNumberLabel.Text = "1 Meal";
                Meal1Prev.IsVisible = true;
                Meal1Prev.Opacity = 0;
                Meal1Prev.FadeTo(1, 250);
                Meal1Button.Background = Color.FromRgb(75, 158, 227);
                Meal2Label.IsVisible = false;
                Meal2Name.IsVisible = false;
                Meal3Label.IsVisible = false;
                Meal3Name.IsVisible = false;
                Meal4Label.IsVisible = false;
                Meal4Name.IsVisible = false;
                Meal5Label.IsVisible = false;
                Meal5Name.IsVisible = false;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal5Name.Opacity = 0;
                Meal5Label.Opacity = 0;
                Meal4Label.Opacity = 0;
                Meal4Name.Opacity = 0;
                Meal3Name.Opacity = 0;
                Meal3Label.Opacity = 0;
                Meal2Label.Opacity = 0;
                Meal2Name.Opacity = 0;
                break;
            case 2:
                MealNumberLabel.Text = "2 Meals";
                Meal2Prev.IsVisible = true;
                Meal2Prev.Opacity = 0;
                Meal2Prev.FadeTo(1, 250);
                Meal2Button.Background = Color.FromRgb(75, 158, 227);

                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal5Name.Opacity = 0;
                Meal5Label.Opacity = 0;
                Meal4Label.Opacity = 0;
                Meal4Name.Opacity = 0;
                Meal3Name.Opacity = 0;
                Meal3Label.Opacity = 0;
                Meal3Label.IsVisible = false;
                Meal3Name.IsVisible = false;
                Meal4Label.IsVisible = false;
                Meal4Name.IsVisible = false;
                Meal5Label.IsVisible = false;
                Meal5Name.IsVisible = false;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                break;
            case 3:
                MealNumberLabel.Text = "3 Meals";
                Meal3Prev.IsVisible = true;
                Meal3Prev.Opacity = 0;
                Meal3Prev.FadeTo(1, 250);
                Meal3Button.Background = Color.FromRgb(75, 158, 227);

                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal5Name.Opacity = 0;
                Meal5Label.Opacity = 0;
                Meal4Label.Opacity = 0;
                Meal4Name.Opacity = 0;
                Meal4Label.IsVisible = false;
                Meal4Name.IsVisible = false;
                Meal5Label.IsVisible = false;
                Meal5Name.IsVisible = false;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                break;
            case 4:
                MealNumberLabel.Text = "4 Meals";
                Meal4Prev.IsVisible = true;
                Meal4Prev.Opacity = 0;
                Meal4Button.Background = Color.FromRgb(75, 158, 227);
                Meal4Prev.FadeTo(1, 250);
                Meal4Bottom.Opacity = 1;
                Meal4Top.Opacity = .25;
                Meal4Mid.Opacity = .25;

                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal5Name.Opacity = 0;
                Meal5Label.Opacity = 0;
                Meal5Label.IsVisible = false;
                Meal5Name.IsVisible = false;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                break;

            case 4.1:
                MealNumberLabel.Text = "4 Meals";
                Meal4Prev.IsVisible = true;
                Meal4Prev.Opacity = 0;
                Meal4Button.Background = Color.FromRgb(75, 158, 227);
                Meal4Prev.FadeTo(1, 250);
                Meal4Bottom.Opacity = .25;
                Meal4Top.Opacity = 1;
                Meal4Mid.Opacity = .25;

                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal5Name.Opacity = 0;
                Meal5Label.Opacity = 0;
                Meal5Label.IsVisible = false;
                Meal5Name.IsVisible = false;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                break;

            case 4.2:
                MealNumberLabel.Text = "4 Meals";
                Meal4Prev.IsVisible = true;
                Meal4Prev.Opacity = 0;
                Meal4Button.Background = Color.FromRgb(75, 158, 227);
                Meal4Prev.FadeTo(1, 250);
                Meal4Bottom.Opacity = .25;
                Meal4Top.Opacity = .25;
                Meal4Mid.Opacity = 1;


                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal5Name.Opacity = 0;
                Meal5Label.Opacity = 0;
                Meal5Label.IsVisible = false;
                Meal5Name.IsVisible = false;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                break;

            case 5:
                MealNumberLabel.Text = "5 Meals";
                Meal5Prev.IsVisible = true;
                Meal5Prev.Opacity = 0;
                Meal5Button.Background = Color.FromRgb(75, 158, 227);
                Meal5Prev.FadeTo(1, 250);
                Meal5Bottom.Opacity = .25;
                Meal5Top.Opacity = 1;
                Meal5Mid.Opacity = .25;

                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                break;

            case 5.1:
                MealNumberLabel.Text = "5 Meals";
                Meal5Prev.IsVisible = true;
                Meal5Prev.Opacity = 0;
                Meal5Button.Background = Color.FromRgb(75, 158, 227);
                Meal5Prev.FadeTo(1, 250);
                Meal5Bottom.Opacity = 1;
                Meal5Top.Opacity = .25;
                Meal5Mid.Opacity = .25;

                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                break;

            case 5.2:
                MealNumberLabel.Text = "5 Meals";
                Meal5Prev.IsVisible = true;
                Meal5Prev.Opacity = 0;
                Meal5Button.Background = Color.FromRgb(75, 158, 227);
                Meal5Prev.FadeTo(1, 250);
                Meal5Bottom.Opacity = .25;
                Meal5Top.Opacity = .25;
                Meal5Mid.Opacity = 1;


                Meal6Label.Opacity = 0;
                Meal6Name.Opacity = 0;
                Meal6Label.IsVisible = false;
                Meal6Name.IsVisible = false;
                break;

            case 6:
                MealNumberLabel.Text = "6 Meals";
                Meal6Prev.IsVisible = true;
                Meal6Prev.Opacity = 0;
                Meal6Button.Background = Color.FromRgb(75, 158, 227);
                Meal6Prev.FadeTo(1, 250);
                break;
        }

        Shell.SetTabBarIsVisible(this, false);
       

    }

    async void PopulateCurrentNames()
        {
        var mealNames = await _localData.GetMealNames(_date);

        if (mealNames.Count > 0)
        {
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
        else
        {
            Meal1Name.Text = "Meal 1";
            Meal2Name.Text = "Meal 2";
            Meal3Name.Text = "Meal 3";
            Meal4Name.Text = "Meal 4";
            Meal5Name.Text = "Meal 5";
            Meal6Name.Text = "Meal 6";
        }

       
    }


    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            // Get the text values of Meal1Name to Meal6Name entries
            string meal1Name = Meal1Name.Text;
            string meal2Name = Meal2Name.Text;
            string meal3Name = Meal3Name.Text;
            string meal4Name = Meal4Name.Text;
            string meal5Name = Meal5Name.Text;
            string meal6Name = Meal6Name.Text;

            // Update the MealNames database
            await _localData.UpdateMealName(1, meal1Name, _date);
            await _localData.UpdateMealName(2, meal2Name, _date);
            await _localData.UpdateMealName(3, meal3Name, _date);
            await _localData.UpdateMealName(4, meal4Name, _date);
            await _localData.UpdateMealName(5, meal5Name, _date);
            await _localData.UpdateMealName(6, meal6Name, _date);

            await _localData.UpdateMealNumber(mealLayout, _date);

            var userID = await _localData.GetUserID();
            var loggedFoods = await _localData.GetLoggedFoods(userID, _date);

            switch (mealLayout)
            {
                case 1:
                    var more1Foods = loggedFoods.Where(x => x.MealType > 1);
                    foreach (var food in more1Foods)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 1);
                    }
                    break;

                case 2:
                    var more2Foods = loggedFoods.Where(x => x.MealType > 2);
                    foreach (var food in more2Foods)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 2);
                    }
                    break;

                case 3:
                    var more3Foods = loggedFoods.Where(x => x.MealType > 3);
                    foreach (var food in more3Foods)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 3);
                    }
                    break;

                case 4:
                    var more4Foods1 = loggedFoods.Where(x => x.MealType > 4);
                    foreach (var food in more4Foods1)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 4);
                    }
                    break;
                case 4.1:
                    var more4Foods2 = loggedFoods.Where(x => x.MealType > 4);
                    foreach (var food in more4Foods2)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 4);
                    }
                    break;
                case 4.2:
                    var more4Foods3 = loggedFoods.Where(x => x.MealType > 4);
                    foreach (var food in more4Foods3)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 4);
                    }
                    break;

                case 5:
                    var more5Foods1 = loggedFoods.Where(x => x.MealType > 5);
                    foreach (var food in more5Foods1)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 5);
                    }
                    break;
                case 5.1:
                    var more5Foods2 = loggedFoods.Where(x => x.MealType > 5);
                    foreach (var food in more5Foods2)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 5);
                    }
                    break;
                case 5.2:
                    var more5Foods3 = loggedFoods.Where(x => x.MealType > 5);
                    foreach (var food in more5Foods3)
                    {
                        await _localData.UpdateLoggedFoodMeal(food.LoggedFoodID, 5);
                    }
                    break;
            }


        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        await MopupService.Instance.PopAsync();
        _sender.PopulateMealNames();
    }

    async void Meal1Button_Clicked(System.Object sender, System.EventArgs e)
    {
        MealNumberLabel.Text = "1 Meal";

        mealLayout = 1;

        Meal1Button.Background = Color.FromRgb(75, 158, 227);
        Meal2Button.Background = Colors.Transparent;
        Meal3Button.Background = Colors.Transparent;
        Meal4Button.Background = Colors.Transparent;
        Meal5Button.Background = Colors.Transparent;
        Meal6Button.Background = Colors.Transparent;

        Meal1Prev.IsVisible = true;
        Meal2Prev.IsVisible = false;
        Meal3Prev.IsVisible = false;
        Meal4Prev.IsVisible = false;
        Meal5Prev.IsVisible = false;
        Meal6Prev.IsVisible = false;

        
        Meal2Label.FadeTo(0);
        Meal3Label.FadeTo(0);
        Meal4Label.FadeTo(0);
        Meal5Label.FadeTo(0);
        Meal6Label.FadeTo(0);

        Meal2Name.FadeTo(0);
        Meal3Name.FadeTo(0);
        Meal4Name.FadeTo(0);
        Meal5Name.FadeTo(0);
        await Meal6Name.FadeTo(0);
        Meal2Label.IsVisible = false;
        Meal2Name.IsVisible = false;
        Meal3Label.IsVisible = false;
        Meal3Name.IsVisible = false;
        Meal4Label.IsVisible = false;
        Meal4Name.IsVisible = false;
        Meal5Label.IsVisible = false;
        Meal5Name.IsVisible = false;
        Meal6Label.IsVisible = false;
        Meal6Name.IsVisible = false;
    }

    async void Meal2Button_Clicked(System.Object sender, System.EventArgs e)
    {
        MealNumberLabel.Text = "2 Meals";

        mealLayout = 2;

        Meal2Button.Background = Color.FromRgb(75, 158, 227);
        Meal1Button.Background = Colors.Transparent;
        Meal3Button.Background = Colors.Transparent;
        Meal4Button.Background = Colors.Transparent;
        Meal5Button.Background = Colors.Transparent;
        Meal6Button.Background = Colors.Transparent;

        Meal1Prev.IsVisible = false;
        Meal2Prev.IsVisible = true;
        Meal3Prev.IsVisible = false;
        Meal4Prev.IsVisible = false;
        Meal5Prev.IsVisible = false;
        Meal6Prev.IsVisible = false;

        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
       

        Meal2Label.FadeTo(1);
        Meal3Label.FadeTo(0);
        Meal4Label.FadeTo(0);
        Meal5Label.FadeTo(0);
        Meal6Label.FadeTo(0);

        Meal2Name.FadeTo(1);
        Meal3Name.FadeTo(0);
        Meal4Name.FadeTo(0);
        Meal5Name.FadeTo(0);
        await Meal6Name.FadeTo(0);
        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = false;
        Meal3Name.IsVisible = false;
        Meal4Label.IsVisible = false;
        Meal4Name.IsVisible = false;
        Meal5Label.IsVisible = false;
        Meal5Name.IsVisible = false;
        Meal6Label.IsVisible = false;
        Meal6Name.IsVisible = false;
    }

   async void Meal3Button_Clicked(System.Object sender, System.EventArgs e)
    {
        MealNumberLabel.Text = "3 Meals";

        mealLayout = 3;

        Meal3Button.Background = Color.FromRgb(75, 158, 227);
        Meal2Button.Background = Colors.Transparent;
        Meal1Button.Background = Colors.Transparent;
        Meal4Button.Background = Colors.Transparent;
        Meal5Button.Background = Colors.Transparent;
        Meal6Button.Background = Colors.Transparent;

        Meal1Prev.IsVisible = false;
        Meal2Prev.IsVisible = false;
        Meal3Prev.IsVisible = true;
        Meal4Prev.IsVisible = false;
        Meal5Prev.IsVisible = false;
        Meal6Prev.IsVisible = false;

        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = true;
        Meal3Name.IsVisible = true;
       

        Meal2Label.FadeTo(1);
        Meal3Label.FadeTo(1);
        Meal4Label.FadeTo(0);
        Meal5Label.FadeTo(0);
        Meal6Label.FadeTo(0);

        Meal2Name.FadeTo(1);
        Meal3Name.FadeTo(1);
        Meal4Name.FadeTo(0);
        Meal5Name.FadeTo(0);
        await Meal6Name.FadeTo(0);
        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = true;
        Meal3Name.IsVisible = true;
        Meal4Label.IsVisible = false;
        Meal4Name.IsVisible = false;
        Meal5Label.IsVisible = false;
        Meal5Name.IsVisible = false;
        Meal6Label.IsVisible = false;
        Meal6Name.IsVisible = false;
    }

    async void Meal4Button_Clicked(System.Object sender, System.EventArgs e)
    {
        MealNumberLabel.Text = "4 Meals";

        mealLayout = 4.2;

        Meal4Top.FadeTo(.25, 200);
        Meal4Mid.FadeTo(1, 150);
        Meal4Bottom.FadeTo(.25, 200);

       

        Meal4Button.Background = Color.FromRgb(75, 158, 227);
        Meal2Button.Background = Colors.Transparent;
        Meal3Button.Background = Colors.Transparent;
        Meal1Button.Background = Colors.Transparent;
        Meal5Button.Background = Colors.Transparent;
        Meal6Button.Background = Colors.Transparent;

        Meal1Prev.IsVisible = false;
        Meal2Prev.IsVisible = false;
        Meal3Prev.IsVisible = false;
        Meal4Prev.IsVisible = true;
        Meal5Prev.IsVisible = false;
        Meal6Prev.IsVisible = false;

        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = true;
        Meal3Name.IsVisible = true;
        Meal4Label.IsVisible = true;
        Meal4Name.IsVisible = true;
       
        Meal2Label.FadeTo(1);
        Meal3Label.FadeTo(1);
        Meal4Label.FadeTo(1);
        Meal5Label.FadeTo(0);
        Meal6Label.FadeTo(0);

        Meal2Name.FadeTo(1);
        Meal3Name.FadeTo(1);
        Meal4Name.FadeTo(1);
        Meal5Name.FadeTo(0);
        await Meal6Name.FadeTo(0);
        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = true;
        Meal3Name.IsVisible = true;
        Meal4Label.IsVisible = true;
        Meal4Name.IsVisible = true;
        Meal5Label.IsVisible = false;
        Meal5Name.IsVisible = false;
        Meal6Label.IsVisible = false;
        Meal6Name.IsVisible = false;
    }

    async void Meal5Button_Clicked(System.Object sender, System.EventArgs e)
    {
        MealNumberLabel.Text = "5 Meals";

        mealLayout = 5.2;

        Meal5Top.FadeTo(.25, 200);
        Meal5Mid.FadeTo(1, 150);
        Meal5Bottom.FadeTo(.25, 200);


        Meal5Button.Background = Color.FromRgb(75, 158, 227);
        Meal2Button.Background = Colors.Transparent;
        Meal3Button.Background = Colors.Transparent;
        Meal4Button.Background = Colors.Transparent;
        Meal1Button.Background = Colors.Transparent;
        Meal6Button.Background = Colors.Transparent;

        Meal1Prev.IsVisible = false;
        Meal2Prev.IsVisible = false;
        Meal3Prev.IsVisible = false;
        Meal4Prev.IsVisible = false;
        Meal5Prev.IsVisible = true;
        Meal6Prev.IsVisible = false;

        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = true;
        Meal3Name.IsVisible = true;
        Meal4Label.IsVisible = true;
        Meal4Name.IsVisible = true;
        Meal5Label.IsVisible = true;
        Meal5Name.IsVisible = true;
       

        Meal2Label.FadeTo(1);
        Meal3Label.FadeTo(1);
        Meal4Label.FadeTo(1);
        Meal5Label.FadeTo(1);
        Meal6Label.FadeTo(0);

        Meal2Name.FadeTo(1);
        Meal3Name.FadeTo(1);
        Meal4Name.FadeTo(1);
        Meal5Name.FadeTo(1);
        await Meal6Name.FadeTo(0);
        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = true;
        Meal3Name.IsVisible = true;
        Meal4Label.IsVisible = true;
        Meal4Name.IsVisible = true;
        Meal5Label.IsVisible = true;
        Meal5Name.IsVisible = true;
        Meal6Label.IsVisible = false;
        Meal6Name.IsVisible = false;
    }

    async void Meal6Button_Clicked(System.Object sender, System.EventArgs e)
    {
        MealNumberLabel.Text = "6 Meals";

        mealLayout = 6;

        Meal6Button.Background = Color.FromRgb(75, 158, 227);
        Meal2Button.Background = Colors.Transparent;
        Meal3Button.Background = Colors.Transparent;
        Meal4Button.Background = Colors.Transparent;
        Meal5Button.Background = Colors.Transparent;
        Meal1Button.Background = Colors.Transparent;

        Meal1Prev.IsVisible = false;
        Meal2Prev.IsVisible = false;
        Meal3Prev.IsVisible = false;
        Meal4Prev.IsVisible = false;
        Meal5Prev.IsVisible = false;
        Meal6Prev.IsVisible = true;

        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = true;
        Meal3Name.IsVisible = true;
        Meal4Label.IsVisible = true;
        Meal4Name.IsVisible = true;
        Meal5Label.IsVisible = true;
        Meal5Name.IsVisible = true;
        Meal6Label.IsVisible = true;
        Meal6Name.IsVisible = true;

        Meal2Label.FadeTo(1);
        Meal3Label.FadeTo(1);
        Meal4Label.FadeTo(1);
        Meal5Label.FadeTo(1);
        Meal6Label.FadeTo(1);
        Meal2Name.FadeTo(1);
        Meal3Name.FadeTo(1);
        Meal4Name.FadeTo(1);
        Meal5Name.FadeTo(1);
        await Meal6Name.FadeTo(1);
        Meal2Label.IsVisible = true;
        Meal2Name.IsVisible = true;
        Meal3Label.IsVisible = true;
        Meal3Name.IsVisible = true;
        Meal4Label.IsVisible = true;
        Meal4Name.IsVisible = true;
        Meal5Label.IsVisible = true;
        Meal5Name.IsVisible = true;
        Meal6Label.IsVisible = true;
        Meal6Name.IsVisible = true;

    }

    void Meal4Top_Clicked(System.Object sender, System.EventArgs e)
    {
        mealLayout = 4.1;

        Meal4Top.FadeTo(1, 50);
        Meal4Mid.FadeTo(.25, 100);
        Meal4Bottom.FadeTo(.25, 100);

    }

    void Meal4Mid_Clicked(System.Object sender, System.EventArgs e)
    {
        mealLayout = 4.2;

        Meal4Top.FadeTo(.5, 100);
        Meal4Mid.FadeTo(1, 50);
        Meal4Bottom.FadeTo(.5, 100);

    }

    void Meal4Bottom_Clicked(System.Object sender, System.EventArgs e)
    {
        mealLayout = 4;

        Meal4Top.FadeTo(.25, 100);
        Meal4Mid.FadeTo(.25, 100);
        Meal4Bottom.FadeTo(1, 50);

    }

    void Meal5Top_Clicked(System.Object sender, System.EventArgs e)
    {
        mealLayout = 5;

        Meal5Top.FadeTo(1, 100);
        Meal5Mid.FadeTo(.25, 50);
        Meal5Bottom.FadeTo(.25, 50);
    }

    void Meal5Mid_Clicked(System.Object sender, System.EventArgs e)
    {
        mealLayout = 5.2;

        Meal5Top.FadeTo(.25, 50);
        Meal5Mid.FadeTo(1, 100);
        Meal5Bottom.FadeTo(.25, 50);
    }

    void Meal5Bottom_Clicked(System.Object sender, System.EventArgs e)
    {
        mealLayout = 5.1;

        Meal5Top.FadeTo(.25, 50);
        Meal5Mid.FadeTo(.25, 50);
        Meal5Bottom.FadeTo(1, 100);

    }

    void Button_Clicked_1(System.Object sender, System.EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }


    void MealFocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
 

        MainGrid.TranslateTo(MainGrid.X, -355, 500, Easing.CubicOut);
        MealSelectFrame.FadeTo(0.25, 500, Easing.CubicInOut);

        CloseWhenBackgroundIsClicked = false;
    }

    void MealUnfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        MainGrid.TranslateTo(MainGrid.X, -25, 500, Easing.CubicOut);
        MealSelectFrame.FadeTo(1, 500, Easing.CubicInOut);

        CloseWhenBackgroundIsClicked = true;

        
    }
}
