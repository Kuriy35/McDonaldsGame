using System.Linq;

namespace McDonalds.Models.Orders
{
    public class SpicyBurgerDecorator : ProductDecorator
    {
        public SpicyBurgerDecorator(Burger burger) : base(burger)
        {
            base.Name = "Spicy " + burger.Name;
            base.Price = burger.Price + 1.0m;
            base.RequiredResources["spices"] = 1;   
        }

        public Product ToProduct()
        {
            return this.GetDecoratedProduct();
        }
    }

    public class DoubleCheeseBurgerDecorator : ProductDecorator
    {
        public DoubleCheeseBurgerDecorator(Burger burger) : base(burger)
        {
            base.Name = "Double Cheese " + burger.Name;
            base.Price = burger.Price + 1.5m;
            base.RequiredResources["cheese"] = 2;
        }

        public Product ToProduct()
        {
            return this.GetDecoratedProduct();
        }
    }

    public class BaconBurgerDecorator : ProductDecorator
    {
        public BaconBurgerDecorator(Burger burger) : base(burger)
        {
            base.Name = "Bacon " + burger.Name;
            base.Price = burger.Price + 1.8m;
            base.RequiredResources["bacon"] = 2;
        }

        public Product ToProduct()
        {
            return this.GetDecoratedProduct();
        }
    }

    public class CheeseFriesDecorator : ProductDecorator
    {
        public CheeseFriesDecorator(Fries fries) : base(fries)
        {
            base.Name = "Cheese " + fries.Name;
            base.Price = fries.Price + 1.2m;
            base.RequiredResources["cheese"] = 2;
        }

        public Product ToProduct()
        {
            return this.GetDecoratedProduct();
        }
    }

    public class IcedDrinkDecorator : ProductDecorator
    {
        public IcedDrinkDecorator(Drink drink) : base(drink)
        {
            base.Name = "Iced " + drink.Name;
            base.Price = drink.Price + 0.5m;
            base.RequiredResources["ice"] = 3;
        }

        public Product ToProduct()
        {
            return this.GetDecoratedProduct();
        }
    }
}