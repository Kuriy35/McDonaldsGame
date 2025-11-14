using System;
using System.Collections.ObjectModel;
using System.Linq;
using McDonalds.Models.Customers;
using McDonalds.Models.Orders;
using McDonalds.Models.Restaurant;

namespace McDonalds.ViewModels
{
    public class RestaurantViewModel : ViewModelBase
    {
        private Restaurant _restaurant;
        private ObservableCollection<CustomerViewModel> _customers;
        private CustomerViewModel _currentCustomerAtCounter;
        private int _satisfiedCustomers;
        private int _totalCustomers;
        private ObservableCollection<TableViewModel> _tables;

        public Restaurant Restaurant
        {
            get => _restaurant;
            private set => _restaurant = value;
        }

        public ObservableCollection<TableViewModel> Tables
        {
            get => _tables;
            set => SetProperty(ref _tables, value);
        }

        public ObservableCollection<CustomerViewModel> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        public CustomerViewModel CurrentCustomerAtCounter
        {
            get => _currentCustomerAtCounter;
            set
            {
                if (SetProperty(ref _currentCustomerAtCounter, value))
                {
                    OnPropertyChanged(nameof(CurrentCustomerAtCounter));
                }
            }
        }

        public int CustomerCount => Customers?.Count ?? 0;

        public RestaurantViewModel(Restaurant restaurant = null)
        {
            Restaurant = restaurant ?? new Restaurant("No Name");
            _customers = new ObservableCollection<CustomerViewModel>();
            _satisfiedCustomers = 0;
            _totalCustomers = 0;

            _tables = new ObservableCollection<TableViewModel>();
            foreach (var table in Restaurant.DiningArea.Tables)
            {
                _tables.Add(new TableViewModel(table));
            }
        }

        public void Update(TimeSpan deltaTime)
        {
            foreach (var customer in Customers.ToList())
            {
                customer.UpdateWaitTime((float)deltaTime.TotalSeconds);

                if (customer.StateType == CustomerState.Leaving)
                {
                    RemoveCustomer(customer);
                }
            }

            foreach (var tableVM in Tables)
            {
                tableVM.UpdateTable();
            }

            if (CurrentCustomerAtCounter == null && Restaurant.Counters.Count > 0)
            {
                if (Restaurant.Counters[0].CustomerQueue.Count > 0)
                {
                    Restaurant.Counters[0].FinishServing();
                    var nextCustomer = Restaurant.Counters[0].ServeNextCustomer();

                    if (nextCustomer != null)
                    {
                        nextCustomer.MakeOrder(new OrderBuilder());

                        var customerVM = Customers.FirstOrDefault(c => c.Customer == nextCustomer);
                        if (customerVM != null)
                        {
                            CurrentCustomerAtCounter = customerVM;
                            customerVM.RefreshProperties();
                        }
                    }
                }
            }
        }

        public void AddCustomer(Customer customer)
        {
            var customerVM = new CustomerViewModel(customer);
            Customers.Add(customerVM);
            Restaurant.AddCustomer(customer);
            Restaurant.Counters[0].AddCustomerToQueue(customer);
            OnPropertyChanged(nameof(CustomerCount));
        }

        public void RemoveCustomer(CustomerViewModel customerVM)
        {
            Customer customer = customerVM.Customer;

            if (customer.IsHappy)
            {
                _satisfiedCustomers++;
            }
            _totalCustomers++;

            var tableVM = Tables.FirstOrDefault(t => t.CurrentCustomer?.Customer == customer);
            if (tableVM != null)
            {
                tableVM.FreeTable();
            }

            Customers.Remove(customerVM);
            Restaurant.RemoveCustomer(customer);

            if (CurrentCustomerAtCounter == customerVM)
            {
                TakeNextCustomerOrder();
            }

            OnPropertyChanged(nameof(CustomerCount));
        }

        public void TakeNextCustomerOrder()
        {
            Restaurant.Counters[0].FinishServing();
            CurrentCustomerAtCounter = null;
        }

        public void RejectCustomerOrder(OrderViewModel orderVM)
        {
            var customerVM = Customers.FirstOrDefault(c => c.CurrentOrder?.Order.Id == orderVM.Order.Id);
            if (customerVM != null)
            {
                customerVM.State = new LeavingState();
            }
        }

        public CustomerViewModel FindCustomerByOrder(OrderViewModel orderVM)
        {
            return Customers.FirstOrDefault(c => c.CurrentOrder?.Order.Id == orderVM.Order.Id);
        }

        public int GetSatisfiedCustomersCount()
        {
            return _satisfiedCustomers;
        }

        public int GetTotalCustomersCount()
        {
            return _totalCustomers;
        }

        public TableViewModel GetFreeTableVM()
        {
            Table freeTable = Restaurant.DiningArea.GetFreeTable();
            return Tables.FirstOrDefault(t => !t.IsOccupied);
        }
    }
}