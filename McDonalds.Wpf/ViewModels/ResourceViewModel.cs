using System;
using System.Text.RegularExpressions;
using McDonalds.Models;

namespace McDonalds.ViewModels
{
    public class ResourceViewModel : ViewModelBase
    {
        private readonly Resource _resource;
        private int _tradeQuantity;
        private bool _isHighlighted;

        public ResourceViewModel(Resource resource)
        {
            _resource = resource ?? throw new ArgumentNullException(nameof(resource));
            _tradeQuantity = 1;
            _isHighlighted = false;
        }

        public int Id => _resource.Id;
        public string Name => _resource.Name;

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_resource.Name))
                    return string.Empty;

                var text = _resource.Name.Replace("_", " ");
                text = Regex.Replace(text, @"\b\w", m => m.Value.ToUpper());
                return text;
            }
        }

        public int Quantity
        {
            get => _resource.Quantity;
            set
            {
                if (_resource.Quantity != value)
                {
                    _resource.Quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal BuyPrice => _resource.BuyPrice;
        public decimal SellPrice => _resource.SellPrice;

        public int TradeQuantity
        {
            get => _tradeQuantity;
            set
            {
                if (value < 1) value = 1;
                if (SetProperty(ref _tradeQuantity, value))
                {
                    OnPropertyChanged();
                }
            }
        }

        public Resource Resource => _resource;

        public bool IsHighlighted
        {
            get => _isHighlighted;
            set => SetProperty(ref _isHighlighted, value);
        }

        public void RefreshFromResource()
        {
            OnPropertyChanged(nameof(Quantity));
            OnPropertyChanged(nameof(BuyPrice));
            OnPropertyChanged(nameof(SellPrice));
        }
    }
}

