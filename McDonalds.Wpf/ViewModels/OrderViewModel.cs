using System;
using System.Collections.ObjectModel;
using System.Linq;
using McDonalds.Models.Orders;

namespace McDonalds.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private Order _order;
        private ObservableCollection<ProductViewModel> _products;

        public Order Order => _order;

        public Guid Id => _order.Id;

        public ObservableCollection<ProductViewModel> Products
        {
            get => _products;
            private set => SetProperty(ref _products, value);
        }

        public decimal TotalPrice => _order.TotalPrice;

        public bool IsCompleted
        {
            get => _order.IsCompleted;
            set
            {
                if (_order.IsCompleted != value)
                {
                    _order.IsCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDelivered
        {
            get => _order.IsDelivered;
            set
            {
                if (_order.IsDelivered != value)
                {
                    _order.IsDelivered = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CreationTime => _order.CreationTime;

        public OrderViewModel(Order order)
        {
            _order = order;
            _products = new ObservableCollection<ProductViewModel>();

            // Ініціалізуємо колекцію ProductViewModel
            if (_order.Products != null)
            {
                foreach (var product in _order.Products)
                {
                    _products.Add(new ProductViewModel(product));
                }
            }

            // Підписуємось на зміни в колекції продуктів
            _order.Products.CollectionChanged += (sender, e) =>
            {
                // Оновлюємо колекцію ViewModel продуктів при зміні базової колекції
                if (e.NewItems != null)
                {
                    foreach (Product product in e.NewItems)
                    {
                        Products.Add(new ProductViewModel(product));
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (Product product in e.OldItems)
                    {
                        var productVM = Products.FirstOrDefault(p => p.Product == product);
                        if (productVM != null)
                        {
                            Products.Remove(productVM);
                        }
                    }
                }

                OnPropertyChanged(nameof(TotalPrice));
            };
        }

        public bool AreAllProductsReady()
        {
            return _order.AreAllProductsReady();
        }
    }
}