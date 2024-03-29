﻿using System;
using System.ComponentModel;

namespace HealthModeApp.Models
{
    public class UserInfo : INotifyPropertyChanged
    {
        int _userID;
        public int UserID
        {
            get => _userID;
            set
            {
                if (_userID == value)
                    return;

                _userID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserID)));
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value)
                    return;
                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username == value)
                    return;
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password == value)
                    return;
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        private string _salt;
        public string Salt
        {
            get => _salt;
            set
            {
                if (_salt == value)
                    return;
                _salt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Salt)));
            }
        }


        private DateTime? _expDate;
        public DateTime? ExpDate
        {
            get => _expDate;
            set
            {
                if (_expDate == value)
                    return;
                _expDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpDate)));
            }
        }

        private int? _pictureID; 
        public int? ProfilePictureID
        {
            get => _pictureID;
            set
            {
                if (_pictureID == value)
                    return;
                _pictureID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProfilePictureID)));
            }
        }

        private string _pictureBGColor;
        public string PictureBGColor
        {
            get => _pictureBGColor;
            set
            {
                if (_pictureBGColor == value)
                    return;
                _pictureBGColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PictureBGColor)));
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value)
                    return;
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        private int? _flairID;
        public int? FlairID
        {
            get => _flairID;
            set
            {
                if (_flairID == value)
                    return;
                _flairID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FlairID)));
            }
        }

        private int? _weightPlan;
        public int? WeightPlan
        {
            get => _weightPlan;
            set
            {
                if (_weightPlan == value)
                    return;
                _weightPlan = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WeightPlan)));
            }
        }

        private string _mainGoals;
        public string MainGoals
        {
            get => _mainGoals;
            set
            {
                if (_mainGoals == value)
                    return;
                _mainGoals = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainGoals)));
            }
        }

        private string _units;
        public string Units
        {
            get => _units;
            set
            {
                if (_units == value)
                    return;
                _units = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Units)));
            }
        }

        private int? _sex;
        public int? Sex
        {
            get => _sex;
            set
            {
                if (_sex == value)
                    return;
                _sex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sex)));
            }
        }

        private decimal? _heightCm;
        public decimal? HeightCm
        {
            get => _heightCm;
            set
            {
                if (_heightCm == value)
                    return;
                _heightCm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeightCm)));
            }
        }

        private DateTime? _birthday;
        public DateTime? Birthday
        {
            get => _birthday;
            set
            {
                if (_birthday == value)
                    return;
                _birthday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Birthday)));
            }
        }

        private decimal? _weight;
        public decimal? Weight
        {
            get => _weight;
            set
            {
                if (_weight == value) return;
                _weight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weight)));
            }
        }

        decimal? _goalWeight;
        public decimal? GoalWeight
        {
            get => _goalWeight;
            set
            {
                if (_goalWeight == value) return;
                _goalWeight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GoalWeight)));
            }
        }

        int? _activityLevel;
        public int? ActivityLevel
        {
            get => _activityLevel;
            set
            {
                if (_activityLevel == value) return;
                _activityLevel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivityLevel)));
            }
        }

        int? _calorieGoal;
        public int? CalorieGoal
        {
            get => _calorieGoal;
            set
            {
                if (_calorieGoal == value) return;
                _calorieGoal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalorieGoal)));
            }
        }

        int? _waterGoal;
        public int? WaterGoal
        {
            get => _waterGoal;
            set
            {
                if (_waterGoal == value) return;
                _waterGoal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WaterGoal)));
            }
        }


        string? _recoveryCode;
        public string? RecoveryCode
        {
            get => _recoveryCode;
            set
            {
                if (_recoveryCode == value) return;
                _recoveryCode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecoveryCode)));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}

