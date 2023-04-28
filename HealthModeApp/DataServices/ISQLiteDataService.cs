using HealthModeApp.Models.SQLite;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthModeApp.DataServices
{
    public interface ISQLiteDataService
    {
      

        Task AddCustomFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK);
        Task UpdateCustomFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK);
        Task RemoveCustomFood(int foodID);
        Task<IEnumerable<CustomFoods>> GetCustomFoods();


        Task<IEnumerable<LoggedFoodTable>> GetLoggedFoods(int userID, DateTime selectedDate);
        Task<LoggedFoodTable> GetLoggedFoodDetails(int userID, int loggedFoodID, string page);
        Task<IEnumerable<LoggedFoodTable>> GetLoggedFoodID(int userID, DateTime selectedDate);
        Task RemoveLoggedFood(int loggedFoodID);
        Task AddLoggedFood(int userID, DateTime date, int mealType, DateTime time, decimal servingAmount, decimal totalGrams, int foodID, string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK);
        Task UpdateLoggedFood(int userID, DateTime date, int mealType, DateTime time, decimal servingAmount, decimal totalGrams, int foodID, string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK);

        Task<List<string>> GetMealNames();
        Task AddMealName(MealNames mealName);
        Task UpdateMealName(int mealID, string mealName);

        Task<bool> GetPopUpSeen(string popupName);
        Task AddPopUpSeen(string popupName, bool popupSeen);

        Task<int> GetUserID();
        Task<UserData> GetUserAsync(int userID);

        Task AddUserAsync(int userID, string email, string username, string password, bool seesAds, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, int weight, int goalWeight, int activityLevel);
        Task UpdateUserAsync(int userID, string email = null, string username = null, string password = null, bool? seesAds = null, int? weightPlan = null, string mainGoals = null, string units = null, int? sex = null, decimal? heightCm = null, DateTime? birthday = null, int? weight = null, int? goalWeight = null, int? activityLevel = null);
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
            int vitaminDGoal, int vitaminEGoal, int vitaminKGoal);
        Task UpdateNutritionGoals(int userID, DateTime date, int? calorieGoal, int? carbGoal, int? fatGoal, int? proteinGoal,
                int? satdFatGoal, int? pUnSatFatGoal, int? mUnSatFatGoal, int? transFatGoal, int? sugarGoal,
                int? ironGoal, int? calciumGoal, int? potassiumGoal, int? sodiumGoal, int? cholesterolGoal,
                int? vitaminAGoal, int? thiaminGoal, int? riboflavinGoal, int? niacinGoal, int? vitaminB5Goal,
                int? vitaminB6Goal, int? biotinGoal, int? cobalamineGoal, int? folicAcidGoal, int? vitaminCGoal,
                int? vitaminDGoal, int? vitaminEGoal, int? vitaminKGoal);

    }
}
