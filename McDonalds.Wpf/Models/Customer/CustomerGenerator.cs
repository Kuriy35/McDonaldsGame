using System;
using McDonalds.Models.Orders;

namespace McDonalds.Models.Customers
{
    public class CustomerGenerator
    {
        public float MinSpawnTime { get; set; }
        public float MaxSpawnTime { get; set; }
        public float CurrentSpawnTime { get; set; }
        public OrderComplexity DefaultComplexity { get; set; }
        public IPatienceStrategy Strategy { get; set; }
        private Random random;

        public CustomerGenerator(OrderComplexity complexity = OrderComplexity.Medium, float minTime = 10.0f, float maxTime = 30.0f)
        {
            MinSpawnTime = minTime;
            MaxSpawnTime = maxTime;
            CurrentSpawnTime = 0;
            DefaultComplexity = complexity;
            random = new Random();
        }

        public void UpdateSpawnTimer(float deltaTime)
        {
            CurrentSpawnTime -= deltaTime;
        }

        public Customer GenerateCustomer()
        {
            if (CurrentSpawnTime <= 0)
            {
                CurrentSpawnTime = MinSpawnTime + (float)random.NextDouble() * (MaxSpawnTime - MinSpawnTime);

                int customerType = random.Next(10);

                if (customerType < 2)
                    Strategy = new PatientCustomerStrategy();
                else if (customerType < 5)
                    Strategy = new ImpatientCustomerStrategy();
                else
                    Strategy = new NormalCustomerStrategy();

                return new Customer();
            }

            return null;
        }
    }
}
