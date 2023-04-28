using System;
using HealthModeApp.Models;
namespace HealthModeApp.DataServices
{
	public interface IRestDataService
	{

        Task<List<NutritionModel>> GetNutritionInfoNameAsync(string name, int limit = 50, int offset = 0);

        Task<NutritionModel> GetNutritionInfoBarcodeAsync(string barcode);

        Task AddNutritionInfoAsync(NutritionModel nutritionModel);
		Task UpdateNutritionInfoAsync(NutritionModel nutritionModel);
		Task DeleteNutritionInfoAsync(int foodId);

        Task<bool> AddUserAsync(string email, string username, string hashedPassword, string salt, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, int weight, int goalWeight, int activityLevel, int calorieGoal);
        Task DeleteUserAsync(int userId);
        Task UpdateUserInfoAsync(UserInfo userInfoModel);
        Task<string> CheckUserUniqueAsync(string email, string username);

        Task<(UserInfo, bool)> GetUserInfoOnLoginAsync(int userID);
        Task<string> GetSaltByEmailAsync(string email);
        Task<string> LoginAsync(string email, string hashedPassword);
        Task<int> GetUserIDByEmailAsync(string email);

        Task<bool> GetSeesAdsAsync(int userID);

        Task UpdateExpDateAsync(int userID, double hoursToAdd);


    }
}

