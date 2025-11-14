using McDonalds.Models.Customers;
using System;
using System.Windows;

namespace McDonalds.Models.Restaurant
{
    public class Table
    {
        public int Number { get; private set; }
        public bool IsOccupied { get; set; }
        public Customer CurrentCustomer { get; set; }
        public Point TablePosition { get; private set; }

        public Table(int number, Point position)
        {
            Number = number;
            TablePosition = position;
            IsOccupied = false;
            CurrentCustomer = null;
        }

        public void OccupyTable(Customer customer)
        {
            IsOccupied = true;
            CurrentCustomer = customer;
        }

        public void FreeTable()
        {
            IsOccupied = false;
            CurrentCustomer = null;
        }
    }
}