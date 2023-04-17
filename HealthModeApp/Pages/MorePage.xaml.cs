using System.Diagnostics;
using HealthModeApp.DataServices;

namespace HealthModeApp.Pages;

public partial class MorePage : ContentPage
{
    private readonly IRestDataService _dataService;
    public readonly ISQLiteDataService _localData;

    public MorePage()
    {
        InitializeComponent();

    }

    public MorePage(IRestDataService dataService, ISQLiteDataService localData)
    {
        InitializeComponent();
        _dataService = dataService;
        _localData = localData;
      
    }

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmation", "Are you sure you want to logout?", "Yes", "No");
       
        if (answer == true)
        {
         
            await _localData.DeleteUser();
            await Navigation.PushModalAsync(new LoginPage(_dataService, _localData));
            // In your page code behind, you can access the TabBar like this:
            var shell = Shell.Current;
            var tabBar = shell.FindByName<TabBar>("TabBar");

            // To change the selected tab, you can set the CurrentItem property of the TabBar:
            tabBar.CurrentItem = tabBar.Items[2];
           
        }

        

    }

}
