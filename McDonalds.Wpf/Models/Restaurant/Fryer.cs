using McDonalds.Models.Orders;

namespace McDonalds.Models.Restaurant
{
    public class Fryer : KitchenEquipment
    {
        public Fryer() : base("Fryer", "pack://application:,,,/Resources/Images/Kitchen/fryer.png")
        {
            ProcessingTime = 6.0f;
        }

        public override bool CanProcess(Product product)
        {
            return (product is Fries ||
                product is CheeseFriesDecorator) && product.State != ProductState.Ready && product.State != ProductState.Burned;
        }

        public override void StartProcessing(Product product)
        {
            if (CanProcess(product))
            {
                IsBusy = true;
                CurrentProcessingTime = 0;
                CurrentProduct = product;
                product.Process();
            }
        }

        public override void StartProcessing()
        {
            Fries Fries = new Fries();

            IsBusy = true;
            CurrentProcessingTime = 0;
            CurrentProduct = Fries;
            Fries.Process();
        }
    }
}