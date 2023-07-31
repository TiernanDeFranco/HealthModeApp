using System;
using HealthModeApp.Models;
namespace HealthModeApp.DataServices
{
	public interface IRestDataService
	{
        Task PasswordRecovery(string email);
        Task<string> CodeVerification(string email, string recoveryCode);
        Task PasswordChange(string email, string recoveryCode, string newPassword);

        Task<List<NutritionModel>> GetNutritionInfoNameAsync(string name, int limit = 50, int offset = 0);

        Task<NutritionModel> GetNutritionInfoBarcodeAsync(string barcode);
        Task<NutritionModel> GetFoodUploadExistsAlready(string barcode);

        Task AddNutritionInfoAsync(NutritionModel nutritionModel, string email, string password, int userID);

        Task<bool> AddUserAsync(string email, string username, string hashedPassword, string salt, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, decimal weight, decimal goalWeight, int activityLevel, int calorieGoal, int waterGoal); 
        Task DeleteUserAsync(int userID, string email, string password);
        Task UpdateUserInfoAsync(UserInfo userInfoModel, string email, string password, int userID);
        Task<string> CheckUserUniqueAsync(string email, string username);

        Task<(UserInfo, bool, string, string, bool, string)> GetUserInfoOnLoginAsync(int userID, string email, string password);
        Task<string> GetSaltByEmailAsync(string email);
        Task<string> LoginAsync(string email, string hashedPassword);
        Task<int> GetUserIDByEmailAsync(string email);

        Task<bool> GetSeesAdsAsync(int userID);

        Task UpdateExpDateAsync(string email, int userID, double hoursToAdd);

        Task<List<ExerciseModel>> GetExercises(string name, int limit = 50, int offset = 0);
    }
}

