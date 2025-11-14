using System;
using McDonalds.Models.Core;
using McDonalds.Models.Restaurant;
using McDonalds.Models.Customers;
using McDonalds.Models.Orders;
using System.Collections.Generic;
using System.Windows;

namespace McDonalds.Models.Factory
{
    public class EasyDifficultyFactory : DifficultyFactory
    {
        public override Restaurant.Restaurant CreateRestaurant()
        {
            var restaurant = new Restaurant.Restaurant("McDonald's (Easy Mode)");

            List<Point> defaultPositions = new List<Point>();
            defaultPositions.AddRange(new[] {
                new Point(100, 100),
                new Point(300, 100),
                new Point(500, 100),
                new Point(200, 300),
                new Point(400, 300),
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

            kitchen.AddEquipment(new Grill()); 
            kitchen.AddEquipment(new Fryer()); 
            kitchen.AddEquipment(new DrinksVendingMachine()); 

            return kitchen;
        }

        public override CustomerGenerator CreateCustomerGenerator()
        {
            return new CustomerGenerator(OrderComplexity.Simple, 10, 15);
        }

        public override GameGoals CreateGameGoals()
        {
            return new GameGoals
            {
                MinimumSatisfaction = 75, 
                MinimumMoney = 150,       
                WorkdayDuration = TimeSpan.FromMinutes(2)
            };
        }

        public override void SetResources()
        {
            ResourceManager.Instance.SetResourcesByDifficulty(GameDifficulty.Easy);
        }
    }
}