using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
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

        public class UserInfoResponse
        {
            public UserInfo UserInfo { get; set; }
            public bool SeesAds { get; set; }
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

        public async Task<bool> AddUserAsync(string email, string username, string hashedPassword, string salt, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, int weight, int goalWeight, int activityLevel)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return false;
            }

            try
            {
                UserInfo userInfo = new UserInfo()
                {
                    Email = email,
                    Username = username,
                    Password = hashedPassword,
                    Salt = salt,
                    WeightPlan = weightPlan,
                    MainGoals = mainGoals,
                    Units = units,
                    Sex = sex,
                    HeightCm = heightCm,
                    Birthday = birthday,
                    Weight = weight,
                    GoalWeight = goalWeight,
                    ActivityLevel = activityLevel
                };

                string jsonUser = JsonSerializer.Serialize<UserInfo>(userInfo, _jsonSerializerOptions);
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                Debug.WriteLine("JSON User:");
                Debug.WriteLine(jsonUser);

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/userinfo", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully added user to database");
                    return true;
                }
                else
                {
                    Debug.WriteLine("-----> Non-Http 2xx Response StatusCode Failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Whoops, exception: {ex.Message}");
                return false;
            }
        }



        public async Task DeleteUserAsync(int userId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/userinfo/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully deleted user");
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

        public async Task UpdateUserInfoAsync(UserInfo userInfoModel)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return;
            }

            try
            {
                string jsonUserInfo = JsonSerializer.Serialize<UserInfo>(userInfoModel, _jsonSerializerOptions);
                StringContent content = new StringContent(jsonUserInfo, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{_url}/healthmode/userinfo/{userInfoModel.UserID}", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully updated user information");
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
        }

       



        public async Task<string> CheckUserUniqueAsync(string email, string username)
        {
            string result = "AllClear";
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return result;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/userinfo/{email}/{username}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    var deserializedObject = JsonSerializer.Deserialize<Dictionary<string, string>>(content, _jsonSerializerOptions);

                    if (deserializedObject.TryGetValue("message", out string message))
                    {
                        result = message;
                    }
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

            return result;
        }

        public async Task<(UserInfo, bool)> GetUserInfoOnLoginAsync(int userID)
        {
            (UserInfo, bool) result = (null, true);
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return result;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/userinfo/userID/{userID}");

                
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var responseObj = JsonSerializer.Deserialize<UserInfoResponse>(content, _jsonSerializerOptions);
                    Debug.WriteLine(responseObj.SeesAds);
                    Debug.WriteLine(responseObj.UserInfo);
                    return (responseObj.UserInfo, responseObj.SeesAds);
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

            return result;
        }


        public async Task<string> GetSaltByEmailAsync(string email)
        {
            string salt = null;
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return salt;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/userinfo/salt/{email}");

                if (response.IsSuccessStatusCode)
                {
                    salt = await response.Content.ReadAsStringAsync();
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
            return salt;
        }




        public async Task<int> GetUserIDByEmailAsync(string email)
        {
            int userID = -1;
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return userID;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/userinfo/email/{email}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    userID = int.Parse(content);
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Non-Http 2xx Response: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Whoops, exception: {ex.Message}");
            }

            return userID;
        }


        public async Task<string> LoginAsync(string email, string hashedPassword)
        {
            string result = "";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/login/{email}/{hashedPassword}");

                if (response.IsSuccessStatusCode)
                {
                    result = "AllClear";
                }
                else
                {
                    result = "Unauthorized";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

    }
}

