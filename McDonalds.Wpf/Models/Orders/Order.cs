using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace McDonalds.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public decimal TotalPrice => Products?.Sum(p => p.Price) ?? 0;
        public bool IsCompleted { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime CreationTime { get; set; }

        public Order()
        {
            Id = Guid.NewGuid();
            Products = new ObservableCollection<Product>();
            CreationTime = DateTime.Now;
            IsCompleted = false;
            IsDelivered = false;
        }

        public bool AreAllProductsReady()
        {
            return Products != null && Products.Count > 0 && Products.All(p => p.State == ProductState.Ready);
        }
    }

    public enum OrderComplexity
    {
        Simple,
        Medium,
        Complex
    }
}