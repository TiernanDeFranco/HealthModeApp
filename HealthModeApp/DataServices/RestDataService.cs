using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using HealthModeApp.Models;

namespace HealthModeApp.DataServices
{
    public class RestDataService : IRestDataService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RestDataService()
        {
            _httpClient = new HttpClient();
            _baseAddress = "https://localhost:7002";
            _url = $"{_baseAddress}/api/healthmode";


            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

 #region NutritionInfoDBRegion
        public async Task AddNutritionInfoAsync(NutritionModel nutritionModel)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return;
            }

            try
            {
                string jsonNutrition = JsonSerializer.Serialize<NutritionModel>(nutritionModel, _jsonSerializerOptions);
               StringContent content = new StringContent(jsonNutrition, Encoding.UTF8, "application/json");

               

                //StringContent content = new StringContent("{\n\t\"barcode\": \"33\",\n\t\"foodName\": \"33\",\n\t\"servingSize\": \"25\",\n\t\"servingType\": \"g\",\n\t\"calories\": \"100\",\n\t\"protein\": \"6\",\n\t\"carbs\": \"3\",\n\t\"fat\": \"5\",\n\t\"satFat\": \"1.5\",\n\t\"cholesterol\": \"185\",\n\t\"sodium\": \"70\",\n\t\"calcium\": \"2\",\n\t\"iron\": \"4\",\n\t\"potassium\": \"70\",\n\t\"vitaminA\": \"6\",\n\t\"FolicAcid\": \"2\"\n}", Encoding.UTF8, "application/json");
                Debug.WriteLine("JSON Nutrition:");
                Debug.WriteLine(jsonNutrition);
            

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/food", content);


                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully added to database");
                }
                else
                {
                    Debug.WriteLine("-----> Non-Http 2xx Response StatusCode Failed");

                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Whoops, exception: {ex.Message}");
            }

            return;
        }

        public async Task DeleteNutritionInfoAsync(int foodId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/food/{foodId}");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully deleted entry");
                }
                else
                {
                    Debug.WriteLine("-----> Non-Http 2xx Response");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Whoops, exception: {ex.Message}");
            }

            return;
        }

        public async Task<List<NutritionModel>> GetAllNutritionInfoAsync()
        {
            List<NutritionModel> nutritionList = new List<NutritionModel>();
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return nutritionList;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/food");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    nutritionList = JsonSerializer.Deserialize<List<NutritionModel>>(content, _jsonSerializerOptions);
                }
                else
                {
                    Debug.WriteLine("-----> Non-Http 2xx Response");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Whoops, exception: {ex.Message}");

            }
            return nutritionList;
        }

        public async Task UpdateNutritionInfoAsync(NutritionModel nutritionModel)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return;
            }

            try
            {
                string jsonNutrition = JsonSerializer.Serialize<NutritionModel>(nutritionModel, _jsonSerializerOptions);
                StringContent content = new StringContent(jsonNutrition, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{_url}/healthmode/food/{nutritionModel.FoodId}", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully added to database");
                }
                else
                {
                    Debug.WriteLine("-----> Non-Http 2xx Response");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Whoops, exception: {ex.Message}");
            }

            return;
        }
     #endregion 
    }
}

