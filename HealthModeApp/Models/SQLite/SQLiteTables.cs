using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;
using SQLiteTable = SQLite.TableAttribute;

namespace HealthModeApp.Models.SQLite
{
	public class SQLiteTables
	{
        [SQLiteTable("FoodBaseTable")]
        public class FoodBaseTable
        {
            [PrimaryKey, AutoIncrement]
            public int FoodID { get; set; }

            public string Barcode { get; set; }

            public string FoodName { get; set; }
            public string Brand { get; set; }
            public decimal ServingSize { get; set; }
            public string ServingName { get; set; }

            public decimal Calories { get; set; }

            public decimal Carbs { get; set; }
            public decimal Sugar { get; set; }
            public decimal AddSugar { get; set; }
            public decimal SugarAlc { get; set; }
            public decimal Fiber { get; set; }
            public decimal NetCarb { get; set; }

            public decimal Fat { get; set; }
            public decimal SatFat { get; set; }
            public decimal PUnSatFat { get; set; }
            public decimal MUnSatFat { get; set; }
            public decimal TransFat { get; set; }

            public decimal Protein { get; set; }

            public decimal Iron { get; set; }
            public decimal Calcium { get; set; }
            public decimal Potassium { get; set; }
            public decimal Sodium { get; set; }
            public decimal Cholesterol { get; set; }

            public decimal VitaminA { get; set; }
            public decimal Thiamin { get; set; }
            public decimal Riboflavin { get; set; }
            public decimal Niacin { get; set; }
            public decimal B5 { get; set; }
            public decimal B6 { get; set; }
            public decimal B7 { get; set; }
            public decimal FolicAcid { get; set; }
            public decimal B12 { get; set; }

            public decimal VitaminC { get; set; }
            public decimal VitaminD { get; set; }
            public decimal VitaminE { get; set; }
            public decimal VitaminK { get; set; }
        }

       

        [SQLiteTable("LoggedFoodTable")]
        public class LoggedFoodTable
        {
            [PrimaryKey, AutoIncrement]
            public int LoggedFoodID { get; set; }

            [Indexed]
            public DateTime Date { get; set; }

            public int MealType { get; set; }

            public DateTime Time { get; set; }

            public decimal ServingAmount { get; set; }

            public int FoodID { get; set; }

            public string Barcode { get; set; }

            public string FoodName { get; set; }
            public string Brand { get; set; }
            public decimal ServingSize { get; set; }
            public string ServingName { get; set; }

            public decimal Calories { get; set; }

            public decimal Carbs { get; set; }
            public decimal Sugar { get; set; }
            public decimal AddSugar { get; set; }
            public decimal SugarAlc { get; set; }
            public decimal Fiber { get; set; }
            public decimal NetCarb { get; set; }

            public decimal Fat { get; set; }
            public decimal SatFat { get; set; }
            public decimal PUnSatFat { get; set; }
            public decimal MUnSatFat { get; set; }
            public decimal TransFat { get; set; }

            public decimal Protein { get; set; }

            public decimal Iron { get; set; }
            public decimal Calcium { get; set; }
            public decimal Potassium { get; set; }
            public decimal Sodium { get; set; }
            public decimal Cholesterol { get; set; }

            public decimal VitaminA { get; set; }
            public decimal Thiamin { get; set; }
            public decimal Riboflavin { get; set; }
            public decimal Niacin { get; set; }
            public decimal B5 { get; set; }
            public decimal B6 { get; set; }
            public decimal B7 { get; set; }
            public decimal FolicAcid { get; set; }
            public decimal B12 { get; set; }

            public decimal VitaminC { get; set; }
            public decimal VitaminD { get; set; }
            public decimal VitaminE { get; set; }
            public decimal VitaminK { get; set; }
        }
    }
}

