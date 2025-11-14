using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using McDonalds.Commands;
using McDonalds.Models.Orders;
using McDonalds.Models.Restaurant;
using System.Collections.Generic;

namespace McDonalds.ViewModels
{
    public class KitchenViewModel : ViewModelBase
    {
        private Kitchen _kitchen;
        private ObservableCollection<KitchenEquipmentViewModel> _equipment;
        private ObservableCollection<OrderViewModel> _orderQueue;
        private ObservableCollection<Product> _readyProducts;
        private ObservableCollection<GroupedProductViewModel> _groupedReadyProducts;
        private Order _selectedOrder;
        private GameViewModel _gameViewModel;

        public ObservableCollection<KitchenEquipmentViewModel> Equipment
        {
            get => _equipment;
            set => SetProperty(ref _equipment, value);
        }

        public ObservableCollection<OrderViewModel> OrderQueue
        {
            get => _orderQueue;
            set => SetProperty(ref _orderQueue, value);
        }

        public ObservableCollection<Product> ReadyProducts
        {
            get => _readyProducts;
            set => SetProperty(ref _readyProducts, value);
        }

        public ObservableCollection<GroupedProductViewModel> GroupedReadyProducts
        {
            get => _groupedReadyProducts;
            set => SetProperty(ref _groupedReadyProducts, value);
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public ObservableCollection<ResourceViewModel> Resources
        {
            get => _gameViewModel?.Resources;
        }

        public ICommand BuyCommand
        {
            get => _gameViewModel?.BuyCommand;
        }

        public ICommand SellCommand
        {
            get => _gameViewModel?.SellCommand;
        }

        public ICommand UseEquipmentCommand { get; }
        public ICommand TakeProductCommand { get; }
        public ICommand AssembleOrderCommand { get; }
        public ICommand PurchaseResourceCommand { get; }

        public void SetGameViewModel(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            OnPropertyChanged(nameof(Resources));
            OnPropertyChanged(nameof(BuyCommand));
            OnPropertyChanged(nameof(SellCommand));
        }

        public KitchenViewModel(Kitchen kitchen = null)
        {
            _kitchen = kitchen ?? new Kitchen();
            _equipment = new ObservableCollection<KitchenEquipmentViewModel>();
            if (kitchen != null)
            {
                foreach (var equipmentItem in kitchen.Equipment)
                {
                    _equipment.Add(new KitchenEquipmentViewModel(equipmentItem));
                }
            }
            _orderQueue = new ObservableCollection<OrderViewModel>();
            _readyProducts = new ObservableCollection<Product>();
            _groupedReadyProducts = new ObservableCollection<GroupedProductViewModel>();

            UseEquipmentCommand = new RelayCommand(UseEquipment);
            TakeProductCommand = new RelayCommand(TakeProduct);
            AssembleOrderCommand = new RelayCommand(AssembleOrder, CanAssembleOrder);

            _readyProducts.CollectionChanged += (s, e) => UpdateGroupedProducts();
        }

        public void Update(TimeSpan deltaTime)
        {
            foreach (var equipment in Equipment)
            {
                equipment.Update((float)deltaTime.TotalSeconds);
            }

            foreach (var orderVM in OrderQueue)
            {
                foreach (var product in orderVM.Products)
                {
                    product.UpdatePreparation(deltaTime);
                }
            }
        }

        private void UpdateGroupedProducts()
        {
            var grouped = _readyProducts
                .GroupBy(p => p.Name)
                .Select(g => new GroupedProductViewModel(
                    g.Key,
                    g.Count(),
                    g.First().State,
                    g.First().IconPath
                ))
                .ToList();

            GroupedReadyProducts.Clear();
            foreach (var item in grouped)
            {
                GroupedReadyProducts.Add(item);
            }
        }

        public void AddOrderToQueue(OrderViewModel orderVM)
        {
            OrderQueue.Add(orderVM);
        }

        private void UseEquipment(object parameter)
        {
            if (parameter is KitchenEquipmentViewModel equipmentVM)
            {
                if (!equipmentVM.IsBusy && !equipmentVM.HasReadyProduct)
                {
                    equipmentVM.Equipment.StartProcessing();
                }
                else if (equipmentVM.HasReadyProduct)
                {
                    TakeProduct(equipmentVM);
                }
            }
        }

        private void TakeProduct(object parameter)
        {
            if (parameter is KitchenEquipmentViewModel equipmentVM && equipmentVM.Equipment.HasReadyProduct)
            {
                Product readyProduct = equipmentVM.TakeReadyProduct();
                if (readyProduct != null && readyProduct.State == ProductState.Ready)
                {
                    ReadyProducts.Add(readyProduct);
                }
            }
        }

        private void AssembleOrder(object parameter)
        {
            if (parameter is OrderViewModel orderVM && !orderVM.IsCompleted)
            {
                bool allProductsAvailable = true;
                var productsToConsume = new List<Product>();

                foreach (var productVM in orderVM.Products)
                {
                    Product baseProduct = productVM.Product;
                    while (baseProduct is ProductDecorator decorator)
                    {
                        baseProduct = decorator.GetDecoratedProduct();
                        productsToConsume.Add(decorator);
                    }
                    productsToConsume.Add(baseProduct);

                    var matchingProduct = ReadyProducts.FirstOrDefault(p =>
                        p.GetType() == baseProduct.GetType() &&
                        p.Name == baseProduct.Name &&
                        p.State == ProductState.Ready);

                    if (matchingProduct == null)
                    {
                        allProductsAvailable = false;
                        break;
                    }
                }

                if (allProductsAvailable)
                {
                    foreach (var productVM in orderVM.Products)
                    {
                        Type baseProductType = productVM.Product.GetType();
                        while (baseProductType.BaseType == typeof(ProductDecorator))
                        {
                            baseProductType = baseProductType.BaseType;
                        }

                        var matchingProduct = ReadyProducts.FirstOrDefault(p =>
                            p.GetType() == baseProductType &&
                            p.Name == productVM.Product.Name &&
                            p.State == ProductState.Ready);

                        if (matchingProduct != null)
                        {
                            ReadyProducts.Remove(matchingProduct);
                            productVM.State = ProductState.Ready;
                        }

                        orderVM.IsCompleted = true;
                    }

                    orderVM.IsCompleted = true;
                }
            }
        }

        private bool CanAssembleOrder(object parameter)
        {
            if (parameter is OrderViewModel orderVM && !orderVM.IsCompleted)
            {
                foreach (var productVM in orderVM.Products)
                {
                    var matchingProduct = ReadyProducts.FirstOrDefault(p =>
                        p.GetType() == productVM.Product.GetType() &&
                        p.Name == productVM.Product.Name &&
                        p.State == ProductState.Ready);

                    if (matchingProduct == null)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}