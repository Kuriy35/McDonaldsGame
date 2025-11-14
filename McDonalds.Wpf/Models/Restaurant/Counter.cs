using McDonalds.Models.Customers;
using System.Collections.Generic;

namespace McDonalds.Models.Restaurant
{
    public class Counter
    {
        public Queue<Customer> CustomerQueue { get; private set; }
        public bool IsOccupied { get; private set; }
        public Customer CurrentCustomer { get; private set; }

        public Counter()
        {
            CustomerQueue = new Queue<Customer>();
            IsOccupied = false;
            CurrentCustomer = null;
        }

        public void AddCustomerToQueue(Customer customer)
        {
            CustomerQueue.Enqueue(customer);
        }

        public Customer ServeNextCustomer()
        {
            if (CustomerQueue.Count > 0 && !IsOccupied)
            {
                CurrentCustomer = CustomerQueue.Dequeue();
                IsOccupied = true;
                return CurrentCustomer;
            }
            return null;
        }

        public void FinishServing()
        {
            IsOccupied = false;
            CurrentCustomer = null;
        }
    }
}