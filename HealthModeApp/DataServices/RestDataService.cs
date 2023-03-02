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
            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5209" : "https://localhost:7002";
            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }


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

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/healthmode", content);

                if(response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully added to database");
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

        public Task DeleteNutritionInfoAsync(int foodId)
        {
            throw new NotImplementedException();
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
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/healthmode");

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

        public Task UpdateNutritionInfoAsync(NutritionModel nutritionModel)
        {
            throw new NotImplementedException();
        }
    }
}

