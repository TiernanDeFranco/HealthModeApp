using System;
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
            if (db != null)
            {

                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HealthModeDB.db3");
                db = new SQLiteAsyncConnection(dbPath);


                await db.CreateTableAsync<FoodBaseTable>();
                await db.CreateTableAsync<LoggedFoodTable>();
            }
        }


        #region FoodBaseDB
        public async Task AddFood(string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK)
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

        public async Task RemoveFood(int foodID)
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

        public async Task<IEnumerable<LoggedFoodTable>> GetLoggedFoods(DateTime selectedDate)
        {
            var loggedFoods = await db.Table<LoggedFoodTable>().Where(f => f.Date == selectedDate).ToListAsync();

            return loggedFoods;
        }

        public async Task RemoveLoggedFood(int loggedFoodID)
        {

            await db.DeleteAsync<LoggedFoodTable>(loggedFoodID);
        }

        public async Task AddLoggedFood(int loggedFoodID, DateTime date, int mealType, DateTime time, decimal servingAmount, int foodID, string barcode, string foodName, string brand, decimal servingSize, string servingName, decimal calories, decimal carbs, decimal sugar, decimal addSugar, decimal sugarAlc, decimal fiber, decimal netCarb, decimal fat, decimal satFat, decimal pUnSatFat, decimal mUnSatFat, decimal transFat, decimal protein, decimal iron, decimal calcium, decimal potassium, decimal sodium, decimal cholesterol, decimal vitaminA, decimal thiamin, decimal riboflavin, decimal niacin, decimal b5, decimal b6, decimal b7, decimal folicAcid, decimal b12, decimal vitaminC, decimal vitaminD, decimal vitaminE, decimal vitaminK)
        {
            var loggedFoodInfo = new LoggedFoodTable
            {
                LoggedFoodID = loggedFoodID,
                Date = date,
                MealType = mealType,
                Time = time,
                ServingAmount = servingAmount,
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




        #endregion
    }
}

