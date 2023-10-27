namespace HealthModeApp.CustomControls;

public partial class MacroSelector : ContentView
{
	public MacroSelector()
	{
		InitializeComponent();
	}

    Dictionary<string, Dictionary<string, string>> nutrientTranslations = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "Carbs", new Dictionary<string, string>
                    {
                        {"English", "Carbs"},
                        {"Spanish", "Carbos"},
                        {"Chinese", "碳水化合物"},
                        {"French", "Glucides"}
                    }
                },
                {
                    "Fat", new Dictionary<string, string>
                    {
                        {"English", "Fat"},
                        {"Spanish", "Grasa"},
                        {"Chinese", "脂肪"},
                        {"French", "Lipide"}
                    }
                },
                {
                    "Protein", new Dictionary<string, string>
                    {
                        {"English", "Protein"},
                        {"Spanish", "Proteína"},
                        {"Chinese", "蛋白质"},
                        {"French", "Protéine"}
                    }
                }
            };

    List<string> languages = new List<string> { "English", "Spanish", "Chinese", "French" };
    int currentIndex = 0;

    async void MacroSelectorTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		await BaseBorder.FadeTo(0.5, 100, Easing.CubicInOut);
        await BaseBorder.FadeTo(1, 100, Easing.CubicInOut);

        // Get the next language
        string nextLanguage = languages[currentIndex];
        currentIndex = (currentIndex + 1) % languages.Count;

        // Update text based on the next language
        CarbIcon.Text = nutrientTranslations["Carbs"][nextLanguage];
        FatIcon.Text = nutrientTranslations["Fat"][nextLanguage];
        ProteinIcon.Text = nutrientTranslations["Protein"][nextLanguage];

    }


}
