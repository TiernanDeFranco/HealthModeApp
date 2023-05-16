using HealthModeApp.DataServices;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class FoodJournalSettings : ContentPage
{
	private readonly ISQLiteDataService _localData;

	public FoodJournalSettings(ISQLiteDataService localData)
	{
		InitializeComponent();
       
		_localData = localData;
        PopulateCurrentNames();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);
        NavigationPage.SetHasNavigationBar(this, false);

    }

    async void PopulateCurrentNames()
        {
        var mealNames = await _localData.GetMealNames();

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
            await _localData.UpdateMealName(1, meal1Name);
            await _localData.UpdateMealName(2, meal2Name);
            await _localData.UpdateMealName(3, meal3Name);
            await _localData.UpdateMealName(4, meal4Name);
            await _localData.UpdateMealName(5, meal5Name);
            await _localData.UpdateMealName(6, meal6Name);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
         await Navigation.PopModalAsync();
    }
}
