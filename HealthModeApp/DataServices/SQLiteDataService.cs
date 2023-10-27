using System;
using System.Diagnostics;
using System.Text.Json;
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


        private async Task SetUpDB()
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HealthModeDB.db3");
                db = new SQLiteAsyncConnection(dbPath);

                await db.CreateTableAsync<UserData>();
                await db.CreateTableAsync<CustomFoods>();
                await db.CreateTableAsync<LoggedFoodTable>();
                await db.CreateTableAsync<RecentFoods>();
                await db.CreateTableAsync<PopUpMemory>();
                await db.CreateTableAsync<NutritionGoals>();
                await db.CreateTableAsync<MicroPercentageGoals>();
                await db.CreateTableAsync<MealNames>();
                await db.CreateTableAsync<MealNumber>();
                await db.CreateTableAsync<WeightTable>();
                await db.CreateTableAsync<WaterTable>();
                await db.CreateTableAsync<WaterGoalTable>();
                await db.CreateTableAsync<Translations>();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Database setup failed: {ex.Message}");
            }
        }

        public class TranslationObject
        {
            public string Key { get; set; }
            public string Translation { get; set; }
        }

       



        public async Task AddTranslation(string serializedList)
        {
            Debug.WriteLine($"Serialized List: {serializedList}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            // Deserialize the JSON string to a List of TranslationObject
            List<TranslationObject> translationsFromApi = JsonSerializer.Deserialize<List<TranslationObject>>(serializedList, options);

            if (translationsFromApi == null || !translationsFromApi.Any())
            {
                Debug.WriteLine("Deserialization failed or list is empty.");
                return;
            }

           


            // Iterate over the deserialized list
            foreach (var translationObj in translationsFromApi)
                {

                    translationObj.Translation.Replace("\n", "");
                    translationObj.Key.Replace("\n", "");
                // Check if the key already exists in the database
                var existingTranslation = await db.Table<Translations>().Where(t => t.Key == translationObj.Key).FirstOrDefaultAsync();

                    if (existingTranslation != null)
                    {
                        // Update the existing record
                        existingTranslation.Translation = translationObj.Translation;
                        await db.UpdateAsync(existingTranslation);
                    }
                    else
                    {
                        // Insert a new record
                        var newTranslation = new Translations
                        {
                            Key = translationObj.Key,
                            Translation = translationObj.Translation
                        };
                        await db.InsertAsync(newTranslation);
                    }
                }
        }


        public async Task<string> GetTranslationByKey(string key)
        {
           

            try
            {
                var translationEntry = await db.Table<Translations>()
                                               .Where(t => t.Key == key)
                                               .FirstOrDefaultAsync();
                return translationEntry.Translation;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return key;
            }
                     
        }


        #region CustomFoodDB
        public async Task AddCustomFood(CustomFoods food)
		{
            var foodInfo = new CustomFoods
            {
                Barcode = food.Barcode,
                FoodName = food.FoodName,
                Brand = food.Brand,
                ServingSize = food.ServingSize,
                ServingUnit = food.ServingUnit,
                Grams = food.Grams,
                ServingName = food.ServingName,
                Calories = food.Calories,
                Carbs = food.Carbs,
                Sugar = food.Sugar,
                AddSugar = food.AddSugar,
                SugarAlc = food.SugarAlc,
                Fiber = food.Fiber,
                NetCarb = food.NetCarb,
                Fat = food.Fat,
                SatFat = food.SatFat,
                PUnSatFat = food.PUnSatFat,
                MUnSatFat = food.MUnSatFat,
                TransFat = food.TransFat,
                Protein = food.Protein,
                Iron = food.Iron,
                Calcium = food.Calcium,
                Potassium = food.Potassium,
                Sodium = food.Sodium,
                Cholesterol = food.Cholesterol,
                VitaminA = food.VitaminA,
                Thiamin = food.Thiamin,
                Riboflavin = food.Riboflavin,
                Niacin = food.Niacin,
                B5 = food.B5,
                B6 = food.B6,
                B7 = food.B7,
                FolicAcid = food.FolicAcid,
                B12 = food.B12,
                VitaminC = food.VitaminC,
                VitaminD = food.VitaminD,
                VitaminE = food.VitaminE,
                VitaminK = food.VitaminK,
                MealType = food.MealType,
                Category = food.Category
            };

            await db.InsertAsync(foodInfo);

        }

        public async Task UpdateCustomFood(string barcode, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK, int mealType, string category)
        {
            var foodInfo = new CustomFoods
            {
                Barcode = barcode,
                FoodName = foodName,
                Brand = brand,
                ServingSize = servingSize,
                ServingUnit = servingUnit,
                Grams = grams,
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
                VitaminK = vitaminK,
                MealType = mealType,
                Category = category
            };

            await db.UpdateAsync(foodInfo);

        }

        public async Task RemoveCustomFood(string barcodeValue)
        {
            var customFood = await db.Table<CustomFoods>().FirstOrDefaultAsync(f => f.Barcode == barcodeValue);

            if (customFood != null)
            {
                await db.DeleteAsync<CustomFoods>(customFood.FoodID);
            }
        }


        public async Task<List<CustomFoods>> GetCustomFoods()
		{
             
           var foodInfo = await db.Table<CustomFoods>().ToListAsync();
            return foodInfo;
		}

        public async Task<CustomFoods> GetCustomFoodByBarcode(string barcode)
        {
            try
            {
                Debug.WriteLine("Searching Cusotm");
                var foodInfo = await db.Table<CustomFoods>().FirstOrDefaultAsync(f => f.Barcode == barcode);
                if (foodInfo != null)
                {
                    Debug.WriteLine(foodInfo.Barcode);
                    return foodInfo;
                  
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<CustomFoods>> GetCustomFoodByName(string name)
        {
            try
            {
                Debug.WriteLine("Searching Custom");
                var foodInfo = await db.Table<CustomFoods>()
                                       .Where(food => food.FoodName.Contains(name) || food.Brand.Contains(name))
                                       .ToListAsync();

                return foodInfo;
            }
            catch (Exception)
            {
                return null;
            }
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


        public async Task<LoggedFoodTable> GetLoggedFoodDetails(int userID, int loggedFoodID)
        {
            var loggedFood = await db.Table<LoggedFoodTable>()
                .FirstOrDefaultAsync(f => f.UserID == userID && f.LoggedFoodID == loggedFoodID);

            if (loggedFood == null)
            {
                return null;
            }

            return loggedFood;

            
        }







        public async Task RemoveLoggedFood(int loggedFoodID)
        {

            await db.DeleteAsync<LoggedFoodTable>(loggedFoodID);
        }

        public async Task AddLoggedFood(int userID, DateTime date, int mealType, DateTime time, int servingSizeSelected, decimal servingAmount, decimal totalGrams, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal? calories, decimal? carbs, decimal? sugar, decimal? addSugar, decimal? sugarAlc, decimal? fiber, decimal? netCarb, decimal? fat, decimal? satFat, decimal? pUnSatFat, decimal? mUnSatFat, decimal? transFat, decimal? protein, decimal? iron, decimal? calcium, decimal? potassium, decimal? sodium, decimal? cholesterol, decimal? vitaminA, decimal? thiamin, decimal? riboflavin, decimal? niacin, decimal? b5, decimal? b6, decimal? b7, decimal? folicAcid, decimal? b12, decimal? vitaminC, decimal? vitaminD, decimal? vitaminE, decimal? vitaminK, bool verified)
        {
            var loggedFoodInfo = new LoggedFoodTable
            {
                UserID = userID,
                Date = date,
                MealType = mealType,
                Time = time,
                ServingSizeSelected = servingSizeSelected,
                ServingAmount = servingAmount,
                TotalGrams = totalGrams,
                FoodName = foodName,
                Brand = brand,
                ServingSize = servingSize,
                ServingUnit = servingUnit,
                Grams = grams,
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
                VitaminK = vitaminK,
                Verified = verified
            };

           await db.InsertAsync(loggedFoodInfo);
        }

        public async Task UpdateLoggedFood(int loggedFoodID, int userID, DateTime date, int mealType, DateTime time, int servingSizeSelected, decimal servingAmount, decimal totalGrams, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal? calories, decimal? carbs, decimal? sugar, decimal? addSugar, decimal? sugarAlc, decimal? fiber, decimal? netCarb, decimal? fat, decimal? satFat, decimal? pUnSatFat, decimal? mUnSatFat, decimal? transFat, decimal? protein, decimal? iron, decimal? calcium, decimal? potassium, decimal? sodium, decimal? cholesterol, decimal? vitaminA, decimal? thiamin, decimal? riboflavin, decimal? niacin, decimal? b5, decimal? b6, decimal? b7, decimal? folicAcid, decimal? b12, decimal? vitaminC, decimal? vitaminD, decimal? vitaminE, decimal? vitaminK)
        {
            var loggedFoodInfo = await db.Table<LoggedFoodTable>().FirstOrDefaultAsync(m => m.LoggedFoodID == loggedFoodID);

            if (loggedFoodInfo == null)
            {
                throw new ArgumentException("Logged food not found");
            }

            loggedFoodInfo.UserID = userID;
            loggedFoodInfo.Date = date;
            loggedFoodInfo.MealType = mealType;
            loggedFoodInfo.Time = time;
            loggedFoodInfo.ServingSizeSelected = servingSizeSelected;
            loggedFoodInfo.ServingAmount = servingAmount;
            loggedFoodInfo.TotalGrams = totalGrams;
            loggedFoodInfo.FoodName = foodName;
            loggedFoodInfo.Brand = brand;
            loggedFoodInfo.ServingSize = servingSize;
            loggedFoodInfo.ServingUnit = servingUnit;
            loggedFoodInfo.Grams = grams;
            loggedFoodInfo.ServingName = servingName;
            loggedFoodInfo.Calories = calories;
            loggedFoodInfo.Carbs = carbs;
            loggedFoodInfo.Sugar = sugar;
            loggedFoodInfo.AddSugar = addSugar;
            loggedFoodInfo.SugarAlc = sugarAlc;
            loggedFoodInfo.Fiber = fiber;
            loggedFoodInfo.NetCarb = netCarb;
            loggedFoodInfo.Fat = fat;
            loggedFoodInfo.SatFat = satFat;
            loggedFoodInfo.PUnSatFat = pUnSatFat;
            loggedFoodInfo.MUnSatFat = mUnSatFat;
            loggedFoodInfo.TransFat = transFat;
            loggedFoodInfo.Protein = protein;
            loggedFoodInfo.Iron = iron;
            loggedFoodInfo.Calcium = calcium;
            loggedFoodInfo.Potassium = potassium;
            loggedFoodInfo.Sodium = sodium;
            loggedFoodInfo.Cholesterol = cholesterol;
            loggedFoodInfo.VitaminA = vitaminA;
            loggedFoodInfo.Thiamin = thiamin;
            loggedFoodInfo.Riboflavin = riboflavin;
            loggedFoodInfo.Niacin = niacin;
            loggedFoodInfo.B5 = b5;
            loggedFoodInfo.B6 = b6;
            loggedFoodInfo.B7 = b7;
            loggedFoodInfo.FolicAcid = folicAcid;
            loggedFoodInfo.B12 = b12;
            loggedFoodInfo.VitaminC = vitaminC;
            loggedFoodInfo.VitaminD = vitaminD;
            loggedFoodInfo.VitaminE = vitaminE;
            loggedFoodInfo.VitaminK = vitaminK;

            await db.UpdateAsync(loggedFoodInfo);
        }

        public async Task UpdateLoggedFoodMeal(int loggedFoodID, int mealNum)
        {
            var loggedFoodInfo = await db.Table<LoggedFoodTable>().FirstOrDefaultAsync(m => m.LoggedFoodID == loggedFoodID);

            if (loggedFoodInfo == null)
            {
                throw new ArgumentException("Logged food not found");
            }

            loggedFoodInfo.MealType = mealNum;

            await db.UpdateAsync(loggedFoodInfo);
        }

        public async Task<List<string>> GetMealNames(DateTime date)
        {
            List<string> mealNames = new List<string>();

            int userID = await GetUserID();

            var closestEntry = await db.Table<MealNames>().Where(x => x.UserID == userID && x.MealDate <= date).OrderByDescending(x => x.MealDate)
                                                                                          .FirstOrDefaultAsync();
            if (closestEntry != null)
            {
                var meals = await db.Table<MealNames>().Where(x => x.MealDate == closestEntry.MealDate).ToListAsync();

            foreach (var meal in meals)
                {
                    mealNames.Add(meal.MealName);
                }
                return mealNames;
            }

            return null;
        }


        public async Task AddMealName(MealNames mealName, int mealNum, DateTime date)
        {
            int userID = await GetUserID();

            try
            {
                var meal = new MealNames { MealNum = mealNum, MealName = mealName.MealName, MealDate = date, UserID = userID};
                await db.InsertAsync(meal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public async Task UpdateMealName(int mealNum, string mealName, DateTime date)
        {
            int userID = await GetUserID();
            try
            {
                // Check if there's an existing MealNames object with the matching mealNum and date
                var existingMeal = await db.Table<MealNames>()
                    .FirstOrDefaultAsync(x => x.UserID == userID && x.MealNum == mealNum && x.MealDate == date);

                if (existingMeal != null)
                {
                    // Update the MealName property of the existing object
                    existingMeal.MealName = mealName;

                    // Update the record in the database
                    await db.UpdateAsync(existingMeal);
                }
                else
                {
                    // If no existing entry, create a new MealNames object
                    var newMeal = new MealNames
                    {
                        MealNum = mealNum,
                        MealName = mealName,
                        MealDate = date,
                        UserID = userID
                        
                    };

                    // Insert the new record into the database
                    await db.InsertAsync(newMeal);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        public async Task<double> SetMealNumber(DateTime date)
        {
            try
            {
                var userID = await GetUserID();

                // Check if an entry with the userID already exists
                var existingEntry = await db.Table<MealNumber>()
                                      .Where(x => x.UserID == userID && x.MealDate <= date)
                                      .OrderByDescending(x => x.MealDate)
                                      .FirstOrDefaultAsync();

                if (existingEntry == null)
                {
                    // If no entry exists with the userID, then add the new entry
                    var mealNum = new MealNumber
                    {
                        UserID = userID,
                        MealNum = 4.1
                    };
                    await db.InsertAsync(mealNum);
                    return 4.1;
                }
                // If an entry with the userID already exists, you can handle it as needed
                else
                {
                   
                    return existingEntry.MealNum;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 4.1;
            }
        }


        public async Task UpdateMealNumber(double mealNumber, DateTime date)
        {
            try
            {
                var userID = await GetUserID();

                // Find the entry with the matching userID and date
                var existingEntry = await db.Table<MealNumber>()
                    .Where(entry => entry.UserID == userID && entry.MealDate == date)
                    .FirstOrDefaultAsync();

                if (existingEntry != null)
                {
                    // Update the existing entry's MealNum with the new value
                    existingEntry.MealNum = mealNumber;

                    // Update the entry in the database
                    await db.UpdateAsync(existingEntry);
                    Debug.WriteLine("Updated to " + mealNumber + " Meals");
                }
                else
                {
                    // If no entry with the userID and date exists, create a new row
                    var newEntry = new MealNumber
                    {
                        UserID = userID,
                        MealDate = date,
                        MealNum = mealNumber
                    };

                    // Insert the new record into the database
                    await db.InsertAsync(newEntry);
                    Debug.WriteLine("Added new entry for " + date + " with " + mealNumber + " Meals");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }





        #endregion

        #region RecentFood

        int maxRecentFoodEntries = 100;

        public async Task AddOrUpdateRecentFood(string serializedFood)
        {
            

            // Check if this serialized food already exists in the FoodReference table
            var existingFoodReference = await db.Table<RecentFoods>()
                                                 .FirstOrDefaultAsync(f => f.FoodString == serializedFood);

            if (existingFoodReference != null)
            {
                // If it exists, update its AddedDate to make it the most recent
                existingFoodReference.DateAdded = DateTime.Now;
                await db.UpdateAsync(existingFoodReference);
            }
            else
            {
                // If not, insert this new serialized food with the current date and time
                var newFoodReference = new RecentFoods { FoodString = serializedFood, DateAdded = DateTime.Now };
                await db.InsertAsync(newFoodReference);
            }

            // Ensure there are only 20 entries at most, deleting the oldest if there are more
            var count = await db.Table<RecentFoods>().CountAsync();
            if (count > maxRecentFoodEntries)
            {
                var oldestFoodReference = await db.Table<RecentFoods>()
                                                   .OrderBy(f => f.DateAdded)
                                                   .FirstOrDefaultAsync();
                if (oldestFoodReference != null)
                {
                    await db.DeleteAsync(oldestFoodReference);
                }
            }

            return;
        }

        public async Task<List<CustomFoods>> GetRecentFoods()
        {
            // Fetch the 20 most recent serialized foods
            var foodReferences = await db.Table<RecentFoods>()
                                          .OrderByDescending(f => f.DateAdded)
                                          .Take(maxRecentFoodEntries)
                                          .ToListAsync();

            // Deserialize each entry to get the actual FoodTable objects
            List<CustomFoods> recentFoods = foodReferences.Select(fr => JsonSerializer.Deserialize<CustomFoods>(fr.FoodString)).ToList();

            return recentFoods;
        }


        #endregion

        #region Water

        public async Task AddWaterGoal(int userID, DateTime dateSet, int waterGoalML)
        {
            try
            {
                

                var waterGoal = new WaterGoalTable
                {
                    UserID = userID,
                    Date = dateSet,
                    WaterGoal = waterGoalML
                };

                await db.InsertAsync(waterGoal);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        public async Task UpdateWaterGoal(DateTime dateSet, int waterGoalML)
        {
            try
            {
                var userID = await GetUserID();

                var yesterday = dateSet.AddDays(-1);
                var tomorrow = dateSet.AddDays(1);

                var existingGoal = await db.Table<WaterGoalTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                existingGoal.WaterGoal = waterGoalML;

                await db.UpdateAsync(existingGoal);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task<int> GetWaterGoal(DateTime date)
        {
            try
            {
                var userID = await GetUserID();



                var water = await db.Table<WaterGoalTable>()
                                       .Where(x => x.UserID == userID && x.Date <= date)
                                       .OrderByDescending(x => x.Date)
                                       .FirstOrDefaultAsync();

                return water.WaterGoal;
   
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return 2000;
        }

        public async Task<bool> DoesWaterGoalExist(int userID)
        {
            try
            {

                var nutritionGoal = await db.Table<WaterGoalTable>()
                                            .Where(x => x.UserID == userID)
                                            .FirstOrDefaultAsync();


                if (nutritionGoal != null)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                Debug.WriteLine("Get Error");
                return false;
            }
        }
        #endregion

        



        #region UserStuff
        public async Task<int> GetUserID()
        {
            try
            {
                int userID = 0;
                // Replace "db" with your actual database object
                var user = await db.Table<UserData>().FirstOrDefaultAsync();

                if (user != null)
                {
                    userID = user.UserID;
                    return userID;
                }

                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }


        public async Task<bool> DoesUserTableExist()
        {
            try
            {
                // Replace "db" with your actual database object
                await db.Table<UserData>().FirstOrDefaultAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }


        public async Task AddUserAsync(int userID, string email, string username, string password, bool seesAds, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, decimal weight, decimal goalWeight, int activityLevel, string flair, string flairColor, bool isBlackText, string pictureBG, string picturePath, string title)
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
                    ActivityLevel = activityLevel,
                    Flair = flair,
                    FlairColor = flairColor,
                    IsBlackText = isBlackText,
                    PictureBGColor = pictureBG,
                    PicturePath = picturePath,
                    Title = title
                };

                await db.InsertAsync(newUser);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }

            return;
        }

        public async Task<UserData> GetUserAsync(int userID)
        {
            try
            {
                // Replace "db" with your actual database object
                var user = await db.Table<UserData>().Where(u => u.UserID == userID).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }


        public async Task UpdateUserAsync(int userID, string email = null, string username = null, string password = null, bool? seesAds = null, int? weightPlan = null, string mainGoals = null, string units = null, int? sex = null, decimal? heightCm = null, DateTime? birthday = null, decimal? weight = null, decimal? goalWeight = null, int? activityLevel = null, string flair = null, string flairColor = null, bool? isBlackText = null, string pictureBG = null, string picturePath = null, string title = null)
        {
            // Replace "db" with your actual database object
            try
            {
                var user = await db.Table<UserData>().Where(u => u.UserID == userID).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (email != null)
                {
                    user.Email = email;
                }

                if (username != null)
                {
                    user.Username = username;
                }

                if (password != null)
                {
                    user.Password = password;
                }

                if (seesAds != null)
                {
                    user.SeesAds = seesAds.Value;
                }

                if (weightPlan != null)
                {
                    user.WeightPlan = weightPlan.Value;
                }

                if (mainGoals != null)
                {
                    user.MainGoals = mainGoals;
                }

                if (units != null)
                {
                    user.Units = units;
                }

                if (sex != null)
                {
                    user.Sex = sex.Value;
                }

                if (heightCm != null)
                {
                    user.HeightCm = heightCm.Value;
                }

                if (birthday != null)
                {
                    user.Birthday = birthday.Value;
                }

                if (weight != null)
                {
                    user.Weight = weight.Value;
                }

                if (goalWeight != null)
                {
                    user.GoalWeight = goalWeight.Value;
                }

                if (activityLevel != null)
                {
                    user.ActivityLevel = activityLevel.Value;
                }

                if (flair != null)
                {
                    user.Flair = flair;
                }

                if (flairColor != null)
                {
                    user.FlairColor = flairColor;
                }

                if (isBlackText != null)
                {
                    user.IsBlackText = (bool)isBlackText;
                }

                if (pictureBG != null)
                {
                    user.PictureBGColor = pictureBG;
                }

                if (picturePath != null)
                {
                    user.PicturePath = picturePath;
                }

                if (title != null)
                {
                    user.Title = title;
                }

                await db.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }


        public async Task UpdatePFP(string pfpPath, string hexCode)
        {
            try
            {
                var user = await db.Table<UserData>().FirstOrDefaultAsync();

                if (user == null)
                {
                    Debug.WriteLine("User not found");
                    return;
                }

                user.PicturePath = pfpPath;

                user.PictureBGColor = hexCode;

                await db.UpdateAsync(user);

                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return;
            }


        }

        public async Task UpdateFlair(string flairName, string flairColor, bool isBlackText)
        {
            try
            {
                var user = await db.Table<UserData>().FirstOrDefaultAsync();

                if (user == null)
                {
                    Debug.WriteLine("User not found");
                    return;
                }

                user.Flair = flairName;

                user.FlairColor = flairColor;

                user.IsBlackText = isBlackText;

                await db.UpdateAsync(user);

                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return;
            }


        }


        public async Task UpdateSeesAdsAsync(bool seesAds)
        {
            // Replace "db" with your actual database object
            try
            {
                var user = await db.Table<UserData>().FirstOrDefaultAsync();

                if (user == null)
                {
                    Debug.WriteLine("User not found");
                    return;
                }

                user.SeesAds = seesAds;

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

        public async Task<bool> GetSeesAds()
        {
            var user = await db.Table<UserData>().FirstOrDefaultAsync();
            if (user != null)
            {
                return user.SeesAds;
            }
            return true;
        }

        #endregion

        #region Nutrition

        public async Task<string> GetFoodEnergyUnit()
        {
            var user = await db.Table<UserData>().FirstOrDefaultAsync();
            if (user != null)
            {
                var unitsData = JsonSerializer.Deserialize<List<string>>(user.Units);
                if (unitsData.Count >= 3) // Check if the list has at least 3 entries
                {
                    return unitsData[3]; // Return the 4th entry (index 3)
                }
                else
                {
                    // Return a default value if the list doesn't have enough entries
                    return "cal";
                }
            }

            // Return a default value if the user object is null
            return null;
        }


        public async Task AddNutritionGoals(int userID, DateTime dateSet, int calorieGoal, int carbGoal, int fatGoal, int proteinGoal,
        int satdFatGoal, int pUnSatFatGoal, int mUnSatFatGoal, int transFatGoal, int sugarGoal, int fiberGoal,
        int ironGoal, int calciumGoal, int potassiumGoal, int sodiumGoal, int cholesterolGoal,
        int vitaminAGoal, int thiaminGoal, int riboflavinGoal, int niacinGoal, int vitaminB5Goal,
        int vitaminB6Goal, int biotinGoal, int cobalamineGoal, int folicAcidGoal, int vitaminCGoal,
        int vitaminDGoal, int vitaminEGoal, int vitaminKGoal)
        {
            try
            {
                Debug.WriteLine("ADD");
                Debug.WriteLine(dateSet);

                // A row with the given userID does not exist, add a new row
                var newGoal = new NutritionGoals()
                {
                    UserID = userID,
                    DateSet = dateSet,
                    CalorieGoal = calorieGoal,
                    CarbGoal = carbGoal,
                    FatGoal = fatGoal,
                    ProteinGoal = proteinGoal,
                    SatdFatGoal = satdFatGoal,
                    PUnSatFatGoal = pUnSatFatGoal,
                    MUnSatFatGoal = mUnSatFatGoal,
                    TransFatGoal = transFatGoal,
                    SugarGoal = sugarGoal,
                    FiberGoal = fiberGoal,
                    IronGoal = ironGoal,
                    CalciumGoal = calciumGoal,
                    PotassiumGoal = potassiumGoal,
                    SodiumGoal = sodiumGoal,
                    CholesterolGoal = cholesterolGoal,
                    VitaminAGoal = vitaminAGoal,
                    ThiaminGoal = thiaminGoal,
                    RiboflavinGoal = riboflavinGoal,
                    NiacinGoal = niacinGoal,
                    VitaminB5Goal = vitaminB5Goal,
                    VitaminB6Goal = vitaminB6Goal,
                    BiotinGoal = biotinGoal,
                    CobalamineGoal = cobalamineGoal,
                    FolicAcidGoal = folicAcidGoal,
                    VitaminCGoal = vitaminCGoal,
                    VitaminDGoal = vitaminDGoal,
                    VitaminEGoal = vitaminEGoal,
                    VitaminKGoal = vitaminKGoal
                };

                await db.InsertAsync(newGoal);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }


        public async Task UpdateNutritionGoals(int userID, DateTime date, int? calorieGoal, int? carbGoal, int? fatGoal, int? proteinGoal,
                int? satdFatGoal, int? pUnSatFatGoal, int? mUnSatFatGoal, int? transFatGoal, int? sugarGoal, int? fiberGoal,
                int? ironGoal, int? calciumGoal, int? potassiumGoal, int? sodiumGoal, int? cholesterolGoal,
                int? vitaminAGoal, int? thiaminGoal, int? riboflavinGoal, int? niacinGoal, int? vitaminB5Goal,
                int? vitaminB6Goal, int? biotinGoal, int? cobalamineGoal, int? folicAcidGoal, int? vitaminCGoal,
                int? vitaminDGoal, int? vitaminEGoal, int? vitaminKGoal)
        {
            try
            {
                Debug.WriteLine("UPDATE");
                Debug.WriteLine(date);



                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingGoal = await db.Table<NutritionGoals>()
                                            .Where(g => g.UserID == userID && g.DateSet > yesterday && g.DateSet < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingGoal != null)
                {
                    existingGoal.UserID = userID;
                    existingGoal.CalorieGoal = calorieGoal ?? existingGoal.CalorieGoal;
                    existingGoal.CarbGoal = carbGoal ?? existingGoal.CarbGoal;
                    existingGoal.FatGoal = fatGoal ?? existingGoal.FatGoal;
                    existingGoal.ProteinGoal = proteinGoal ?? existingGoal.ProteinGoal;
                    existingGoal.SatdFatGoal = satdFatGoal ?? existingGoal.SatdFatGoal;
                    existingGoal.PUnSatFatGoal = pUnSatFatGoal ?? existingGoal.PUnSatFatGoal;
                    existingGoal.MUnSatFatGoal = mUnSatFatGoal ?? existingGoal.MUnSatFatGoal;
                    existingGoal.TransFatGoal = transFatGoal ?? existingGoal.TransFatGoal;
                    existingGoal.SugarGoal = sugarGoal ?? existingGoal.SugarGoal;
                    existingGoal.FiberGoal = fiberGoal ?? existingGoal.FiberGoal;
                    existingGoal.IronGoal = ironGoal ?? existingGoal.IronGoal;
                    existingGoal.CalciumGoal = calciumGoal ?? existingGoal.CalciumGoal;
                    existingGoal.PotassiumGoal = potassiumGoal ?? existingGoal.PotassiumGoal;
                    existingGoal.SodiumGoal = sodiumGoal ?? existingGoal.SodiumGoal;
                    existingGoal.CholesterolGoal = cholesterolGoal ?? existingGoal.CholesterolGoal;
                    existingGoal.VitaminAGoal = vitaminAGoal ?? existingGoal.VitaminAGoal;
                    existingGoal.ThiaminGoal = thiaminGoal ?? existingGoal.ThiaminGoal;
                    existingGoal.RiboflavinGoal = riboflavinGoal ?? existingGoal.RiboflavinGoal;
                    existingGoal.NiacinGoal = niacinGoal ?? existingGoal.NiacinGoal;
                    existingGoal.VitaminB5Goal = vitaminB5Goal ?? existingGoal.VitaminB5Goal;
                    existingGoal.VitaminB6Goal = vitaminB6Goal ?? existingGoal.VitaminB6Goal;
                    existingGoal.BiotinGoal = biotinGoal ?? existingGoal.BiotinGoal;
                    existingGoal.CobalamineGoal = cobalamineGoal ?? existingGoal.CobalamineGoal;
                    existingGoal.FolicAcidGoal = folicAcidGoal ?? existingGoal.FolicAcidGoal;
                    existingGoal.VitaminCGoal = vitaminCGoal ?? existingGoal.VitaminCGoal;
                    existingGoal.VitaminDGoal = vitaminDGoal ?? existingGoal.VitaminDGoal;
                    existingGoal.VitaminEGoal = vitaminEGoal ?? existingGoal.VitaminEGoal;
                    existingGoal.VitaminKGoal = vitaminKGoal ?? existingGoal.VitaminKGoal;


                    await db.UpdateAsync(existingGoal);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                Debug.WriteLine("Update Error");
            }
        }

        public async Task<NutritionGoals> GetNutritionGoals(int userID, DateTime date)
        {
            try
            {
                var nutritionGoals = await db.Table<NutritionGoals>()
                                      .Where(x => x.UserID == userID && x.DateSet <= date)
                                      .OrderByDescending(x => x.DateSet)
                                      .FirstOrDefaultAsync();

                if (nutritionGoals == null)
                {
                    Debug.WriteLine("No nutrition goals found.");
                    return null;
                }

                if (nutritionGoals.FiberGoal == 0)
                {
                    nutritionGoals.FiberGoal = (int)Math.Round(28.0 / 2000.0 * nutritionGoals.CalorieGoal);
                }

                return nutritionGoals;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                return null;
            }
        }


        public async Task<bool> NutritionGoalDateExists(int userID)
        {
            try
            {
               

                var nutritionGoal = await db.Table<NutritionGoals>()
                                            .Where(x => x.UserID == userID && x.DateSet == DateTime.Today)
                                            .FirstOrDefaultAsync();


                if (nutritionGoal != null)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                Debug.WriteLine("Get Error");
                return false;
            }
        }



        public async Task ChangeMicroPercentGoals(int userID, DateTime dateSet, MicroPercentageGoals newGoal)
        {
            try
            {
                newGoal.UserID = userID;
                newGoal.DateSet = dateSet;

                var existingGoal = await db.Table<MicroPercentageGoals>()
                                           .Where(g => g.UserID == userID && g.DateSet == dateSet)
                                           .FirstOrDefaultAsync();

                if (existingGoal != null)
                {
                    // Update the existing record
                    newGoal.GoalID = existingGoal.GoalID; // Assuming 'Id' is your primary key
                    await db.UpdateAsync(newGoal);
                }
                else
                {
                    // Insert new record
                    await db.InsertAsync(newGoal);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task<MicroPercentageGoals> GetMicroPercentages(int userID, DateTime dateSet)
        {
            try
            {
                var existingGoal = await db.Table<MicroPercentageGoals>()
                                           .Where(g => g.UserID == userID && g.DateSet == dateSet)
                                           .FirstOrDefaultAsync();

                return existingGoal;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }



        public async Task<bool> GetWeightEntryExists(int userID, DateTime date)
        {
            try
            {
                var yesterday = DateTime.Today.AddDays(-1);
                var tomorrow = DateTime.Today.AddDays(1);

                var weight = await db.Table<WeightTable>()
                                            .Where(x => x.UserID == userID && x.Date > yesterday && x.Date < tomorrow)
                                            .FirstOrDefaultAsync();


                if (weight != null)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }



        #endregion

        #region BodyProgress

        public async Task AddWeightEntry(int userID, DateTime date, decimal weight, byte[]? progress)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    Weight = weight,
                    ProgressPicture = progress
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task AddBodyFatEntry(int userID, DateTime date, decimal bodyFat)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    BodyFat = bodyFat
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task AddNeckEntry(int userID, DateTime date, decimal neckCirc)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    Neck = neckCirc
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task AddChestEntry(int userID, DateTime date, decimal chestCirc)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    Chest = chestCirc
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task AddArmsEntry(int userID, DateTime date, decimal armCirc)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    Arms = armCirc
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task AddWaistEntry(int userID, DateTime date, decimal waistCirc)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    Waist = waistCirc
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task AddHipsEntry(int userID, DateTime date, decimal hipsCirc)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    Hips = hipsCirc
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task AddThighsEntry(int userID, DateTime date, decimal thighCirc)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    Thighs = thighCirc
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task AddCalvesEntry(int userID, DateTime date, decimal calvesCirc)
        {
            try
            {
                var newWeight = new WeightTable
                {
                    UserID = userID,
                    Date = date,
                    Calves = calvesCirc
                };
                await db.InsertAsync(newWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateWeightEntry(int userID, DateTime date, decimal weight, byte[]? progress)
        {
            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.Weight = weight;
                    existingWeight.ProgressPicture = progress;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateBodyFatEntry(int userID, DateTime date, decimal bodyFat)
        {

            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.BodyFat = bodyFat;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateNeckEntry(int userID, DateTime date, decimal neckCirc)
        {

            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.Neck = neckCirc;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateChestEntry(int userID, DateTime date, decimal chestCirc)
        {

            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.Chest = chestCirc;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateArmsEntry(int userID, DateTime date, decimal armsCirc)
        {

            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.Arms = armsCirc;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateWaistEntry(int userID, DateTime date, decimal waistCirc)
        {

            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.Waist = waistCirc;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateHipsEntry(int userID, DateTime date, decimal hipsCirc)
        {

            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.Hips = hipsCirc;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateThighsEntry(int userID, DateTime date, decimal thighsCirc)
        {

            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.Thighs = thighsCirc;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateCalvesEntry(int userID, DateTime date, decimal calvesCirc)
        {

            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var existingWeight = await db.Table<WeightTable>()
                                            .Where(g => g.UserID == userID && g.Date > yesterday && g.Date < tomorrow)
                                            .FirstOrDefaultAsync();

                if (existingWeight != null)
                {
                    existingWeight.UserID = userID;
                    existingWeight.Date = date;
                    existingWeight.Calves = calvesCirc;
                };
                await db.UpdateAsync(existingWeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task<decimal> GetWeight(int userID, DateTime date)
        {
            try
            {
      
                var weight = await db.Table<WeightTable>()
                                        .Where(x => x.UserID == userID && x.Date <= date && x.Weight != 0)
                                      .OrderByDescending(x => x.Date)
                                      .FirstOrDefaultAsync();


                if (weight != null)
                {
                    return weight.Weight;
                }
                return (decimal)-0;
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }

        public async Task<decimal> GetBodyFat(int userID, DateTime date)
        {
            try
            {

                var weight = await db.Table<WeightTable>()
                                        .Where(x => x.UserID == userID && x.Date <= date && x.BodyFat != 0)
                                      .OrderByDescending(x => x.Date)
                                      .FirstOrDefaultAsync();


                if (weight != null)
                {
                    return weight.BodyFat;
                }
                return (decimal)-0;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }

        public async Task<decimal> GetNeck(int userID, DateTime date)
        {
            try
            {

                var weight = await db.Table<WeightTable>()
                                        .Where(x => x.UserID == userID && x.Date <= date && x.Neck != 0)
                                      .OrderByDescending(x => x.Date)
                                      .FirstOrDefaultAsync();


                if (weight != null)
                {
                    return weight.Neck;
                }
                return (decimal)-0;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }

        public async Task<decimal> GetChest(int userID, DateTime date)
        {
            try
            {

                var weight = await db.Table<WeightTable>()
                                        .Where(x => x.UserID == userID && x.Date <= date && x.Chest != 0)
                                      .OrderByDescending(x => x.Date)
                                      .FirstOrDefaultAsync();


                if (weight != null)
                {
                    return weight.Chest;
                }
                return (decimal)-0;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }

        public async Task<decimal> GetArms(int userID, DateTime date)
        {
            try
            {
                var weight = await db.Table<WeightTable>()
                    .Where(x => x.UserID == userID && x.Date <= date && x.Arms != 0)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                if (weight != null)
                {
                    return weight.Arms;
                }
                return (decimal)-0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }

        public async Task<decimal> GetWaist(int userID, DateTime date)
        {
            try
            {
                var weight = await db.Table<WeightTable>()
                    .Where(x => x.UserID == userID && x.Date <= date && x.Waist != 0)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                if (weight != null)
                {
                    return weight.Waist;
                }
                return (decimal)-0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }

        public async Task<decimal> GetHips(int userID, DateTime date)
        {
            try
            {
                var weight = await db.Table<WeightTable>()
                    .Where(x => x.UserID == userID && x.Date <= date && x.Hips != 0)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                if (weight != null)
                {
                    return weight.Hips;
                }
                return (decimal)-0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }

        public async Task<decimal> GetThighs(int userID, DateTime date)
        {
            try
            {
                var weight = await db.Table<WeightTable>()
                    .Where(x => x.UserID == userID && x.Date <= date && x.Thighs != 0)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                if (weight != null)
                {
                    return weight.Thighs;
                }
                return (decimal)-0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }

        public async Task<decimal> GetCalves(int userID, DateTime date)
        {
            try
            {
                var weight = await db.Table<WeightTable>()
                    .Where(x => x.UserID == userID && x.Date <= date && x.Calves != 0)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                if (weight != null)
                {
                    return weight.Calves;
                }
                return (decimal)-0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (decimal)-0;
            }
        }


        public async Task<bool> GetWeightForDay(int userID, DateTime date)
        {
            try
            {
                var yesterday = date.AddDays(-1);
                var tomorrow = date.AddDays(1);

                var weight = await db.Table<WeightTable>()
                                        .Where(x => x.UserID == userID && x.Date > yesterday && x.Date < tomorrow)
                                        .FirstOrDefaultAsync();

                if (weight != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }


        public async Task<List<WeightTable>> GetWeightEntries(int userID)
        {
            try
            {

                var weight = await db.Table<WeightTable>()
                                        .Where(x => x.UserID == userID)
                                        .ToListAsync();


                if (weight != null)
                {
                    return weight;
                }
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }



        public async Task<List<WeightTable>> GetWeightsIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Weight != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    // Project the results into a new list of WeightTable objects with selected properties
                    if (weights != null)
                    {
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Weight = x.Weight,
                            ProgressPicture = x.ProgressPicture
                        }).ToList();

                        return result;
                    }
                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Weight != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    // Project the results into a new list of WeightTable objects with selected properties
                    if (weights != null)
                    {
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Weight = x.Weight,
                            ProgressPicture = x.ProgressPicture
                        }).ToList();

                        return result;
                    }
                    return null;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }


        public async Task<List<WeightTable>> GetBodyFatIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.BodyFat != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            BodyFat = x.BodyFat
                        }).ToList();

                        return result;
                    }
                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.BodyFat != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            BodyFat = x.BodyFat
                        }).ToList();

                        return result;
                    }
                    return null;
                }

                

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<WeightTable>> GetNeckIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Neck != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Neck = x.Neck
                        }).ToList();
                        return result;
                    }

                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Neck != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Neck = x.Neck
                        }).ToList();

                        return result;
                    }
                    return null;
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<WeightTable>> GetChestIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Chest != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Chest = x.Chest
                        }).ToList();
                        return result;
                    }

                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Chest!= 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Chest = x.Chest
                        }).ToList();

                        return result;
                    }
                    return null;
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<WeightTable>> GetArmsIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Arms != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Arms = x.Arms
                        }).ToList();
                        return result;
                    }

                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Arms != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Arms = x.Arms
                        }).ToList();

                        return result;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<WeightTable>> GetWaistIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Waist != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Waist = x.Waist
                        }).ToList();
                        return result;
                    }

                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Waist != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Waist = x.Waist
                        }).ToList();

                        return result;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<WeightTable>> GetHipsIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Hips != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Hips = x.Hips
                        }).ToList();
                        return result;
                    }

                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Hips != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Hips = x.Hips
                        }).ToList();

                        return result;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<WeightTable>> GetThighsIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Thighs != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Thighs = x.Thighs
                        }).ToList();
                        return result;
                    }

                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Thighs != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Thighs = x.Thighs
                        }).ToList();

                        return result;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<WeightTable>> GetCalvesIndex(int userID, int index)
        {
            try
            {
                var startDate = DateTime.MinValue;
                switch (index)
                {
                    case 0:
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case 1:
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case 2:
                        startDate = DateTime.Today.AddMonths(-2);
                        break;
                    case 3:
                        startDate = DateTime.Today.AddMonths(-3);
                        break;
                    case 4:
                        startDate = DateTime.Today.AddMonths(-6);
                        break;
                    case 5:
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                    default:
                        break;
                }

                var weights = new List<WeightTable>();
                if (startDate != DateTime.MinValue)
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Calves != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Calves = x.Calves
                        }).ToList();
                        return result;
                    }

                    return null;
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today && x.Calves != 0)
                        .OrderBy(x => x.Date)
                        .ToListAsync();

                    if (weights != null)
                    {
                        // Project the results into a new list of WeightTable objects with selected properties
                        var result = weights.Select(x => new WeightTable
                        {
                            WeightID = x.WeightID,
                            UserID = x.UserID,
                            Date = x.Date,
                            Calves = x.Calves
                        }).ToList();

                        return result;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }



        public async Task DeleteWeightEntry(int weightID)
        {

            try
            {
                var allWeightEntries = await db.Table<WeightTable>().ToListAsync();

                foreach (var weightEntry in allWeightEntries)
                {
                    await db.DeleteAsync(weightEntry);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task<bool> DoesWeightEntryExist(int userID)
        {
            try
            {
                var exists = await db.Table<WeightTable>()
                    .Where(x => x.UserID == userID)
                    .FirstOrDefaultAsync();

                if (exists != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Water
        public async Task AddWaterEntry(int userID, DateTime date, decimal volume)
        {
            try
            {
                var newVolume = new WaterTable
                {
                    UserID = userID,
                    Date = date,
                    WaterVolume = volume
                };
                await db.InsertAsync(newVolume);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateWaterEntry(int waterID, decimal volume)
        {

            try
            {

                var existingWater = await db.Table<WaterTable>()
                                            .Where(g => g.WaterID == waterID )
                                            .FirstOrDefaultAsync();

                if (existingWater != null)
                {
                    existingWater.WaterVolume = volume;
                };
                await db.UpdateAsync(existingWater);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task DeleteWaterEntry(int waterID)
        {

            try
            {

                var existingWater = await db.Table<WaterTable>()
                                            .Where(g => g.WaterID == waterID)
                                            .FirstOrDefaultAsync();

                if (existingWater != null)
                {
                    await db.DeleteAsync(existingWater);
                };
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task<List<WaterTable>> GetWaterEntries(int userID, DateTime selectedDate)
        {
            var yesterday = selectedDate.AddDays(-1);
            var tomorrow = selectedDate.AddDays(1);

            var loggedWater = await db.Table<WaterTable>()
                                       .Where(f => f.UserID == userID && f.Date > yesterday && f.Date < tomorrow)
                                       .ToListAsync();
            return loggedWater;
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

    }
}

