using System.Collections.Generic;

namespace McDonalds.Models.Orders
{
    public abstract class ProductDecorator : Product
    {
        protected Product _decoratedProduct;

        protected ProductDecorator(Product product)
        {
            _decoratedProduct = product;
            Name = _decoratedProduct.Name;
            Price = _decoratedProduct.Price;
            State = _decoratedProduct.State;
            RequiredResources = new Dictionary<string, int>(_decoratedProduct.RequiredResources);
            PreparationTime = _decoratedProduct.PreparationTime;
            ElapsedTime = _decoratedProduct.ElapsedTime;
        }

        public override void Process()
        {
            _decoratedProduct.Process();
        }

        public override void UpdateState()
        {
            _decoratedProduct.UpdateState();
        }

        public Product GetDecoratedProduct()
        {
            _decoratedProduct.Name = Name;
            _decoratedProduct.Price = Price;
            _decoratedProduct.State = State;
            _decoratedProduct.RequiredResources = RequiredResources;
            _decoratedProduct.PreparationTime = PreparationTime;
            _decoratedProduct.ElapsedTime = ElapsedTime;
            return _decoratedProduct;
        }
    }
}