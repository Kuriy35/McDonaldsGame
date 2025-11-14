using System.Collections.Generic;
using System;

namespace McDonalds.Models.Orders
{
    public class Fries : Product
    {
        public enum FriesSize
        {
            Small,
            Medium,
            Large
        }

        private readonly string _iconPath = "pack://application:,,,/Resources/Images/Products/fries.png";
        public FriesSize Size { get; set; }

        public Fries(ProductSharedState sharedState = null, FriesSize size = FriesSize.Medium)
        {
            Size = size;
            IconPath = _iconPath;

            if (sharedState != null)
            {
                Name = $"{Size} Fries";
                Price = sharedState.BasePrice;
                RequiredResources = new Dictionary<string, int>(sharedState.BaseResources);
                PreparationTime = sharedState.BasePreparationTime;
            }
            else
            {
                Name = $"{Size} Fries";
                Price = GetBasePrice();
                RequiredResources = new Dictionary<string, int>();
                PreparationTime = TimeSpan.FromSeconds(6);
            }

            UpdateResources();
        }

        private decimal GetBasePrice()
        {
            decimal price = 0;

            switch (Size)
            {
                case FriesSize.Small:
                    price = 3.99m;
                    break;
                case FriesSize.Medium:
                    price = 4.99m;
                    break;
                case FriesSize.Large:
                    price = 5.99m;
                    break;
                default:
                    price = 4.99m;
                    break;
            }

            return price;
        }

        private void UpdateResources()
        {
            RequiredResources.Clear();
            RequiredResources.Add("potato", GetPotatoAmount());
            RequiredResources.Add("salt", 1);
            RequiredResources.Add("oil", GetOilAmount());
        }

        private int GetPotatoAmount()
        {
            int potatoAmount = 0;

            switch (Size)
            {
                case FriesSize.Small:
                    potatoAmount = 1;
                    break;
                case FriesSize.Medium:
                    potatoAmount = 2;
                    break;
                case FriesSize.Large:
                    potatoAmount = 3;
                    break;
                default: 
                    potatoAmount = 2;
                    break;
            }

            return potatoAmount;
        }

        private int GetOilAmount()
        {
            int oilAmount = 0;

            switch (Size)
            {
                case FriesSize.Small:
                    oilAmount = 1;
                    break;
                case FriesSize.Medium:
                    oilAmount = 2;
                    break;
                case FriesSize.Large:
                    oilAmount = 3;
                    break;
                default:
                    oilAmount = 2;
                    break;
            }

            return oilAmount;
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
            if (ElapsedTime.TotalSeconds >= PreparationTime.TotalSeconds * 1.5)
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