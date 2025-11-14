using System;
using System.Collections.Generic;
using McDonalds.Models.Orders;
using McDonalds.Models.Core;

namespace McDonalds.Models.Restaurant
{
    public class Kitchen
    {
        public List<KitchenEquipment> Equipment { get; private set; }

        public Kitchen()
        {
            Equipment = new List<KitchenEquipment>();
        }

        public void AddEquipment(KitchenEquipment equipment)
        {
            Equipment.Add(equipment);
        }
    }

    //public class KitchenFacade
    //{
    //    private readonly Kitchen _kitchen;
    //    private readonly ResourceManager _resourceManager;

    //    public KitchenFacade(Kitchen kitchen)
    //    {
    //        _kitchen = kitchen ?? throw new ArgumentNullException(nameof(kitchen));
    //        _resourceManager = ResourceManager.Instance;
    //    }

    //    public bool ProcessProduct(Product product)
    //    {
    //        var equipment = FindAvailableEquipment(product);
    //        if (equipment == null) return false;

    //        if (_resourceManager.HasResources(product))
    //        {
    //            equipment.StartProcessing(product);
    //            return true;
    //        }
    //        return false;
    //    }

    //    public Product TakeReadyProduct(KitchenEquipment equipment)
    //    {
    //        return equipment?.TakeReadyProduct();
    //    }

    //    public void UpdateEquipment(float deltaTime)
    //    {
    //        foreach (var equipment in _kitchen.Equipment)
    //        {
    //            equipment.UpdateProcessing(deltaTime);
    //        }
    //    }

    //    public bool ProcessOrder(Order order)
    //    {
    //        foreach (var product in order.Products)
    //        {
    //            if (!ProcessProduct(product))
    //                return false;
    //        }
    //        return true;
    //    }

    //    public List<Product> GetReadyProducts()
    //    {
    //        var readyProducts = new List<Product>();
    //        foreach (var equipment in _kitchen.Equipment)
    //        {
    //            if (equipment.HasReadyProduct)
    //            {
    //                readyProducts.Add(equipment.TakeReadyProduct());
    //            }
    //        }
    //        return readyProducts;
    //    }

    //    private KitchenEquipment FindAvailableEquipment(Product product)
    //    {
    //        foreach (var equipment in _kitchen.Equipment)
    //        {
    //            if (equipment.CanProcess(product) && !equipment.IsBusy)
    //                return equipment;
    //        }
    //        return null;
    //    }

    //    public bool IsEquipmentAvailable(Type equipmentType)
    //    {
    //        foreach (var equipment in _kitchen.Equipment)
    //        {
    //            if (equipment.GetType() == equipmentType && !equipment.IsBusy)
    //                return true;
    //        }
    //        return false;
    //    }

    //    public int GetBusyEquipmentCount()
    //    {
    //        int count = 0;
    //        foreach (var equipment in _kitchen.Equipment)
    //        {
    //            if (equipment.IsBusy) count++;
    //        }
    //        return count;
    //    }
    //}
}