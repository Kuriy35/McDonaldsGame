using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using McDonalds.Commands;
using McDonalds.Models.Orders;
using McDonalds.Models.Restaurant;
using McDonalds.Models.Core;
using static McDonalds.Models.Orders.Fries;

namespace McDonalds.ViewModels
{
    /// <summary>
    /// ViewModel для кухонного обладнання - виправлений
    /// </summary>
    public class KitchenEquipmentViewModel : ViewModelBase
    {
        private ObservableCollection<ProductOption> _availableProducts;
        private ProductOption _selectedProductOption;
        private bool _menuVisibility = false; // Змінено тип на bool
        private KitchenEquipment _equipment;

        public KitchenEquipment Equipment
        {
            get => _equipment;
            set
            {
                _equipment = value;
                OnPropertyChanged();
            }
        }

        public string Name => _equipment.Name;
        public string IconPath => _equipment.IconPath;

        public bool IsBusy
        {
            get => _equipment.IsBusy;
            private set
            {
                // Property is set within the equipment class
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        public bool IsAvailable => !IsBusy && !HasReadyProduct;

        public float ProcessingProgress
        {
            get => _equipment.ProcessingProgress;
            set
            {
                // Property is set within the equipment class
                OnPropertyChanged();
            }
        }

        public bool HasReadyProduct
        {
            get => _equipment.HasReadyProduct;
            private set
            {
                // Property is set within the equipment class
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        public ObservableCollection<ProductOption> AvailableProducts
        {
            get => _availableProducts;
            set => SetProperty(ref _availableProducts, value);
        }

        public ProductOption SelectedProductOption
        {
            get => _selectedProductOption;
            set => SetProperty(ref _selectedProductOption, value);
        }

        // Змінено тип на bool (замість Visibility)
        public bool MenuVisibility
        {
            get => _menuVisibility;
            set => SetProperty(ref _menuVisibility, value);
        }

        public ICommand ShowMenuCommand { get; }
        public ICommand SelectProductCommand { get; }
        public ICommand CloseMenuCommand { get; }

        public KitchenEquipmentViewModel(KitchenEquipment equipment)
        {
            _equipment = equipment ?? throw new ArgumentNullException(nameof(equipment));

            ShowMenuCommand = new RelayCommand(ShowMenu);
            SelectProductCommand = new RelayCommand(SelectProduct);
            CloseMenuCommand = new RelayCommand(CloseMenu);

            InitializeAvailableProducts();
        }

        public void Update(float deltaTime)
        {
            bool wasHasReadyProduct = HasReadyProduct;
            float oldProgress = ProcessingProgress;

            _equipment.UpdateProcessing(deltaTime);

            if (oldProgress != _equipment.ProcessingProgress)
            {
                ProcessingProgress = _equipment.ProcessingProgress;
            }

            if (IsBusy != _equipment.IsBusy)
            {
                IsBusy = _equipment.IsBusy;
            }

            if (wasHasReadyProduct != _equipment.HasReadyProduct)
            {
                HasReadyProduct = _equipment.HasReadyProduct;
            }
        }

        public bool CanProcess(Product product)
        {
            return _equipment.CanProcess(product);
        }

        public void StartProcessing(Product product)
        {
            _equipment.StartProcessing(product);
            IsBusy = _equipment.IsBusy;
            HasReadyProduct = _equipment.HasReadyProduct;
            ProcessingProgress = _equipment.ProcessingProgress;
        }

        public Product TakeReadyProduct()
        {
            Product product = _equipment.TakeReadyProduct();
            if (product != null)
            {
                HasReadyProduct = _equipment.HasReadyProduct;
                ProcessingProgress = _equipment.ProcessingProgress;
            }

            return product;
        }

        private void InitializeAvailableProducts()
        {
            AvailableProducts = new ObservableCollection<ProductOption>();

            if (Equipment is Grill)
            {
                // Стандартний бургер
                AvailableProducts.Add(new ProductOption
                {
                    Name = "Standard Burger",
                    CreateProduct = () => new Burger()
                });

                // Cheeseburger
                AvailableProducts.Add(new ProductOption
                {
                    Name = "Cheeseburger",
                    CreateProduct = () => new Burger { HasCheese = true }
                });

                // Bacon Burger
                AvailableProducts.Add(new ProductOption
                {
                    Name = "Bacon Burger",
                    CreateProduct = () => new BaconBurgerDecorator(new Burger()).ToProduct()
                });

                // Spicy Burger
                AvailableProducts.Add(new ProductOption
                {
                    Name = "Spicy Burger",
                    CreateProduct = () => new SpicyBurgerDecorator(new Burger()).ToProduct()
                });

                // Double Cheese Burger
                AvailableProducts.Add(new ProductOption
                {
                    Name = "Double Cheese Burger",
                    CreateProduct = () => new DoubleCheeseBurgerDecorator(new Burger()).ToProduct()
                });

                // Bacon Spicy Burger
                AvailableProducts.Add(new ProductOption
                {
                    Name = "Bacon Spicy Burger",
                    CreateProduct = () => new BaconBurgerDecorator((Burger)new SpicyBurgerDecorator(new Burger()).ToProduct()).ToProduct()
                });
            }
            else if (Equipment is Fryer)
            {
                // Картопля фрі (різні розміри, з сиром, велика порція)
                foreach (FriesSize size in Enum.GetValues(typeof(FriesSize)))
                {
                    AvailableProducts.Add(new ProductOption
                    {
                        Name = $"Fries ({size})",
                        CreateProduct = () => new Fries(null, size)
                    });

                    AvailableProducts.Add(new ProductOption
                    {
                        Name = $"Cheese Fries ({size})",
                        CreateProduct = () => new CheeseFriesDecorator(new Fries(null, size)).ToProduct()
                    });
                }
            }
            else if (Equipment is DrinksVendingMachine)
            {
                foreach (DrinkType type in Enum.GetValues(typeof(DrinkType)))
                {
                    foreach (DrinkSize size in Enum.GetValues(typeof(DrinkSize)))
                    {
                        // Звичайний напій
                        AvailableProducts.Add(new ProductOption
                        {
                            Name = $"{type} ({size})",
                            CreateProduct = () => new Drink(null, type, size)
                        });

                        // З льодом
                        AvailableProducts.Add(new ProductOption
                        {
                            Name = $"{type} ({size}, Iced)",
                            CreateProduct = () => new IcedDrinkDecorator(new Drink(null, type, size)).ToProduct()
                        });
                    }
                }
            }
        }

        private void ShowMenu(object parameter)
        {
            // Оновити доступність продуктів
            foreach (var option in AvailableProducts)
            {
                option.CheckResources();
            }
            MenuVisibility = true; // Змініть на true замість Visibility.Visible
        }

        private void SelectProduct(object parameter)
        {
            if (SelectedProductOption != null && SelectedProductOption.IsAvailable)
            {
                Product productToProcess = SelectedProductOption.CreateProduct();
                StartProcessing(productToProcess);
                MenuVisibility = false;
            }
        }

        private void CloseMenu(object parameter)
        {
            MenuVisibility = false; // Змініть на false замість Visibility.Collapsed
        }
    }
}