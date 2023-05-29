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
            _baseAddress = "https://5vw1lstyzd.execute-api.us-east-2.amazonaws.com/beta";
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


        public async Task AddNutritionInfoAsync(NutritionModel nutritionModel, string email, string password, int userID)
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



                //StringContent content = new StringContent("{\n\t\"barcode\": \"test\",\n\t\"foodName\": \"Food NameTest\",\n\t\"mealType\": \"0\",\n\t\"servingUnit\": \"mL\",\n\t\"grams\": \"28\",\n\t\"calories\": \"100\",\n\t\"brand\": \"Brand\",\n\t\"servingSize\": \"28\",\n\t\"servingName\": \"About 15 chips\",\n\t\"carbs\": \"5\",\n\t\"fat\": \"7\",\n\t\"protein\": \"4\",\n\t\"category\": \"Processed/Packaged\"\n\n\t\n}");    Debug.WriteLine("JSON Nutrition:");
                Debug.WriteLine(jsonNutrition);
            

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/uploadedfood/userinfo/{email}/{password}/{userID}", content);


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

        public async Task<List<NutritionModel>> GetNutritionInfoNameAsync(string name, int limit = 50, int offset = 0)
        {
            List<NutritionModel> nutritionList = new List<NutritionModel>();
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return nutritionList;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/food?name={name}&limit={limit}&offset={offset}");

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
            catch (Exception ex)
            {
                Debug.WriteLine($"Whoops, exception: {ex.Message}");

            }
            return nutritionList;
        }

        public async Task<NutritionModel> GetNutritionInfoBarcodeAsync(string barcode)
        {
            NutritionModel nutritionItem = null;
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return nutritionItem;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/food/bybarcode/{barcode}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    nutritionItem = JsonSerializer.Deserialize<NutritionModel>(content, _jsonSerializerOptions);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Debug.WriteLine("-----> Resource Not Found");
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
            return nutritionItem;
        }

        public async Task<NutritionModel> GetFoodUploadExistsAlready(string barcode)
        {
            NutritionModel nutritionItem = null;
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return nutritionItem;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/uploadedfood/bybarcode/{barcode}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    nutritionItem = JsonSerializer.Deserialize<NutritionModel>(content, _jsonSerializerOptions);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Debug.WriteLine("-----> Resource Not Found");
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
            return nutritionItem;
        }


        
        #endregion

        public async Task<bool> AddUserAsync(string email, string username, string hashedPassword, string salt, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, decimal weight, decimal goalWeight, int activityLevel, int calorieGoal)
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
                    ActivityLevel = activityLevel,
                    CalorieGoal = calorieGoal
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

      

        public async Task DeleteUserAsync(int userID, string email, string password)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/userinfo/{email}/{password}/{userID}");

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

        public async Task UpdateUserInfoAsync(UserInfo userInfoModel, string email, string password, int userID)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return;
            }

            try
            {
                UserInfo userInfo = new UserInfo()
                {
                    Email = userInfoModel.Email,
                    Username = userInfoModel.Username,
                    Password = userInfoModel.Password,
                    Salt = userInfoModel.Salt,
                    WeightPlan = userInfoModel.WeightPlan,
                    MainGoals = userInfoModel.MainGoals,
                    Units = userInfoModel.Units,
                    Sex = userInfoModel.Sex,
                    HeightCm = userInfoModel.HeightCm,
                    Birthday = userInfoModel.Birthday,
                    Weight = userInfoModel.Weight,
                    GoalWeight = userInfoModel.GoalWeight,
                    ActivityLevel = userInfoModel.ActivityLevel,
                    CalorieGoal = userInfoModel.CalorieGoal
                };

                string jsonUserInfo = JsonSerializer.Serialize<UserInfo>(userInfo, _jsonSerializerOptions);
                StringContent content = new StringContent(jsonUserInfo, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{_url}/userinfo/{email}/{password}/{userID}", content);

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


        public async Task UpdateExpDateAsync(string email, int userID, double hoursToAdd)
        {
            Debug.WriteLine($"Updating user {userID} with {hoursToAdd} hours");

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return;
            }

            try
            {
                var json = JsonSerializer.Serialize(hoursToAdd);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{_url}/expDate/user/{email}/{userID}/hours/{hoursToAdd}", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully updated user ExpDate");
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

        public async Task<(UserInfo, bool)> GetUserInfoOnLoginAsync(int userID, string email, string password)
        {
            (UserInfo, bool) result = (null, true);
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> No Internet");
                return result;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/userinfo/{email}/{password}/{userID}");

                
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

        public async Task<bool> GetSeesAdsAsync(int userID)
        {
            bool result = true;
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
                    return responseObj.SeesAds;
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

