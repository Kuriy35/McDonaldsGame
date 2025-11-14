using System;

namespace McDonalds.Models.Customers
{
    public class PatientCustomerStrategy : IPatienceStrategy
    {
        private Random _random = new Random();

        public float CalculateBasePatience()
        {
            return 50.0f + (float)_random.NextDouble() * 50.0f;
        }

        public bool IsSatisfied(float waitTime, float patience)
        {
            return waitTime < patience * 0.9f;
        }
    }

    public class NormalCustomerStrategy : IPatienceStrategy
    {
        private Random _random = new Random();

        public float CalculateBasePatience()
        {
            return 30.0f + (float)_random.NextDouble() * 35.0f;
        }

        public bool IsSatisfied(float waitTime, float patience)
        {
            return waitTime < patience * 0.8f;
        }
    }

    public class ImpatientCustomerStrategy : IPatienceStrategy
    {
        private Random _random = new Random();

        public float CalculateBasePatience()
        {
            return 15.0f + (float)_random.NextDouble() * 20.0f;
        }

        public bool IsSatisfied(float waitTime, float patience)
        {
            return waitTime < patience * 0.7f;
        }
    }
}