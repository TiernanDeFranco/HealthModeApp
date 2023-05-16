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


        private async void SetUpDB()
		{
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HealthModeDB.db3");
                db = new SQLiteAsyncConnection(dbPath);


                await db.CreateTableAsync<UserData>();

                await db.CreateTableAsync<CustomFoods>();
                await db.CreateTableAsync<LoggedFoodTable>();

                await db.CreateTableAsync<PopUpMemory>();

                await db.CreateTableAsync<NutritionGoals>();

                await db.CreateTableAsync<MealNames>();

            await db.CreateTableAsync<WeightTable>();
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

        public async Task RemoveCustomFood(int foodID)
		{

            await db.DeleteAsync<CustomFoods>(foodID);
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

        public async Task AddLoggedFood(int userID, DateTime date, int mealType, TimeSpan time, int servingSizeSelected, decimal servingAmount, decimal totalGrams, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal? calories, decimal? carbs, decimal? sugar, decimal? addSugar, decimal? sugarAlc, decimal? fiber, decimal? netCarb, decimal? fat, decimal? satFat, decimal? pUnSatFat, decimal? mUnSatFat, decimal? transFat, decimal? protein, decimal? iron, decimal? calcium, decimal? potassium, decimal? sodium, decimal? cholesterol, decimal? vitaminA, decimal? thiamin, decimal? riboflavin, decimal? niacin, decimal? b5, decimal? b6, decimal? b7, decimal? folicAcid, decimal? b12, decimal? vitaminC, decimal? vitaminD, decimal? vitaminE, decimal? vitaminK)
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
                VitaminK = vitaminK
            };

           await db.InsertAsync(loggedFoodInfo);
        }

        public async Task UpdateLoggedFood(int loggedFoodID, int userID, DateTime date, int mealType, TimeSpan time, int servingSizeSelected, decimal servingAmount, decimal totalGrams, string foodName, string brand, decimal servingSize, string servingUnit, decimal grams, string servingName, decimal? calories, decimal? carbs, decimal? sugar, decimal? addSugar, decimal? sugarAlc, decimal? fiber, decimal? netCarb, decimal? fat, decimal? satFat, decimal? pUnSatFat, decimal? mUnSatFat, decimal? transFat, decimal? protein, decimal? iron, decimal? calcium, decimal? potassium, decimal? sodium, decimal? cholesterol, decimal? vitaminA, decimal? thiamin, decimal? riboflavin, decimal? niacin, decimal? b5, decimal? b6, decimal? b7, decimal? folicAcid, decimal? b12, decimal? vitaminC, decimal? vitaminD, decimal? vitaminE, decimal? vitaminK)
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



        public async Task<List<string>> GetMealNames()
        {
            List<string> mealNames = new List<string>();

            var meals = await db.Table<MealNames>().ToListAsync();

            foreach (var meal in meals)
            {
                mealNames.Add(meal.MealName);
            }

            return mealNames;
        }


        public async Task AddMealName(MealNames mealName)
        {
            try
            {
                var meal = new MealNames { MealName = mealName.MealName};
                await db.InsertAsync(meal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public async Task UpdateMealName(int mealID, string mealName)
        {
            try
            {
                // Find the existing MealNames object in the database with the matching MealID and UserID
                var existingMeal = await db.Table<MealNames>().FirstOrDefaultAsync(m => m.MealID == mealID);

                if (existingMeal != null)
                {
                    // Update the MealName property of the existing object
                    existingMeal.MealName = mealName;

                    // Update the record in the database
                    await db.UpdateAsync(existingMeal);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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


        public async Task AddUserAsync(int userID, string email, string username, string password, bool seesAds, int weightPlan, string mainGoals, string units, int sex, decimal heightCm, DateTime birthday, decimal weight, decimal goalWeight, int activityLevel)
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


        public async Task UpdateUserAsync(int userID, string email = null, string username = null, string password = null, bool? seesAds = null, int? weightPlan = null, string mainGoals = null, string units = null, int? sex = null, decimal? heightCm = null, DateTime? birthday = null, decimal? weight = null, decimal? goalWeight = null, int? activityLevel = null)
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

                await db.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                throw;
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

        public async Task<string> GetFoodEnergyUnit()
        {
            var user = await db.Table<UserData>().FirstOrDefaultAsync();
            if (user != null)
            {
                var unitsData = JsonSerializer.Deserialize<List<string>>(user.Units);
                if (unitsData.Count >= 3) // Check if the list has at least 3 entries
                {
                    return unitsData[2]; // Return the 3rd entry (index 2)
                }
                else
                {
                    // Return a default value if the list doesn't have enough entries
                    return "Default value";
                }
            }

            // Return a default value if the user object is null
            return null;
        }


        public async Task AddNutritionGoals(int userID, DateTime dateSet, int calorieGoal, int carbGoal, int fatGoal, int proteinGoal,
        int satdFatGoal, int pUnSatFatGoal, int mUnSatFatGoal, int transFatGoal, int sugarGoal,
        int ironGoal, int calciumGoal, int potassiumGoal, int sodiumGoal, int cholesterolGoal,
        int vitaminAGoal, int thiaminGoal, int riboflavinGoal, int niacinGoal, int vitaminB5Goal,
        int vitaminB6Goal, int biotinGoal, int cobalamineGoal, int folicAcidGoal, int vitaminCGoal,
        int vitaminDGoal, int vitaminEGoal, int vitaminKGoal)
        {
            try
            {

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
                int? satdFatGoal, int? pUnSatFatGoal, int? mUnSatFatGoal, int? transFatGoal, int? sugarGoal,
                int? ironGoal, int? calciumGoal, int? potassiumGoal, int? sodiumGoal, int? cholesterolGoal,
                int? vitaminAGoal, int? thiaminGoal, int? riboflavinGoal, int? niacinGoal, int? vitaminB5Goal,
                int? vitaminB6Goal, int? biotinGoal, int? cobalamineGoal, int? folicAcidGoal, int? vitaminCGoal,
                int? vitaminDGoal, int? vitaminEGoal, int? vitaminKGoal)
        {
            try
            {
                var yesterday = DateTime.Today.AddDays(-1);
                var tomorrow = DateTime.Today.AddDays(1);

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

                return nutritionGoals;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> NutritionGoalDateExists(int userID, DateTime date)
        {
            try
            {
                var yesterday = DateTime.Today.AddDays(-1);
                var tomorrow = DateTime.Today.AddDays(1);

                var nutritionGoal = await db.Table<NutritionGoals>()
                                            .Where(x => x.UserID == userID && x.DateSet > yesterday && x.DateSet < tomorrow)
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


        public async Task<decimal> GetWeight(int userID, DateTime date)
        {
            try
            {
      
                var weight = await db.Table<WeightTable>()
                                        .Where(x => x.UserID == userID && x.Date <= date)
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
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today)
                        .OrderBy(x => x.Date)
                        .ToListAsync();
                }
                else
                {
                    weights = await db.Table<WeightTable>()
                        .Where(x => x.UserID == userID && x.Date >= startDate && x.Date <= DateTime.Today)
                        .OrderBy(x => x.Date)
                        .ToListAsync();
                }

                return weights.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return null;
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


    }
}

