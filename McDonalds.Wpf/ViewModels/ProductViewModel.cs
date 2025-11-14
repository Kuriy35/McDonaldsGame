using System;
using McDonalds.Models.Orders;

namespace McDonalds.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private Product _product;

        public Product Product => _product;

        public string Name => _product.Name;

        public decimal Price => _product.Price;

        public ProductState State
        {
            get => _product.State;
            set
            {
                if (_product.State != value)
                {
                    _product.State = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public TimeSpan PreparationTime => _product.PreparationTime;

        public TimeSpan ElapsedTime
        {
            get => _product.ElapsedTime;
            set
            {
                if (_product.ElapsedTime != value)
                {
                    _product.ElapsedTime = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Progress));
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public double Progress => _product.PreparationTime.TotalSeconds > 0
            ? Math.Min(1.0, _product.ElapsedTime.TotalSeconds / _product.PreparationTime.TotalSeconds)
            : 0;

        public ProductViewModel(Product product)
        {
            _product = product;
        }

        public void UpdatePreparation(TimeSpan deltaTime)
        {
            _product.UpdatePreparation(deltaTime);
            OnPropertyChanged(nameof(ElapsedTime));
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(State));
        }

        public Product GetBaseProduct()
        {
            Product current = _product;
            while (current is ProductDecorator decorator)
            {
                current = decorator.GetDecoratedProduct();
            }
            return current;
        }
    }
}