using System.Collections.Generic;
using System;

namespace McDonalds.Models.Orders
{
    public class ProductSharedState
    {
        public string BaseName { get; }
        public decimal BasePrice { get; }
        public Dictionary<string, int> BaseResources { get; }
        public TimeSpan BasePreparationTime { get; }

        public ProductSharedState(string name, decimal price, Dictionary<string, int> resources, TimeSpan prepTime)
        {
            BaseName = name;
            BasePrice = price;
            BaseResources = new Dictionary<string, int>(resources);
            BasePreparationTime = prepTime;
        }
    }

    public class ProductFlyweightFactory
    {
        private readonly Dictionary<string, ProductSharedState> _sharedStates = new Dictionary<string, ProductSharedState>();

        public ProductSharedState GetSharedState(string key)
        {
            if (!_sharedStates.ContainsKey(key))
            {
                switch (key)
                {
                    case "StandardBurger":
                        var burgerResources = new Dictionary<string, int>
                    {
                        {"beef_patty", 1}, {"bun", 2}, {"cheese", 1},
                        {"lettuce", 1}, {"tomato", 1}, {"onion", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Burger", 5.99m, burgerResources, TimeSpan.FromSeconds(5));
                        break;

                    case "Cheeseburger":
                        var cheeseburgerResources = new Dictionary<string, int>
                    {
                        {"beef_patty", 1}, {"bun", 2}, {"cheese", 2},
                        {"lettuce", 1}, {"tomato", 1}, {"onion", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Cheeseburger", 6.49m, cheeseburgerResources, TimeSpan.FromSeconds(5));
                        break;

                    case "BaconBurger":
                        var baconBurgerResources = new Dictionary<string, int>
                    {
                        {"beef_patty", 1}, {"bun", 2}, {"cheese", 1},
                        {"lettuce", 1}, {"tomato", 1}, {"onion", 1},
                        {"bacon", 2}
                    };
                        _sharedStates[key] = new ProductSharedState("Bacon Burger", 7.49m, baconBurgerResources, TimeSpan.FromSeconds(6));
                        break;

                    case "SpicyBurger":
                        var spicyBurgerResources = new Dictionary<string, int>
                    {
                        {"beef_patty", 1}, {"bun", 2}, {"cheese", 1},
                        {"lettuce", 1}, {"tomato", 1}, {"onion", 1},
                        {"spicy_sauce", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Spicy Burger", 6.99m, spicyBurgerResources, TimeSpan.FromSeconds(6));
                        break;

                    case "DoubleCheeseBurger":
                        var doubleCheeseResources = new Dictionary<string, int>
                    {
                        {"beef_patty", 1}, {"bun", 2}, {"cheese", 2},
                        {"lettuce", 1}, {"tomato", 1}, {"onion", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Double Cheese Burger", 7.29m, doubleCheeseResources, TimeSpan.FromSeconds(6));
                        break;

                    case "BaconSpicyBurger":
                        var baconSpicyResources = new Dictionary<string, int>
                    {
                        {"beef_patty", 1}, {"bun", 2}, {"cheese", 1},
                        {"lettuce", 1}, {"tomato", 1}, {"onion", 1},
                        {"bacon", 2}, {"spicy_sauce", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Bacon Spicy Burger", 8.49m, baconSpicyResources, TimeSpan.FromSeconds(7));
                        break;

                    // Fries
                    case "SmallFries":
                        var smallFriesResources = new Dictionary<string, int>
                    {
                        {"potato", 1}, {"salt", 1}, {"oil", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Small Fries", 2.99m, smallFriesResources, TimeSpan.FromSeconds(4));
                        break;

                    case "MediumFries":
                        var mediumFriesResources = new Dictionary<string, int>
                    {
                        {"potato", 2}, {"salt", 1}, {"oil", 2}
                    };
                        _sharedStates[key] = new ProductSharedState("Medium Fries", 3.99m, mediumFriesResources, TimeSpan.FromSeconds(6));
                        break;

                    case "LargeFries":
                        var largeFriesResources = new Dictionary<string, int>
                    {
                        {"potato", 3}, {"salt", 1}, {"oil", 3}
                    };
                        _sharedStates[key] = new ProductSharedState("Large Fries", 4.99m, largeFriesResources, TimeSpan.FromSeconds(8));
                        break;

                    case "SmallCheeseFries":
                        var smallCheeseFriesResources = new Dictionary<string, int>
                    {
                        {"potato", 1}, {"salt", 1}, {"oil", 1}, {"cheese", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Small Cheese Fries", 3.99m, smallCheeseFriesResources, TimeSpan.FromSeconds(5));
                        break;

                    case "MediumCheeseFries":
                        var mediumCheeseFriesResources = new Dictionary<string, int>
                    {
                        {"potato", 2}, {"salt", 1}, {"oil", 2}, {"cheese", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Medium Cheese Fries", 4.99m, mediumCheeseFriesResources, TimeSpan.FromSeconds(7));
                        break;

                    case "LargeCheeseFries":
                        var largeCheeseFriesResources = new Dictionary<string, int>
                    {
                        {"potato", 3}, {"salt", 1}, {"oil", 3}, {"cheese", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Large Cheese Fries", 5.99m, largeCheeseFriesResources, TimeSpan.FromSeconds(9));
                        break;

                    case "SmallCola":
                        var smallColaResources = new Dictionary<string, int>
                    {
                        {"cola", 1}, {"cup", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Cola (Small)", 1.99m, smallColaResources, TimeSpan.FromSeconds(2));
                        break;

                    case "MediumCola":
                        var mediumColaResources = new Dictionary<string, int>
                    {
                        {"cola", 2}, {"cup", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Cola (Medium)", 2.5m, mediumColaResources, TimeSpan.FromSeconds(3));
                        break;

                    case "LargeCola":
                        var largeColaResources = new Dictionary<string, int>
                    {
                        {"cola", 3}, {"cup", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Cola (Large)", 3.99m, largeColaResources, TimeSpan.FromSeconds(4));
                        break;

                    case "SmallIcedCola":
                        var smallIcedColaResources = new Dictionary<string, int>
                    {
                        {"cola", 1}, {"cup", 1}, {"ice", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Iced Cola (Small)", 2.29m, smallIcedColaResources, TimeSpan.FromSeconds(3));
                        break;

                    case "MediumIcedCola":
                        var mediumIcedColaResources = new Dictionary<string, int>
                    {
                        {"cola", 2}, {"cup", 1}, {"ice", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Iced Cola (Medium)", 2.79m, mediumIcedColaResources, TimeSpan.FromSeconds(4));
                        break;

                    case "LargeIcedCola":
                        var largeIcedColaResources = new Dictionary<string, int>
                    {
                        {"cola", 3}, {"cup", 1}, {"ice", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Iced Cola (Large)", 3.49m, largeIcedColaResources, TimeSpan.FromSeconds(5));
                        break;

                    case "SmallCoffee":
                        var smallCoffeeResources = new Dictionary<string, int>
                    {
                        {"coffee", 1}, {"cup", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Coffee (Small)", 1.99m, smallCoffeeResources, TimeSpan.FromSeconds(3));
                        break;

                    case "MediumCoffee":
                        var mediumCoffeeResources = new Dictionary<string, int>
                    {
                        {"coffee", 2}, {"cup", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Coffee (Medium)", 2.8m, mediumCoffeeResources, TimeSpan.FromSeconds(4));
                        break;

                    case "LargeCoffee":
                        var largeCoffeeResources = new Dictionary<string, int>
                    {
                        {"coffee", 3}, {"cup", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Coffee (Large)", 3.5m, largeCoffeeResources, TimeSpan.FromSeconds(5));
                        break;

                    case "SmallIcedCoffee":
                        var smallIcedCoffeeResources = new Dictionary<string, int>
                    {
                        {"coffee", 1}, {"cup", 1}, {"ice", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Iced Coffee (Small)", 2.29m, smallIcedCoffeeResources, TimeSpan.FromSeconds(4));
                        break;

                    case "MediumIcedCoffee":
                        var mediumIcedCoffeeResources = new Dictionary<string, int>
                    {
                        {"coffee", 2}, {"cup", 1}, {"ice", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Iced Coffee (Medium)", 3.29m, mediumIcedCoffeeResources, TimeSpan.FromSeconds(5));
                        break;

                    case "LargeIcedCoffee":
                        var largeIcedCoffeeResources = new Dictionary<string, int>
                    {
                        {"coffee", 3}, {"cup", 1}, {"ice", 1}
                    };
                        _sharedStates[key] = new ProductSharedState("Iced Coffee (Large)", 3.99m, largeIcedCoffeeResources, TimeSpan.FromSeconds(6));
                        break;

                    default:
                        throw new ArgumentException("Unsupported product type");
                }
            }
            return _sharedStates[key];
        }
    }
}