using McDonalds.Models.Orders;

namespace McDonalds.Models.Restaurant
{
    public class DrinksVendingMachine : KitchenEquipment
    {
        public DrinksVendingMachine() : base("Drinks machine", "pack://application:,,,/Resources/Images/Kitchen/drinks.png")
        {
            ProcessingTime = 3.0f;
        }

        public override bool CanProcess(Product product)
        {
            return (product is Drink ||
                product is IcedDrinkDecorator) && product.State != ProductState.Ready && product.State != ProductState.Burned;
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
            Drink drink = new Drink();

            IsBusy = true;
            CurrentProcessingTime = 0;
            CurrentProduct = drink;
            drink.Process();
        }
    }
}