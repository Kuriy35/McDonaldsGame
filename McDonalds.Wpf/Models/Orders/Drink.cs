using System.Collections.Generic;
using System.IO;
using System;
using static McDonalds.Models.Orders.Fries;

namespace McDonalds.Models.Orders
{
    public enum DrinkType
    {
        Cola,
        Juice,
        Coffee,
        Water
    }

    public enum DrinkSize
    {
        Small,
        Medium,
        Large
    }


    public class Drink : Product
    {
        private readonly string _iconPath = "pack://application:,,,/Resources/Images/Products/drink.png";
        public DrinkType Type { get; set; }
        public DrinkSize Size { get; set; }

        public Drink(ProductSharedState sharedState = null, DrinkType type = DrinkType.Cola, DrinkSize size = DrinkSize.Medium)
        {
            Type = type;
            Size = size;
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
                UpdateName();
                UpdatePrice();
                RequiredResources = new Dictionary<string, int>();
                PreparationTime = TimeSpan.FromSeconds(3);
            }

            UpdateRequiredResources();
        }

        private void UpdateName()
        {
            Name = $"{Type} ({Size})";
        }

        private void UpdatePrice()
        {
            decimal basePrice;
            switch (Type)
            {
                case DrinkType.Cola:
                    basePrice = 2.5m;
                    break;
                case DrinkType.Juice:
                    basePrice = 3.0m;
                    break;
                case DrinkType.Coffee:
                    basePrice = 2.8m;
                    break;
                case DrinkType.Water:
                    basePrice = 1.5m;
                    break;
                default:
                    basePrice = 2.5m;
                    break;
            }

            switch (Size)
            {
                case DrinkSize.Small:
                    basePrice *= 0.8m;
                    break;
                case DrinkSize.Medium:
                    break;
                case DrinkSize.Large:
                    basePrice *= 1.3m;
                    break;
                default:
                    break;
            }
        }

        private void UpdateRequiredResources()
        {
            RequiredResources.Clear();

            string resourceType = Type.ToString().ToLower();
            int amount;

            switch (Size)
            {
                case DrinkSize.Small:
                    amount = 1;
                    break;
                case DrinkSize.Medium:
                    amount = 2;
                    break;
                case DrinkSize.Large:
                    amount = 3;
                    break;
                default:
                    amount = 2;
                    break;
            }

            RequiredResources.Add(resourceType, amount);
            RequiredResources.Add("cup", 1);
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
            if (ElapsedTime >= PreparationTime)
            {
                State = ProductState.Ready;
            }
        }
    }
}