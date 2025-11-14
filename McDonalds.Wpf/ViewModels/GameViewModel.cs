using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using McDonalds.Commands;
using McDonalds.Wpf;
using McDonalds.Models.Core;
using McDonalds.Models.Customers;
using McDonalds.Models.Factory;
using McDonalds.Models.Orders;
using McDonalds.Models.Restaurant;
using McDonalds.Repositories;
using System.Windows.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace McDonalds.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private RestaurantViewModel _restaurantViewModel;
        private KitchenViewModel _kitchenViewModel;
        private GameManager _gameManager;
        private string _statusMessage;
        private bool _isGameOver;
        private double _money;
        private TimeSpan _currentTime;
        private TimeSpan _workdayDuration;
        private double _customerSatisfaction;
        private int _activeCustomersCount;
        private CustomerGenerator _customerGenerator;
        private GameGoals _currentGoals;
        private Visibility _goalsVisibility;
        private DispatcherTimer _timer;
        private ObservableCollection<ResourceViewModel> _resources;
        private ResourceRepository _resourceRepository;

        public Visibility GoalsVisibility
        {
            get => _goalsVisibility;
            set => SetProperty(ref _goalsVisibility, value);
        }

        public RestaurantViewModel RestaurantViewModel
        {
            get => _restaurantViewModel;
            set => SetProperty(ref _restaurantViewModel, value);
        }

        public KitchenViewModel KitchenViewModel
        {
            get => _kitchenViewModel;
            set => SetProperty(ref _kitchenViewModel, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsGameOver
        {
            get => _isGameOver;
            set => SetProperty(ref _isGameOver, value);
        }

        public double Money
        {
            get => _money;
            set => SetProperty(ref _money, value);
        }

        public TimeSpan CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }

        public TimeSpan WorkdayDuration
        {
            get => _workdayDuration;
            set => SetProperty(ref _workdayDuration, value);
        }

        public double CustomerSatisfaction
        {
            get => _customerSatisfaction;
            set => SetProperty(ref _customerSatisfaction, value);
        }

        public int ActiveCustomersCount
        {
            get => _activeCustomersCount;
            set => SetProperty(ref _activeCustomersCount, value);
        }

        public GameGoals CurrentGoals
        {
            get => _currentGoals;
            set => SetProperty(ref _currentGoals, value);
        }

        public ICommand AcceptOrderCommand { get; }
        public ICommand RejectOrderCommand { get; }
        public ICommand ServeOrderCommand { get; }
        public ICommand EndDayCommand { get; }
        public ICommand ToggleGoalsVisibilityCommand { get; }
        public ICommand BuyCommand { get; }
        public ICommand SellCommand { get; }

        public ObservableCollection<ResourceViewModel> Resources
        {
            get => _resources;
            set => SetProperty(ref _resources, value);
        }

        public GameViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _gameManager = GameManager.Instance;
            _restaurantViewModel = new RestaurantViewModel();
            _kitchenViewModel = new KitchenViewModel();
            _resourceRepository = App.Services.GetRequiredService<ResourceRepository>();
            _resources = new ObservableCollection<ResourceViewModel>();

            AcceptOrderCommand = new RelayCommand(AcceptOrder);
            RejectOrderCommand = new RelayCommand(RejectOrder);
            ServeOrderCommand = new RelayCommand(ServeOrder);
            EndDayCommand = new RelayCommand(_ => EndDay());
            ToggleGoalsVisibilityCommand = new RelayCommand(ToggleGoalsVisibility);
            BuyCommand = new RelayCommand(Buy);
            SellCommand = new RelayCommand(Sell);

            ResourceManager.ResourceQuantityChanged += OnResourceQuantityChanged;

            GoalsVisibility = Visibility.Visible;
        }

        private void OnResourceQuantityChanged(string resourceName, int newQuantity)
        {
            var vm = Resources.FirstOrDefault(r => r.Name == resourceName);
            if (vm != null)
            {
                int index = Resources.IndexOf(vm);
                if (index != -1)
                {
                    var updatedVm = new ResourceViewModel(vm.Resource)
                    {
                        TradeQuantity = vm.TradeQuantity
                    };
                    updatedVm.Quantity = newQuantity;
                    updatedVm.IsHighlighted = true;

                    Resources[index] = updatedVm;

                    Task.Delay(500).ContinueWith(_ =>
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            updatedVm.IsHighlighted = false;
                        }));
                }
            }
        }

        public void StartNewGame(GameDifficulty difficulty)
        {
            _gameManager.StartGame(difficulty);

            DifficultyFactory factory = DifficultyFactory.GetFactory(difficulty);

            Restaurant restaurant = _gameManager.CurrentRestaurant;
            _customerGenerator = factory.CreateCustomerGenerator();
            factory.SetResources();
            LoadResources();

            RestaurantViewModel = new RestaurantViewModel(restaurant);
            KitchenViewModel = new KitchenViewModel(restaurant.Kitchen);
            KitchenViewModel.SetGameViewModel(this);

            CurrentGoals = _gameManager.CurrentGoals;

            Money = _gameManager.Money;
            CurrentTime = _gameManager.CurrentTime;
            WorkdayDuration = _gameManager.WorkdayDuration;
            CustomerSatisfaction = 0;
            ActiveCustomersCount = 0;
            IsGameOver = false;
            StatusMessage = $"День почався! Складність: {difficulty}";

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += (sender, e) => Update(TimeSpan.FromSeconds(0.1));
            _timer.Start();
        }

        private void Update(TimeSpan deltaTime)
        {
            if (IsGameOver) return;

            var satisfiedCustomers = RestaurantViewModel.GetSatisfiedCustomersCount();
            var totalCustomers = RestaurantViewModel.GetTotalCustomersCount();

            _gameManager.UpdateCustomersCount(satisfiedCustomers, totalCustomers);
            _gameManager.CalculateCustomersSatisfaction();
            _gameManager.UpdateGame(deltaTime);

            Money = _gameManager.Money;
            CurrentTime = _gameManager.CurrentTime;
            CustomerSatisfaction = _gameManager.CustomersSatisfaction;

            if (_customerGenerator != null)
            {
                _customerGenerator.UpdateSpawnTimer((float)deltaTime.TotalSeconds);
                Customer newCustomer = _customerGenerator.GenerateCustomer();
                if (newCustomer != null)
                {
                    RestaurantViewModel.AddCustomer(newCustomer);
                }
            }

            ActiveCustomersCount = RestaurantViewModel.CustomerCount;

            if (CurrentTime >= WorkdayDuration)
            {
                EndDay();
            }

            RestaurantViewModel.Update(deltaTime);
            KitchenViewModel.Update(deltaTime);
        }

        private void AcceptOrder(object orderParameter)
        {
            if (orderParameter is OrderViewModel orderVM)
            {
                bool allResourcesAvailable = true;
                string missingResource = null;

                foreach (var productVM in orderVM.Products)
                {
                    var product = productVM.Product;

                    Product baseProduct = product;
                    while (baseProduct is ProductDecorator decorator)
                    {
                        baseProduct = decorator.GetDecoratedProduct();
                    }

                    if (!ResourceManager.Instance.HasResources(baseProduct))
                    {
                        allResourcesAvailable = false;
                        missingResource = baseProduct.Name;
                        break;
                    }
                }

                if (allResourcesAvailable)
                {
                    KitchenViewModel.AddOrderToQueue(orderVM);
                    StatusMessage = "Замовлення прийнято!";
                    RestaurantViewModel.CurrentCustomerAtCounter.WaitForOrder();

                    CustomerViewModel customerVM = RestaurantViewModel.FindCustomerByOrder(orderVM);
                    TableViewModel tableVM = RestaurantViewModel.GetFreeTableVM();

                    if (tableVM != null)
                    {
                        customerVM.AssignTable(tableVM);
                    }

                    RestaurantViewModel.TakeNextCustomerOrder();
                }
                else
                {
                    StatusMessage = $"Недостатньо ресурсів для {missingResource}!";
                    RejectOrder(orderVM);
                }
            }
        }

        private void RejectOrder(object orderParameter)
        {
            if (orderParameter is OrderViewModel order)
            {
                RestaurantViewModel.RejectCustomerOrder(order);
                StatusMessage = "Замовлення відхилено!";
            }
        }

        private void ServeOrder(object orderParameter)
        {
            if (orderParameter is OrderViewModel orderVM)
            {
                CustomerViewModel customer = RestaurantViewModel.FindCustomerByOrder(orderVM);
                if (customer != null)
                {
                    if (orderVM.AreAllProductsReady())
                    {
                        customer.ReceiveOrder(orderVM);
                        _gameManager.UpdateBalance((double)orderVM.TotalPrice);

                        StatusMessage = "Замовлення доставлено!";
                        KitchenViewModel.OrderQueue.Remove(orderVM);
                    }
                    else
                    {
                        StatusMessage = "Замовлення ще не зібране!";
                    }
                }
                else
                {
                    StatusMessage = "Клієнт пішов, він не дочекався замовлення!";
                    KitchenViewModel.OrderQueue.Remove(orderVM);
                }
            }
        }

        private void EndDay()
        {
            IsGameOver = true;
            _timer.Stop();
            bool goalsReached = _gameManager.EndGame();
            _mainViewModel.CurrentViewModel = new EndDayViewModel(_mainViewModel, goalsReached, Money, CustomerSatisfaction);
        }

        private void ToggleGoalsVisibility(object obj)
        {
            GoalsVisibility = (GoalsVisibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void LoadResources()
        {
            Resources.Clear();
            var allResources = _resourceRepository.GetAll().ToList();
            foreach (var resource in allResources)
            {
                Resources.Add(new ResourceViewModel(resource));
            }
        }
        private void Buy(object parameter)
        {
            if (parameter is ResourceViewModel resourceVM)
            {
                int quantity = resourceVM.TradeQuantity;
                if (quantity <= 0)
                {
                    StatusMessage = "Кількість повинна бути більше 0!";
                    return;
                }

                decimal totalCost = resourceVM.BuyPrice * quantity;

                if ((decimal)Money < totalCost)
                {
                    decimal missing = totalCost - (decimal)Money;
                    StatusMessage = $"Недостатньо грошей! Не вистачає {missing:F2} $ для покупки {quantity} × {resourceVM.DisplayName}";
                    return;
                }

                _gameManager.UpdateBalance(-(double)totalCost);
                Money = _gameManager.Money;

                ResourceManager.Instance.BuyResource(resourceVM.Name, quantity);

                StatusMessage = $"Куплено {quantity} × {resourceVM.DisplayName} за {totalCost:F2} $";
            }
        }

        private void Sell(object parameter)
        {
            if (parameter is ResourceViewModel resourceVM)
            {
                int quantity = resourceVM.TradeQuantity;
                if (quantity <= 0)
                {
                    StatusMessage = "Кількість повинна бути більше 0!";
                    return;
                }

                if (resourceVM.Quantity < quantity)
                {
                    int missing = quantity - resourceVM.Quantity;
                    StatusMessage = $"Недостатньо товару! Не вистачає {missing} × {resourceVM.DisplayName} для продажу";
                    return;
                }

                decimal totalIncome = resourceVM.SellPrice * quantity;

                _gameManager.UpdateBalance((double)totalIncome);
                Money = _gameManager.Money;

                ResourceManager.Instance.SellResource(resourceVM.Name, quantity);

                StatusMessage = $"Продано {quantity} × {resourceVM.DisplayName} за {totalIncome:F2} $";
            }
        }
    }
}