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
        }


        #region CustomFoodDB
        public async Task AddCustomFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK)
		{
            var foodInfo = new CustomFoods
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

        public async Task UpdateCustomFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK)
        {
            var foodInfo = new CustomFoods
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

        public async Task RemoveCustomFood(int foodID)
		{

            await db.DeleteAsync<CustomFoods>(foodID);
		}

		public async Task<IEnumerable<CustomFoods>> GetCustomFoods()
		{
             
           var foodInfo = await db.Table<CustomFoods>().ToListAsync();
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


        public async Task UpdateUserAsync(int userID, string email = null, string username = null, string password = null, bool? seesAds = null, int? weightPlan = null, string mainGoals = null, string units = null, int? sex = null, decimal? heightCm = null, DateTime? birthday = null, int? weight = null, int? goalWeight = null, int? activityLevel = null)
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





    }
}

