using HealthModeApp.Models.SQLite;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthModeApp.DataServices
{
    public interface ISQLiteDataService
    {
        Task AddTranslation(string serializedList);
        Task<string> GetTranslationByKey(string key);

        Task AddCustomFood(CustomFoods food);
        Task UpdateCustomFood(string barcode, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK, int mealType, string category);
        Task RemoveCustomFood(string barcodeValue);
        Task<List<CustomFoods>> GetCustomFoods();
        Task<CustomFoods> GetCustomFoodByBarcode(string barcode);
        Task<List<CustomFoods>> GetCustomFoodByName(string name);

        Task<List<CustomFoods>> GetRecentFoods();
        Task AddOrUpdateRecentFood(string serializedFood);


        Task<IEnumerable<LoggedFoodTable>> GetLoggedFoods(int userID, DateTime selectedDate);
        Task<LoggedFoodTable> GetLoggedFoodDetails(int userID, int loggedFoodID);
        Task<IEnumerable<LoggedFoodTable>> GetLoggedFoodID(int userID, DateTime selectedDate);
        Task RemoveLoggedFood(int loggedFoodID);
        Task AddLoggedFood(int userID, DateTime date, int mealType, DateTime time, int servingSizeSelected, decimal servingAmount, decimal totalGrams, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal? calories, decimal? carbs, decimal? sugar, decimal? addSugar, decimal? sugarAlc, decimal? fiber, decimal? netCarb, decimal? fat, decimal? satFat, decimal? pUnSatFat, decimal? mUnSatFat, decimal? transFat, decimal? protein, decimal? iron, decimal? calcium, decimal? potassium, decimal? sodium, decimal? cholesterol, decimal? vitaminA, decimal? thiamin, decimal? riboflavin, decimal? niacin, decimal? b5, decimal? b6, decimal? b7, decimal? folicAcid, decimal? b12, decimal? vitaminC, decimal? vitaminD, decimal? vitaminE, decimal? vitaminK, bool verified);
        Task UpdateLoggedFood(int loggedFoodID, int userID, DateTime date, int mealType, DateTime time, int servingSizeSelected, decimal servingAmount, decimal totalGrams, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal? calories, decimal? carbs, decimal? sugar, decimal? addSugar, decimal? sugarAlc, decimal? fiber, decimal? netCarb, decimal? fat, decimal? satFat, decimal? pUnSatFat, decimal? mUnSatFat, decimal? transFat, decimal? protein, decimal? iron, decimal? calcium, decimal? potassium, decimal? sodium, decimal? cholesterol, decimal? vitaminA, decimal? thiamin, decimal? riboflavin, decimal? niacin, decimal? b5, decimal? b6, decimal? b7, decimal? folicAcid, decimal? b12, decimal? vitaminC, decimal? vitaminD, decimal? vitaminE, decimal? vitaminK);
        Task UpdateLoggedFoodMeal(int loggedFoodID, int mealNum);
        Task<List<string>> GetMealNames(DateTime date);
        Task AddMealName(MealNames mealName, int mealNum, DateTime date);
        Task UpdateMealName(int mealNum, string mealName, DateTime date);

        Task<double> SetMealNumber(DateTime date);
        Task UpdateMealNumber(double mealNumber, DateTime date);

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

        Task UpdatePFP(string pfpPath, string hexCode);
        Task UpdateFlair(string flairName, string flairColor, bool isBlackText);

        Task<string> GetFoodEnergyUnit();



        Task<NutritionGoals> GetNutritionGoals(int userID, DateTime date);
        Task<bool> NutritionGoalDateExists(int userID);
        Task AddNutritionGoals(int userID, DateTime date, int calorieGoal, int carbGoal, int fatGoal, int proteinGoal,
            int satdFatGoal, int pUnSatFatGoal, int mUnSatFatGoal, int transFatGoal, int sugarGoal, int fiberGoal,
            int ironGoal, int calciumGoal, int potassiumGoal, int sodiumGoal, int cholesterolGoal,
            int vitaminAGoal, int thiaminGoal, int riboflavinGoal, int niacinGoal, int vitaminB5Goal,
            int vitaminB6Goal, int biotinGoal, int cobalamineGoal, int folicAcidGoal, int vitaminCGoal,
            int vitaminDGoal, int vitaminEGoal, int vitaminKGoal);
        Task UpdateNutritionGoals(int userID, DateTime date, int? calorieGoal, int? carbGoal, int? fatGoal, int? proteinGoal,
                int? satdFatGoal, int? pUnSatFatGoal, int? mUnSatFatGoal, int? transFatGoal, int? sugarGoal, int? fiberGoal,
                int? ironGoal, int? calciumGoal, int? potassiumGoal, int? sodiumGoal, int? cholesterolGoal,
                int? vitaminAGoal, int? thiaminGoal, int? riboflavinGoal, int? niacinGoal, int? vitaminB5Goal,
                int? vitaminB6Goal, int? biotinGoal, int? cobalamineGoal, int? folicAcidGoal, int? vitaminCGoal,
                int? vitaminDGoal, int? vitaminEGoal, int? vitaminKGoal);

        Task ChangeMicroPercentGoals(int userID, DateTime dateSet, MicroPercentageGoals newGoal);
        Task<MicroPercentageGoals> GetMicroPercentages(int userID, DateTime dateSet);


        Task<bool> GetWeightEntryExists(int userID, DateTime date);
        Task AddWeightEntry(int userID, DateTime date, decimal weight, byte[]? progress);
        Task AddBodyFatEntry(int userID, DateTime date, decimal bodyFat);
        Task AddNeckEntry(int userID, DateTime date, decimal neckCirc);
        Task AddChestEntry(int userID, DateTime date, decimal chestCirc);
        Task AddArmsEntry(int userID, DateTime date, decimal armsCirc);
        Task AddWaistEntry(int userID, DateTime date, decimal waistCirc);
        Task AddHipsEntry(int userID, DateTime date, decimal hipsCirc);
        Task AddThighsEntry(int userID, DateTime date, decimal thighsCirc);
        Task AddCalvesEntry(int userID, DateTime date, decimal calvesCirc);

        Task UpdateWeightEntry(int userID, DateTime date, decimal weight, byte[]? progress);
        Task UpdateBodyFatEntry(int userID, DateTime date, decimal bodyFat);
        Task UpdateNeckEntry(int userID, DateTime date, decimal neckCirc);
        Task UpdateChestEntry(int userID, DateTime date, decimal chestCirc);
        Task UpdateArmsEntry(int userID, DateTime date, decimal armsCirc);
        Task UpdateWaistEntry(int userID, DateTime date, decimal waistCirc);
        Task UpdateHipsEntry(int userID, DateTime date, decimal hipsCirc);
        Task UpdateThighsEntry(int userID, DateTime date, decimal thighsCirc);
        Task UpdateCalvesEntry(int userID, DateTime date, decimal calvesCirc);

        Task<decimal> GetWeight(int userID, DateTime date);
        Task<decimal> GetBodyFat(int userID, DateTime date);
        Task<decimal> GetNeck(int userID, DateTime date);
        Task<decimal> GetChest(int userID, DateTime date);
        Task<decimal> GetArms(int userID, DateTime date);
        Task<decimal> GetWaist(int userID, DateTime date);
        Task<decimal> GetHips(int userID, DateTime date);
        Task<decimal> GetThighs(int userID, DateTime date);
        Task<decimal> GetCalves(int userID, DateTime date);

        Task<List<WeightTable>> GetWeightEntries(int userID);
        Task<List<WeightTable>> GetWeightsIndex(int userID, int index);
        Task<List<WeightTable>> GetBodyFatIndex(int userID, int index);
        Task<List<WeightTable>> GetNeckIndex(int userID, int index);
        Task<List<WeightTable>> GetChestIndex(int userID, int index);
        Task<List<WeightTable>> GetArmsIndex(int userID, int index);
        Task<List<WeightTable>> GetWaistIndex(int userID, int index);
        Task<List<WeightTable>> GetHipsIndex(int userID, int index);
        Task<List<WeightTable>> GetThighsIndex(int userID, int index);
        Task<List<WeightTable>> GetCalvesIndex(int userID, int index);

        Task<bool> DoesWeightEntryExist(int userID);
        Task DeleteWeightEntry(int weightID);
        Task<bool> GetWeightForDay(int userID, DateTime date);

        Task AddWaterEntry(int userID, DateTime date, decimal volume);
        Task UpdateWaterEntry(int waterID, decimal volume);
        Task DeleteWaterEntry(int waterID);
        Task<List<WaterTable>> GetWaterEntries(int userID, DateTime selectedDate);

        Task AddWaterGoal(int userID, DateTime dateSet, int waterGoalML);
        Task UpdateWaterGoal(DateTime dateSet, int waterGoalML);
        Task<int> GetWaterGoal(DateTime date);
        Task<bool> DoesWaterGoalExist(int userID);
    }
}
