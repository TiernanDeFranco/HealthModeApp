﻿using System;
using System.Diagnostics;
using SQLite;
using static HealthModeApp.Models.SQLite.SQLiteTables;

namespace HealthModeApp.DataServices
{
    public class SQLiteDataService : ISQLiteDataService
    {
		private SQLiteAsyncConnection db;

        public SQLiteDataService()
        {
            SetUpDB();
        }


        private async void SetUpDB()
		{
           

                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HealthModeDB.db3");
                db = new SQLiteAsyncConnection(dbPath);

                await Task.Delay(TimeSpan.FromSeconds(1));

                await db.CreateTableAsync<UserData>();

                await db.CreateTableAsync<FoodBaseTable>();
                await db.CreateTableAsync<LoggedFoodTable>();

                await db.CreateTableAsync<PopUpMemory>();

        }


        #region FoodBaseDB
        public async Task AddBaseFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK)
		{
            var foodInfo = new FoodBaseTable
            {
                Barcode = barcode,
                FoodName = foodName,
                Brand = brand,
                ServingSize = servingSize,
                ServingName = servingName,
                Calories = calories,
                Carbs = carbs,
                Sugar = sugar,
                AddSugar = addSugar,
                SugarAlc = sugarAlc,
                Fiber = fiber,
                NetCarb = netCarb,
                Fat = fat,
                SatFat = satFat,
                PUnSatFat = pUnSatFat,
                MUnSatFat = mUnSatFat,
                TransFat = transFat,
                Protein = protein,
                Iron = iron,
                Calcium = calcium,
                Potassium = potassium,
                Sodium = sodium,
                Cholesterol = cholesterol,
                VitaminA = vitaminA,
                Thiamin = thiamin,
                Riboflavin = riboflavin,
                Niacin = niacin,
                B5 = b5,
                B6 = b6,
                B7 = b7,
                FolicAcid = folicAcid,
                B12 = b12,
                VitaminC = vitaminC,
                VitaminD = vitaminD,
                VitaminE = vitaminE,
                VitaminK = vitaminK
            };

            await db.InsertAsync(foodInfo);

        }

