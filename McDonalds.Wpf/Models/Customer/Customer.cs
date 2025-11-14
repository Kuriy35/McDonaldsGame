using System;
using System.Windows;
using McDonalds.Models.Orders;
using McDonalds.Models.Restaurant;

namespace McDonalds.Models.Customers
{
    public class Customer
    {
        private ICustomerState _state;
        private bool _isHappy;
        private IPatienceStrategy _patienceStrategy;

        public ICustomerState State
        {
            get => _state;
            set
            {
                _state = value;
                CurrentWaitTime = 0;
            }
        }

        public bool IsHappy
        {
            get => _isHappy;
            set
            {
                _isHappy = value;
            }
        }

        public IPatienceStrategy PatienceStrategy
        {
            get => _patienceStrategy;
            set
            {
                _patienceStrategy = value;
            }
        }

        public Order CurrentOrder { get; set; }
        public float Patience { get; set; }
        public float CurrentWaitTime { get; set; }
        public float WaitTimeProgress { get; set; }

        public bool HasOrder => CurrentOrder != null;

        public Table AssignedTable { get; private set; }

        public Point TablePosition => AssignedTable?.TablePosition ?? new Point(0, 0);

        public Point SpawnPosition { get; } = new Point(15, 15);
        public Point CurrentPosition
        {
            get
            {
                return State.Type == CustomerState.InQueue || State.Type == CustomerState.Ordering
                    ? SpawnPosition
                    : TablePosition;
            }
        }

        public Customer(IPatienceStrategy strategy = null)
        {
            State = new InQueueState();
            CurrentOrder = null;
            _patienceStrategy = strategy ?? new NormalCustomerStrategy();
            Patience = _patienceStrategy.CalculateBasePatience();
            CurrentWaitTime = 0;
        }

        public void AssignTable(Table table)
        {
            if (table != null && !table.IsOccupied)
            {
                AssignedTable = table;
                table.OccupyTable(this);
            }
        }

        public void LeaveTable()
        {
            if (AssignedTable != null)
            {
                AssignedTable.FreeTable();
                AssignedTable = null;
            }
        }

        public void MakeOrder(OrderBuilder builder)
        {
            OrderDirector director = new OrderDirector(builder);
            int orderType = new Random().Next(10);

            if (orderType < 5) CurrentOrder = director.CreateStandardMeal();
            else if (orderType < 8) CurrentOrder = director.CreateChildMeal();
            else CurrentOrder = director.CreateBigMeal();
            //else CurrentOrder = director.CreateRandomMeal();

            State = new OrderingState();
        }

        public void WaitForOrder()
        {
            State = new WaitingOrderState();
        }

        public void ReceiveOrder(Order order)
        {
            if (CurrentOrder != null && CurrentOrder.Id == order.Id)
            {
                CurrentOrder.IsDelivered = true;
                State = new EatingState();
                CurrentWaitTime = 0;
            }
        }

        public void UpdateWaitTime(float deltaTime)
        {
            CalculateHappiness();
            State.Handle(this, deltaTime);
        }

        public void CalculateHappiness()
        {
            IsHappy = CurrentOrder != null && CurrentOrder.IsDelivered && _patienceStrategy.IsSatisfied(CurrentWaitTime, Patience);
        }
    }
}