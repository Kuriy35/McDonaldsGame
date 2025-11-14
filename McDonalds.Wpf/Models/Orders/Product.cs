using System;
using System.Collections.Generic;
using System.IO;
using McDonalds.Models.Core;
using McDonalds.ViewModels;

namespace McDonalds.Models.Orders
{
    public enum ProductState
    {
        Raw,
        Processing,
        Ready,
        Burned
    }

    public abstract class Product
    {
        public string Name { get; set; }
        public string IconPath { get; protected set; }
        public decimal Price { get; set; }
        public ProductState State { get; set; }
        public Dictionary<string, int> RequiredResources { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }

        protected Product()
        {
            State = ProductState.Raw;
            RequiredResources = new Dictionary<string, int>();
            ElapsedTime = TimeSpan.Zero;
            PreparationTime = TimeSpan.FromSeconds(5);
        }

        public abstract void Process();

        public void UpdatePreparation(TimeSpan deltaTime)
        {
            if (State == ProductState.Processing)
            {
                ElapsedTime += deltaTime;
                UpdateState();
            }
        }
        
        public virtual void UpdateState()
        {
            if (ElapsedTime.TotalSeconds >= PreparationTime.TotalSeconds * 1.5)
            {
                State = ProductState.Burned;
            }
            else if (ElapsedTime >= PreparationTime)
            {
                State = ProductState.Ready;
            }
        }
    }

    public class ProductOption : ViewModelBase
    {
        private string _name;
        private Func<Product> _createProduct;
        private bool _isAvailable;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Func<Product> CreateProduct
        {
            get => _createProduct;
            set => SetProperty(ref _createProduct, value);
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }

        public bool IsNotAvailable => !IsAvailable;

        public void CheckResources()
        {
            if (CreateProduct != null)
            {
                Product temp = CreateProduct();
                IsAvailable = ResourceManager.Instance.HasResources(temp);
            }
            else
            {
                IsAvailable = false;
            }
        }
    }

}