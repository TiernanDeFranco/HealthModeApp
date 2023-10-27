using System;
using System.ComponentModel;

namespace HealthModeApp.Models
{
    public class ProfilePictures : INotifyPropertyChanged
    {
        int _pictureID;
        public int PictureID
        {
            get => _pictureID;
            set
            {
                if (_pictureID == value)
                    return;

                _pictureID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PictureID)));
            }
        }

        string _pictureName;
        public string PictureName
        {
            get => _pictureName;
            set
            {
                if (_pictureName == value)
                    return;

                _pictureName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PictureName)));
            }
        }

        string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath == value)
                    return;

                _filePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilePath)));
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

        ImageSource _dummyImage;
        public ImageSource DummyImage
        {
            get => _dummyImage;
            set
            {
                if (_dummyImage == value)
                    return;

                _dummyImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DummyImage)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

