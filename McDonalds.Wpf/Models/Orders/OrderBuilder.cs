using System;
using static McDonalds.Models.Orders.Fries;

namespace McDonalds.Models.Orders
{
    public class OrderBuilder
    {
        private Order _order;
        private ProductFlyweightFactory _flyweightFactory;

        public OrderBuilder(ProductFlyweightFactory flyweightFactory = null)
        {
            _order = new Order();
            _flyweightFactory = flyweightFactory;
        }

        public OrderBuilder AddBurger(bool cheese = true, bool lettuce = true, bool tomato = true, bool onion = true, bool spicy = false, bool doubleCheese = false, bool bacon = false)
        {
            Burger burger;

            if (_flyweightFactory != null)
            {
                var sharedState = _flyweightFactory.GetSharedState("StandardBurger");
                burger = new Burger(sharedState, cheese, lettuce, tomato, onion);
            }
            else
            {
                burger = new Burger(null, cheese, lettuce, tomato, onion);
            }


            Product decoratedBurger = burger;

            if (spicy)
            {
                if (decoratedBurger is Burger burgerItem)
                    decoratedBurger = new SpicyBurgerDecorator(burgerItem).ToProduct();
                else
                    throw new InvalidOperationException("Spicy decorator can only be applied to burgers");

            }
            if (doubleCheese)
            {
                if (decoratedBurger is Burger burgerItem)
                    decoratedBurger = new DoubleCheeseBurgerDecorator(burgerItem).ToProduct();
                else
                    throw new InvalidOperationException("Double cheese decorator can only be applied to burgers");
            }
            if (bacon)
            {
                if (decoratedBurger is Burger burgerItem)
                    decoratedBurger = new BaconBurgerDecorator(burgerItem).ToProduct();
                else
                    throw new InvalidOperationException("Bacon decorator can only be applied to burgers");
            }

            _order.Products.Add(decoratedBurger);
            return this;
        }

        public OrderBuilder AddFries(FriesSize size = FriesSize.Medium, bool withCheese = false, bool largePortion = false)
        {
            Fries fries;

            if (_flyweightFactory != null && size == FriesSize.Medium)
            {
                var sharedState = _flyweightFactory.GetSharedState("MediumFries");
                fries = new Fries(sharedState, size);
            }
            else
            {
                fries = new Fries(null, size);
            }

            Product decoratedFries = fries;

            if (withCheese)
            {
                if (decoratedFries is Fries friesItem)
                    decoratedFries = new CheeseFriesDecorator(friesItem).ToProduct();
                else
                    throw new InvalidOperationException("With cheese decorator can only be applied to fries");

            }

            _order.Products.Add(decoratedFries);
            return this;
        }

        public OrderBuilder AddDrink(DrinkType type = DrinkType.Cola, DrinkSize size = DrinkSize.Medium, bool iced = false)
        {
            Drink drink;

            if (_flyweightFactory != null && size == DrinkSize.Medium)
            {
                var sharedState = type == DrinkType.Cola
                    ? _flyweightFactory.GetSharedState("MediumCola")
                    : _flyweightFactory.GetSharedState("MediumCoffee");
                drink = new Drink(sharedState, type, size);
            }
            else
            {
                drink = new Drink(null, type, size);
            }

            Product decoratedDrink = drink;

            if (iced)
            {
                if (decoratedDrink is Drink drinkItem)
                    decoratedDrink = new IcedDrinkDecorator(drinkItem).ToProduct();
                else
                    throw new InvalidOperationException("With cheese decorator can only be applied to fries");

            }

            _order.Products.Add(decoratedDrink);
            return this;
        }

        public Order Build()
        {
            Order result = _order;
            _order = new Order();
            return result;
        }
    }
}