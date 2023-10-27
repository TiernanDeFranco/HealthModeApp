using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.Pages.FoodJournalPage;

public partial class MealMicroBreakdown
{
	decimal totalSugar;
    decimal totalFiber;
	decimal totalSatFat;
	decimal totalPUnSatFat;
	decimal totalMUnSatFat;
	decimal totalTransFat;
	decimal totalCholesterol;
	decimal totalSodium;
	decimal totalCalcium;
	decimal totalIron;
	decimal totalPotassium;

	decimal totalVitaminA;
	decimal totalVitaminC;
	decimal totalVitaminD;
	decimal totalVitaminE;
	decimal totalVitaminK;

	decimal totalThiamin;
	decimal totalRiboflavin;
	decimal totalNiacin;
	decimal totalB5;
	decimal totalB6;
	decimal totalBiotin;
	decimal totalFolate;
	decimal totalB12;

	public MealMicroBreakdown(List<LoggedFoodTable> loggedFoods)
	{
		InitializeComponent();

		foreach (var food in loggedFoods)
		{
            totalSugar += food.Sugar.HasValue ? food.Sugar.Value : 0;
            totalFiber += food.Fiber.HasValue ? food.Fiber.Value : 0;
            totalSatFat += food.SatFat.HasValue ? food.SatFat.Value : 0;
            totalPUnSatFat += food.PUnSatFat.HasValue ? food.PUnSatFat.Value : 0;
            totalMUnSatFat += food.MUnSatFat.HasValue ? food.MUnSatFat.Value : 0;
            totalTransFat += food.TransFat.HasValue ? food.TransFat.Value : 0;
            totalCholesterol += food.Cholesterol.HasValue ? food.Cholesterol.Value : 0;
            totalSodium += food.Sodium.HasValue ? food.Sodium.Value : 0;
            totalCalcium += food.Calcium.HasValue ? food.Calcium.Value : 0;
            totalIron += food.Iron.HasValue ? food.Iron.Value : 0;
            totalPotassium += food.Potassium.HasValue ? food.Potassium.Value : 0;

            totalVitaminA += food.VitaminA.HasValue ? food.VitaminA.Value : 0;
            totalVitaminC += food.VitaminC.HasValue ? food.VitaminC.Value : 0;
            totalVitaminD += food.VitaminD.HasValue ? food.VitaminD.Value : 0;
            totalVitaminE += food.VitaminE.HasValue ? food.VitaminE.Value : 0;
            totalVitaminK += food.VitaminK.HasValue ? food.VitaminK.Value : 0;

            totalThiamin += food.Thiamin.HasValue ? food.Thiamin.Value : 0;
            totalRiboflavin += food.Riboflavin.HasValue ? food.Riboflavin.Value : 0;
            totalNiacin += food.Niacin.HasValue ? food.Niacin.Value : 0;
            totalB5 += food.B5.HasValue ? food.B5.Value : 0;
            totalB6 += food.B6.HasValue ? food.B6.Value : 0;
            totalBiotin += food.B7.HasValue ? food.B7.Value : 0;  // Assuming B7 maps to Biotin
            totalFolate += food.FolicAcid.HasValue ? food.FolicAcid.Value : 0;  // Assuming FolicAcid maps to Folate
            totalB12 += food.B12.HasValue ? food.B12.Value : 0;
        }

        SugarLabel.Text = totalSugar.ToString("0.#") + "g";
        FiberLabel.Text = totalFiber.ToString("0.#") + "g";
        SatFatLabel.Text = totalSatFat.ToString("0.#") + "g";
        PUnSatFatLabel.Text = totalPUnSatFat.ToString("0.#") + "g";
        MUnSatFatLabel.Text = totalMUnSatFat.ToString("0.#") + "g";
        TransFatLabel.Text = totalTransFat.ToString("0.#") + "g";
        CholesterolLabel.Text = totalCholesterol.ToString("0.#") + "mg";
        SodiumLabel.Text = totalSodium.ToString("0.#") + "mg";
        CalciumLabel.Text = totalCalcium.ToString("0.#") + "mg";
        IronLabel.Text = totalIron.ToString("0.#") + "mg";
        PotassiumLabel.Text = totalPotassium.ToString("0.#") + "mg";

        VitaminALabel.Text = totalVitaminA.ToString("0.#") + "µg";
        VitaminCLabel.Text = totalVitaminC.ToString("0.#") + "mg";
        VitaminDLabel.Text = totalVitaminD.ToString("0.#") + "µg";
        VitaminELabel.Text = totalVitaminE.ToString("0.#") + "mg";
        VitaminKLabel.Text = totalVitaminK.ToString("0.#") + "µg";

        ThiaminLabel.Text = totalThiamin.ToString("0.#") + "mg";
        RiboflavinLabel.Text = totalRiboflavin.ToString("0.#") + "mg";
        NiacinLabel.Text = totalNiacin.ToString("0.#") + "mg";
        B5Label.Text = totalB5.ToString("0.#") + "mg";
        B6Label.Text = totalB6.ToString("0.#") + "mg";
        BiotinLabel.Text = totalBiotin.ToString("0.#") + "µg";
        FolateLabel.Text = totalFolate.ToString("0.#") + "µg";
        CobalaminLabel.Text = totalB12.ToString("0.#") + "µg";


    }
}
