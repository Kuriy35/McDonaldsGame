using McDonalds.Models;
using McDonalds.Models.Core;

namespace McDonalds.Data
{
    public static class SeedData
    {
        public static void Initialize(McDonaldsContext context)
        {
            context.Database.EnsureCreated();

            if (context.DifficultyResources.Any())
                return;

            var difficultyResources = new[]
            {
                CreateDR(GameDifficulty.Easy, "beef_patty", 30, 2.80m, 2.20m),
                CreateDR(GameDifficulty.Easy, "bun", 70, 0.40m, 0.30m),
                CreateDR(GameDifficulty.Easy, "bacon", 40, 3.20m, 2.60m),
                CreateDR(GameDifficulty.Easy, "spices", 30, 1.10m, 0.85m),
                CreateDR(GameDifficulty.Easy, "cheese", 40, 1.60m, 1.25m),
                CreateDR(GameDifficulty.Easy, "lettuce", 40, 0.45m, 0.35m),
                CreateDR(GameDifficulty.Easy, "tomato", 40, 0.65m, 0.50m),
                CreateDR(GameDifficulty.Easy, "onion", 40, 0.55m, 0.42m),
                CreateDR(GameDifficulty.Easy, "potato", 70, 0.25m, 0.18m),
                CreateDR(GameDifficulty.Easy, "salt", 50, 0.12m, 0.08m),
                CreateDR(GameDifficulty.Easy, "oil", 120, 1.30m, 1.00m),
                CreateDR(GameDifficulty.Easy, "cola", 70, 0.85m, 0.65m),
                CreateDR(GameDifficulty.Easy, "juice", 40, 1.10m, 0.85m),
                CreateDR(GameDifficulty.Easy, "coffee", 40, 1.60m, 1.25m),
                CreateDR(GameDifficulty.Easy, "water", 70, 0.35m, 0.25m),
                CreateDR(GameDifficulty.Easy, "cup", 90, 0.18m, 0.12m),
                CreateDR(GameDifficulty.Easy, "ice", 80, 0.06m, 0.04m),

                CreateDR(GameDifficulty.Medium, "beef_patty", 20, 3.10m, 2.30m),
                CreateDR(GameDifficulty.Medium, "bun", 55, 0.47m, 0.33m),
                CreateDR(GameDifficulty.Medium, "bacon", 30, 3.50m, 2.70m),
                CreateDR(GameDifficulty.Medium, "spices", 25, 1.25m, 0.95m),
                CreateDR(GameDifficulty.Medium, "cheese", 35, 1.75m, 1.35m),
                CreateDR(GameDifficulty.Medium, "lettuce", 30, 0.52m, 0.38m),
                CreateDR(GameDifficulty.Medium, "tomato", 30, 0.72m, 0.55m),
                CreateDR(GameDifficulty.Medium, "onion", 30, 0.62m, 0.47m),
                CreateDR(GameDifficulty.Medium, "potato", 60, 0.30m, 0.22m),
                CreateDR(GameDifficulty.Medium, "salt", 45, 0.15m, 0.10m),
                CreateDR(GameDifficulty.Medium, "oil", 130, 1.45m, 1.05m),
                CreateDR(GameDifficulty.Medium, "cola", 60, 0.95m, 0.70m),
                CreateDR(GameDifficulty.Medium, "juice", 35, 1.25m, 0.95m),
                CreateDR(GameDifficulty.Medium, "coffee", 35, 1.75m, 1.35m),
                CreateDR(GameDifficulty.Medium, "water", 60, 0.40m, 0.28m),
                CreateDR(GameDifficulty.Medium, "cup", 100, 0.22m, 0.15m),
                CreateDR(GameDifficulty.Medium, "ice", 80, 0.08m, 0.05m),

                CreateDR(GameDifficulty.Hard, "beef_patty", 15, 3.50m, 2.40m),
                CreateDR(GameDifficulty.Hard, "bun", 40, 0.60m, 0.35m),
                CreateDR(GameDifficulty.Hard, "bacon", 25, 3.90m, 2.80m),
                CreateDR(GameDifficulty.Hard, "spices", 20, 1.50m, 1.05m),
                CreateDR(GameDifficulty.Hard, "cheese", 30, 2.00m, 1.45m),
                CreateDR(GameDifficulty.Hard, "lettuce", 25, 0.65m, 0.42m),
                CreateDR(GameDifficulty.Hard, "tomato", 25, 0.85m, 0.60m),
                CreateDR(GameDifficulty.Hard, "onion", 25, 0.75m, 0.52m),
                CreateDR(GameDifficulty.Hard, "potato", 50, 0.40m, 0.25m),
                CreateDR(GameDifficulty.Hard, "salt", 40, 0.20m, 0.12m),
                CreateDR(GameDifficulty.Hard, "oil", 100, 1.80m, 1.10m),
                CreateDR(GameDifficulty.Hard, "cola", 50, 1.10m, 0.75m),
                CreateDR(GameDifficulty.Hard, "juice", 30, 1.50m, 1.05m),
                CreateDR(GameDifficulty.Hard, "coffee", 30, 2.00m, 1.45m),
                CreateDR(GameDifficulty.Hard, "water", 50, 0.50m, 0.32m),
                CreateDR(GameDifficulty.Hard, "cup", 80, 0.28m, 0.17m),
                CreateDR(GameDifficulty.Hard, "ice", 60, 0.12m, 0.07m),
            };

            context.DifficultyResources.AddRange(difficultyResources);
            context.SaveChanges();

            var easyResources = difficultyResources
                .Where(dr => dr.Difficulty == GameDifficulty.Easy)
                .Select(dr => new Resource
                {
                    Name = dr.ResourceName,
                    Quantity = dr.BaseQuantity,
                    BuyPrice = dr.BuyPrice,
                    SellPrice = dr.SellPrice
                });

            context.Resources.AddRange(easyResources);
            context.SaveChanges();
        }

        private static DifficultyResource CreateDR(GameDifficulty diff, string name, int qty, decimal buy, decimal sell)
        {
            return new DifficultyResource
            {
                Difficulty = diff,
                ResourceName = name,
                BaseQuantity = qty > 0 ? qty : 1,
                BuyPrice = buy > 0 ? buy : 0.01m,
                SellPrice = sell > 0 && sell < buy ? sell : buy * 0.85m
            };
        }
    }
}