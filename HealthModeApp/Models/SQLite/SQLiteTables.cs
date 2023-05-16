using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;
using SQLiteTable = SQLite.TableAttribute;

namespace HealthModeApp.Models.SQLite
{
	public class SQLiteTables
	{
        [SQLiteTable("CustomFoods")]
        public class CustomFoods
        {
            [PrimaryKey, AutoIncrement]
            public int FoodID { get; set; }

            public string Barcode { get; set; }

            public string FoodName { get; set; }
            public string Brand { get; set; }
            public decimal ServingSize { get; set; }
          
            public string ServingUnit { get; set; }
            public decimal Grams { get; set; }
            public string ServingName { get; set; }

            public decimal Calories { get; set; }

            public decimal Carbs { get; set; }
            public decimal? Sugar { get; set; }
            public decimal? AddSugar { get; set; }
            public decimal? SugarAlc { get; set; }
            public decimal? Fiber { get; set; }
            public decimal? NetCarb { get; set; }

            public decimal Fat { get; set; }
            public decimal? SatFat { get; set; }
            public decimal? PUnSatFat { get; set; }
            public decimal? MUnSatFat { get; set; }
            public decimal? TransFat { get; set; }

            public decimal Protein { get; set; }

            public decimal? Iron { get; set; }
            public decimal? Calcium { get; set; }
            public decimal? Potassium { get; set; }
            public decimal? Sodium { get; set; }
            public decimal? Cholesterol { get; set; }

            public decimal? VitaminA { get; set; }
            public decimal? Thiamin { get; set; }
            public decimal? Riboflavin { get; set; }
            public decimal? Niacin { get; set; }
            public decimal? B5 { get; set; }
            public decimal? B6 { get; set; }
            public decimal? B7 { get; set; }
            public decimal? FolicAcid { get; set; }
            public decimal? B12 { get; set; }

            public decimal? VitaminC { get; set; }
            public decimal? VitaminD { get; set; }
            public decimal? VitaminE { get; set; }
            public decimal? VitaminK { get; set; }

            public int MealType { get; set; }
            public string Category { get; set; }
        }

       

        [SQLiteTable("LoggedFoodTable")]
        public class LoggedFoodTable
        {
            [PrimaryKey, AutoIncrement]
            public int LoggedFoodID { get; set; }

            public int UserID { get; set; }

            [Indexed]
            public DateTime Date { get; set; }

            public int MealType { get; set; }

            public TimeSpan Time { get; set; }

            public int ServingSizeSelected { get; set; }

            public decimal ServingAmount { get; set; }
            public decimal TotalGrams { get; set; }

            public int FoodID { get; set; }

            public string Barcode { get; set; }

            public string FoodName { get; set; }
            public string Brand { get; set; }
            public decimal ServingSize { get; set; }
            public string ServingUnit{ get; set; }
            public decimal Grams { get; set; }
            public string ServingName { get; set; }

            public decimal? Calories { get; set; }

            public decimal? Carbs { get; set; }
            public decimal? Sugar { get; set; }
            public decimal? AddSugar { get; set; }
            public decimal? SugarAlc { get; set; }
            public decimal? Fiber { get; set; }
            public decimal? NetCarb { get; set; }

            public decimal? Fat { get; set; }
            public decimal? SatFat { get; set; }
            public decimal? PUnSatFat { get; set; }
            public decimal? MUnSatFat { get; set; }
            public decimal? TransFat { get; set; }

            public decimal? Protein { get; set; }

            public decimal? Iron { get; set; }
            public decimal? Calcium { get; set; }
            public decimal? Potassium { get; set; }
            public decimal? Sodium { get; set; }
            public decimal? Cholesterol { get; set; }

            public decimal? VitaminA { get; set; }
            public decimal? Thiamin { get; set; }
            public decimal? Riboflavin { get; set; }
            public decimal? Niacin { get; set; }
            public decimal? B5 { get; set; }
            public decimal? B6 { get; set; }
            public decimal? B7 { get; set; }
            public decimal? FolicAcid { get; set; }
            public decimal? B12 { get; set; }

            public decimal? VitaminC { get; set; }
            public decimal? VitaminD { get; set; }
            public decimal? VitaminE { get; set; }
            public decimal? VitaminK { get; set; }
        }



        [SQLiteTable("PopUpMemory")]
        public class PopUpMemory
        {
            [PrimaryKey, AutoIncrement]
            public int PopUpMemID { get; set; }

            public string PopUpName { get; set; }

            public bool Seen { get; set; }
        }

        [SQLiteTable("UserData")]
        public class UserData
        {
            [PrimaryKey]
            public int DataID { get; set; }

            public int UserID { get; set; }

            public string Email { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }

            public bool SeesAds { get; set; }

            public int WeightPlan { get; set; }

            public string MainGoals { get; set; }

            public string Units { get; set; }

            public int Sex { get; set; }

            public decimal HeightCm { get; set; }

            public DateTime Birthday { get; set; }

            public decimal Weight { get; set; }

            public decimal GoalWeight { get; set; }

            public int ActivityLevel { get; set; }
        }

     

        [SQLiteTable("MealNames")]
        public class MealNames
        {
            [PrimaryKey, AutoIncrement]
            public int MealID { get; set; }

            public string MealName { get; set; }

        }


        [SQLiteTable("NutritionGoals")]
        public class NutritionGoals
        {
            [PrimaryKey, AutoIncrement]
            public int GoalID { get; set; }

            public int UserID { get; set; }

            public DateTime DateSet { get; set; }

            public int CalorieGoal { get; set; }

            public int CarbGoal { get; set; }

            public int FatGoal { get; set; }

            public int ProteinGoal { get; set; }

            public int SatdFatGoal { get; set; }

            public int PUnSatFatGoal { get; set; }

            public int MUnSatFatGoal { get; set; }

            public int TransFatGoal { get; set; }

            public int SugarGoal { get; set; }

            public int IronGoal { get; set; }

            public int CalciumGoal { get; set; }

            public int PotassiumGoal { get; set; }

            public int SodiumGoal { get; set; }

            public int CholesterolGoal { get; set; }

            public int VitaminAGoal { get; set; }

            public int ThiaminGoal { get; set; }

            public int RiboflavinGoal { get; set; }

            public int NiacinGoal { get; set; }

            public int VitaminB5Goal { get; set; }

            public int VitaminB6Goal { get; set; }

            public int BiotinGoal { get; set; }

            public int CobalamineGoal { get; set; }

            public int FolicAcidGoal { get; set; }

            public int VitaminCGoal { get; set; }

            public int VitaminDGoal { get; set; }

            public int VitaminEGoal { get; set; }

            public int VitaminKGoal { get; set; }
        }


        [SQLiteTable("WeightTable")]
        public class WeightTable
        {
            [PrimaryKey, AutoIncrement]
            public int WeightID { get; set; }

            public DateTime Date { get; set; }

            public int UserID { get; set; }

            public decimal Weight { get; set; }

            public byte[] ProgressPicture { get; set; }

        }

    }
}

