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

	}
}

