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

        string _barcode;
        public string Barcode { get => _barcode; set { if (_barcode == value) return; _barcode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Barcode))); } }

        string _foodname;
        public string FoodName { get => _foodname; set { if (_foodname == value) return; _foodname = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoodName))); } }

        string _brand;
        public string Brand { get => _brand; set { if (_brand == value) return; _brand = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Brand))); } }

        decimal _servingsize;
        public decimal ServingSize { get => _servingsize; set { if (_servingsize == value) return; _servingsize = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServingSize))); } }

        string _servingname;
        public string ServingName { get => _servingname; set { if (_servingname == value) return; _servingname = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServingName))); } }

        decimal _calories;
        public decimal Calories { get => _calories; set { if (_calories == value) return; _calories = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Calories))); } }

        decimal _carbs;
        public decimal Carbs { get => _carbs; set { if (_carbs == value) return; _carbs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Carbs))); } }

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

        decimal _fat;
        public decimal Fat { get => _fat; set { if (_fat == value) return; _fat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Fat))); } }

            decimal _satfat;
            public decimal SatFat { get => _satfat; set { if (_satfat == value) return; _satfat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SatFat))); } }

            decimal _punsatfat;
            public decimal PUnSatFat { get => _punsatfat; set { if (_punsatfat == value) return; _punsatfat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PUnSatFat))); } }

            decimal _munsatfat;
            public decimal MUnSatFat { get => _munsatfat; set { if (_munsatfat == value) return; _munsatfat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MUnSatFat))); } }

            decimal _transfat;
            public decimal TransFat { get => _transfat; set { if (_transfat == value) return; _transfat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransFat))); } }

        decimal _protein;
        public decimal Protein { get => _protein; set { if (_protein == value) return; _protein = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Protein))); } }

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

        decimal _thiamin;
        public decimal Thiamin { get => _thiamin; set { if (_thiamin == value) return; _thiamin = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Thiamin))); } }

        decimal _riboflavin;
        public decimal Riboflavin { get => _riboflavin; set { if (_riboflavin == value) return; _riboflavin = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Riboflavin))); } }

        decimal _niacin;
        public decimal Niacin { get => _niacin; set { if (_niacin == value) return; _niacin = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Niacin))); } }

        decimal _b5;
        public decimal B5 { get => _b5; set { if (_b5 == value) return; _b5 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(B5))); } }

        decimal _b6;
        public decimal B6 { get => _b6; set { if (_b6 == value) return; _b6 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(B6))); } }

        decimal _b7;
        public decimal B7 { get => _b7; set { if (_b7 == value) return; _b7 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(B7))); } }

        decimal _folicacid;
        public decimal FolicAcid { get => _folicacid; set { if (_folicacid == value) return; _folicacid = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FolicAcid))); } }

        decimal _b12;
        public decimal B12 { get => _b12; set { if (_b12 == value) return; _b12 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(B12))); } }

        decimal _vitaminc;
        public decimal VitaminC { get => _vitaminc; set { if (_vitaminc == value) return; _vitaminc = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VitaminC))); } }

        decimal _vitamind;
        public decimal VitaminD { get => _vitamind; set { if (_vitamind == value) return; _vitamind = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VitaminD))); } }

        decimal _vitamine;
        public decimal VitaminE { get => _vitamine; set { if (_vitamine == value) return; _vitamine = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VitaminE))); } }

        decimal _vitamink;
        public decimal VitaminK { get => _vitamink; set { if (_vitamink == value) return; _vitamink = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VitaminK))); } }

       
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

