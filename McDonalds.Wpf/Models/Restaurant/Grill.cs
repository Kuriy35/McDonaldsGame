using McDonalds.Models.Orders;

namespace McDonalds.Models.Restaurant
{
    public class Grill : KitchenEquipment
    {
        public Grill() : base("Grill", "pack://application:,,,/Resources/Images/Kitchen/grill.png")
        {
            ProcessingTime = 8.0f;
        }

        public override bool CanProcess(Product product)
        {
            return (product is Burger || 
                product is SpicyBurgerDecorator ||
                product is DoubleCheeseBurgerDecorator ||
                product is BaconBurgerDecorator) && product.State != ProductState.Ready && product.State != ProductState.Burned;
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
            Burger burger = new Burger();

            IsBusy = true;
            CurrentProcessingTime = 0;
            CurrentProduct = burger;
            burger.Process();
        }
    }
}