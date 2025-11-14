using System;
using McDonalds.Models.Core;
using McDonalds.Models.Restaurant;
using McDonalds.Models.Customers;

namespace McDonalds.Models.Factory
{
    public abstract class DifficultyFactory
    {
        public abstract Restaurant.Restaurant CreateRestaurant();
        public abstract Kitchen CreateKitchen();
        public abstract CustomerGenerator CreateCustomerGenerator();
        public abstract GameGoals CreateGameGoals();

        public abstract void SetResources();

        public static DifficultyFactory GetFactory(GameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case GameDifficulty.Easy:
                    return new EasyDifficultyFactory();
                case GameDifficulty.Medium:
                    return new MediumDifficultyFactory();
                case GameDifficulty.Hard:
                    return new HardDifficultyFactory();
                default:
                    throw new ArgumentException("Unknown difficulty level");
            }
        }
    }
}