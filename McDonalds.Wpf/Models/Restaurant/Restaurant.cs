using McDonalds.Models.Customers;
using System.Collections.Generic;

namespace McDonalds.Models.Restaurant
{
    public class Restaurant
    {
        public string Name { get; set; }
        public Kitchen Kitchen { get; set; }
        public DiningArea DiningArea { get; set; }
        public List<Counter> Counters { get; set; }
        public List<Customer> ActiveCustomers { get; private set; }

        public Restaurant(string name)
        {
            Name = name;
            Kitchen = new Kitchen();
            DiningArea = new DiningArea();
            Counters = new List<Counter>();
            ActiveCustomers = new List<Customer>();
        }

        public void AddCustomer(Customer customer)
        {
            ActiveCustomers.Add(customer);
        }

        public void AddCounters(int countersCount)
        {
            for (int i = 0; i < countersCount; i++)
            {
                Counters.Add(new Counter());
            }
        }

        public void RemoveCustomer(Customer customer)
        {
            ActiveCustomers.Remove(customer);
        }
    }
}