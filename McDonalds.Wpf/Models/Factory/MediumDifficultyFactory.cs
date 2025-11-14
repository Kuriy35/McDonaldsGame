using System;
using McDonalds.Models.Core;
using McDonalds.Models.Restaurant;
using McDonalds.Models.Customers;
using McDonalds.Models.Orders;
using System.Collections.Generic;
using System.Windows;

namespace McDonalds.Models.Factory
{
    public class MediumDifficultyFactory : DifficultyFactory
    {
        public override Restaurant.Restaurant CreateRestaurant()
        {
            var restaurant = new Restaurant.Restaurant("McDonald's (Medium Mode)");

            List<Point> defaultPositions = new List<Point>();
            defaultPositions.AddRange(new[] {
                new Point(100, 100),
                new Point(300, 100),
                new Point(500, 100),
                new Point(200, 300),
                new Point(400, 300),
                new Point(100, 500),
                new Point(300, 500),
                new Point(500, 500),
            });

            var diningArea = new DiningArea(defaultPositions);
            restaurant.DiningArea = diningArea;

            restaurant.AddCounters(1);

            restaurant.Kitchen = CreateKitchen();

            return restaurant;
        }

        public override Kitchen CreateKitchen()
        {
            var kitchen = new Kitchen();

            for (int i = 0; i < 2; ++i)
            {
                kitchen.AddEquipment(new Grill()); 
                kitchen.AddEquipment(new Fryer()); 
                kitchen.AddEquipment(new DrinksVendingMachine()); 
            }

            return kitchen;
        }

        public override CustomerGenerator CreateCustomerGenerator()
        {
            return new CustomerGenerator(OrderComplexity.Medium, 5, 10);
        }

        public override GameGoals CreateGameGoals()
        {
            return new GameGoals
            {
                MinimumSatisfaction = 85, 
                MinimumMoney = 500,       
                WorkdayDuration = TimeSpan.FromMinutes(5) 
            };
        }

        public override void SetResources()
        {
            ResourceManager.Instance.SetResourcesByDifficulty(GameDifficulty.Medium);
        }
    }
}