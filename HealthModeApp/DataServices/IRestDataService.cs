using System;
using HealthModeApp.Models;
namespace HealthModeApp.DataServices
{
	public interface IRestDataService
	{
        Task<string> GetTranslations(int newLanguage);

        Task PasswordRecovery(string email, int language);
        Task<string> CodeVerification(string email, string recoveryCode);
        Task PasswordChange(string email, string recoveryCode, string newPassword);
        Task<bool> InternalPasswordChange(string email, string oldPassword, string newPassword);

        Task<List<NutritionModel>> GetNutritionInfoNameAsync(string name, int limit = 50, int offset = 0);

        Task<NutritionModel> GetNutritionInfoBarcodeAsync(string barcode);
        Task<NutritionModel> GetFoodUploadExistsAlready(string barcode);

        Task AddNutritionInfoAsync(NutritionModel nutritionModel, string email, string password, int userID);

        Task<bool> AddUserAsync(string email, string username, string hashedPassword, string salt, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, decimal weight, decimal goalWeight, int activityLevel, int calorieGoal, int waterGoal); 
        Task DeleteUserAsync(int userID, string email, string password);
        Task<bool> UpdateUserInfoAsync(UserInfo userInfoModel, string email, string password, int userID);
        Task<string> UpdateUsername(string email, string password, string newUsername);
        Task<string> CheckUserUniqueAsync(string email, string username);

        Task<(UserInfo, bool, string, string, bool, string)> GetUserInfoOnLoginAsync(int userID, string email, string password);
        Task<string> GetSaltByEmailAsync(string email);
        Task<(string, string)> LoginAsync(string email, string hashedPassword);
        Task<int> GetUserIDByEmailAsync(string email);


        Task<int> GetPfpID(int userID);
        Task<string> GetPfpPath(int pfpID);
        Task<List<ProfilePictures>> GetPfpList(int userID);
        Task<List<FlairTable>> GetFlairList(int userID);

        Task<bool> GetSeesAdsAsync(int userID);

        Task UpdateExpDateAsync(string email, int userID, double hoursToAdd);

        Task<List<URLModel>> GetLink(int linkCategory);

        Task<List<ExerciseModel>> GetExercises(string name, int limit = 50, int offset = 0);
    }
}

