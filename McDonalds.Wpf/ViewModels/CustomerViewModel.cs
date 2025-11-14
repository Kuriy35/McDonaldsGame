using System;
using System.Windows;
using McDonalds.Models.Customers;
using McDonalds.Models.Orders;
using McDonalds.Models.Restaurant;

namespace McDonalds.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private Customer _customer;
        private OrderViewModel _currentOrder;
        private TableViewModel _assignedTable;

        public Customer Customer => _customer;

        public ICustomerState State
        {
            get => _customer.State;
            set
            {
                if (_customer.State != value)
                {
                    _customer.State = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StateType));
                    OnPropertyChanged(nameof(IsHappy));
                }
            }
        }

        public CustomerState StateType => _customer.State.Type;

        public OrderViewModel CurrentOrder
        {
            get => _currentOrder;
            private set
            {
                if (_currentOrder != value)
                {
                    _currentOrder = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasOrder));
                }
            }
        }

        public float Patience
        {
            get => _customer.Patience;
            set
            {
                if (_customer.Patience != value)
                {
                    _customer.Patience = value;
                    OnPropertyChanged();
                }
            }
        }

        public float CurrentWaitTime
        {
            get => _customer.CurrentWaitTime;
            set
            {
                if (_customer.CurrentWaitTime != value)
                {
                    _customer.CurrentWaitTime = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(WaitTimeProgress));
                    OnPropertyChanged(nameof(IsHappy));
                }
            }
        }

        public float WaitTimeProgress
        {
            get => _customer.WaitTimeProgress;
            set 
            {
                OnPropertyChanged();
            }
        }

        public bool IsHappy => _customer.IsHappy;

        public bool HasOrder => _customer.HasOrder;

        public TableViewModel AssignedTable
        {
            get => _assignedTable;
            private set
            {
                if (_assignedTable != value)
                {
                    _assignedTable = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentPosition));
                }
            }
        }

        public Point CurrentPosition => _customer.CurrentPosition;

        public CustomerViewModel(Customer customer)
        {
            _customer = customer;

            // Ініціалізуємо OrderViewModel, якщо у клієнта вже є замовлення
            if (_customer.CurrentOrder != null)
            {
                _currentOrder = new OrderViewModel(_customer.CurrentOrder);
            }
        }

        public void AssignTable(TableViewModel tableVM)
        {
            if (tableVM != null && !tableVM.IsOccupied)
            {
                _customer.AssignTable(tableVM.Table);
                AssignedTable = tableVM;
                tableVM.CurrentCustomer = this;
                OnPropertyChanged(nameof(CurrentPosition));
            }
        }

        public void LeaveTable()
        {
            if (AssignedTable != null)
            {
                _customer.LeaveTable();
                AssignedTable.CurrentCustomer = null;
                AssignedTable = null;
                OnPropertyChanged(nameof(CurrentPosition));
            }
        }

        public void WaitForOrder()
        {
            Customer.WaitForOrder();
        }

        public void ReceiveOrder(OrderViewModel orderVM)
        {
            if (CurrentOrder != null && CurrentOrder.Order.Id == orderVM.Order.Id)
            {
                Customer.ReceiveOrder(orderVM.Order);
                OnPropertyChanged(nameof(IsHappy));
            }
        }

        public void UpdateWaitTime(float deltaTime)
        {
            // Оновлюємо базову модель
            _customer.UpdateWaitTime(deltaTime);

            // Оновлюємо відповідні властивості ViewModel
            OnPropertyChanged(nameof(CurrentWaitTime));
            OnPropertyChanged(nameof(WaitTimeProgress));
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(IsHappy));

            // Якщо клієнт вийшов, оновлюємо статус столу
            if (StateType == CustomerState.Leaving && AssignedTable != null)
            {
                LeaveTable();
            }
        }

        public void RefreshProperties()
        {
            // Якщо у клієнта з'явилось замовлення, яке ще не відображено у ViewModel
            if (_customer.CurrentOrder != null && (_currentOrder == null || _currentOrder.Order != _customer.CurrentOrder))
            {
                CurrentOrder = new OrderViewModel(_customer.CurrentOrder);
            }

            // Оновлюємо всі інші властивості
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(Patience));
            OnPropertyChanged(nameof(CurrentWaitTime));
            OnPropertyChanged(nameof(WaitTimeProgress));
            OnPropertyChanged(nameof(IsHappy));
            OnPropertyChanged(nameof(HasOrder));
            OnPropertyChanged(nameof(CurrentPosition));
        }
    }
}