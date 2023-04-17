using System;
using HealthModeApp.Models;
namespace HealthModeApp.DataServices
{
	public interface IRestDataService
	{

		Task<List<NutritionModel>> GetAllNutritionInfoAsync();
		Task AddNutritionInfoAsync(NutritionModel nutritionModel);
		Task UpdateNutritionInfoAsync(NutritionModel nutritionModel);
		Task DeleteNutritionInfoAsync(int foodId);

        Task<bool> AddUserAsync(string email, string username, string hashedPassword, string salt, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, int weight, int goalWeight, int activityLevel);
        Task DeleteUserAsync(int userId);
        Task UpdateUserInfoAsync(UserInfo userInfoModel);
        Task<string> CheckUserUniqueAsync(string email, string username);

        Task<(UserInfo, bool)> GetUserInfoOnLoginAsync(int userID);
        Task<string> GetSaltByEmailAsync(string email);
        Task<string> LoginAsync(string email, string hashedPassword);
        Task<int> GetUserIDByEmailAsync(string email);

    }
}

