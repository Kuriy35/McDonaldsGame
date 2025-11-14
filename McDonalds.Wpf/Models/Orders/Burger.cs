using System;
using System.Collections.Generic;

namespace McDonalds.Models.Orders
{
    public class Burger : Product
    {
        private readonly string _iconPath = "pack://application:,,,/Resources/Images/Products/burger.png";
        public bool HasCheese { get; set; }
        public bool HasLettuce { get; set; }
        public bool HasTomato { get; set; }
        public bool HasOnion { get; set; }

        public Burger(ProductSharedState sharedState = null, bool cheese = false, bool lettuce = false, bool tomato = false, bool onion = false)
        {
            IconPath = _iconPath;
            if (sharedState != null)
            {
                Name = sharedState.BaseName;
                Price = sharedState.BasePrice;
                RequiredResources = new Dictionary<string, int>(sharedState.BaseResources);
                PreparationTime = sharedState.BasePreparationTime;
            }
            else
            {
                Name = "Burger";
                Price = 5.99m;
                PreparationTime = TimeSpan.FromSeconds(5);
                RequiredResources = new Dictionary<string, int>
                {
                    {"beef_patty", 1},
                    {"bun", 2}
                };
            }

            HasCheese = true;
            HasLettuce = true;
            HasTomato = true;
            HasOnion = true;

            UpdateResourcesBasedOnIngredients();
        }

        private void UpdateResourcesBasedOnIngredients()
        {
            if (HasCheese)
            {
                RequiredResources["cheese"] = 1;
            }
            if (HasLettuce)
            {
                RequiredResources["lettuce"] = 1;
            }
            if (HasTomato)
            {
                RequiredResources["tomato"] = 1;
            }
            if (HasOnion)
            {
                RequiredResources["onion"] = 1;
            }
        }

        public override void Process()
        {
            if (State == ProductState.Raw)
            {
                State = ProductState.Processing;
            }
        }

        public override void UpdateState()
        {
            if (ElapsedTime.TotalSeconds >= PreparationTime.TotalSeconds * 1.35)
            {
                State = ProductState.Burned;
            }
            else if (ElapsedTime >= PreparationTime)
            {
                State = ProductState.Ready;
            }
        }
    }
}