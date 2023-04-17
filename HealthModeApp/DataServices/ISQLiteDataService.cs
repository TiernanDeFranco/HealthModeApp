using HealthModeApp.Models.SQLite;
using static HealthModeApp.Models.SQLite.SQLiteTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthModeApp.DataServices
{
    public interface ISQLiteDataService
    {
      

        Task AddBaseFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK);
        Task UpdateBaseFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK);
        Task RemoveBaseFood(int foodID);
        Task<IEnumerable<FoodBaseTable>> GetFood();


        Task<IEnumerable<LoggedFoodTable>> GetLoggedFoods(int userID, DateTime selectedDate);
        Task<LoggedFoodTable> GetLoggedFoodDetails(int userID, int loggedFoodID, string page);
        Task<IEnumerable<LoggedFoodTable>> GetLoggedFoodID(int userID, DateTime selectedDate);
        Task RemoveLoggedFood(int loggedFoodID);
        Task AddLoggedFood(int userID, DateTime date, int mealType, DateTime time, decimal servingAmount, decimal totalGrams, int foodID, string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK);
        Task UpdateLoggedFood(int userID, DateTime date, int mealType, DateTime time, decimal servingAmount, decimal totalGrams, int foodID, string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK);

        Task<bool> GetPopUpSeen(string popupName);
        Task AddPopUpSeen(string popupName, bool popupSeen);

        Task<int> GetUserID();
        Task AddUserAsync(int userID, string email, string username, string password, bool seesAds, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, int weight, int goalWeight, int activityLevel);
        Task UpdateUserAsync(int userID, string email, string username, string password, bool seesAds, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, int weight, int goalWeight, int activityLevel);
        Task DeleteUser();
        Task<(string Email, string Password)> GetUserCredentials();


    }
}