        public async Task UpdateBaseFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK)
        {
            var foodInfo = new FoodBaseTable
            {
                Barcode = barcode,
                FoodName = foodName,
                Brand = brand,
                ServingSize = servingSize,
                ServingName = servingName,
                Calories = calories,
                Carbs = carbs,
                Sugar = sugar,
                AddSugar = addSugar,
                SugarAlc = sugarAlc,
                Fiber = fiber,
                NetCarb = netCarb,
                Fat = fat,
                SatFat = satFat,
                PUnSatFat = pUnSatFat,
                MUnSatFat = mUnSatFat,
                TransFat = transFat,
                Protein = protein,
                Iron = iron,
                Calcium = calcium,
                Potassium = potassium,
                Sodium = sodium,
                Cholesterol = cholesterol,
                VitaminA = vitaminA,
                Thiamin = thiamin,
                Riboflavin = riboflavin,
                Niacin = niacin,
                B5 = b5,
                B6 = b6,
                B7 = b7,
                FolicAcid = folicAcid,
                B12 = b12,
                VitaminC = vitaminC,
                VitaminD = vitaminD,
                VitaminE = vitaminE,
                VitaminK = vitaminK
            };

            await db.UpdateAsync(foodInfo);

        }

        public async Task RemoveBaseFood(int foodID)
		{

            await db.DeleteAsync<FoodBaseTable>(foodID);
		}

		public async Task<IEnumerable<FoodBaseTable>> GetFood()
		{
             
           var foodInfo = await db.Table<FoodBaseTable>().ToListAsync();
            return foodInfo;
		}
        #endregion

        #region LoggedFood

        public async Task<IEnumerable<LoggedFoodTable>> GetLoggedFoods(int userID, DateTime selectedDate)
        {
            var loggedFoods = await db.Table<LoggedFoodTable>()
                                      .Where(f => f.UserID == userID && f.Date == selectedDate)
                                      .OrderBy(f => f.Time)
                                      .ToListAsync();


            var journalData = loggedFoods.Select(f => new LoggedFoodTable
            {
                LoggedFoodID = f.LoggedFoodID,
                FoodName = f.FoodName,
                Carbs = f.Carbs,
                Fat = f.Fat,
                Protein = f.Protein,
                Calories = f.Calories,
                MealType = f.MealType
            }).ToList();

            return journalData;
        }

        public async Task<IEnumerable<LoggedFoodTable>> GetLoggedFoodID(int userID, DateTime selectedDate)
        {
            var loggedFoods = await db.Table<LoggedFoodTable>()
                                      .Where(f => f.UserID == userID && f.Date == selectedDate)
                                      .OrderBy(f => f.Time)
                                      .ToListAsync();

            var IDData = loggedFoods.Select(f => new LoggedFoodTable
            {
                LoggedFoodID = f.LoggedFoodID,
            }).ToList();

            return IDData;
        }


        public async Task<LoggedFoodTable> GetLoggedFoodDetails(int userID, int loggedFoodID, string page)
        {
            var loggedFood = await db.Table<LoggedFoodTable>()
                .FirstOrDefaultAsync(f => f.UserID == userID && f.LoggedFoodID == loggedFoodID);

            if (loggedFood == null)
            {
                return null;
            }

            switch (page)
            {
                case "MealPage":
                    return new LoggedFoodTable
                    {
                        LoggedFoodID = loggedFood.LoggedFoodID,
                        FoodName = loggedFood.FoodName,
                        Brand = loggedFood.Brand,
                        ServingAmount = loggedFood.ServingAmount,
                        TotalGrams = loggedFood.TotalGrams,
                        Carbs = loggedFood.Carbs,
                        Fat = loggedFood.Fat,
                        Protein = loggedFood.Protein,
                        Calories = loggedFood.Calories
                    };
                case "FoodInfoPage":
                    return loggedFood;
                default:
                    return null;
            }
        }







        public async Task RemoveLoggedFood(int loggedFoodID)
        {

            await db.DeleteAsync<LoggedFoodTable>(loggedFoodID);
        }

        public async Task AddLoggedFood(int userID, DateTime date, int mealType, DateTime time, decimal servingAmount, decimal totalGrams, int foodID, string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK)
        {
            var loggedFoodInfo = new LoggedFoodTable
            {
                UserID = userID,
                Date = date,
                MealType = mealType,
                Time = time,
                ServingAmount = servingAmount,
                TotalGrams = totalGrams,
                FoodID = foodID,
                Barcode = barcode,
                FoodName = foodName,
                Brand = brand,
                ServingSize = servingSize,
                ServingName = servingName,
                Calories = calories,
                Carbs = carbs,
                Sugar = sugar,
                AddSugar = addSugar,
                SugarAlc = sugarAlc,
                Fiber = fiber,
                NetCarb = netCarb,
                Fat = fat,
                SatFat = satFat,
                PUnSatFat = pUnSatFat,
                MUnSatFat = mUnSatFat,
                TransFat = transFat,
                Protein = protein,
                Iron = iron,
                Calcium = calcium,
                Potassium = potassium,
                Sodium = sodium,
                Cholesterol = cholesterol,
                VitaminA = vitaminA,
                Thiamin = thiamin,
                Riboflavin = riboflavin,
                Niacin = niacin,
                B5 = b5,
                B6 = b6,
                B7 = b7,
                FolicAcid = folicAcid,
                B12 = b12,
                VitaminC = vitaminC,
                VitaminD = vitaminD,
                VitaminE = vitaminE,
                VitaminK = vitaminK
            };

           await db.InsertAsync(loggedFoodInfo);
        }

        public async Task UpdateLoggedFood(int userID, DateTime date, int mealType, DateTime time, decimal servingAmount, decimal totalGrams, int foodID, string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK)
        {
            var loggedFoodInfo = new LoggedFoodTable
            {
                UserID = userID,
                Date = date,
                MealType = mealType,
                Time = time,
                ServingAmount = servingAmount,
                TotalGrams = totalGrams,
                FoodID = foodID,
                Barcode = barcode,
                FoodName = foodName,
                Brand = brand,
                ServingSize = servingSize,
                ServingName = servingName,
                Calories = calories,
                Carbs = carbs,
                Sugar = sugar,
                AddSugar = addSugar,
                SugarAlc = sugarAlc,
                Fiber = fiber,
                NetCarb = netCarb,
                Fat = fat,
                SatFat = satFat,
                PUnSatFat = pUnSatFat,
                MUnSatFat = mUnSatFat,
                TransFat = transFat,
                Protein = protein,
                Iron = iron,
                Calcium = calcium,
                Potassium = potassium,
                Sodium = sodium,
                Cholesterol = cholesterol,
                VitaminA = vitaminA,
                Thiamin = thiamin,
                Riboflavin = riboflavin,
                Niacin = niacin,
                B5 = b5,
                B6 = b6,
                B7 = b7,
                FolicAcid = folicAcid,
                B12 = b12,
                VitaminC = vitaminC,
                VitaminD = vitaminD,
                VitaminE = vitaminE,
                VitaminK = vitaminK
            };

            await db.UpdateAsync(loggedFoodInfo);
        }




        #endregion

        public async Task<bool> GetPopUpSeen(string popupName)
        {
            var popUpMemory = await db.Table<PopUpMemory>().FirstOrDefaultAsync(p => p.PopUpName == popupName);
            if (popUpMemory != null)
            {
                return popUpMemory.Seen;
            }
            else
            {
                return false; // default value if popupId not found in table
            }
        }

        public async Task AddPopUpSeen(string popupName, bool popupSeen)
        {
            var popUpMemory = new PopUpMemory
            {
                PopUpName = popupName,
                Seen = popupSeen
            };
            await db.InsertAsync(popUpMemory);
        }

        public async Task<int> GetUserID()
        {
            int userID = 0;

            // Replace "db" with your actual database object
            var user = await db.Table<UserData>().FirstOrDefaultAsync();

            if (user != null)
            {
                userID = user.UserID;
            }

            return userID;
        }

        public async Task AddUserAsync(int userID, string email, string username, string password, bool seesAds, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, int weight, int goalWeight, int activityLevel)
        {

            // Replace "db" with your actual database object
            try
            {
                var newUser = new UserData()
                {
                    UserID = userID,
                    Email = email,
                    Username = username,
                    Password = password,
                    SeesAds = seesAds,
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

                await db.InsertAsync(newUser);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }

            return;
        }

        public async Task UpdateUserAsync(int userID, string email, string username, string password, bool seesAds, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, int weight, int goalWeight, int activityLevel)
        {

            // Replace "db" with your actual database object
            try
            {
                var user = new UserData()
                {
                    UserID = userID,
                    Email = email,
                    Username = username,
                    Password = password,
                    SeesAds = seesAds,
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

                await db.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }

            return;
        }


        public async Task DeleteUser()
        {
            try
            {
                await db.DeleteAllAsync<UserData>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
            return;
        }


        public async Task<(string Email, string Password)> GetUserCredentials()
        {
            var user = await db.Table<UserData>().FirstOrDefaultAsync();
            if (user != null)
            {
                return (user.Email, user.Password);
            }
            return (null, null);
        }


    }
}

