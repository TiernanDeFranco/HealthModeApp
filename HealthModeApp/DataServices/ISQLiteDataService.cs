using HealthModeApp.Models.SQLite;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthModeApp.DataServices
{
    public interface ISQLiteDataService
    {


        Task AddCustomFood(CustomFoods food);
        Task UpdateCustomFood(string barcode, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK, int mealType, string category);
        Task RemoveCustomFood(string barcodeValue);
        Task<List<CustomFoods>> GetCustomFoods();
        Task<CustomFoods> GetCustomFoodByBarcode(string barcode);
        Task<List<CustomFoods>> GetCustomFoodByName(string name);


        Task<IEnumerable<LoggedFoodTable>> GetLoggedFoods(int userID, DateTime selectedDate);
        Task<LoggedFoodTable> GetLoggedFoodDetails(int userID, int loggedFoodID);
        Task<IEnumerable<LoggedFoodTable>> GetLoggedFoodID(int userID, DateTime selectedDate);
        Task RemoveLoggedFood(int loggedFoodID);
        Task AddLoggedFood(int userID, DateTime date, int mealType, TimeSpan time, int servingSizeSelected, decimal servingAmount, decimal totalGrams, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal? calories, decimal? carbs, decimal? sugar, decimal? addSugar, decimal? sugarAlc, decimal? fiber, decimal? netCarb, decimal? fat, decimal? satFat, decimal? pUnSatFat, decimal? mUnSatFat, decimal? transFat, decimal? protein, decimal? iron, decimal? calcium, decimal? potassium, decimal? sodium, decimal? cholesterol, decimal? vitaminA, decimal? thiamin, decimal? riboflavin, decimal? niacin, decimal? b5, decimal? b6, decimal? b7, decimal? folicAcid, decimal? b12, decimal? vitaminC, decimal? vitaminD, decimal? vitaminE, decimal? vitaminK);
        Task UpdateLoggedFood(int loggedFoodID, int userID, DateTime date, int mealType, TimeSpan time, int servingSizeSelected, decimal servingAmount, decimal totalGrams, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal? calories, decimal? carbs, decimal? sugar, decimal? addSugar, decimal? sugarAlc, decimal? fiber, decimal? netCarb, decimal? fat, decimal? satFat, decimal? pUnSatFat, decimal? mUnSatFat, decimal? transFat, decimal? protein, decimal? iron, decimal? calcium, decimal? potassium, decimal? sodium, decimal? cholesterol, decimal? vitaminA, decimal? thiamin, decimal? riboflavin, decimal? niacin, decimal? b5, decimal? b6, decimal? b7, decimal? folicAcid, decimal? b12, decimal? vitaminC, decimal? vitaminD, decimal? vitaminE, decimal? vitaminK);
        Task<List<string>> GetMealNames();
        Task AddMealName(MealNames mealName);
        Task UpdateMealName(int mealID, string mealName);

        Task<bool> GetPopUpSeen(string popupName);
        Task AddPopUpSeen(string popupName, bool popupSeen);


        Task<bool> DoesUserTableExist();

        Task<int> GetUserID();
        Task<UserData> GetUserAsync(int userID);

        Task AddUserAsync(int userID, string email, string username, string password, bool seesAds, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, decimal weight, decimal goalWeight, int activityLevel, string suffix, string suffixColor, bool isBlackText, string pictureBG, string picturePath, string title);
        Task UpdateUserAsync(int userID, string email = null, string username = null, string password = null, bool? seesAds = null, int? weightPlan = null, string mainGoals = null, string units = null, int? sex = null, decimal? heightCm = null, DateTime? birthday = null, decimal? weight = null, decimal? goalWeight = null, int? activityLevel = null, string? suffix = null, string? suffixColor = null, bool? isBlackText = null, string pictureBG = null, string picturePath = null, string title = null);
        Task DeleteUser();
        Task<(string Email, string Password)> GetUserCredentials();

        Task<bool> GetSeesAds();
        Task UpdateSeesAdsAsync(bool seesAds);

        Task<string> GetFoodEnergyUnit();



        Task<NutritionGoals> GetNutritionGoals(int userID, DateTime date);
        Task<bool> NutritionGoalDateExists(int userID, DateTime date);
        Task AddNutritionGoals(int userID, DateTime date, int calorieGoal, int carbGoal, int fatGoal, int proteinGoal,
            int satdFatGoal, int pUnSatFatGoal, int mUnSatFatGoal, int transFatGoal, int sugarGoal,
            int ironGoal, int calciumGoal, int potassiumGoal, int sodiumGoal, int cholesterolGoal,
            int vitaminAGoal, int thiaminGoal, int riboflavinGoal, int niacinGoal, int vitaminB5Goal,
            int vitaminB6Goal, int biotinGoal, int cobalamineGoal, int folicAcidGoal, int vitaminCGoal,
            int vitaminDGoal, int vitaminEGoal, int vitaminKGoal, int water);
        Task UpdateNutritionGoals(int userID, DateTime date, int? calorieGoal, int? carbGoal, int? fatGoal, int? proteinGoal,
                int? satdFatGoal, int? pUnSatFatGoal, int? mUnSatFatGoal, int? transFatGoal, int? sugarGoal,
                int? ironGoal, int? calciumGoal, int? potassiumGoal, int? sodiumGoal, int? cholesterolGoal,
                int? vitaminAGoal, int? thiaminGoal, int? riboflavinGoal, int? niacinGoal, int? vitaminB5Goal,
                int? vitaminB6Goal, int? biotinGoal, int? cobalamineGoal, int? folicAcidGoal, int? vitaminCGoal,
                int? vitaminDGoal, int? vitaminEGoal, int? vitaminKGoal, int? water);

        Task<bool> GetWeightEntryExists(int userID, DateTime date);
        Task AddWeightEntry(int userID, DateTime date, decimal weight, byte[]? progress);
        Task UpdateWeightEntry(int userID, DateTime date, decimal weight, byte[]? progress);

        Task<decimal> GetWeight(int userID, DateTime date);
        Task<List<WeightTable>> GetWeightEntries(int userID);
        Task<List<WeightTable>> GetWeightsIndex(int userID, int index);
        Task<bool> DoesWeightEntryExist(int userID);
        Task DeleteWeightEntry(int weightID);
        Task<bool> GetWeightForDay(int userID, DateTime date);

        Task AddWaterEntry(int userID, DateTime date, decimal volume);
        Task UpdateWaterEntry(int waterID, decimal volume);
        Task DeleteWaterEntry(int waterID);
        Task<List<WaterTable>> GetWaterEntries(int userID, DateTime selectedDate);


           }
}
