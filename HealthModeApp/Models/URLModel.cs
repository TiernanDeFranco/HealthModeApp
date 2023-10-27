using System;
using System.ComponentModel;

namespace HealthModeApp.Models
{
	public class URLModel : INotifyPropertyChanged
    {
        int _linkID;
        public int LinkID
        {
            get => _linkID;
            set
            {
                if (_linkID == value)
                    return;

                _linkID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LinkID)));
            }
        }

        int _linkCat;
        public int LinkCategory
        {
            get => _linkCat;
            set
            {
                if (_linkCat == value)
                    return;

                _linkCat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LinkCategory)));
            }
        }

        string _linkName;
        public string LinkName
        {
            get => _linkName;
            set
            {
                if (_linkName == value)
                    return;

                _linkName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LinkName)));
            }
        }

        decimal _itemPrice;
        public decimal ItemPrice
        {
            get => _itemPrice;
            set
            {
                if (_itemPrice == value)
                    return;

                _itemPrice = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemPrice)));
            }
        }

        string _linkURL;
        public string LinkURL
        {
            get => _linkURL;
            set
            {
                if (_linkURL == value)
                    return;

                _linkURL = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LinkURL)));
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
    }
}

