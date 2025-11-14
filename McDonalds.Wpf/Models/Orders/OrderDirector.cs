using System;
using System.Windows.Navigation;
using static McDonalds.Models.Orders.Fries;

namespace McDonalds.Models.Orders
{
    public abstract class OrderDirectorBase
    {
        protected OrderBuilder _builder;
        protected Random _random;
        protected bool ShouldAddBurger;
        protected bool ShouldAddFries;
        protected bool ShouldAddDrink;

        public OrderDirectorBase(OrderBuilder builder)
        {
            _builder = builder;
            _random = new Random();
        }

        public Order CreateMeal()
        {
            if (ShouldAddBurger)
            {
                AddBurger();
            }

            if (ShouldAddFries)
            {
                AddFries();
            }

            if (ShouldAddDrink)
            {
                AddDrink();
            }

            return _builder.Build();
        }

        protected abstract void AddBurger();
        protected abstract void AddFries();
        protected abstract void AddDrink();
    }

    public class StandartMealDirector : OrderDirectorBase
    {
        public StandartMealDirector(OrderBuilder builder) : base(builder)
        {
            ShouldAddBurger = true;
            ShouldAddDrink = true;
            ShouldAddFries = true;
        }

        protected override void AddBurger()
        {
            _builder.AddBurger(cheese: true, lettuce: true, tomato: true, onion: true);
        }

        protected override void AddFries()
        {
            _builder.AddFries(FriesSize.Medium);
        }

        protected override void AddDrink()
        {
            _builder.AddDrink(DrinkType.Cola, DrinkSize.Medium);
        }
    }

    public class ChildMealDirector : OrderDirectorBase
    {
        public ChildMealDirector(OrderBuilder builder) : base(builder)
        {
            ShouldAddBurger = true;
            ShouldAddDrink = true;
            ShouldAddFries = true;
        }

        protected override void AddBurger()
        {
            _builder.AddBurger(cheese: true, lettuce: false, tomato: false, onion: false);
        }

        protected override void AddFries()
        {
            _builder.AddFries(FriesSize.Small);
        }

        protected override void AddDrink()
        {
            _builder.AddDrink(DrinkType.Juice, DrinkSize.Small);
        }
    }

    public class BigMealDirector : OrderDirectorBase
    {
        public BigMealDirector(OrderBuilder builder) : base(builder)
        {
            ShouldAddBurger = true;
            ShouldAddDrink = true;
            ShouldAddFries = true;
        }

        protected override void AddBurger()
        {
            _builder.AddBurger(cheese: true, lettuce: false, tomato: false, onion: false);
            _builder.AddBurger(cheese: true, lettuce: false, tomato: false, onion: false);
        }

        protected override void AddFries()
        {
            _builder.AddFries(FriesSize.Large);
            _builder.AddFries(FriesSize.Medium);
        }

        protected override void AddDrink()
        {
            _builder.AddDrink(DrinkType.Cola, DrinkSize.Large);
            _builder.AddDrink(DrinkType.Juice, DrinkSize.Large);
        }
    }

    public class OrderDirector
    {
        private OrderBuilder _builder;

        public OrderDirector(OrderBuilder builder)
        {
            _builder = builder;
        }

        public Order CreateStandardMeal()
        {
            OrderDirectorBase director = new StandartMealDirector(_builder);
            return director.CreateMeal();
        }

        public Order CreateChildMeal()
        {
            OrderDirectorBase director = new ChildMealDirector(_builder);
            return director.CreateMeal();
        }

        public Order CreateBigMeal()
        {
            OrderDirectorBase director = new BigMealDirector(_builder);
            return director.CreateMeal();
        }
    }
}