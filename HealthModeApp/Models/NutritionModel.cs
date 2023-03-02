using System;
using System.ComponentModel;

namespace HealthModeApp.Models
{
    public class NutritionModel : INotifyPropertyChanged
    {
        int _foodid;
        public int FoodId
        {
            get => _foodid;
            set
            {
                if (_foodid == value)
                    return;

                _foodid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoodId)));
            }
        }

        int _barcode;
        public int Barcode { get => _barcode; set { if (_barcode == value) return; _barcode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Barcode))); } }

        string _foodname;
        public string FoodName { get => _foodname; set { if (_foodname == value) return; _foodname = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoodName))); } }

        decimal _servingsize;
        public decimal ServingSize { get => _servingsize; set { if (_servingsize == value) return; _servingsize = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServingSize))); } }

        string _servingtype;
        public  string ServingType { get => _servingtype; set { if (_servingtype == value) return; _servingtype = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServingType))); } }

        int _calories;
        public int Calories { get => _calories; set { if (_calories == value) return; _calories = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Calories))); } }

        int _protein;
        public int Protein { get => _protein; set { if (_protein == value) return; _protein = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Protein))); } }

        int _carbs;
        public int Carbs { get => _carbs; set { if (_carbs == value) return; _carbs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Carbs))); } }

        int _fat;
        public int Fat { get => _fat; set { if (_fat == value) return; _fat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Fat))); } }

        decimal _satfat;
        public decimal SatFat { get => _satfat; set { if (_satfat == value) return; _satfat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SatFat))); } }

        decimal _punsatfat;
        public decimal PUnSatFat { get => _punsatfat; set { if (_punsatfat == value) return; _punsatfat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PUnSatFat))); } }

        decimal _munsatfat;
        public decimal MUnSatFat { get => _munsatfat; set { if (_munsatfat == value) return; _munsatfat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MUnSatFat))); } }

        decimal _transfat;
        public decimal TransFat { get => _transfat; set { if (_transfat == value) return; _transfat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransFat))); } }

        decimal _sugar;
        public decimal Sugar { get => _sugar; set { if (_sugar == value) return; _sugar = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sugar))); } }

        decimal _addsugar;
        public decimal AddSugar { get => _addsugar; set { if (_addsugar == value) return; _addsugar = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddSugar))); } }

        decimal _sugaralc;
        public decimal SugarAlc { get => _sugaralc; set { if (_sugaralc == value) return; _sugaralc = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SugarAlc))); } }

        decimal _fiber;
        public decimal Fiber { get => _fiber; set { if (_fiber == value) return; _fiber = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Fiber))); } }

        decimal _netcarb;
        public decimal NetCarb { get => _netcarb; set { if (_netcarb == value) return; _netcarb = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NetCarb))); } }

        decimal _iron;
        public decimal Iron { get => _iron; set { if (_iron == value) return; _iron = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Iron))); } }

        decimal _calcium;
        public decimal Calcium { get => _calcium; set { if (_calcium == value) return; _calcium = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Calcium))); } }

        decimal _potassium;
        public decimal Potassium { get => _potassium; set { if (_potassium == value) return; _potassium = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Potassium))); } }

        decimal _sodium;
        public decimal Sodium { get => _sodium; set { if (_sodium == value) return; _sodium = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sodium))); } }

        decimal _cholesterol;
        public decimal Cholesterol { get => _cholesterol; set { if (_cholesterol == value) return; _cholesterol = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cholesterol))); } }

        decimal _vitamina;
        public decimal VitaminA { get => _vitamina; set { if (_vitamina == value) return; _vitamina = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VitaminA))); } }

        decimal _vitaminc;
        public decimal VitaminC { get => _vitaminc; set { if (_vitaminc == value) return; _vitaminc = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VitaminC))); } }

        decimal _vitamind;
        public decimal VitaminD { get => _vitamind; set { if (_vitamind == value) return; _vitamind = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VitaminD))); } }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}

