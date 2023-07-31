using System;
using System.ComponentModel;

namespace HealthModeApp.Models
{
    public class ExerciseModel : INotifyPropertyChanged
    {
		int _exerciseID;
        public int ExerciseID
        {
            get => _exerciseID;
            set
            {
                if (_exerciseID == value)
                    return;

                _exerciseID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExerciseID)));
            }
        }

        string _exerciseName;
        public string ExerciseName
        {
            get => _exerciseName;
            set
            {
                if (_exerciseName == value)
                    return;

                _exerciseName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExerciseName)));
            }
        }

        string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;

                _description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        byte[] _thumbnail;
        public byte[] Thumbnail
        {
            get => _thumbnail;
            set
            {
                if (_thumbnail == value)
                    return;

                _thumbnail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Thumbnail)));
            }
        }

        byte[] _video;
        public byte[] Video
        {
            get => _video;
            set
            {
                if (_video == value)
                    return;

                _video = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Video)));
            }
        }

        string _equipment;
        public string Equipment
        {
            get => _equipment;
            set
            {
                if (_equipment == value)
                    return;

                _equipment = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Equipment)));
            }
        }

        string _primaryMuscles;
        public string PrimaryMuscles
        {
            get => _primaryMuscles;
            set
            {
                if (_primaryMuscles == value)
                    return;

                _primaryMuscles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrimaryMuscles)));
            }
        }

        string _secondaryMuscles;
        public string SecondaryMuscles
        {
            get => _secondaryMuscles;
            set
            {
                if (_secondaryMuscles == value)
                    return;

                _secondaryMuscles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SecondaryMuscles)));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}

