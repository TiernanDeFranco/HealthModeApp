using System;
using System.ComponentModel;

namespace HealthModeApp.Models
{
    public class FlairTable : INotifyPropertyChanged
    {
        int _flairID;
        public int FlairID
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

        string _flairName;
        public string FlairName
        {
            get => _flairName;
            set
            {
                if (_flairName == value)
                    return;

                _flairName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FlairName)));
            }
        }

        string _colorCode;
        public string ColorCode
        {
            get => _colorCode;
            set
            {
                if (_colorCode == value)
                    return;

                _colorCode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorCode)));
            }
        }

        bool _isBlackText;
        public bool IsBlackText
        {
            get => _isBlackText;
            set
            {
                if (_isBlackText == value)
                    return;

                _isBlackText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBlackText)));
            }
        }

        string _unlockReason;
        public string UnlockReason
        {
            get => _unlockReason;
            set
            {
                if (_unlockReason == value)
                    return;

                _unlockReason = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnlockReason)));
            }
        }

        Color _flairBG;
        public Color FlairBG
        {
            get => _flairBG;
            set
            {
                if (_flairBG == value)
                    return;

                _flairBG = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FlairBG)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

